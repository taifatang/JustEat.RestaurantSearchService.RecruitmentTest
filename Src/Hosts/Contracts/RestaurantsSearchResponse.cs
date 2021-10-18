using System.Collections.Generic;
using RestaurantSearchService.Domain.Models;

namespace Hosts.Contracts
{
    public class RestaurantsSearchResponse
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}