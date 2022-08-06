using System.Text.Json;
using RazorLight;
using SharedCommon.Exceptions.StatusCodes._500;

namespace Mailing.Worker.Engine;

public class ViewEngineRenderer : IViewEngineRenderer
{
    private readonly RazorLightEngine _razorLightEngine;
    private readonly ILogger<ViewEngineRenderer> _logger;

    public ViewEngineRenderer(RazorLightEngine razorLightEngine, ILogger<ViewEngineRenderer> logger)
    {
        _razorLightEngine = razorLightEngine;
        _logger = logger;
    }

    public async Task<string> RenderAsStringAsync<TModel>(string @eventTemplate, string @eventModel)
    {
        try
        {
            var model = JsonSerializer.Deserialize<TModel>(@eventModel, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var html = await _razorLightEngine.CompileRenderAsync(@eventTemplate, model);
            return html;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "FATAL: cannot render HTML from razor file\n {Message}", e.Message);
            throw new UnavailableServiceException();
        }
    }
}