using System.Reflection;
using FluentValidation;
using Meeting.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.DependencyInjection;
using SharedCommon.RegisterModules;

namespace Meeting.Application.DependencyInjection;

public static class ApplicationServiceInjection
{
	public static IServiceCollection InjectApplicationLayer(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.RegisterMongoDbModule<ScheduleMeetingDbContext>(
			assembly: Assembly.GetExecutingAssembly(),
			contextConfiguration: cof =>
			{
				cof.Connection = configuration.GetValue<string>("MeetingDataContext:ConnectionString");
				cof.DatabaseName = configuration.GetValue<string>("MeetingDataContext:Database");
			});

		services.RegisterHandleExceptionMiddlewareModule();
		services.RegisterPipelineValidationBehaviorModule();

		return services;
	}
}
