using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Helper_Classes;

public class Wind
{
    [JsonPropertyName("speed")]
    public double? Speed { get; set; }
}
