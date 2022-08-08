using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Commands.ResetPassword;

public class ResetAccountCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string AppCode { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}

public class ResetPasswordAccountQueryValidation : AbstractValidator<ResetAccountCommand>
{
    public ResetPasswordAccountQueryValidation()
    {
        RuleFor(q => q.AppCode)
            .NotEmpty().WithMessage("AppCode is required");

        RuleFor(q => q.NewPassword)
            .MinimumLength(6).WithMessage("Password must be longer than 6 characters")
            .NotEmpty().WithMessage("Password is required");
    }
}