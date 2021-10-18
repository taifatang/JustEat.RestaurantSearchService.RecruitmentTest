using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using RestaurantSearchService.Domain.JustEatService.Contracts;
using TestStack.BDDfy;

namespace RestaurantSearchService.InMemoryTests.Fixtures
{
    [Story(AsA = "Customer",
        IWant = "Submit an outcode",
        SoThat = "I can see a restaurants that's currently opened")]
    public class SearchRestaurantsFixture : AcceptanceTestBase
    {
        private HttpResponseMessage _response;
        private IEnumerable<Restaurant> _restaurants = new List<Restaurant>();

        [Test]
        public void Search_restaurants()
        {
            this.Given(_ => _.RestaurantsAreOpenLocally())
                .When(_ => _.ISearchByOutCode("nw9"))
                .Then(_ => StatusCodeShouldBe(HttpStatusCode.OK))
                .Then(_ => RestaurantsAreReturned())
                .BDDfy();
        }

        [Test]
        public void Search_restaurants_returns_not_found_if_no_restaurants_are_found()
        {
            this.Given(_ => _.NoRestaurantsAreFoundLocally())
                .When(_ => _.ISearchByOutCode("nw9"))
                .Then(_ => StatusCodeShouldBe(HttpStatusCode.NotFound))
                .Then(_ => NoRestaurantsAreReturned())
                .BDDfy();
        }

        [Test]
        public void Search_restaurants_returns_not_found_if_no_restaurants_are_open()
        {
            this.Given(_ => _.RestaurantsAreClosedLocally())
                .When(_ => _.ISearchByOutCode("nw9"))
                .Then(_ => StatusCodeShouldBe(HttpStatusCode.NotFound))
                .Then(_ => NoRestaurantsAreReturned())
                .BDDfy();
        }

        [TestCase(null)]
        [TestCase("")]
        public void Search_restaurants_returns_bad_request_if_outCode_is_not_provided(string outCode)
        {
            this.Given(_ => _.RestaurantsAreOpenLocally())
                .When(_ => _.ISearchByOutCode(outCode))
                .Then(_ => StatusCodeShouldBe(HttpStatusCode.BadRequest))
                .BDDfy();
        }

        public void NoRestaurantsAreFoundLocally()
        {
            _restaurants = Enumerable.Empty<Restaurant>();

            JustEatClientStub.QueueNextResponse(new RestaurantsSearchResponse() { Restaurants = _restaurants });
        }

        public void RestaurantsAreClosedLocally()
        {
            _restaurants = new List<Restaurant>()
            {
                new Restaurant("John", 5.0, false, new[] {"Japan"}),
                new Restaurant("Jane", 4.5, false, new[] {"Chinese", "Thai"})
            };

            JustEatClientStub.QueueNextResponse(new RestaurantsSearchResponse() { Restaurants = _restaurants });
        }

        public void RestaurantsAreOpenLocally()
        {
            _restaurants = new List<Restaurant>()
            {
                new Restaurant("John", 5.0, true, new[] {"Japan"}),
                new Restaurant("Jane", 4.5, true, new[] {"Chinese", "Thai"}),
                new Restaurant("Janet", 3.5, false, new[] {"Chinese", "Thai"}),
                new Restaurant("Josh", 2.5, false, new[] {"Chinese", "Thai"})
            };

            JustEatClientStub.QueueNextResponse(new RestaurantsSearchResponse() { Restaurants = _restaurants });
        }

        public async Task ISearchByOutCode(string outCode)
        {
            _response = await ApiClient.GetAsync($"Search?outcode={outCode}");
        }

        public void StatusCodeShouldBe(HttpStatusCode statusCode)
        {
            _response.StatusCode.Should().Be(statusCode);
        }

        public async Task RestaurantsAreReturned()
        {
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Hosts.Contracts.RestaurantsSearchResponse>(content);

            result.Restaurants.Should().NotBeEmpty();

            var expectedOpenedRestaurants = _restaurants
                .Where(x=>x.IsOpenNow)
                .Select(x => x.Name);

            result.Restaurants.Select(x=>x.Name).Should().BeEquivalentTo(expectedOpenedRestaurants);
        }

        public async Task NoRestaurantsAreReturned()
        {
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Hosts.Contracts.RestaurantsSearchResponse>(content);

            result.Restaurants.Should().BeNull();
        }
    }
}
