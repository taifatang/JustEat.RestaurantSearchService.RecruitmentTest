using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NUnit.Framework;
using RestaurantSearchService.Domain.Exceptions;
using RestaurantSearchService.Domain.JustEat;
using RestaurantSearchService.Helper;
using RestaurantSearchService.Infrastructure.JustEat;

namespace RestaurantSearchService.UnitTests
{
    [TestFixture]
    public class JustEatHttpClientTests
    {
        private FakeHttpClient _fakeHttpClient;
        private JustEatHttpClient _justEatHttpClient;

        [SetUp]
        public void SetUp()
        {
            _fakeHttpClient = FakeHttpClientFactory.Create(new Uri("https://justeat.local"));

            _justEatHttpClient = new JustEatHttpClient(_fakeHttpClient, NullLogger<JustEatHttpClient>.Instance);
        }

        [Test]
        public async Task Search_restaurants()
        {
            var response = new RestaurantsSearchResponse()
            {
                Restaurants = new List<Restaurant>()
                {
                    new Restaurant("John", 5.0, true, new []{"Japan"})
                }
            };
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(response));
            });

            var result = await _justEatHttpClient.SearchRestaurants("nw9");

            result.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task Search_restaurants_using_correct_endpoint()
        {
            var expectedOutCode = "nw9";
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(new RestaurantsSearchResponse()
                {
                    Restaurants = new List<Restaurant>()
                    {
                        new Restaurant("John", 5.0, true, new []{"Japan"})
                    }
                }));
            });

             await _justEatHttpClient.SearchRestaurants(expectedOutCode);

             _fakeHttpClient.LastRequest.RequestUri.Should().Be($"https://justeat.local/restaurants/bypostcode/{expectedOutCode}");
        }

        [Test]
        public void Search_restaurants_unsuccessful()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.BadRequest;
            });

            Assert.ThrowsAsync<UnexpectedResponseException>(() => _justEatHttpClient.SearchRestaurants("nw9"));
        }

        [Test]
        public void Search_restaurants_returns_no_content()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
            });

            Assert.ThrowsAsync<UnexpectedResponseException>(() => _justEatHttpClient.SearchRestaurants("nw9"));
        }
    }
}