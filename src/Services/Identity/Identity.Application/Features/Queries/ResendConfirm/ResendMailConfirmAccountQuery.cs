using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Queries.ResendConfirm;

public class ResendMailConfirmAccountQuery : IRequest<JsonHttpResponse<Unit>>
{
    public string Email { get; init; }

    public ResendMailConfirmAccountQuery(string email)
    {
        Email = email;
    }
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