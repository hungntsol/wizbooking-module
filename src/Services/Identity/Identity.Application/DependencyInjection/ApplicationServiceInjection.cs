using System.Reflection;
using Identity.Application.Middlewares;
using Identity.Application.PipelineBehaviours;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.DependencyInjection;

public static class ApplicationServiceInjection
{
    public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.RegisterMapster();

        services.AddMediatR(Assembly.GetExecutingAssembly());

        // pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // middleware
        services.AddTransient<HandleExceptionMiddleware>();


        return services;
    }

    private static IServiceCollection RegisterMapster(this IServiceCollection services)
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
