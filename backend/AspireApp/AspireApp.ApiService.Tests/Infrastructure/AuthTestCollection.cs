namespace AspireApp.ApiService.Tests.Infrastructure;

/// <summary>
/// Groups all auth integration tests so they share a single
/// <see cref="ApiWebApplicationFactory"/> (and its backing PostgreSQL container)
/// and run sequentially.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public class AuthTestCollection : ICollectionFixture<ApiWebApplicationFactory>
{
    public const string Name = "Auth";
}
