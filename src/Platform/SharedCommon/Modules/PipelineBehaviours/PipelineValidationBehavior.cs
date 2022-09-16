using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace SharedCommon.Modules.PipelineBehaviours;

internal sealed class PipelineValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public PipelineValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
		RequestHandlerDelegate<TResponse> next)
	{
		// validate
		await ValidateRequest(request, cancellationToken);

		// next
		return await next();
	}

	private async Task ValidateRequest(TRequest request, CancellationToken cancellationToken)
	{
		if (_validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);

			var validationResults = await SelectValidationResult(context, cancellationToken);
			var failures = SelectFailures(validationResults);

			HandleFailures(failures);
		}
	}

	private static void HandleFailures(ICollection<ValidationFailure> failures)
	{
		if (failures.Count > 0)
		{
			throw new ValidationException(failures.FirstOrDefault()!.ErrorMessage, failures);
		}
	}

	private static ICollection<ValidationFailure> SelectFailures(IEnumerable<ValidationResult> validationResults)
	{
		var failures = validationResults.SelectMany(q => q.Errors)
			.Where(q => q is not null)
			.ToList();
		return failures;
	}

	private async Task<ValidationResult[]> SelectValidationResult(IValidationContext context,
		CancellationToken cancellationToken = default)
	{
		var validationResults =
			await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
		return validationResults;
	}
}
