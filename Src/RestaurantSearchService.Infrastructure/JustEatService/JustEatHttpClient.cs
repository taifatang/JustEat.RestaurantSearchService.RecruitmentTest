using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestaurantSearchService.Domain.Exceptions;
using RestaurantSearchService.Domain.JustEatService;
using RestaurantSearchService.Domain.JustEatService.Contracts;

namespace RestaurantSearchService.Infrastructure.JustEatService
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

        public async Task<RestaurantsSearchResponse> SearchRestaurantsAsync(string outCode)
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

            _logger.LogWarning("Received successful response from Just Eat {statusCode} but unexpected empty content", response.StatusCode);

            throw new UnexpectedResponseException(nameof(JustEatHttpClient));
        }
    }
}
