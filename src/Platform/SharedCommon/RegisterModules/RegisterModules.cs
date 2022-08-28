using System.Text;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedCommon.Commons.LoggerAdapter;
using SharedCommon.Commons.Middelwares;
using SharedCommon.Commons.PipelineBehaviours;
using SharedCommon.ServiceModules.AccountContext;

namespace SharedCommon.RegisterModules;

public static class RegisterModules
{
	public static void RegisterLoggerAdapter(this IServiceCollection services)
	{
		services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
		services.AddTransient(typeof(ILoggerAdapter), typeof(LoggerAdapter<>));
	}

	/// <summary>
	/// Register Mapster for mapping object
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection RegisterMappingModule(this IServiceCollection services)
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

	/// <summary>
	/// Register authentication for service
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection RegisterAuthModule(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddTransient<IAccountAccessorContextService, AccountAccessorContextService>();
		services.AddAuthorization();

		var authSection = configuration.GetSection("AuthSettings");
		var signingKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(authSection.GetValue<string>("SecretKey")));
		ArgumentNullException.ThrowIfNull(signingKey, nameof(signingKey));
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = signingKey,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

		return services;
	}

	public static IServiceCollection RegisterHandleExceptionMiddlewareModule(this IServiceCollection services)
	{
		services.AddTransient<HandleExceptionMiddleware>();
		return services;
	}

	public static IServiceCollection RegisterPipelineValidationBehaviorModule(this IServiceCollection services)
	{
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
		return services;
	}
}
