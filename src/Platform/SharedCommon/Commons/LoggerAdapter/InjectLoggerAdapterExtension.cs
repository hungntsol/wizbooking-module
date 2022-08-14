using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Commons.LoggerAdapter;

public static class InjectLoggerAdapterExtension
{
    public static void InjectLoggerAdapter(this IServiceCollection services)
    {
        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }
}
