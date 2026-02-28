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
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        /*
         * IF DOING LOCAL EF CORE MIGRATION:
         * Aspire generates a new Postgres connection string each time it spins up resource.
         * paste connection string here - aspire dashboard --> aspireapp --> connection string
         * Ensure the API is not running during migrations.
         */
        optionsBuilder.UseNpgsql("Host=localhost;Port=62350;Username=postgres;Password=d)x3X6w57DdthED*zqdwhF;Database=aspireapp");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}