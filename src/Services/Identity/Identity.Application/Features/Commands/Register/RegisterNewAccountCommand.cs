using FluentValidation;
using SharedCommon.Commons;

namespace Identity.Application.Features.Commands.Register;
public class RegisterNewAccountCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string Gender { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public RegisterNewAccountCommand(string email, string password, string gender, string firstName, string lastName)
    {
        Email = email;
        Password = password;
        Gender = gender;
        FirstName = firstName;
        LastName = lastName;
    }
}

public sealed class RegisterNewAccountCommandValidation : AbstractValidator<RegisterNewAccountCommand>
{
    public RegisterNewAccountCommandValidation()
    {
        RuleFor(q => q.Email)
            .EmailAddress().WithMessage("Email is not valid")
            .NotEmpty().WithMessage("Email is required");

        RuleFor(q => q.Password)
            .MinimumLength(6).WithMessage("Password must longer than 6 characters")
            .NotEmpty().WithMessage("Password is required");

        RuleFor(q => q.FirstName)
            .NotEmpty().WithMessage("Firstname is required");

        RuleFor(q => q.LastName)
            .NotEmpty().WithMessage("Lastname is required");
    }
}
