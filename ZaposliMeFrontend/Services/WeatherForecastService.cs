using System.Net.Http.Json;
using ZaposliMe.Frontend.Models;

namespace ZaposliMe.Frontend.Services
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WeatherForecast>> GetForecastsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("/WeatherForecast");
        }
    }
}
