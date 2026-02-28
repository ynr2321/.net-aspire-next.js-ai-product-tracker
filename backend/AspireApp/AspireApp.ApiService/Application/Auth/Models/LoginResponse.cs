namespace AspireApp.ApiService.Application.Auth.Models;

public record LoginResponse
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public List<string> Roles { get; init; } = [];
}
