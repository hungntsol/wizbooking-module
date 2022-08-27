using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Commons.LoggerAdapter;

public static class InjectLoggerAdapterExtension
{
	public static void RegisterLoggerAdapter(this IServiceCollection services)
	{
		services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
	}
}
