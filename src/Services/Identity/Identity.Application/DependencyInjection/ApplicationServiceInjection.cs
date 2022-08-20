using System.Reflection;
using Identity.Application.Middlewares;
using Identity.Application.PipelineBehaviours;
using Identity.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EfCore.DependencyInjection;

namespace Identity.Application.DependencyInjection;

public static class ApplicationServiceInjection
{
	public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.RegisterEfCoreModule<IdentityDataContext>(Assembly.GetExecutingAssembly());

		// pipeline
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		// middleware
		services.AddTransient<HandleExceptionMiddleware>();

		return services;
	}
}
