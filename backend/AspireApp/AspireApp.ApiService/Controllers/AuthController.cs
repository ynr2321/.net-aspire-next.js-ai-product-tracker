using AspireApp.ApiService.Application.Auth;
using AspireApp.ApiService.Application.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        AuthResult result = await _authService.RegisterAsync(request.Email, request.Password);

        return result.Status switch
        {
            AuthResultStatus.Success => Ok(new { result.Message }),
            AuthResultStatus.Conflict => Conflict(new { result.Message }),
            AuthResultStatus.ValidationError => BadRequest(new { result.Errors }),
            _ => StatusCode(500)
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        return result.Status switch
        {
            AuthResultStatus.Success => Ok(result.Data),
            AuthResultStatus.Unauthorized => Unauthorized(new { result.Message }),
            _ => StatusCode(500)
        };
    }
}
