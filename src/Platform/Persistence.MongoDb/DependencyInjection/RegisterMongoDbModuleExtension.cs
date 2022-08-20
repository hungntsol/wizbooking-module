using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.Abstract;
using Persistence.MongoDb.Internal;

namespace Persistence.MongoDb.DependencyInjection;

public static class RegisterMongoDbModuleExtension
{
	public static IServiceCollection RegisterMongoDbModule<TDbContext>(this IServiceCollection services,
		Action<MongoContextConfiguration> contextConfiguration) where TDbContext : class, IMongoDbContext
	{
		services.AddScoped<IMongoDbContext, TDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		var configuration = new MongoContextConfiguration();
		contextConfiguration.Invoke(configuration);

		services.AddSingleton(configuration);


		return services;
	}

	public static void InitInternalAfterSetup(IServiceCollection services)
	{
		using var scope = services.BuildServiceProvider().CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
		dbContext.InternalCreateIndexesAsync();
	}
}
