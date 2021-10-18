using System.Collections.Generic;

namespace RestaurantSearchService.Domain.JustEatService.Contracts
{
    public class RestaurantsSearchResponse
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}