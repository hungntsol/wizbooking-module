using MediatR;
using Meeting.Domain.Entities;
using Meeting.Infrastructure.Repositories.Abstracts;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Mediator.Command;
using SharedCommon.ServiceModules.AccountContext;
using SharedCommon.UnitOfWork;

namespace Meeting.Application.Features.HostUserSupply.Commands;

public class
	CreateNewHostUserSupplyingCommandHandler : PlatformCommandHandler<CreateNewHostUserSupplyingCommand,
		JsonHttpResponse<Unit>>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IHostUserSupplyingRepository _hostUserSupplyingRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateNewHostUserSupplyingCommandHandler(IMediator mediator,
		IUnitOfWork unitOfWork,
		IHostUserSupplyingRepository hostUserSupplyingRepository,
		IAccountAccessorContextService accountAccessorContextService) : base(mediator)
	{
		_unitOfWork = unitOfWork;
		_hostUserSupplyingRepository = hostUserSupplyingRepository;
		_accountAccessorContextService = accountAccessorContextService;
	}

	protected override async Task<JsonHttpResponse<Unit>> InternalHandleAsync(CreateNewHostUserSupplyingCommand command,
		CancellationToken cancellationToken)
	{
		var userId = _accountAccessorContextService.GetIdentifier();
		var newSupply = HostUserSupplying.New(userId, command.Name, command.Description, command.Tags);

		await _hostUserSupplyingRepository.InsertOneAsync(newSupply, cancellationToken);

		return JsonHttpResponse<Unit>.Ok(Unit.Value, "Create new supply service success");
	}
}
