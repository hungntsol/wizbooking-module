using Identity.Application.DependencyInjection;
using Identity.Application.Middlewares;
using Identity.Infrastructure.DependencyInjection;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Identity.Api.ServiceExtensions;

public static class ApplicationConfigureExtension
{
    public static IServiceCollection Setup(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            var jwtSchema = DefineOpenApiSecuritySchema();
            config.AddSecurityDefinition(jwtSchema.Reference.Id, jwtSchema);
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSchema, ArraySegment<string>.Empty }
            });
        });

        services.InjectInfrastructure(configuration);
        services.InjectApplication(configuration);
        services.InjectAuthentication(configuration);

        return services;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.DisplayOperationId();
                setup.DisplayRequestDuration();
            });
        }

        app.MigrateAppDataContext();

        app.UseHttpsRedirection();

        app.UseMiddleware<HandleExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    /// <summary>
	/// Migrate database to latest version if it is not yet updated.
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	private static WebApplication MigrateAppDataContext(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();

        if (!dataContext.Database.CanConnect())
        {
            dataContext.Database.Migrate();
            dataContext.Database.EnsureCreated();
        }

        return app;
    }

    private static OpenApiSecurityScheme DefineOpenApiSecuritySchema()
    {
        return new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "Jwt",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Enter ONLY `jwtToken` here",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
    }
}
