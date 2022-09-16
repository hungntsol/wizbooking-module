namespace Identity.Application.Features.Queries.ConfirmAccount;

public class ConfirmAccountQuery : IRequest<JsonHttpResponse>
{
	public string AppCode { get; set; } = null!;
}

public class ConfirmAccountQueryValidation : AbstractValidator<ConfirmAccountQuery>
{
	public ConfirmAccountQueryValidation()
	{
		RuleFor(q => q.AppCode)
			.NotEmpty().WithMessage("AppCode is required");
	}
}
