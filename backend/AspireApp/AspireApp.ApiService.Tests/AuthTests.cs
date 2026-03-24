using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AspireApp.ApiService.Tests.Infrastructure;

namespace AspireApp.ApiService.Tests;

[Collection(AuthTestCollection.Name)]
public class AuthTests
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidNewUser_ReturnsSuccessAndMessage()
    {
        var payload = new { Email = "register-valid@test.com", Password = "Test1234!" };

        var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("User registered successfully.", body.GetProperty("message").GetString());
    }

    [Fact]
    public async Task Register_WithExistingEmail_ReturnsConflict()
    {
        const string email = "register-duplicate@test.com";
        await _factory.SeedUserAsync(email, "Test1234!", "User");

        var payload = new { Email = email, Password = "Test1234!" };

        var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("A user with this email already exists.", body.GetProperty("message").GetString());
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsSuccessWithToken()
    {
        const string email = "login-valid@test.com";
        const string password = "Test1234!";
        await _factory.SeedUserAsync(email, password, "User");

        var payload = new { Email = email, Password = password };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("token").GetString()));
        Assert.Equal(email, body.GetProperty("email").GetString());
        Assert.Contains("User", body.GetProperty("roles").EnumerateArray().Select(r => r.GetString()));
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var payload = new { Email = "nonexistent@test.com", Password = "WrongPass1!" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Invalid email or password.", body.GetProperty("message").GetString());
    }
}
