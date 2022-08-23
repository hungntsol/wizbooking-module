using System.Reflection;
using Identity.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EfCore.DependencyInjection;
using SharedCommon.RegisterModules;

namespace Identity.Application.DependencyInjection;

public static class ApplicationServiceInjection
{
	public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.RegisterEfCoreModule<IdentityDataContext>(Assembly.GetExecutingAssembly());
		services.RegisterHandleExceptionMiddlewareModule();
		services.RegisterPipelineValidationBehaviorModule();

		return services;
	}
}
