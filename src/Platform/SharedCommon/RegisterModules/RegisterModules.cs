using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.RegisterModules;

public static class RegisterModules
{
    public static IServiceCollection RegisterMapping(this IServiceCollection services)
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
