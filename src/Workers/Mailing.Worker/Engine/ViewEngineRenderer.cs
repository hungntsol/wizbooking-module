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

    public async Task<string> RenderAsStringAsync<TModel>(string viewName, TModel model)
    {
        var html = await _razorLightEngine.CompileRenderAsync(viewName, model);

        if (!string.IsNullOrEmpty(html))
            return html;

        _logger.LogCritical("FATAL: cannot render HTML from razor file");
        throw new UnavailableServiceException();
    }
}