using Azure;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

namespace AspireApp.AzureCostMonitoringService.Services.Metrics;

public class MetricsQueryService(MetricsQueryClient metricsClient, ILogger<MetricsQueryService> logger)
    : IMetricsQueryService
{
    public async Task<double> GetRequestCountAsync(string resourceId, TimeSpan window, CancellationToken cancellationToken)
    {
        // Azure Container Apps expose a "Requests" metric under the Microsoft.App/containerApps provider.
        // We query with Total aggregation to get the sum of requests per time-grain bucket.
        Response<MetricsQueryResult> response = await metricsClient.QueryResourceAsync(
            resourceId,
            ["Requests"],
            new MetricsQueryOptions
            {
                TimeRange = new QueryTimeRange(window),
                Granularity = TimeSpan.FromMinutes(1),
                Aggregations = { MetricAggregationType.Total }
            },
            cancellationToken);

        MetricResult? metric = response.Value.Metrics.FirstOrDefault();
        if (metric is null)
        {
            logger.LogWarning("No 'Requests' metric returned for resource {ResourceId}", resourceId);
            return 0;
        }

        double total = 0;
        foreach (MetricTimeSeriesElement? timeSeries in metric.TimeSeries)
        {
            foreach (MetricValue? value in timeSeries.Values)
            {
                total += value.Total ?? 0;
            }
        }

        return total;
    }
}
