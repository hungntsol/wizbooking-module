using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.LayerRegister;

internal static class AppSettingLoadingExtension
{
	internal static IServiceCollection LoadInfraAppSettingOptions(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<AuthAppSetting>(configuration.GetSection("AuthSettings"));
		services.Configure<DomainClientAppSetting>(configuration.GetSection("DomainClient"));

		return services;
	}
}
