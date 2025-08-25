using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Helper_Classes;

public class MainData
{
    [JsonPropertyName("temp")]
    public double? Temp { get; set; }
    [JsonPropertyName("temp_min")]
    public double? TempMin { get; set; }
    [JsonPropertyName("temp_max")]
    public double? TempMax { get; set; }
}
