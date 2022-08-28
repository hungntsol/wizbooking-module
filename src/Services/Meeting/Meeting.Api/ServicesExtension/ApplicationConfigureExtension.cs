using Meeting.Application.DependencyInjection;
using Meeting.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SharedCommon.RegisterModules;

namespace Meeting.Api.ServicesExtension;

public static class ApplicationConfigureExtension
{
	public static IServiceCollection Setup(this IServiceCollection services, IConfiguration configuration)
	{
		// Add services to the container.

		services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(conf =>
		{
			conf.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Meeting", Version = "v1" });
			var jwtSchema = DefineOpenApiSecuritySchema();
			conf.AddSecurityDefinition(jwtSchema.Reference.Id, jwtSchema);
			conf.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ jwtSchema, ArraySegment<string>.Empty }
			});
		});

		services.RegisterLoggerAdapter();

		services.RegisterAuthModule(configuration);

		services.InjectApplicationLayer(configuration);
		services.InjectInfrastructureLayer(configuration);

		return services;
	}

	public static WebApplication Configure(this WebApplication app)
	{
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
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
