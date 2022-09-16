using System.Text.Json;
using RazorLight;
using SharedCommon.Commons.Exceptions.StatusCodes._500;
using SharedCommon.Modules.LoggerAdapter;

namespace Mailing.Worker.Engine;

public class ViewEngineRenderer : IViewEngineRenderer
{
	private readonly ILoggerAdapter<ViewEngineRenderer> _logger;
	private readonly RazorLightEngine _razorLightEngine;

	public ViewEngineRenderer(RazorLightEngine razorLightEngine, ILoggerAdapter<ViewEngineRenderer> logger)
	{
		_razorLightEngine = razorLightEngine;
		_logger = logger;
	}

	public async Task<string> RenderAsStringAsync<TModel>(string eventTemplate, string? eventModel = null)
	{
		try
		{
			var html = await _razorLightEngine.CompileRenderAsync(eventTemplate,
				DeserializeEventModel<TModel>(eventModel));
			return html;
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "FATAL: cannot render HTML from razor file\n {Message}", e.Message);
			throw new UnavailableServiceException();
		}
	}

	private static TModel? DeserializeEventModel<TModel>(string? eventModel)
	{
		if (string.IsNullOrEmpty(eventModel))
		{
			return default;
		}

		return JsonSerializer.Deserialize<TModel>(eventModel, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});
	}
}
