using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Icons.Material;
using Blazorise.Material;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Services.Configuration;

namespace BlazorApp {
    public class Program {
        public static async Task Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.BrowserConsole()
                .CreateLogger();

            try {
                var builder = WebAssemblyHostBuilder.CreateDefault(args);

                builder.Services
                    .AddBlazorise(options => { options.ChangeTextOnKeyPress = true; })
                    .AddMaterialProviders()
                    .AddMaterialIcons();

                builder.Services.AddScoped(sp => new HttpClient
                    { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
                builder.Services.AddResourceCalculatorServices(builder.Configuration);

                builder.Services.AddScoped<StupidService>();
                builder.Services.AddScoped<Repository>();

                builder.Logging.ClearProviders();
                builder.Logging.AddSerilog();


                builder.RootComponents.Add<App>("#app");
                await builder.Build().RunAsync();
            }
            catch (Exception ex) {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }
    }
}