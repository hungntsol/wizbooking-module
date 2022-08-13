namespace Identity.Application.Features.Queries.Profile;

internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, JsonHttpResponse<GetProfileResultView>>
{
    private readonly IAccountAccessorService _accountAccessorService;
    private readonly IUserAccountCoreRepository _userAccountRepository;

    public GetProfileQueryHandler(IUserAccountCoreRepository userAccountRepository,
        IAccountAccessorService accountAccessorService)
    {
        _userAccountRepository = userAccountRepository;
        _accountAccessorService = accountAccessorService;
    }

    public async Task<JsonHttpResponse<GetProfileResultView>> Handle(GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        var account =
            await _userAccountRepository.FindOneAsync(q => q.Email == _accountAccessorService.GetEmail(),
                cancellationToken);

        ArgumentNullException.ThrowIfNull(account);

        var profile = account.Adapt<GetProfileResultView>();

        return JsonHttpResponse<GetProfileResultView>.Ok(profile);
    }
}
