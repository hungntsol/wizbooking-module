using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCommon.RegisterModules;

namespace Identity.Api.ServiceExtensions;

internal static class AuthenticationExtension
{
	internal static IServiceCollection InjectAuthentication(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.RegisterAuthModule(configuration);
		return services;
	}
}
