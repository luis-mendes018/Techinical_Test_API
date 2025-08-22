using Microsoft.AspNetCore.Mvc;
using Technical_Test.Application.Interfaces;

namespace Technical_Test.API.Controllers;

[Route("api/v1/weather")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        try
        {
            var weatherData = await _weatherService.GetCurrentWeatherAsync(city);

            if (weatherData == null)
            {
                return NotFound();
            }

            return Ok(weatherData);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An internal server error occurred." });
        }
    }

    [HttpGet("record-data")]
    public async Task<IActionResult> GetRecordedData()
    {
        try
        {
            var weatherRecords = await _weatherService.GetRecordedDataAsync();

            if (weatherRecords == null)
            {
                return NotFound();
            }

            return Ok(weatherRecords);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
        }
    }
}
