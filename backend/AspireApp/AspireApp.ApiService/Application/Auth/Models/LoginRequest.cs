using System.ComponentModel.DataAnnotations;

namespace AspireApp.ApiService.Application.Auth.Models;

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password
);
