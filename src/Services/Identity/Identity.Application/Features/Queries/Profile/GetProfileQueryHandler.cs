using SharedCommon.Modules.JwtAuth.AccountContext;

namespace Identity.Application.Features.Queries.Profile;

internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, JsonHttpResponse>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IUserAccountRepository _userAccountRepository;

	public GetProfileQueryHandler(IUserAccountRepository userAccountRepository,
		IAccountAccessorContextService accountAccessorContextService)
	{
		_userAccountRepository = userAccountRepository;
		_accountAccessorContextService = accountAccessorContextService;
	}

	public async Task<JsonHttpResponse> Handle(GetProfileQuery request,
		CancellationToken cancellationToken)
	{
		var account =
			await _userAccountRepository.FindOneAsync(q => q.Email == _accountAccessorContextService.GetEmail(),
				cancellationToken);

		ArgumentNullException.ThrowIfNull(account);

		var profile = account.Adapt<GetProfileResultView>();

		return JsonHttpResponse.Success(profile);
	}
}
