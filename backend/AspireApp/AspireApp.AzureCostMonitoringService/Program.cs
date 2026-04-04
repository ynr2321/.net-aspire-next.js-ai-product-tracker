using Azure.Identity;
using Azure.Monitor.Query;
using Azure.ResourceManager;
using AspireApp.AzureCostMonitoringService;
using AspireApp.AzureCostMonitoringService.Models;
using AspireApp.AzureCostMonitoringService.Services.Metrics;
using AspireApp.AzureCostMonitoringService.Services.ResourceShutdown;

/* -------------------------------------------------------------------------------------
 * This worker service will monitor azure resources using a MetricsQueryService,
 * and dependign on rules set in configuration, will STOP resources that break rules
 * using ResourceShutDownService
 * -------------------------------------------------------------------------------------
 */
public class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Aspire service defaults: OpenTelemetry, health checks, resilience
        builder.AddServiceDefaults();

        // Typed configuration
        builder.Services.Configure<CostMonitoringOptions>(
            builder.Configuration.GetSection(CostMonitoringOptions.SectionName));

        // Azure clients — DefaultAzureCredential supports managed identity in Azure Container Apps
        // and falls back to Azure CLI / Visual Studio credentials for local development.
        // No secrets need to be stored; managed identity is sufficient when deployed.
        DefaultAzureCredential credential = new DefaultAzureCredential();
        builder.Services.AddSingleton(new ArmClient(credential));
        builder.Services.AddSingleton(new MetricsQueryClient(credential));

        // Services
        builder.Services.AddSingleton<IMetricsQueryService, MetricsQueryService>();
        builder.Services.AddSingleton<IResourceShutdownService, ResourceShutdownService>();

        // Worker
        builder.Services.AddHostedService<CostMonitoringWorker>();

        IHost host = builder.Build();
        host.Run();
    }
}
