using AspireApp.ApiService.Data.Entities;
using AspireApp.ApiService.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<ApiHealthLog> ApiHealthLogs { get; set; }

    // Override to configure how the tables will be structured in the database - see ApiHealthLogConfiguration as an example of custom config
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.ApplyConfiguration(new ApiHealthLogConfiguration());
    }
}
