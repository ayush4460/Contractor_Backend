using Contractor_Backend.API.HealthChecks;
using Contractor_Backend.API.Middleware;
using Contractor_Backend.Persistence.DbContext;
using Contractor_Backend.Persistence.UnitOfWork;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────
// 🔐 0. Load Environment Variables from .env
// ─────────────────────────────────────────────────────────────

Env.Load();

var env = builder.Environment;

// ─────────────────────────────────────────────────────────────
// 🔧 1. Configure Services (Dependency Injection)
// ─────────────────────────────────────────────────────────────

builder.Services.AddControllers(); // No need to attach FluentValidation here

builder.Services
    .AddFluentValidationAutoValidation() // Enables automatic validation on model binding
    .AddFluentValidationClientsideAdapters(); // Optional: enables client-side support if needed

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // Registers all validators


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAppHealthChecks();
builder.Services.AddEndpointsApiExplorer();

if (env.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Contractor Backend API",
            Version = "v1",
            Description = "API for managing contractor workflows"
        });
    });
}

// ─────────────────────────────────────────────────────────────
// 🌐 2. Configure CORS Policy
// ─────────────────────────────────────────────────────────────

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(
            Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGIN") ?? "http://localhost:3000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ─────────────────────────────────────────────────────────────
// 🛢️ 3. Configure Database Connection (MySQL)
// ─────────────────────────────────────────────────────────────

var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ─────────────────────────────────────────────────────────────
// 🚀 4. Build App
// ─────────────────────────────────────────────────────────────

var app = builder.Build();

// ─────────────────────────────────────────────────────────────
// 🧱 5. Configure Middleware Pipeline
// ─────────────────────────────────────────────────────────────

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contractor Backend API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); // ✅ Global exception handler

app.UseHttpsRedirection();
app.UseCors("DefaultCorsPolicy");
app.UseAuthorization();

app.MapControllers();

// ─────────────────────────────────────────────────────────────
// ❤️ 6. Health Check Endpoint
// ─────────────────────────────────────────────────────────────

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// ─────────────────────────────────────────────────────────────
// 🏁 7. Run the App
// ─────────────────────────────────────────────────────────────

app.Run();
