using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.PostgreSql.FlexibleServers;

namespace AspireApp.AzureCostMonitoringService.Services.ResourceShutdown;

/// <summary>
/// Service whose responsibiliy is to shutdown / stop azure resources
/// </summary>
public class ResourceShutdownService(ArmClient armClient, ILogger<ResourceShutdownService> logger)
    : IResourceShutdownService
{
    public async Task StopContainerAppAsync(
        string subscriptionId, string resourceGroup, string appName, CancellationToken cancellationToken)
    {
        ResourceIdentifier resourceId = ContainerAppResource.CreateResourceIdentifier(subscriptionId, resourceGroup, appName);
        ContainerAppResource containerApp = armClient.GetContainerAppResource(resourceId);

        try
        {
            logger.LogInformation("Stopping Container App {AppName} in {ResourceGroup}...", appName, resourceGroup);
            await containerApp.StopAsync(WaitUntil.Started, cancellationToken);
            logger.LogInformation("Stop initiated for Container App {AppName}", appName);
        }
        catch (RequestFailedException ex) when (ex.Status == 409)
        {
            logger.LogWarning("Container App {AppName} is already stopped or in a conflicting state", appName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to stop Container App {AppName}", appName);
        }
    }

    public async Task StopPostgresServerAsync(
        string subscriptionId, string resourceGroup, string serverName, CancellationToken cancellationToken)
    {
        ResourceIdentifier resourceId = PostgreSqlFlexibleServerResource.CreateResourceIdentifier(subscriptionId, resourceGroup, serverName);
        PostgreSqlFlexibleServerResource server = armClient.GetPostgreSqlFlexibleServerResource(resourceId);

        try
        {
            logger.LogInformation("Stopping PostgreSQL Flexible Server {ServerName} in {ResourceGroup}...", serverName, resourceGroup);
            await server.StopAsync(WaitUntil.Started, cancellationToken);
            logger.LogInformation("Stop initiated for PostgreSQL Flexible Server {ServerName}", serverName);
        }
        catch (RequestFailedException ex) when (ex.Status == 409)
        {
            logger.LogWarning("PostgreSQL Flexible Server {ServerName} is already stopped or in a conflicting state", serverName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to stop PostgreSQL Flexible Server {ServerName}", serverName);
        }
    }
}
