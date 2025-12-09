using Client;
using Client.Services;
using DTO.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared;

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
builder.Services.AddScoped<ZipService>();

await builder.Build().RunAsync();

