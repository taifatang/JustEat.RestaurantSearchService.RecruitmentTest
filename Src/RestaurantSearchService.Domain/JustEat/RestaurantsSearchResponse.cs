using System.Collections.Generic;

namespace RestaurantSearchService.Domain.JustEat
{
    public class RestaurantsSearchResponse
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}