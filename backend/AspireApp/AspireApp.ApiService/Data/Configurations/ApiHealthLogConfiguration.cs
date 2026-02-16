using AspireApp.ApiService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspireApp.ApiService.Data.Configurations;

public class ApiHealthLogConfiguration : IEntityTypeConfiguration<ApiHealthLog>
{
    public void Configure(EntityTypeBuilder<ApiHealthLog> builder)
    {
        builder
            .Property(x => x.Status)
            .HasConversion<string>();
    }
}