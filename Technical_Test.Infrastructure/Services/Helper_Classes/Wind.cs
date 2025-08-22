using System.Text.Json.Serialization;

namespace Technical_Test.Infrastructure.Services.Helper_Classes;

public class Wind
{
    [JsonPropertyName("speed")]
    public double? Speed { get; set; }
}
