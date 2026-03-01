using AspireApp.ApiService.Application.Auth.Models;

namespace AspireApp.ApiService.Application.Auth;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string password);
    Task<AuthResult<LoginResponse>> LoginAsync(string email, string password);
}
