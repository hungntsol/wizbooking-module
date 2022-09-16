using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Modules.Mapping;

public static class MappingInjection
{
	/// <summary>
	/// Register Mapster for mapping object
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddMapping(this IServiceCollection services)
	{
		var config = new TypeAdapterConfig
		{
			RequireExplicitMapping = false,
			RequireDestinationMemberSource = false,
			Compiler = exp => exp.Compile()
		};

		config.Scan();

		services.AddSingleton(config);
		services.AddTransient<IMapper, ServiceMapper>();

		return services;
	}
}
