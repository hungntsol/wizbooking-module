using Identity.Application.Features.Queries.Profile;
using Microsoft.AspNetCore.Http;
using SharedCommon.Modules.LoggerAdapter;

namespace Identity.Application.Features.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, JsonHttpResponse>
{
	private readonly IAuthService _authService;

	private readonly JsonHttpResponse _failedLogin = new()
	{
		IsSuccess = false,
		Status = StatusCodes.Status400BadRequest,
		Data = default,
		Message = "Email or password is incorrect",
		Errors = default
	};

	private readonly ILoggerAdapter<LoginCommandHandler> _logger;
	private readonly IUserAccountRepository _userAccountRepository;

	public LoginCommandHandler(IUserAccountRepository userAccountRepository, IAuthService authService,
		ILoggerAdapter<LoginCommandHandler> logger)
	{
		_userAccountRepository = userAccountRepository;
		_authService = authService;
		_logger = logger;
	}

	public async Task<JsonHttpResponse> Handle(LoginCommand request,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation("Login {Email}", request.Email);

		var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
		if (account is null || !account.ValidatePassword(request.Password))
		{
			return _failedLogin;
		}

		if (!account.IsValid())
		{
			_failedLogin.Message = "This account is disable or not activated";
			return _failedLogin;
		}

		// TODO: trace ip later

		account.LastLogin = DateTime.UtcNow;
		var updateLastLogin =
			await _userAccountRepository.UpdateOneFieldAsync(account,
				q => q.LastLogin!,
				cancellationToken: cancellationToken);

		var loginResultView = new LoginCommandViewResult(
			_authService.GenerateAccessToken(account),
			account.Adapt<GetProfileResultView>());

		return JsonHttpResponse.Success(loginResultView);
	}
}
