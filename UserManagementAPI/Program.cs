using UserManagementAPI.Data;
using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------
// 0. Logging setup
// ------------------------------------------------------------------

// Remove default providers and use only Console so output is clean
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ------------------------------------------------------------------
// 1. Register services
// ------------------------------------------------------------------

// MVC controllers
builder.Services.AddControllers();

// In-memory user store - Singleton means one shared instance for the whole app lifetime
builder.Services.AddSingleton<UserStore>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "User Management API", Version = "v1" });

    // Tell Swagger about the X-API-KEY header so testers can fill it in
    c.AddSecurityDefinition("ApiKey", new()
    {
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name        = "X-API-KEY",
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Enter 'dev-secret' to access POST / PUT / DELETE endpoints"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "ApiKey" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ------------------------------------------------------------------
// 2. Middleware pipeline  (order matters!)
// ------------------------------------------------------------------

// Logs every request with method, path, status code, and duration
app.UseMiddleware<RequestLoggingMiddleware>();

// Blocks POST/PUT/DELETE unless X-API-KEY: dev-secret is present
app.UseMiddleware<AuthMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
