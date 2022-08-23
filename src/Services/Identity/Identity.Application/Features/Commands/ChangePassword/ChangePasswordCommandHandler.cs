namespace Identity.Application.Features.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, JsonHttpResponse<Unit>>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly ILoggerAdapter<ChangePasswordCommandHandler> _loggerAdapter;
	private readonly IUserAccountRepository _userAccountRepository;

	public ChangePasswordCommandHandler(IUserAccountRepository userAccountRepository,
		IAccountAccessorContextService accountAccessorContextService,
		ILoggerAdapter<ChangePasswordCommandHandler> loggerAdapter)
	{
		_userAccountRepository = userAccountRepository;
		_accountAccessorContextService = accountAccessorContextService;
		_loggerAdapter = loggerAdapter;
	}


	public async Task<JsonHttpResponse<Unit>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		var account = await _userAccountRepository.FindOneAsync(q =>
			q.Email.Equals(_accountAccessorContextService.GetEmail()), cancellationToken);
		ArgumentNullException.ThrowIfNull(account);

		if (!account.ValidatePassword(request.CurrentPassword))
		{
			throw new UnauthorizedAccessException();
		}

		account.SetPassword(request.NewPassword);
		await _userAccountRepository.UpdateAsync(account, cancellationToken: cancellationToken);

		return JsonHttpResponse<Unit>.Ok(Unit.Value);
	}
}
