using AspireApp.ApiService.Data.Entities;

namespace AspireApp.ApiService.Application.Auth;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
