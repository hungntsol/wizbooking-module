using Microsoft.AspNetCore.Http;

namespace Identity.Application.Features.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, JsonHttpResponse<LoginCommandViewResult>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IAuthService _authService;

    private readonly JsonHttpResponse<LoginCommandViewResult> _failedLogin = new()
    {
        IsSuccess = false,
        Code = StatusCodes.Status400BadRequest,
        Data = default,
        Message = "Email or password is incorrect",
        Trace = default
    };

    public LoginCommandHandler(IUserAccountRepository userAccountRepository, ILogger<LoginCommandHandler> logger, IAuthService authService)
    {
        _userAccountRepository = userAccountRepository;
        _logger = logger;
        _authService = authService;
    }

    public async Task<JsonHttpResponse<LoginCommandViewResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
        if (account is null || !account.ValidatePassword(request.Password))
            return _failedLogin;

        // TODO: lastLogin, traceIp later

        var loginResultView = new LoginCommandViewResult(_authService.GenerateAccessToken(account), account);

        return JsonHttpResponse<LoginCommandViewResult>.Ok(loginResultView);
    }
}
