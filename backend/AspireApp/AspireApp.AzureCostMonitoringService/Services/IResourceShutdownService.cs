namespace AspireApp.AzureCostMonitoringService.Services;

public interface IResourceShutdownService
{
    Task StopContainerAppAsync(string subscriptionId, string resourceGroup, string appName, CancellationToken cancellationToken);
    Task StopPostgresServerAsync(string subscriptionId, string resourceGroup, string serverName, CancellationToken cancellationToken);
}
