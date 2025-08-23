using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Services.Helper_Classes;

public class OpenWeatherMapResponse
{
    [JsonPropertyName("coord")]
    public Coord Coord { get; set; }
    [JsonPropertyName("weather")]
    public List<WeatherInfo> Weather { get; set; }
    [JsonPropertyName("main")]
    public MainData Main { get; set; }
    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }
    [JsonPropertyName("wind")]
    public Wind Wind { get; set; }
    [JsonPropertyName("sys")]
    public Sys Sys { get; set; }
}
