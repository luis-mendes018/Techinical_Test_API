using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Clients;

public interface IWeatherApiClient
{
    Task<Weather> GetWeatherAsync(string city, string apiKey);
}
