using System.Net.Http.Json;
using ZaposliMe.Service.Models;

namespace ZaposliMe.Service.Services.Weather
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Backend");
        }

        public async Task<List<WeatherForecast>> GetForecastsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("/WeatherForecast");
        }
    }
}
