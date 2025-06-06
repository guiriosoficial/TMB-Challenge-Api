using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonSerializerConfig
{
    public JsonSerializerOptions Options { get; }

    public JsonSerializerConfig()
    {
        Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
