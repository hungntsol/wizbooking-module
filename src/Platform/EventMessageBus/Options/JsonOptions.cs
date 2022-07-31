using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventMessageBus.Options;
public static class JsonOptions
{
	public static JsonSerializerOptions Default => new(JsonSerializerDefaults.Web)
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
	};
}
