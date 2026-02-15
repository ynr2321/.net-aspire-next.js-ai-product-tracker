using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    public HealthCheckController()
    {
    }

    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        return Ok(new { status = "running" });
    }
}