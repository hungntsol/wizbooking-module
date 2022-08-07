using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedCommon.Commons.JsonSerialization;

public static class PlatformJsonSerializerOptions
{
    public static readonly JsonSerializerOptions DefaultOptions;

    static PlatformJsonSerializerOptions()
    {
        DefaultOptions = BuildDefaultOptions();
    }

    public static JsonSerializerOptions BuildDefaultOptions(
        bool useCamelCaseNaming = true,
        bool useJsonStringEnumConverter = true)
    {
        var result = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        if (useCamelCaseNaming)
            result.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        if (useJsonStringEnumConverter)
            result.Converters.Add(new JsonStringEnumConverter());

        return result;
    }
}