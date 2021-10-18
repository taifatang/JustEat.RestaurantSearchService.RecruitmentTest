using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantSearchService.Domain.JustEat;
using Restaurant = RestaurantSearchService.Domain.Models.Restaurant;

namespace RestaurantSearchService.Infrastructure.JustEat
{
    public class JustEatService
    {
        private readonly IJustEatHttpClient _justEatHttpClient;

        public JustEatService(IJustEatHttpClient justEatHttpClient)
        {
            _justEatHttpClient = justEatHttpClient;
        }

        public async Task<IEnumerable<Restaurant>> SearchRestaurants(string outCode)
        {
            var searchResponse = await _justEatHttpClient.SearchRestaurants(outCode);

            return searchResponse.Restaurants
                .Where(x=>x.IsOpenNow)
                .Select(x => new Restaurant()
                {
                    Name = x.Name,
                    Cuisines = x.Cuisines.Select(c => c.Name),
                    Rating = x.Rating.StarRating
                });
        }
    }
}
