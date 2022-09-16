using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Modules.LoggerAdapter;

public static class LoggerAdapterInjection
{
	public static void AddLoggerAdapter(this IServiceCollection services)
	{
		services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
	}
}
