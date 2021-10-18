using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestaurantSearchService.Domain.Exceptions;
using RestaurantSearchService.Domain.JustEat;
using Microsoft.Extensions.Logging;

namespace RestaurantSearchService.Infrastructure.JustEat
{
    public class JustEatHttpClient: IJustEatHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JustEatHttpClient> _logger;

        public JustEatHttpClient(HttpClient httpClient, ILogger<JustEatHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RestaurantsSearchResponse> SearchRestaurants(string outCode)
        {
            var response = await _httpClient.GetAsync($"restaurants/bypostcode/{outCode}");
            var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Received unexpected response from Just Eat {statusCode} {content}", response.StatusCode, content);

                throw new UnexpectedResponseException(nameof(JustEatHttpClient));
            }

            if (content != null)
            {
                return JsonConvert.DeserializeObject<RestaurantsSearchResponse>(content);
            }

            throw new UnexpectedResponseException(nameof(JustEatHttpClient));
        }
    }
}
