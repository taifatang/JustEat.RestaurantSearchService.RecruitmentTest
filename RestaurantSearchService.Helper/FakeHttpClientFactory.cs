using System;

namespace RestaurantSearchService.Helper
{
    public static class FakeHttpClientFactory
    {
        public static FakeHttpClient Create(Uri baseUrl)
        {
            var fakeHttpClient =  new FakeHttpClient(new FakeHttpMessageHandler());
            
            fakeHttpClient.BaseAddress = baseUrl;

            return fakeHttpClient;
        }
    }
}