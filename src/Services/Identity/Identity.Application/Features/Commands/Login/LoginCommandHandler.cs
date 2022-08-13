using Identity.Application.Features.Queries.Profile;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Features.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, JsonHttpResponse<LoginCommandViewResult>>
{
    private readonly IAuthService _authService;

    private readonly JsonHttpResponse<LoginCommandViewResult> _failedLogin = new()
    {
        IsSuccess = false,
        Status = StatusCodes.Status400BadRequest,
        Data = default,
        Message = "Email or password is incorrect",
        Errors = default
    };

    private readonly ILoggerAdapter<LoginCommandHandler> _loggerAdapter;
    private readonly IUserAccountRepository _userAccountRepository;

    public LoginCommandHandler(IUserAccountRepository userAccountRepository, IAuthService authService,
        ILoggerAdapter<LoginCommandHandler> loggerAdapter)
    {
        _userAccountRepository = userAccountRepository;
        _authService = authService;
        _loggerAdapter = loggerAdapter;
    }

    public async Task<JsonHttpResponse<LoginCommandViewResult>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        _loggerAdapter.LogInformation("Login {Email}", request.Email);

        var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
        if (account is null || !account.ValidatePassword(request.Password))
        {
            return _failedLogin;
        }

        if (!account.IsValid())
        {
            _failedLogin.Message = "This account is disable or not actived";
            return _failedLogin;
        }

        // TODO: trace ip later

        account.LastLogin = DateTime.UtcNow;
        var updateLastLogin =
            await _userAccountRepository.UpdateOneFieldAsync(account, q => q.LastLogin!, cancellationToken);

        var loginResultView = new LoginCommandViewResult(
            _authService.GenerateAccessToken(account),
            account.Adapt<GetProfileResultView>());

        return JsonHttpResponse<LoginCommandViewResult>.Ok(loginResultView);
    }
}
