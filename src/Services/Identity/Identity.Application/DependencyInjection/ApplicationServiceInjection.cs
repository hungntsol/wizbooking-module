using FluentValidation;
using Identity.Application.Features.Register;
using Identity.Application.Middlewares;
using Identity.Application.PipelineBehaviours;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.DependencyInjection;
public static class ApplicationServiceInjection
{
    public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddValidatorsFromAssembly(typeof(RegisterNewAccountCommandValidation).Assembly);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // middleware
        services.AddTransient<HandleExceptionMiddleware>();


        return services;
    }
}
