using System.Threading.Tasks;

namespace RestaurantSearchService.Domain.JustEat
{
    public interface IJustEatHttpClient
    {
        Task<RestaurantsSearchResponse> SearchRestaurants(string outCode);
    }
}
