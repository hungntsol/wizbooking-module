using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedCommon.Exceptions.StatusCodes;
using System.Text.Json;

namespace Identity.Application.Middlewares;
public class HandleExceptionMiddleware : IMiddleware
{
    private readonly ILogger<HandleExceptionMiddleware> _logger;

    public HandleExceptionMiddleware(ILogger<HandleExceptionMiddleware> logger)
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
        if (exception is HttpBaseException httpEx)
        {
            return (httpEx.HttpCode, httpEx.StackTrace);
        }

        if (exception is ValidationException validationEx)
        {
            return (400, validationEx.Errors.ToDictionary(q => q.PropertyName, q => q.ErrorMessage));
        }

        return (500, default);
    }
}
