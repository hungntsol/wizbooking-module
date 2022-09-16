using System.Reflection;
using FluentValidation;
using Meeting.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.DependencyInjection;
using SharedCommon.Modules.Middelwares;
using SharedCommon.Modules.PipelineBehaviours;

namespace Meeting.Application.DependencyInjection;

public static class ApplicationServiceInjection
{
	public static IServiceCollection InjectApplicationLayer(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddMongoDbContext<ScheduleMeetingDbContext>(
			assembly: Assembly.GetExecutingAssembly(),
			contextConfiguration: cof =>
			{
				cof.Connection = configuration.GetValue<string>("MeetingDataContext:ConnectionString");
				cof.DatabaseName = configuration.GetValue<string>("MeetingDataContext:Database");
			});

		services.AddDefaultHandleExceptionMiddleware();
		services.AddDefaultPipelineBehaviorValidation();

		return services;
	}
}
