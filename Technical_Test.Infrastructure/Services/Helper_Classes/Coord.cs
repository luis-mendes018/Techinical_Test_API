using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Services.Helper_Classes;

public class Coord
{
    [JsonPropertyName("lon")]
    public double? Lon { get; set; }
    [JsonPropertyName("lat")]
    public double? Lat { get; set; }
}
