using Azure.Identity;
using Azure.ResourceManager;

namespace AspireApp.AzureCostMonitoringService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton(new ArmClient(new DefaultAzureCredential()));
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}
