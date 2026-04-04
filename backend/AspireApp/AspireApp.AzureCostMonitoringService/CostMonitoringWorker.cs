using AspireApp.AzureCostMonitoringService.Enums;
using AspireApp.AzureCostMonitoringService.Models;
using AspireApp.AzureCostMonitoringService.Services.Metrics;
using AspireApp.AzureCostMonitoringService.Services.ResourceShutdown;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Microsoft.Extensions.Options;

namespace AspireApp.AzureCostMonitoringService;

public class CostMonitoringWorker(
    IMetricsQueryService metricsService,
    IResourceShutdownService shutdownService,
    ArmClient armClient,
    IOptions<CostMonitoringOptions> options,
    ILogger<CostMonitoringWorker> logger) : BackgroundService
{
    private readonly CostMonitoringOptions _options = options.Value;
    private string? _subscriptionId;

    // Main loop ----------------------------------------------------------------------
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // log start, displaying polling interval
        logger.LogInformation("Cost monitoring worker starting. Polling every {Interval}s",_options.PollingIntervalSeconds);

        // get azure subscription ID and validate
        _subscriptionId = await ResolveSubscriptionIdAsync(stoppingToken);
        if (string.IsNullOrEmpty(_subscriptionId))
        {
            logger.LogCritical(
                "Could not resolve Azure subscription. Set CostMonitoring:SubscriptionId or " +
                "CostMonitoring:SubscriptionName in configuration. Worker cannot operate.");
            return;
        }

        // log which subscription and resource group are about to be monitored
        logger.LogInformation("Monitoring subscription {SubscriptionId}, resource group {ResourceGroup}",
            _subscriptionId, _options.ResourceGroup);

        // main monitoring loop - monitor resources and stop rule breaking services (rules according to config)
        TimeSpan interval = TimeSpan.FromSeconds(_options.PollingIntervalSeconds);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EvaluateAllRulesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error during monitoring cycle. Continuing next cycle.");
            }

            await Task.Delay(interval, stoppingToken);
        }

        logger.LogInformation("Cost monitoring worker stopping");
    }

    // Private helpers ----------------------------------------------------------------------
    private async Task EvaluateAllRulesAsync(CancellationToken ct)
    {
        List<MonitoringRule> enabledRules = _options.Rules.Where(r => r.Enabled).ToList();
        if (enabledRules.Count == 0)
        {
            logger.LogDebug("No enabled monitoring rules configured");
            return;
        }

        foreach (MonitoringRule rule in enabledRules)
        {
            try
            {
                await EvaluateRuleAsync(rule, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error evaluating rule {RuleName}. Skipping to next rule.", rule.Name);
            }
        }
    }

    private async Task EvaluateRuleAsync(MonitoringRule rule, CancellationToken ct)
    {
        if (!string.Equals(rule.Metric, "Requests", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Extension point — add support for additional metrics (CPU, memory, cost) here.
            // Azure Cost Management data has 24-48h latency and is not suitable for near-real-time
            // enforcement. Request count is used as a proxy for cost because it is available in
            // near-real-time via Azure Monitor and correlates directly with consumption-based billing.
            logger.LogWarning("Unsupported metric '{Metric}' in rule {RuleName}. MVP supports 'Requests' only.",
                rule.Metric, rule.Name);
            return;
        }

        TimeSpan window = TimeSpan.FromMinutes(rule.WindowMinutes);
        double totalRequests = 0;

        foreach (ResourceTarget target in rule.MonitoredResources)
        {
            string? appName = ResolveContainerAppName(target);
            if (appName is null)
            {
                logger.LogWarning("Rule {RuleName}: target {Target} has no associated container app (metrics N/A)",
                    rule.Name, target);
                continue;
            }

            string resourceId = $"/subscriptions/{_subscriptionId}/resourceGroups/{_options.ResourceGroup}" +
                             $"/providers/Microsoft.App/containerApps/{appName}";

            double count = await metricsService.GetRequestCountAsync(resourceId, window, ct);
            logger.LogDebug("Rule {RuleName}: {Target} ({AppName}) = {Count} requests in {Window}min",
                rule.Name, target, appName, count, rule.WindowMinutes);

            totalRequests += count;
        }

        if (totalRequests > rule.Threshold)
        {
            logger.LogWarning(
                "BREACH | Rule: {RuleName} | Metric: {Metric} | Threshold: {Threshold} | " +
                "Observed: {Observed} | Window: {Window}min | Time: {Timestamp} | " +
                "Shutting down: [{Targets}]",
                rule.Name, rule.Metric, rule.Threshold, totalRequests,
                rule.WindowMinutes, DateTimeOffset.UtcNow,
                string.Join(", ", rule.ShutdownTargets));

            await ExecuteShutdownAsync(rule, ct);
        }
        else
        {
            logger.LogInformation("Rule {RuleName}: OK — {Observed}/{Threshold} requests in {Window}min window",
                rule.Name, totalRequests, rule.Threshold, rule.WindowMinutes);
        }
    }

    private async Task ExecuteShutdownAsync(MonitoringRule rule, CancellationToken ct)
    {
        foreach (ResourceTarget target in rule.ShutdownTargets)
        {
            switch (target)
            {
                case ResourceTarget.Frontend:
                    await shutdownService.StopContainerAppAsync(
                        _subscriptionId!, _options.ResourceGroup,
                        _options.Resources.FrontendContainerApp, ct);
                    break;
                case ResourceTarget.Backend:
                    await shutdownService.StopContainerAppAsync(
                        _subscriptionId!, _options.ResourceGroup,
                        _options.Resources.BackendContainerApp, ct);
                    break;
                case ResourceTarget.Database:
                    await shutdownService.StopPostgresServerAsync(
                        _subscriptionId!, _options.ResourceGroup,
                        _options.Resources.PostgresFlexibleServer, ct);
                    break;
            }
        }
    }

    private string? ResolveContainerAppName(ResourceTarget target) => target switch
    {
        ResourceTarget.Frontend => _options.Resources.FrontendContainerApp,
        ResourceTarget.Backend => _options.Resources.BackendContainerApp,
        _ => null // Database is not a container app; it has no request metrics
    };

    private async Task<string?> ResolveSubscriptionIdAsync(CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(_options.SubscriptionId)) return _options.SubscriptionId;

        if (string.IsNullOrEmpty(_options.SubscriptionName))
        {
            logger.LogError("Neither SubscriptionId nor SubscriptionName is configured");
            return null;
        }

        logger.LogInformation("Resolving subscription by display name: '{Name}'", _options.SubscriptionName);

        await foreach (SubscriptionResource sub in armClient.GetSubscriptions().GetAllAsync(ct))
        {
            if (string.Equals(sub.Data.DisplayName, _options.SubscriptionName, StringComparison.OrdinalIgnoreCase))
            {
                return sub.Data.SubscriptionId;
            }
        }

        logger.LogError("No subscription found with display name '{Name}'", _options.SubscriptionName);
        return null;
    }
}
