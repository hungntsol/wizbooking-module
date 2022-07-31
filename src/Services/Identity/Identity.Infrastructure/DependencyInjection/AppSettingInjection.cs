using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;
internal static class AppSettingInjection
{
    internal static IServiceCollection AddAppSettingOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthAppSetting>(configuration.GetSection("AuthSettings"));

        return services;
    }
}
