using MediatR;
using Meeting.Infrastructure.Persistence;
using Meeting.Infrastructure.Repositories.Abstracts;
using SharedCommon.Commons.HttpResponse;

namespace Meeting.Application.Features.Commands.GenerateNewInviteUrl;

public class GenerateNewInviteUrlCommandHandler : IRequestHandler<GenerateNewInviteUrlCommand, JsonHttpResponse<Unit>>
{
	private readonly IScheduleInviteUrlRepository _scheduleInviteUrlRepository;
	private readonly ScheduleMeetingDbContext _scheduleMeetingDbContext;

	public GenerateNewInviteUrlCommandHandler(IScheduleInviteUrlRepository scheduleInviteUrlRepository,
		ScheduleMeetingDbContext scheduleMeetingDbContext)
	{
		_scheduleInviteUrlRepository = scheduleInviteUrlRepository;
		_scheduleMeetingDbContext = scheduleMeetingDbContext;
	}

	public async Task<JsonHttpResponse<Unit>> Handle(GenerateNewInviteUrlCommand request,
		CancellationToken cancellationToken)
	{
		return JsonHttpResponse<Unit>.Ok(Unit.Value);
	}
}
