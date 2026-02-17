using AspireApp.ApiService.Data;

using AspireApp.ApiService.Application.ApiHealthLogs.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using AspireApp.ApiService.Application.Enums;
using AspireApp.ApiService.Data.Entities;

namespace AspireApp.ApiService.Application.ApiHealthLogs;

// TODO replace rough service pattern with mediator as endpoints grow
public class ApiHealthLogService : IApiHealthLogService
{
    private readonly ApplicationDbContext _db;

    public ApiHealthLogService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ApiHealthLogDto> GetMostRecentLogAsync(CancellationToken ct)
    {
        ApiHealthLog recent = await _db.ApiHealthLogs
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .FirstAsync(ct);

        return new ApiHealthLogDto()
        {
            Id = recent.Id,
            Timestamp = recent.CreatedAt,
            Notes = recent.Notes,
            Status = recent.Status.ToString()
        };
    }

    public async Task<ApiHealthLogDto> LogHealthAsync(CancellationToken ct)
    {
        ApiHealthLog log = new ()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Notes = null,
            Status = (await _db.Database.CanConnectAsync(ct)) ? ConnectionStatus.CanConnect : ConnectionStatus.CannotConnect
        };

        // add log
        _db.ApiHealthLogs.Add(log);
        await _db.SaveChangesAsync(ct);

        // todo create some constructors / mapping extension methods to avoid this manual mapping
        return new ApiHealthLogDto()
        { 
            Id = log.Id,
            Timestamp = log.CreatedAt,
            Notes = log.Notes,
            Status = log.Status.ToString()
        };
    }
}

