using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;

    public ChangePasswordCommand(string currentPassword, string newPassword)
    {
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}

public class ChangePasswordCommandValidation : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidation()
    {
        RuleFor(q => q.CurrentPassword)
            .MinimumLength(6).WithMessage("Current password must longer than 6 characters")
            .NotEmpty().WithMessage("Current password is required");
        RuleFor(q => q.NewPassword)
            .MinimumLength(6).WithMessage("New password must longer than 6 characters")
            .NotEmpty().WithMessage("New password is required");
    }
}