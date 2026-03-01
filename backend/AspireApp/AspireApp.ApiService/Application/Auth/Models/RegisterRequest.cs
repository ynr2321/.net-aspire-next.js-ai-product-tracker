using System.ComponentModel.DataAnnotations;

namespace AspireApp.ApiService.Application.Auth.Models;

public record RegisterRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password
);
