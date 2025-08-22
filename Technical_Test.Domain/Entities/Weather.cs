using System.Text.Json.Serialization;

namespace Technical_Test.Domain.Entities;

public class Weather
{
    [JsonPropertyName("lon")]
    public double? Lon { get; set; }

    [JsonPropertyName("lat")]
    public double? Lat { get; set; }

    [JsonPropertyName("temp_min")]
    public double? TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double? TempMax { get; set; }

    [JsonPropertyName("visibility")]
    public int? Visibility { get; set; }

    [JsonPropertyName("sunrise")]
    public long? Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public long? Sunset { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("main")]
    public string? Main { get; set; }

    [JsonPropertyName("speed")]
    public double? Speed { get; set; }
}