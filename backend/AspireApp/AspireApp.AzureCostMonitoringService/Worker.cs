using Azure.ResourceManager;


namespace AspireApp.AzureCostMonitoringService;

public class Worker: BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private ArmClient _armClient;

    public Worker(ArmClient armClient, ILogger<Worker> logger)
    {
        _armClient = armClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
}


// todo consider this achritectur idea from gemini:
//  Sentinel.CostMonitor/
//  ├── Program.cs                 # Bootstraps DI and Host
//  ├── Worker.cs                  # The BackgroundService (Main Loop)
//  │
//  ├── Abstractions/              # Interfaces for decoupling
//  │   ├── IMonitoringStrategy.cs
//  │   ├── IResourceCommand.cs
//  │   └── IResourceEvaluator.cs
//  │
//  ├── Models/                    # Data objects and Config
//  │   ├── ResourceContext.cs     # Metadata about the RG to monitor
//  │   ├── ThresholdConfig.cs     # Loaded from appsettings.json
//  │   └── TelemetryData.cs       # Unified object for cost/metrics
//  │
//  ├── Strategies/                # The "How we check"
//  │   ├── CpuUsageStrategy.cs    # Implementation: Azure Monitor API
//  │   └── ActualCostStrategy.cs  # Implementation: Azure Cost Management API
//  │
//  ├── Commands/                  # The "How we kill"
//  │   ├── StopContainerCommand.cs
//  │   └── DeallocateVmCommand.cs
//  │
//  ├── Services/                  # The "Brain"
//  │   ├── EvaluatorService.cs    # Logic: "If X > Y then Trigger Command"
//  │   └── AzureClientFactory.cs  # Manages ArmClient and Authentication
//  │
//  └── Infrastructure/            # SDK Wrappers
//      └── ArmResourceWrapper.cs  # Logic for Azure.ResourceManager

