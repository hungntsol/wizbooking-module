using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Modules.Middelwares;

public static class ExceptionHandlingMiddlewareInjection
{
	/// <summary>
	/// Handle exception of application
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddDefaultHandleExceptionMiddleware(this IServiceCollection services)
	{
		services.AddTransient<ExceptionHandlingMiddleware>();
		return services;
	}
}
