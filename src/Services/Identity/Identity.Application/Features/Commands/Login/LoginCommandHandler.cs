using Identity.Application.Features.Queries.Profile;
using Microsoft.AspNetCore.Http;
using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, JsonHttpResponse<LoginCommandViewResult>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IAuthService _authService;

    private readonly JsonHttpResponse<LoginCommandViewResult> _failedLogin = new()
    {
        IsSuccess = false,
        Status = StatusCodes.Status400BadRequest,
        Data = default,
        Message = "Email or password is incorrect",
        Errors = default
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

        if (!account.IsValid())
        {
            _failedLogin.Message = "This account is disable or not actived";
            return _failedLogin;
        }

        // TODO: traceIp later

        account.LastLogin = DateTime.UtcNow;
        var updateLastLogin = await _userAccountRepository.UpdateOneFieldAsync(account, q => q.LastLogin!, cancellationToken);

        var loginResultView = new LoginCommandViewResult(
            _authService.GenerateAccessToken(account), 
            account.Adapt<GetProfileResultView>());

        return JsonHttpResponse<LoginCommandViewResult>.Ok(loginResultView);
    }
}
