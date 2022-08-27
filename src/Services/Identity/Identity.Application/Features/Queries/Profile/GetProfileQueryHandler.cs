namespace Identity.Application.Features.Queries.Profile;

internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, JsonHttpResponse<GetProfileResultView>>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IUserAccountRepository _userAccountRepository;

	public GetProfileQueryHandler(IUserAccountRepository userAccountRepository,
		IAccountAccessorContextService accountAccessorContextService)
	{
		_userAccountRepository = userAccountRepository;
		_accountAccessorContextService = accountAccessorContextService;
	}

	public async Task<JsonHttpResponse<GetProfileResultView>> Handle(GetProfileQuery request,
		CancellationToken cancellationToken)
	{
		var account =
			await _userAccountRepository.FindOneAsync(q => q.Email == _accountAccessorContextService.GetEmail(),
				cancellationToken);

		ArgumentNullException.ThrowIfNull(account);

		var profile = account.Adapt<GetProfileResultView>();

		return JsonHttpResponse<GetProfileResultView>.Ok(profile);
	}
}
