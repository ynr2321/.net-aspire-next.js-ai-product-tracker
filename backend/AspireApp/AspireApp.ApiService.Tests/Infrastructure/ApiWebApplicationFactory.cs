using AspireApp.ApiService.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AspireApp.ApiService.Tests.Infrastructure;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string TestJwtKey =
        "IntegrationTest_SuperSecretSigningKey_Long_Enough_For_HMAC_SHA256!!";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .Build();

    /// <summary>
    /// Seed a user directly via <see cref="UserManager{TUser}"/> with real password hashing.
    /// Call only after the host has been built (e.g. after <see cref="CreateClient"/>).
    /// </summary>
    public async Task SeedUserAsync(string email, string password, params string[] roles)
    {
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to seed user '{email}': {errors}");
        }

        foreach (var role in roles)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }

    // -- IAsyncLifetime -------------------------------------------------

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Environment variables are read by WebApplication.CreateBuilder() immediately,
        // making them available before Aspire components validate connection strings
        // and before any top-level Program.cs code reads configuration.
        // This avoids timing issues with ConfigureAppConfiguration + minimal hosting.
        Environment.SetEnvironmentVariable("ConnectionStrings__aspireapp", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("Jwt__Key", TestJwtKey);
        Environment.SetEnvironmentVariable("Jwt__Issuer", "integration-test-issuer");
        Environment.SetEnvironmentVariable("Jwt__Audience", "integration-test-audience");
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__aspireapp", null);
        Environment.SetEnvironmentVariable("Jwt__Key", null);
        Environment.SetEnvironmentVariable("Jwt__Issuer", null);
        Environment.SetEnvironmentVariable("Jwt__Audience", null);

        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
