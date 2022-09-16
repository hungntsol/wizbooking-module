using MapsterMapper;
using SharedCommon.Commons.Exceptions.StatusCodes._500;
using SharedCommon.Modules.JwtAuth.AccountContext;

namespace Identity.Application.Features.Commands.UpdateProfile;

internal class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, JsonHttpResponse>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IMapper _mapper;
	private readonly IUserAccountRepository _userAccountRepository;

	public UpdateProfileCommandHandler(IUserAccountRepository userAccountRepository,
		IAccountAccessorContextService accountAccessorContextService, IMapper mapper)
	{
		_userAccountRepository = userAccountRepository;
		_accountAccessorContextService = accountAccessorContextService;
		_mapper = mapper;
	}

	public async Task<JsonHttpResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
	{
		var account = await _userAccountRepository.FindOneAsync(
			q => q.Email.Equals(_accountAccessorContextService.GetEmail()),
			cancellationToken);
		if (account is null)
		{
			throw new Exception();
		}

		_mapper.Map(request, account);

		try
		{
			await _userAccountRepository.UpdateAsync(account, cancellationToken: cancellationToken);
			return JsonHttpResponse.Success(Unit.Value);
		}
		catch (Exception)
		{
			throw new InternalServerException();
		}
	}
}
