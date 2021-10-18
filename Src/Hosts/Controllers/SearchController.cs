using System.Linq;
using System.Threading.Tasks;
using Hosts.Contracts;
using Microsoft.AspNetCore.Mvc;
using RestaurantSearchService.Infrastructure.JustEat;

namespace Hosts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly JustEatService _justEatService;

        public SearchController(JustEatService justEatService)
        {
            _justEatService = justEatService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] RestaurantsSearchRequest request)
        {
            var restaurants = (await _justEatService.SearchRestaurants(request.OutCode)).ToList();

            if (!restaurants.Any())
            {
                return NotFound();
            }

            return Ok(new RestaurantsSearchResponse()
            {
                Restaurants = restaurants
            });
        }
    }
}
