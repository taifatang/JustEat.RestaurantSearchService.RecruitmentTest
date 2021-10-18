using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Hosts.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using RestaurantSearchService.Infrastructure.JustEatService;

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
        [Produces("application/json")]
        public async Task<IActionResult> Search([FromQuery] RestaurantsSearchRequest request)
        {
            var restaurants = (await _justEatService.SearchRestaurantsAsync(request.OutCode)).ToList();

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
