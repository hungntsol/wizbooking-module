using System.Reflection;
using Identity.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EfCore.DependencyInjection;
using SharedCommon.Modules.Middelwares;
using SharedCommon.Modules.PipelineBehaviours;

namespace Identity.Application.LayerRegister;

public static class RegisterLayerApplicationExtension
{
	public static IServiceCollection RegisterLayerApplication(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddEfCoreDbContext<AppDbContext>(Assembly.GetExecutingAssembly());
		services.AddDefaultHandleExceptionMiddleware();
		services.AddDefaultPipelineBehaviorValidation();

		return services;
	}
}
