namespace Mailing.Worker.Engine;

public interface IViewEngineRenderer
{
    /// <summary>
    ///     Render .cshtml page to string
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="eventTemplate"></param>
    /// <param name="eventModel"></param>
    /// <returns></returns>
    Task<string> RenderAsStringAsync<TModel>(string eventTemplate, string? eventModel = null);
}
