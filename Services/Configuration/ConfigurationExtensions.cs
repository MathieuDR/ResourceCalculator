using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Repositories;

namespace Services.Configuration {
    public static class ConfigurationExtensions {
        // extension method to add DI services
        public static IServiceCollection AddResourceCalculatorServices(this IServiceCollection services,
            IConfiguration configuration) {
            services.AddRepositoriesServices();
            services.AddServices();

            return services;
        }

        //private method to add repositories services
        private static IServiceCollection AddRepositoriesServices(this IServiceCollection services) {
            services.AddBlazoredLocalStorage();
            services.AddBlazoredSessionStorage();
            services.AddScoped<IMachineRepository, MachineRepository>();

            return services;
        }

        //private method to add services to DI
        private static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddScoped<IMachineService, MachineService>();

            return services;
        }
    }
}