using SharedCommon.Modules.JwtAuth;

namespace Identity.Api.ServiceExtensions;

internal static class AuthenticationExtension
{
	internal static IServiceCollection InjectAuthentication(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddJwtAuthModule(configuration);
		return services;
	}
}
