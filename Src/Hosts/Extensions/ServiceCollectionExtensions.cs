using Hosts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestaurantSearchService.Domain.JustEatService;
using RestaurantSearchService.Infrastructure.JustEatService;

namespace Hosts.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JustEatServiceSettings>(configuration.GetSection(JustEatServiceSettings.Position));

            return services;
        }

        public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IJustEatHttpClient, JustEatHttpClient>((provider, client) =>
            {
                client.BaseAddress = provider.GetRequiredService<IOptions<JustEatServiceSettings>>().Value.BaseUrl;
            });

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<JustEatService>();

            return services;
        }
    }
}
