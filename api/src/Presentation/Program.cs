using FastEndpoints;
using FastEndpoints.Swagger;
using ToDoApp.Application;
using ToDoApp.Infrastructure;
using ToDoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.AddInfrastructureServices();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://127.0.0.1:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure pipeline
app.InitialiseDatabase();

// CORS must be before other middleware
app.UseCors("AllowFrontend");

app.UseFastEndpoints(c => 
{
    c.Endpoints.ShortNames = true;
    c.Serializer.Options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

app.UseOpenApi();
app.UseSwaggerUi();

app.Run();
