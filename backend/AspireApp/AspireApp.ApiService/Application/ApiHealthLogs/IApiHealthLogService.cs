using AspireApp.ApiService.Application.ApiHealthLogs.Models;

namespace AspireApp.ApiService.Application.ApiHealthLogs;

public interface IApiHealthLogService
{
    /// <summary>
    /// Checks the health of the API and then logs result to the db
    /// </summary>
    Task<ApiHealthLogDto> LogHealthAsync(CancellationToken ct);

    Task<ApiHealthLogDto> GetMostRecentLogAsync(CancellationToken ct);

}


