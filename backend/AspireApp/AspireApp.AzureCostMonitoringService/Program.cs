using Azure.Identity;
using Azure.Monitor.Query;
using Azure.ResourceManager;
using AspireApp.AzureCostMonitoringService;
using AspireApp.AzureCostMonitoringService.Models;
using AspireApp.AzureCostMonitoringService.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Aspire service defaults: OpenTelemetry, health checks, resilience
        builder.AddServiceDefaults();

        // Typed configuration
        builder.Services.Configure<CostMonitoringOptions>(
            builder.Configuration.GetSection(CostMonitoringOptions.SectionName));

        // Azure clients — DefaultAzureCredential supports managed identity in Azure Container Apps
        // and falls back to Azure CLI / Visual Studio credentials for local development.
        // No secrets need to be stored; managed identity is sufficient when deployed.
        var credential = new DefaultAzureCredential();
        builder.Services.AddSingleton(new ArmClient(credential));
        builder.Services.AddSingleton(new MetricsQueryClient(credential));

        // Services
        builder.Services.AddSingleton<IMetricsQueryService, MetricsQueryService>();
        builder.Services.AddSingleton<IResourceShutdownService, ResourceShutdownService>();

        // Worker
        builder.Services.AddHostedService<CostMonitoringWorker>();

        var host = builder.Build();
        host.Run();
    }
}
