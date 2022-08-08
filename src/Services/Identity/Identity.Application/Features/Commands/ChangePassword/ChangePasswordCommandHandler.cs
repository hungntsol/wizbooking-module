using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Logger;

namespace Identity.Application.Features.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IAccountAccessorService _accountAccessorService;
    private readonly ILoggerAdapter<ChangePasswordCommandHandler> _loggerAdapter;

    public ChangePasswordCommandHandler(IUserAccountRepository userAccountRepository,
        IAccountAccessorService accountAccessorService, ILoggerAdapter<ChangePasswordCommandHandler> loggerAdapter)
    {
        _userAccountRepository = userAccountRepository;
        _accountAccessorService = accountAccessorService;
        _loggerAdapter = loggerAdapter;
    }


    public async Task<JsonHttpResponse<Unit>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var account = await _userAccountRepository.FindOneAsync(q =>
            q.Email.Equals(_accountAccessorService.GetEmail()), cancellationToken);
        ArgumentNullException.ThrowIfNull(account);

        if (!account.ValidatePassword(request.CurrentPassword))
        {
            throw new UnauthorizedAccessException();
        }

        account.SetPassword(request.NewPassword);
        await _userAccountRepository.UpdateAsync(account, cancellationToken);

        return JsonHttpResponse<Unit>.Ok(Unit.Value);
    }
}