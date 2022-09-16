namespace Identity.Application.Features.Commands.Login;

public class LoginCommand : IRequest<JsonHttpResponse>
{
	public LoginCommand(string email, string password)
	{
		Email = email;
		Password = password;
	}

	public string Email { get; init; }
	public string Password { get; init; }
}

public sealed class LoginCommandValidation : AbstractValidator<LoginCommand>
{
	public LoginCommandValidation()
	{
		RuleFor(q => q.Email)
			.EmailAddress().WithMessage("Email is not valid")
			.NotEmpty().WithMessage("Email is required");

		RuleFor(q => q.Password)
			.MinimumLength(6).WithMessage("Password must longer than 6 characters")
			.NotEmpty().WithMessage("Password is required");
	}
}
