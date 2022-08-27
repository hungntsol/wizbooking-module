using System.Reflection;
using FluentValidation;
using Meeting.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
			contextConfiguration: ctx =>
			{
				ctx.DatabaseName = "WizMeeting";
				ctx.ClientSettings = new MongoClientSettings
				{
					Server = new MongoServerAddress("localhost", 27017),
					Credential = MongoCredential.CreateCredential("WizMeeting", "root", "root@123"),
					DirectConnection = true,
					//ReplicaSetName = "meeting_cluster",
					//WriteConcern = new WriteConcern(WriteConcern.WValue.Parse("3"), wTimeout: TimeSpan.Parse("10")),
					ConnectTimeout = TimeSpan.FromSeconds(5)
				};
			});

		services.RegisterHandleExceptionMiddlewareModule();
		services.RegisterPipelineValidationBehaviorModule();

		return services;
	}
}
