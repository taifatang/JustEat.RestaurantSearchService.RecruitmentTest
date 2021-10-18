using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using RestaurantSearchService.Domain.JustEatService;
using RestaurantSearchService.Infrastructure.JustEatService;
using RestaurantSearchService.InMemoryTests.Stubs;
using RestaurantSearchService.TestHelper;

namespace RestaurantSearchService.InMemoryTests
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterStubs(this IServiceCollection services)
        {
            services.AddSingleton<IJustEatHttpClient>(provider => new JustEatClientStub(
                FakeHttpClientFactory.Create(new Uri("https://fakehttpclient.local")),
                new NullLogger<JustEatHttpClient>()));

            return services;
        }
    }
}
