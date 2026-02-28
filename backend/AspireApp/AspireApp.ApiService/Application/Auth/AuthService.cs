using AspireApp.ApiService.Application.Auth.Models;
using AspireApp.ApiService.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspireApp.ApiService.Application.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        ApplicationUser? existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return AuthResult.Conflict("A user with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        IdentityResult result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return AuthResult.ValidationError(result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, "User");

        return AuthResult.Success("User registered successfully.");
    }

    public async Task<AuthResult<LoginResponse>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            return AuthResult<LoginResponse>.Unauthorized("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        return AuthResult<LoginResponse>.Success(new LoginResponse
        {
            Token = token,
            Email = user.Email!,
            Roles = roles.ToList()
        });
    }
}
