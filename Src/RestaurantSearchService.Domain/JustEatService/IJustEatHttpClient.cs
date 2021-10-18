using System.Threading.Tasks;
using RestaurantSearchService.Domain.JustEatService.Contracts;

namespace RestaurantSearchService.Domain.JustEatService
{
    public interface IJustEatHttpClient
    {
        Task<RestaurantsSearchResponse> SearchRestaurantsAsync(string outCode);
    }
}
