using Microsoft.Extensions.Configuration;

using System.Text.Json;
using System.Text.Json.Serialization;

using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Entities;

namespace Technical_Test.Infrastructure.Services;
public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Weather> GetCurrentWeatherAsync(string city)
    {
        var apiKey = _configuration["OpenWeatherMap:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("OpenWeatherMap API Key is not configured.");
        }

        try
        {
            // Codifica o nome da cidade para a URL
            var encodedCity = Uri.EscapeDataString(city);

            var apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={encodedCity}&appid={apiKey}&units=metric";

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<OpenWeatherMapResponse>(jsonString);

            var weatherData = new Weather
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

            return weatherData;
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException($"Error fetching weather data from external API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new ApplicationException($"Error deserializing weather data: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An unexpected error occurred: {ex.Message}", ex);
        }
    }
}


public class Coord
{
    [JsonPropertyName("lon")]
    public double? Lon { get; set; }
    [JsonPropertyName("lat")]
    public double? Lat { get; set; }
}

// Classe para a seção "weather"
public class WeatherInfo
{
    [JsonPropertyName("main")]
    public string? Main { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

// Classe para a seção "main"
public class MainData
{
    [JsonPropertyName("temp")]
    public double? Temp { get; set; }
    [JsonPropertyName("temp_min")]
    public double? TempMin { get; set; }
    [JsonPropertyName("temp_max")]
    public double? TempMax { get; set; }
}

// Classe para a seção "wind"
public class Wind
{
    [JsonPropertyName("speed")]
    public double? Speed { get; set; }
}

// Classe para a seção "sys"
public class Sys
{
    [JsonPropertyName("sunrise")]
    public long? Sunrise { get; set; }
    [JsonPropertyName("sunset")]
    public long? Sunset { get; set; }
}

// Classe principal que representa a resposta completa
public class OpenWeatherMapResponse
{
    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }
    [JsonPropertyName("weather")]
    public List<WeatherInfo>? Weather { get; set; }
    [JsonPropertyName("main")]
    public MainData? Main { get; set; }
    [JsonPropertyName("visibility")]
    public int? Visibility { get; set; }
    [JsonPropertyName("wind")]
    public Wind? Wind { get; set; }
    [JsonPropertyName("sys")]
    public Sys? Sys { get; set; }
}
