using Meeting.Infrastructure.Persistence;
using Meeting.Infrastructure.Persistence.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.DependencyInjection;

namespace Meeting.Infrastructure.DependencyInjection;

public static class InjectInfrastructureExtension
{
	public static IServiceCollection InjectInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.RegisterMongoDbModule<ScheduleMeetingDbContext>(conf => { conf.DatabaseName = "test"; });

		RegisterMongoDbModuleExtension.InitInternalAfterSetup(services);
		ConfigureMappingSource();

		return services;
	}

	private static void ConfigureMappingSource()
	{
		ScheduleInviteUrlMappingSource.Configure();
	}
}
