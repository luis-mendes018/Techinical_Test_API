using System.Text.Json;

using Technical_Test.Domain.Clients;
using Technical_Test.Domain.Entities;
using Technical_Test.Infrastructure.Helper_Classes;

namespace Technical_Test.Infrastructure.External;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Weather> GetWeatherAsync(string city, string apiKey)
    {
        var encodedCity = Uri.EscapeDataString(city);
        var apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={encodedCity}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<OpenWeatherMapResponse>(jsonString);

        return new Weather
        {
            Lon = apiResponse?.Coord?.Lon,
            Lat = apiResponse?.Coord?.Lat,
            TempMin = apiResponse?.Main?.TempMin,
            TempMax = apiResponse?.Main?.TempMax,
            Visibility = apiResponse?.Visibility,
            Sunrise = apiResponse?.Sys?.Sunrise,
            Sunset = apiResponse?.Sys?.Sunset,
            Description = apiResponse?.Weather?.FirstOrDefault()?.Description,
            Main = apiResponse?.Weather?.FirstOrDefault()?.Main,
            Speed = apiResponse?.Wind?.Speed
        };
    }
}
