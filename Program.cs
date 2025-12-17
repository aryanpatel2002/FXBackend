using Microsoft.EntityFrameworkCore;
using HomePageBackend.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
// Logging (helps on Render + local)
// -----------------------------------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// -----------------------------------------------------
// Read DB connection string
// - Local: appsettings.json
// - Render: Environment Variable
// -----------------------------------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// -----------------------------------------------------
// DbContext (SQL Server – DB First)
// -----------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// -----------------------------------------------------
// CORS (ALLOW ALL – works everywhere)
// -----------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// -----------------------------------------------------
// Controllers + JSON
// -----------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -----------------------------------------------------
// Render PORT binding (safe for local too)
// -----------------------------------------------------
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

// -----------------------------------------------------
// Middleware ORDER matters
// -----------------------------------------------------
app.UseRouting();

app.UseCors("AllowAll");

// -----------------------------------------------------
// Swagger (enabled everywhere)
// -----------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomePageBackend API");
    c.RoutePrefix = "swagger"; // /swagger
});

// -----------------------------------------------------
// HTTPS
// - Enabled locally
// - Disabled on Render (Render handles TLS)
// -----------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// -----------------------------------------------------
// Run
// -----------------------------------------------------
try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Application failed to start");
    Console.WriteLine(ex);
    throw;
}
