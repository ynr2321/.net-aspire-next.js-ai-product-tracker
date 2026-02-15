using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AspireApp.ApiService.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use a default connection string for design-time operations
        // Connection string will be different every spin up,
        // so view it on the Aspire dashboard by clicking on the postgres-db-container resource
        // copy it and make sure the API resource is stopped before running dotnet ef database update
        optionsBuilder.UseNpgsql("PLACEHOLDER - GET CONNECTIONS STRING FROM ASPIRE DASHBOARD FOR NOW UNTIL FURTHER CONFIGURATION IS IN PLACE");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}