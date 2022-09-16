using SharedCommon.Modules.JwtAuth.AccountContext;
using SharedCommon.Modules.LoggerAdapter;

namespace Identity.Application.Features.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, JsonHttpResponse>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly ILoggerAdapter<ChangePasswordCommandHandler> _logger;
	private readonly IUserAccountRepository _userAccountRepository;

	public ChangePasswordCommandHandler(IUserAccountRepository userAccountRepository,
		IAccountAccessorContextService accountAccessorContextService,
		ILoggerAdapter<ChangePasswordCommandHandler> logger)
	{
		_userAccountRepository = userAccountRepository;
		_accountAccessorContextService = accountAccessorContextService;
		_logger = logger;
	}


	public async Task<JsonHttpResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
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

		return JsonHttpResponse.Success(Unit.Value);
	}
}
