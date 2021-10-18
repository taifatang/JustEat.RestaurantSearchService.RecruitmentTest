using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestaurantSearchService.Domain.JustEat;
using RestaurantSearchService.Infrastructure.JustEat;

namespace RestaurantSearchService.UnitTests
{
    [TestFixture]
    public class JustEatServiceTests
    {
        private Mock<IJustEatHttpClient> _justEatHttpClientMock;
        private JustEatService _justEatService;
        [SetUp]
        public void SetUp()
        {
            _justEatHttpClientMock = new Mock<IJustEatHttpClient>();
            _justEatService = new JustEatService(_justEatHttpClientMock.Object);

            _justEatHttpClientMock.Setup(x => x.SearchRestaurants("nw9")).ReturnsAsync(new RestaurantsSearchResponse());
        }

        [Test]
        public async Task Search_restaurants()
        {
            _justEatHttpClientMock.Setup(x => x.SearchRestaurants("nw9")).ReturnsAsync(new RestaurantsSearchResponse()
            {
                Restaurants = new List<Restaurant>()
                {
                    new Restaurant("Morrisons Takeaway", 5.00, true, new []{"Japan"}),
                    new Restaurant("Asda Takeaway", 4.50, true, new []{"Turkey", "Indian"})
                }
            });

            var result = await _justEatService.SearchRestaurants("nw9");

            _justEatHttpClientMock.Verify(x => x.SearchRestaurants("nw9"), Times.Once);

            result.Should().HaveCount(2)
                .And.BeEquivalentTo(new[]
                {
                    new { Name = "Morrisons Takeaway", Rating = 5.00, Cuisines = new [] { "Japan" } },
                    new { Name = "Asda Takeaway", Rating = 4.50, Cuisines = new [] { "Turkey", "Indian" } }
                });
        }

        [Test]
        public async Task Search_restaurants_returns_open_restaurants_only()
        {
            _justEatHttpClientMock.Setup(x => x.SearchRestaurants("nw9")).ReturnsAsync(new RestaurantsSearchResponse()
            {
                Restaurants = new List<Restaurant>()
                {
                    new Restaurant("Morrisons Takeaway", 5.00, true, new []{"Japan"}),
                    new Restaurant("Asda Takeaway", 4.50, true, new []{"Turkey"}),
                    new Restaurant("Tesco Takeaway", 10.00, false, new []{"Italian" }),
                    new Restaurant("Sainsbury Takeaway",8.00, false, new []{ "Indian"})
                }
            });

            var result = await _justEatService.SearchRestaurants("nw9");

            result.Should().HaveCount(2)
                .And.BeEquivalentTo(new[]
                {
                    new { Name = "Morrisons Takeaway", Rating = 5.00, Cuisines = new [] { "Japan" } },
                    new { Name = "Asda Takeaway", Rating = 4.50, Cuisines = new [] { "Turkey" } }
                });
        }
    }
}
