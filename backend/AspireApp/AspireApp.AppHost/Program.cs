var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireApp_ApiService>("apiservice");

// Add Next.js app
builder.AddNpmApp("frontend", "../../../frontend", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();

