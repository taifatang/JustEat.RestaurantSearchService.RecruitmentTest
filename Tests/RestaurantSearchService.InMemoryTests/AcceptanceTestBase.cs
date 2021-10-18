using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RestaurantSearchService.Domain.JustEatService;
using RestaurantSearchService.InMemoryTests.Stubs;

namespace RestaurantSearchService.InMemoryTests
{
    public abstract class AcceptanceTestBase
    {
        protected HttpClient ApiClient;
        protected JustEatClientStub JustEatClientStub;

        private CustomWebApplicationFactory _factory;
        
        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
            ApiClient = _factory.CreateClient();
            JustEatClientStub = _factory.Services.GetRequiredService<IJustEatHttpClient>() as JustEatClientStub;
        }

        [TearDown]
        public void TearDown()
        {
            ApiClient?.Dispose();
            _factory?.Dispose();
        }
    }
}