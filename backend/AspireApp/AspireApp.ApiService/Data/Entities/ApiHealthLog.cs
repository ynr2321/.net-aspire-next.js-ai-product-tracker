using AspireApp.ApiService.Application.Enums;

namespace AspireApp.ApiService.Data.Entities;

public class ApiHealthLog
{
    public int Id { get; set; }
    public ConnectionStatus Status { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; } // optional field for extra info
}
