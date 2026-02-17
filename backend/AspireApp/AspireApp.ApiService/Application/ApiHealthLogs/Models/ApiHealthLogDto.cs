using AspireApp.ApiService.Application.Enums;

namespace AspireApp.ApiService.Application.ApiHealthLogs.Models
{
    // TODO setup reinforced typings to auto geenrate itnerfaces for these dtos in the next.js project
    public class ApiHealthLogDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
    }

    
}
