using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AspireApp.ApiService.Data;

/// <summary>
/// Provides a DbContext instance to Entity Framework Core design-time tools (e.g., dotnet ef commands).
/// Required for running migrations locally since Aspire's connection strings are generated at runtime.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // setup configuration access to get connection string for manual migrations
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("ManualMigrations");
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Could not get connection string from configuration 'ConnectionStrings' --> 'ManualMigrations'. Ensure json is formated correctly");
        }

        // We have auto migrations setup in Program.cs so only need to use this factory for manual ef core migrations
        /*
         * IF DOING LOCAL EF CORE MIGRATION:
         * paste connection string in appsettings development json
         * conn string available here at... aspire dashboard --> aspireapp --> connection string
         * Ensure the API is not running during migrations, but db containers are.
         */
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}