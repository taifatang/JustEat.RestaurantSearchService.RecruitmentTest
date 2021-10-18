using System.Collections.Generic;

namespace RestaurantSearchService.Domain.Models
{
    public class Restaurant
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public IEnumerable<string> Cuisines { get; set; }
    }
}
