using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SharedCommon.Modules.PipelineBehaviours;

public static class PipelineValidationBehaviorInjection
{
	/// <summary>
	/// Create pipeline to validate input for mediator
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddDefaultPipelineBehaviorValidation(this IServiceCollection services)
	{
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationBehavior<,>));
		return services;
	}
}
