using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Services.Helper_Classes;

public class WeatherInfo
{
    [JsonPropertyName("main")]
    public string? Main { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
