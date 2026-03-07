/* 
 * This file is the entry point of the application.
 * It sets up the distributed application using the Dapr framework, adding an API service and a Next.js frontend application. 
 * The API service is defined in the Projects.AspireApp_ApiService project, while the Next.js app is located in the ../../../frontend directory.
 * The frontend app is configured to run in development mode and is set up to use HTTP endpoints, with references to the API service for communication.
 */
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// JWT signing key — stored in AppHost user secrets under "Parameters:jwt-key"
var jwtKey = builder.AddParameter("jwt-key", secret: true);

// Stable Postgres password — stored in AppHost user secrets under "Parameters:postgres-password"
var postgresPassword = builder.AddParameter("postgres-password", secret: true);

// Add PostgreSQL database container resource with a database named "aspireapp" and configure a db client with pg admin
var postgres = builder.AddPostgres("postgres-server-container")
    .WithPassword(postgresPassword);

// Only need to config ports, db client and volume in development
if (builder.Environment.IsDevelopment())
{
    postgres
        .WithVolume("postgres-data", "/var/lib/postgresql/data")
        .WithHostPort(5432)
        .WithPgAdmin(pgBuilder =>
        {
            pgBuilder.WithHostPort(5050);
        });
}

var database = postgres.AddDatabase("aspireapp");

// add api service resource
var apiService = builder.AddProject<AspireApp_ApiService>("apiservice")
    .WithReference(database)
    .WithEnvironment("Jwt__Key", jwtKey)
    .WaitFor(postgres);

// Add Next.js app resource
builder.AddJavaScriptApp("frontend", "../../../frontend", "dev")
       .WithHttpEndpoint(env: "PORT")
       .WithExternalHttpEndpoints()
       .WithReference(apiService);

builder.Build().Run();

