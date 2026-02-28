using AspireApp.ApiService.Application.ApiHealthLogs;
using AspireApp.ApiService.Application.ApiHealthLogs.Models;
using AspireApp.ApiService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HealthCheckController : ControllerBase
{
    private readonly IApiHealthLogService _healthLogService;

    public HealthCheckController(IApiHealthLogService service)
    {
        _healthLogService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealthStatus(CancellationToken ct)
    {
        ApiHealthLogDto healthLog = await _healthLogService.LogHealthAsync(ct);

        return Ok(healthLog);
    }
}