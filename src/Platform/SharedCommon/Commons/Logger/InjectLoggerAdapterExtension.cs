using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Commons.Logger;

public static class InjectLoggerAdapterExtension
{
    public static void InjectLoggerAdapter(this IServiceCollection services)
    {
        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }
}