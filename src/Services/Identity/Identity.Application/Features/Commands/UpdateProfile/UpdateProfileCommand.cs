namespace Identity.Application.Features.Commands.UpdateProfile;
public class UpdateProfileCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Bio { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public string Gender { get; init; } = null!;
}

public class UpdateProfileCommandValidation : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidation()
    {
        RuleFor(q => q.LastName)
            .NotEmpty().WithMessage("LastName is required");

        RuleFor(q => q.FirstName)
            .NotEmpty().WithMessage("FirstName is required");

        RuleFor(q => q.DateOfBirth)
            .GreaterThan(new DateTime(1900, 1, 1))
            .When(q => q.DateOfBirth is not null).WithMessage("Invalid birthdate");
    }
}
