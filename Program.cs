using Microsoft.EntityFrameworkCore;
using HomePageBackend.Models;
using HomePageBackend.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Logging (important for Render)
// --------------------------------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// --------------------------------------------------
// Connection string (Render + Local)
// --------------------------------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// --------------------------------------------------
// DbContext
// --------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --------------------------------------------------
// Services
// --------------------------------------------------
builder.Services.AddScoped<ICartService, CartService>();

// --------------------------------------------------
// CORS (Allow all)
// --------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// --------------------------------------------------
// Controllers + Swagger
// --------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --------------------------------------------------
// IMPORTANT: DO NOT manually add app.Urls on Render
// Let ASPNETCORE_URLS handle it
// --------------------------------------------------

// --------------------------------------------------
// Middleware ORDER (CRITICAL)
// --------------------------------------------------
app.UseRouting();

app.UseCors("AllowAll");

// Swagger MUST be after routing
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FX Backend API");
});

// ❌ DO NOT USE HTTPS REDIRECTION ON RENDER
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
