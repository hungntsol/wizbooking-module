namespace Mailing.Worker.Engine;

public interface IViewEngineRenderer
{
    /// <summary>
    /// Render .cshtml page to string
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<string> RenderAsStringAsync<TModel>(string viewName, TModel model);
}