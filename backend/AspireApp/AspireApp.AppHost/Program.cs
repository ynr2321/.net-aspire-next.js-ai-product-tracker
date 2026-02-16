/* 
 * This file is the entry point of the application.
 * It sets up the distributed application using the Dapr framework, adding an API service and a Next.js frontend application. 
 * The API service is defined in the Projects.AspireApp_ApiService project, while the Next.js app is located in the ../../../frontend directory.
 * The frontend app is configured to run in development mode and is set up to use HTTP endpoints, with references to the API service for communication.
 */
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database container resource with a database named "aspireapp" and configure a db client with pg admin
var postgres = builder.AddPostgres("postgres-server-container")
    .WithPgAdmin(pgBuilder =>
    {
        pgBuilder.WithHostPort(5050); // TODO Yusef move to appsettings
    });
    // TODO Yusef add a volume for this contianer

var database = postgres.AddDatabase("aspireapp");

// add api service resource
var apiService = builder.AddProject<AspireApp_ApiService>("apiservice")
    .WithReference(database);

// Add Next.js app resource
builder.AddJavaScriptApp("frontend", "../../../frontend", "dev")
       .WithHttpEndpoint(env: "PORT")
       .WithExternalHttpEndpoints()
       .WithReference(apiService);

builder.Build().Run();

