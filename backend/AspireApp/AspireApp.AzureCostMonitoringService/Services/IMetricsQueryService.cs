namespace AspireApp.AzureCostMonitoringService.Services;

public interface IMetricsQueryService
{
    /// <summary>
    /// Queries the total request count for a container app over the specified time window.
    /// </summary>
    Task<double> GetRequestCountAsync(string resourceId, TimeSpan window, CancellationToken cancellationToken);
}
