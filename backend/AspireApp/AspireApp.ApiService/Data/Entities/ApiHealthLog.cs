using AspireApp.ApiService.Application.Enums;

namespace AspireApp.ApiService.Data.Entities;

public class ApiHealthLog : BaseEntity
{
    public ConnectionStatus Status { get; set; }
    public string? Notes { get; set; }
}
