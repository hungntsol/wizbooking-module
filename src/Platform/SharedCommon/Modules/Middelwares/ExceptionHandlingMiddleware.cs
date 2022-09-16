using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SharedCommon.Commons.Exceptions.StatusCodes;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Modules.LoggerAdapter;

namespace SharedCommon.Modules.Middelwares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
	private readonly ILoggerAdapter<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(ILoggerAdapter<ExceptionHandlingMiddleware> logger)
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

		var responseEx = new JsonHttpResponse(statusCode, false, default, exception.Message, trace);

		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = statusCode;

		var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
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
