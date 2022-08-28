using Meeting.Infrastructure.Repositories.Abstracts;
using Meeting.Infrastructure.Services.Abstracts;
using SharedCommon.ServiceModules.AccountContext;

namespace Meeting.Infrastructure.Services;

public class HostUserSupplyingService : IHostUserSupplyingService
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IHostUserSupplyingRepository _hostUserSupplyingRepository;
	private readonly ILoggerAdapter<HostUserSupplyingService> _logger;

	public HostUserSupplyingService(ILoggerAdapter<HostUserSupplyingService> logger,
		IHostUserSupplyingRepository hostUserSupplyingRepository,
		IAccountAccessorContextService accountAccessorContextService)
	{
		_logger = logger;
		_hostUserSupplyingRepository = hostUserSupplyingRepository;
		_accountAccessorContextService = accountAccessorContextService;
	}


	public async Task<bool> RegisterNew(string name, string description, IList<string> tags)
	{
		var accountIdFromClaim = _accountAccessorContextService.GetIdentifier();

		var existedSupply = await _hostUserSupplyingRepository.AnyAsync(q => q.Name == name);

		if (existedSupply)
		{
			return false;
		}

		var newSupply = new HostUserSupplying(accountIdFromClaim, name, description, true);
		newSupply.AppendTags(tags);

		await _hostUserSupplyingRepository.InsertOneAsync(newSupply);

		return true;
	}
}
