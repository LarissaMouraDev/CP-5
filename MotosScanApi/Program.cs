using MotosScan.Api.Configuration;
using MotosScan.Application.Services;
using MotosScan.Domain.Interfaces.Repositories;
using MotosScan.Infrastructure.Data;
using MotosScan.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MongoDB
builder.Services.Configure(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton();

// Repositórios
builder.Services.AddScoped();
builder.Services.AddScoped();
builder.Services.AddScoped();

// Services
builder.Services.AddScoped();
builder.Services.AddScoped();
builder.Services.AddScoped();

// Controllers
builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioningConfiguration();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: builder.Configuration["MongoDbSettings:ConnectionString"]!,
        name: "mongodb",
        tags: new[] { "database", "mongodb" }
    )
    .AddCheck("api", () =>
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API funcionando"));

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health Check
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();