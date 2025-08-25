using Technical_Test.Application.DTOs;
using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface IWeatherService
{
    Task<PagedResultDto<Weather>> GetRecordedDataAsync(int page, int pageSize);
    Task<Weather> GetCurrentWeatherAsync(string city);
    Task<Weather> GetRecordedDataByIdAsync(int id);
    Task<bool> DeleteDataAsync(int id);
}
