﻿using Identity.Application.LayerRegister;
using Identity.Infrastructure.LayerRegister;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedCommon.Modules.LoggerAdapter;
using SharedCommon.Modules.Middelwares;
using SharedCommon.Utilities;

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
			config.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Identity", Version = "v1" });
			var jwtSchema = DefineOpenApiSecuritySchema();
			config.AddSecurityDefinition(jwtSchema.Reference.Id, jwtSchema);
			config.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ jwtSchema, Utils.List.Empty<string>() }
			});
		});

		services.AddLoggerAdapter();

		services.RegisterLayerInfrastructure(configuration);
		services.RegisterLayerApplication(configuration);
		services.InjectAuthentication(configuration);

		services.AddEventBus(configuration);

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

		app.UseMiddleware<ExceptionHandlingMiddleware>();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		return app;
	}

	/// <summary>
	///     Migrate database to latest version if it is not yet updated.
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	private static void MigrateAppDataContext(this IHost app)
	{
		using var scope = app.Services.CreateScope();
		var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (dataContext.Database.CanConnect())
		{
			return;
		}

		dataContext.Database.Migrate();
		dataContext.Database.EnsureCreated();
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
