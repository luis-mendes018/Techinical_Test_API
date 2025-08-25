using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Helper_Classes;

public class Sys
{
    [JsonPropertyName("sunrise")]
    public long? Sunrise { get; set; }
    [JsonPropertyName("sunset")]
    public long? Sunset { get; set; }
}
