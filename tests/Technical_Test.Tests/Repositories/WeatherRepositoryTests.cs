using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Technical_Test.Domain.Entities;
using Technical_Test.Infrastructure.Repositories;

namespace Technical_Test.Tests.Repositories;

public class WeatherRepositoryTests
{
    private readonly string _fakeConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";

    private static IConfiguration CreateConfig(string conn)
    {
        var dict = new Dictionary<string, string>
        {
           
        };
        if (conn != null)
            dict["ConnectionStrings:DefaultConnection"] = conn;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(dict)
            .Build();
    }

    [Fact]
    public async Task AddAsync_MustCallProcedureComCorrectParameters()
    {
        // Arrange
        var configuration = CreateConfig(_fakeConnectionString);
        var repo = new WeatherRepository(configuration);

        var weather = new Weather
        {
            Lon = 12.34,
            Lat = 56.78,
            TempMin = 10,
            TempMax = 20,
            Visibility = 1000,
            Sunrise = 123456,
            Sunset = 654321,
            Description = "Clear sky",
            Main = "Clear",
            Speed = 5.5
        };

        // Act + Assert
        await Assert.ThrowsAnyAsync<SqlException>(() => repo.AddAsync(weather, "Lisboa"));
    }

    [Fact]
    public void Ctor_ComConnectionStringNull_MustThrowExcecao()
    {
        var configuration = CreateConfig(null);

        Assert.Throws<InvalidOperationException>(() => new WeatherRepository(configuration));
    }

    [Fact]
    public async Task GetAsync_ComConnectionStringInvalid_MustThrowException()
    {
        var configuration = CreateConfig(string.Empty);
        var repo = new WeatherRepository(configuration);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.GetAsync(1, 10));
    }

    [Fact]
    public async Task GetByIdAsync_ComConnectionStringInvalid_MustThrowException()
    {
        var configuration = CreateConfig(string.Empty);
        var repo = new WeatherRepository(configuration);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.GetByIdAsync(1));
    }

    [Fact]
    public async Task DeleteAsync_ComConnectionStringInvalida_MustThrowException()
    {
        var configuration = CreateConfig(string.Empty);
        var repo = new WeatherRepository(configuration);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.DeleteAsync(1));
    }
}
