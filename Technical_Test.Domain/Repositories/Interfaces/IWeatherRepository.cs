using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IWeatherRepository
{
    Task<(IEnumerable<Weather> WeatherData, int TotalCount)> GetAsync(int page, int pageSize);

    Task AddAsync(Weather weather, string city);
    Task<Weather> GetByIdAsync(int id);
    Task<int> DeleteAsync(int id);
}
