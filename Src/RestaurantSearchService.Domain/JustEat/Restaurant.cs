using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RestaurantSearchService.Domain.JustEat
{
    public class Restaurant
    {
        public string Name { get; set; }
        public Rating Rating { get; set; }
        public bool IsOpenNow { get; set; }
        public IEnumerable<Cuisine> Cuisines { get; set; }

        [JsonConstructor]
        public Restaurant()
        {
            
        }

        public Restaurant(string name, double rating, bool isOpenNow,IEnumerable<string> cuisines)
        {
            Name = name;
            Rating = new Rating { StarRating = rating };
            IsOpenNow = isOpenNow;
            Cuisines = cuisines.Select(x => new Cuisine() { Name = x });
        }
    }
}