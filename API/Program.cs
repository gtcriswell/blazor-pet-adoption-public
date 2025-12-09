using API.Business;
using Data.Models;
using DTO.API;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console() // Optional: for console output alongside SQL Server
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs", // The table where logs will be stored
            AutoCreateSqlTable = true // Serilog will create the table if it doesn't exist
        },
        restrictedToMinimumLevel: LogEventLevel.Information // Log events from Information level and above
    )
    .CreateLogger();

builder.Host.UseSerilog();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AdoptContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddHttpClient(); // Registers IHttpClientFactory

builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<ApiSettings>();
builder.Services.AddScoped<IAdoptBusiness, AdoptBusiness>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowWasm", policy =>
    {
        _ = policy
            .WithOrigins("https://localhost:7047", "https://netdevnow.com") // your Blazor WASM URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();


//if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("allowWasm");

app.MapControllers();

app.Run();
