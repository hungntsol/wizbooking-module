using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Commands.ForgetPassword;

public class ForgetPasswordCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string Email { get; init; } = null!;
}

public class ForgetPasswordCommandCommandValidation : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandCommandValidation()
    {
        RuleFor(q => q.Email)
            .EmailAddress().WithMessage("Email is not valid")
            .NotEmpty().WithMessage("Email is required");
    }
}