using Technical_Test.Domain.Entities;

namespace Technical_Test.Domain.Repositories.Interfaces;

public interface IWeatherRepository
{
    Task AddAsync(Weather weather, string city);
    Task<IEnumerable<Weather>> GetAsync();
}
