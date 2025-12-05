using Client;
using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

WebAssemblyHostConfiguration configuration = builder.Configuration;
string? apiUrl = configuration.GetSection("ClientSettings:ApiUrl").Value;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiUrl.ToTrimmedString());
});

// Typed HttpClient
builder.Services.AddScoped<IAdoptService, AdoptService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ZipService>();

await builder.Build().RunAsync();
