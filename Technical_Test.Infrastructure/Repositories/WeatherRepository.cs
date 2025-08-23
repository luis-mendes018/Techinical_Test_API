using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Technical_Test.Domain.Entities;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Infrastructure.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _connectionString;

    public WeatherRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection String not found");
    }

    public async Task AddAsync(Weather weather, string city)
    {

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(
                "dbo.sp_InsertWeatherHistory",
                new
                {
                    weather.Lon,
                    weather.Lat,
                    weather.TempMin,
                    weather.TempMax,
                    weather.Visibility,
                    weather.Sunrise,
                    weather.Sunset,
                    weather.Description,
                    weather.Main,
                    weather.Speed,
                    City = city
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }

    public async Task<IEnumerable<Weather>> GetAsync()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<Weather>(
                "dbo.sp_GetAllWeatherHistory",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }

    public async Task<Weather> GetByIdAsync(int id)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryFirstOrDefaultAsync<Weather>(
                "dbo.sp_GetWeatherHistoryById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            return result ?? throw new InvalidOperationException($"Weather with Id {id} not found.");
        }
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            var rowsAffected = await connection.QueryFirstOrDefaultAsync<int>(
                "dbo.sp_DeleteWeatherHistory",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
    }
}
