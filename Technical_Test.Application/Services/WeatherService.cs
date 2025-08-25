using Microsoft.Extensions.Configuration;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Clients;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;


namespace Technical_Test.Application.Services;
public class WeatherService : IWeatherService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly IConfiguration _configuration;
    private readonly IWeatherRepository _weatherRepository;

    public WeatherService(IWeatherApiClient weatherApiClient, IConfiguration configuration, IWeatherRepository weatherRepository)
    {
        _weatherApiClient = weatherApiClient;
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

        var weatherData = await _weatherApiClient.GetWeatherAsync(city, apiKey);

        await _weatherRepository.AddAsync(weatherData, city);

        return weatherData;
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