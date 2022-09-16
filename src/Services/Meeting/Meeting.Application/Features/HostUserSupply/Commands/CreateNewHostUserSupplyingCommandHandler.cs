using MediatR;
using Meeting.Domain.Entities;
using Meeting.Infrastructure.Persistence;
using Meeting.Infrastructure.Repositories.Abstracts;
using Persistence.MongoDb.Abstract;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Modules.JwtAuth.AccountContext;
using SharedCommon.Modules.Mediator.Command;

namespace Meeting.Application.Features.HostUserSupply.Commands;

public class CreateNewHostUserSupplyingCommandHandler :
	CqrsCommandHandler<CreateNewHostUserSupplyingCommand, JsonHttpResponse>
{
	private readonly IAccountAccessorContextService _accountAccessorContextService;
	private readonly IAtomicWork _atomicWork;
	private readonly IHostUserSupplyingRepository _hostUserSupplyingRepository;
	private readonly ScheduleMeetingDbContext _scheduleMeetingDbContext;

	public CreateNewHostUserSupplyingCommandHandler(IMediator mediator,
		IAtomicWork atomicWork,
		IHostUserSupplyingRepository hostUserSupplyingRepository,
		IAccountAccessorContextService accountAccessorContextService,
		ScheduleMeetingDbContext scheduleMeetingDbContext) : base(mediator)
	{
		_atomicWork = atomicWork;
		_hostUserSupplyingRepository = hostUserSupplyingRepository;
		_accountAccessorContextService = accountAccessorContextService;
		_scheduleMeetingDbContext = scheduleMeetingDbContext;
	}

	protected override async Task<JsonHttpResponse> InternalHandleAsync(CreateNewHostUserSupplyingCommand command,
		CancellationToken cancellationToken)
	{
		var userId = _accountAccessorContextService.GetIdentifier();
		var newSupply = HostUserSupplying.New(userId, command.Name, command.Description, command.Tags);

		using (var session = _scheduleMeetingDbContext.GetSessionHandle())
		{
			session.StartTransaction();
			await _hostUserSupplyingRepository.InsertOneAsync(newSupply, cancellationToken);
			await session.CommitTransactionAsync(cancellationToken: cancellationToken);
		}

		return JsonHttpResponse.Success(Unit.Value, "Create new supply service success");
	}
}
