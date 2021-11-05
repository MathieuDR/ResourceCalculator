using AntDesign.ProLayout;
using Web.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Web.Services.Repositories;

namespace Web {
    public class Program {
        public static async Task Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.BrowserConsole()
                .CreateLogger();

            try {
                var builder = WebAssemblyHostBuilder.CreateDefault(args);
                builder.RootComponents.Add<App>("#app");

                builder.Services.AddScoped(sp => new HttpClient
                    { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

                builder.Services.AddScoped<IMachineRepository, MachineRepository>();
                builder.Services.AddScoped<IMachineService, MachineService>();
                builder.Services.AddAntDesign();
                builder.Services.AddBlazoredLocalStorage();
                builder.Services.Configure<ProSettings>(
                    builder.Configuration.GetSection("ProSettings")); // From wwwroot/appsettings
                builder.Logging.AddSerilog();

                await builder.Build().RunAsync();
            }
            catch (Exception ex) {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }
    }
}