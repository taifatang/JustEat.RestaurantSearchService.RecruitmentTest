using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestaurantSearchService.Domain.JustEat;
using RestaurantSearchService.Helper;
using RestaurantSearchService.Infrastructure.JustEat;

namespace RestaurantSearchService.InMemoryTests.Stubs
{
    public class JustEatClientStub: JustEatHttpClient
    {
        private readonly FakeHttpClient _fakeHttpClient;
        public JustEatClientStub(FakeHttpClient httpClient, ILogger<JustEatHttpClient> logger) : base(httpClient, logger)
        {
            _fakeHttpClient = httpClient;
        }
        public void QueueNextResponse(RestaurantsSearchResponse response)
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(response));
            });
        }
    }
}
