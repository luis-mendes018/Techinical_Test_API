using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IWeatherService
{
    Task<Weather> GetCurrentWeatherAsync(string city);
    Task<IEnumerable<Weather>> GetRecordedDataAsync();
    Task<Weather> GetRecordedDataByIdAsync(int id);
    Task<bool> DeleteDataAsync(int id);
}
