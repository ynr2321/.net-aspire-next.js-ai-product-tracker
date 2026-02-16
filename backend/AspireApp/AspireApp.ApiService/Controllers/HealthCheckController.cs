using AspireApp.ApiService.Data;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public HealthCheckController(ApplicationDbContext db)
    {
        _db = db; // TODO Yusef - later considering implementing repository pattern for better separation of concerns
    }

    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        return Ok(new { status = "running" });
    }
}