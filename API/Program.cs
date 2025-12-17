using API.Business;
using API.Middleware;
using Data.Models;
using DTO.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

string defaultConnection = "DefaultConnection";
string apiSettings = "ApiSettings";
string corsPolicyName = "allowWasm";

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString(defaultConnection),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        restrictedToMinimumLevel: LogEventLevel.Warning
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AdoptContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(defaultConnection))
);

builder.Services.AddHttpClient(); // Registers IHttpClientFactory

// Swagger / OpenAPI configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Adopt API",
        Version = "v1",
        Description = "API for pet adoption — exposes endpoints used by the Blazor WASM client."
    });

    // Include XML comments (enable GenerateDocumentationFile in the API project file)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Optional: Bearer token support for testing secured endpoints in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(apiSettings));
builder.Services.AddScoped<ApiSettings>();
builder.Services.AddScoped<IAdoptBusiness, AdoptBusiness>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        _ = policy
            //.WithOrigins("https://localhost:7047", "https://netdevnow.com")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

// Expose Swagger/OpenAPI JSON and UI. In production you can gate this behind config or only enable for non-prod.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Adopt API v1");
    c.RoutePrefix = "swagger"; // Browse to /swagger
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(corsPolicyName);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
