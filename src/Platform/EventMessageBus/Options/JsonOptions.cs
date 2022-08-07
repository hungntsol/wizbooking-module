using System.Text.Json.Serialization;

namespace EventBusMessage.Options;

public static class JsonOptions
{
    public static JsonSerializerOptions Default => new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };
}