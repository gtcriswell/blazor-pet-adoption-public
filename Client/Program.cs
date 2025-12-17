using Client;
using Client.Middleware;
using Client.Services;
using Client.Validation;
using DTO.Client;
using DTO.User;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared;
using System.ComponentModel.DataAnnotations;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

WebAssemblyHostConfiguration configuration = builder.Configuration;
ClientSettings clientSettingsSection = configuration
    .GetRequiredSection("ClientSettings")
    .Get<ClientSettings>()!;

clientSettingsSection ??= new();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(clientSettingsSection.ApiUrl.ToTrimmedString());
});


builder.Services.AddSingleton(clientSettingsSection);

builder.Services.AddScoped<IAdoptService, AdoptService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ZipService>();

builder.Services.AddTransient<IValidator<VisitorDto>, VisitorDtoValidator>();

var serviceProvider = builder.Services.BuildServiceProvider();
var logService = serviceProvider.GetService<ILogService>();

if (logService != null)
{
    var unhandledExceptionSender = new UnhandledExceptionSender();
    var unhandledExceptionProvider = new UnhandledExceptionProvider(logService);
    builder.Logging.AddProvider(unhandledExceptionProvider);
    builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);
}

await builder.Build().RunAsync();

