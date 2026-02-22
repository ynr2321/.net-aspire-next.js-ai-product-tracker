using AspireApp.ApiService.Application.ApiHealthLogs;
using AspireApp.ApiService.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add Entity Framework Core with PostgreSQL (db name must match one defined in AspireApp.AppHost/Program.cs)
builder.AddNpgsqlDbContext<ApplicationDbContext>("aspireapp");

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Repositories
builder.Services.AddScoped<IApiHealthLogService, ApiHealthLogService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(); // Enable CORS
app.UseAuthorization();
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();