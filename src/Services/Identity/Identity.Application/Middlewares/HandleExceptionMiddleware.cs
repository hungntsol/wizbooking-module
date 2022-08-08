using Microsoft.AspNetCore.Http;
using SharedCommon.Exceptions.StatusCodes;
using System.Text.Json;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Logger;

namespace Identity.Application.Middlewares;

public sealed class HandleExceptionMiddleware : IMiddleware
{
    private readonly ILoggerAdapter<HandleExceptionMiddleware> _logger;

    public HandleExceptionMiddleware(ILoggerAdapter<HandleExceptionMiddleware> logger)
    {
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (statusCode, trace) = GetStatusCode(exception);

        var responseEx = new JsonHttpResponse<Unit>(statusCode, false, default, exception.Message, trace);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(responseEx, options);

        await httpContext.Response.WriteAsync(json);
    }

    private static (int, object?) GetStatusCode(Exception exception)
    {
        return exception switch
        {
            HttpBaseException httpEx => (httpEx.HttpCode, httpEx.StackTrace),
            ValidationException validationEx => (400,
                validationEx.Errors.ToDictionary(q => q.PropertyName, q => q.ErrorMessage)),
            ArgumentNullException argNullException => (400, "Bad request"),
            _ => (500, default)
        };
    }
}