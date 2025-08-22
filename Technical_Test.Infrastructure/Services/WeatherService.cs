using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;
using Technical_Test.Infrastructure.Services.Helper_Classes;

namespace Technical_Test.Infrastructure.Services;
public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IWeatherRepository _weatherRepository;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, IWeatherRepository weatherRepository)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _weatherRepository = weatherRepository;
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

            await _weatherRepository.AddAsync(weatherData, city);

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

    public async Task<IEnumerable<Weather>> GetRecordedDataAsync()
    {
        return await _weatherRepository.GetAsync();
    }

    public async Task<Weather> GetRecordedDataByIdAsync(int id)
    {
        return await _weatherRepository.GetByIdAsync(id);
    }

    public async Task<bool> DeleteDataAsync(int id)
    {
        var rowsAffected = await _weatherRepository.DeleteAsync(id);
        return rowsAffected > 0;
    }
}