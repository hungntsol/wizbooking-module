namespace Identity.Application.Features.Queries.ResendConfirm;

public class ResendMailConfirmAccountQuery : IRequest<JsonHttpResponse>
{
	public ResendMailConfirmAccountQuery(string email)
	{
		Email = email;
	}

	public string Email { get; init; }
}

public class ResendMailConfirmAccountQueryValidation : AbstractValidator<ResendMailConfirmAccountQuery>
{
	public ResendMailConfirmAccountQueryValidation()
	{
		RuleFor(q => q.Email)
			.EmailAddress().WithMessage("Email is not valid")
			.NotEmpty().WithMessage("Email is required");
	}
}
