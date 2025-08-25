using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Technical_Test.Application.Interfaces;

namespace Technical_Test.API.Controllers;

[Route("api/v1/weather")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
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
        catch (Exception ex)
        {
            Console.WriteLine("Error log: {0} ", ex.Message);
            return StatusCode(500, new { message = "An internal server error occurred." });
        }
    }

    [HttpGet("record-data")]
    public async Task<IActionResult> GetRecordedData([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagedResult = await _weatherService.GetRecordedDataAsync(page, pageSize);

            Response.Headers.Append("X-Pagination-Current-Page", pagedResult.CurrentPage.ToString());
            Response.Headers.Append("X-Pagination-Page-Size", pagedResult.PageSize.ToString());
            Response.Headers.Append("X-Pagination-Total-Items", pagedResult.TotalItems.ToString());
            Response.Headers.Append("X-Pagination-Total-Pages", pagedResult.TotalPages.ToString());

            return Ok(pagedResult.Items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
        }
    }


    [HttpGet("record-data/{id}")]
    public async Task<IActionResult> GetRecordedDataById(int id)
    {
        try
        {
            var weatherRecord = await _weatherService.GetRecordedDataByIdAsync(id);

            if (weatherRecord == null)
            {
                return NotFound("Id not found");
            }

            return Ok(weatherRecord);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
        }
    }

    [HttpDelete("delete-data/{id}")]
    public async Task<IActionResult> DeleteRecordedData(int id)
    {
        var user = User.Identity as ClaimsIdentity;

        if (!user.HasClaim(ClaimTypes.Role, "Admin") && !user.HasClaim(ClaimTypes.Role, "Manager"))
        {
            return StatusCode(403, new { message = "Access denied. Only administrators or managers can delete data." });
        }
        try
        {
            var success = await _weatherService.DeleteDataAsync(id);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound("Id not found");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
        }
    }

}
