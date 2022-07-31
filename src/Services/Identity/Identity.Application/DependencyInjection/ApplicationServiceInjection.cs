using Identity.Application.Middlewares;
using Identity.Application.PipelineBehaviours;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.DependencyInjection;
public static class ApplicationServiceInjection
{
    public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMapster();

        // pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // middleware
        services.AddTransient<HandleExceptionMiddleware>();


        return services;
    }

    private static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();

        config.Scan();

        services.AddSingleton(config);
        services.AddTransient<IMapper, ServiceMapper>();

        return services;
    }
}
