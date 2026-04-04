using AspireApp.AzureCostMonitoringService.Enums;

namespace AspireApp.AzureCostMonitoringService.Models;

public class CostMonitoringOptions
{
    public const string SectionName = "CostMonitoring";

    /// <summary>How often to poll Azure Monitor, in seconds.</summary>
    public int PollingIntervalSeconds { get; set; } = 60;

    /// <summary>Azure subscription ID. If empty, resolved from SubscriptionName at startup.</summary>
    public string SubscriptionId { get; set; } = string.Empty;

    /// <summary>Azure subscription display name, used to resolve SubscriptionId when it is not set directly.</summary>
    public string SubscriptionName { get; set; } = "Azure subscription 1";

    public string ResourceGroup { get; set; } = "rg-aspire-ai-product-tracker";

    public ResourceNames Resources { get; set; } = new();

    public List<MonitoringRule> Rules { get; set; } = [];
}

public class ResourceNames
{
    public string FrontendContainerApp { get; set; } = "frontend";
    public string BackendContainerApp { get; set; } = "apiservice";
    public string PostgresFlexibleServer { get; set; } = "postgresserver-wcfqosgiu5c3o";
}

public class MonitoringRule
{
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;

    /// <summary>Metric to evaluate. MVP supports: "Requests".</summary>
    public string Metric { get; set; } = "Requests";

    /// <summary>Time window in minutes to aggregate metrics over.</summary>
    public int WindowMinutes { get; set; } = 10;

    /// <summary>Breach occurs when observed value exceeds this threshold.</summary>
    public double Threshold { get; set; }

    /// <summary>Which container apps to query metrics from. Aggregated across all listed resources.</summary>
    public List<ResourceTarget> MonitoredResources { get; set; } = [];

    /// <summary>Which resources to stop when the threshold is breached.</summary>
    public List<ResourceTarget> ShutdownTargets { get; set; } = [];
}
