using AspireApp.ApiService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ApiHealthLog> ApiHealthLogs { get; set; }
    }
}
