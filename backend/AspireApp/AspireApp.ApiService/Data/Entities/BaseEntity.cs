using AspireApp.ApiService.Data.Interfaces;

namespace AspireApp.ApiService.Data.Entities;

public class BaseEntity : IHaveId, IHaveCreatedAtTimeStamp
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; init; }
}

