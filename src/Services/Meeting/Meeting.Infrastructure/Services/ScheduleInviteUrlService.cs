using Meeting.Infrastructure.Repositories.Abstracts;
using Meeting.Infrastructure.Services.Abstracts;

namespace Meeting.Infrastructure.Services;

public class ScheduleInviteUrlService : IScheduleInviteUrlService
{
	private readonly ILoggerAdapter<ScheduleInviteUrlService> _loggerAdapter;
	private readonly IScheduleInviteUrlRepository _shcScheduleInviteUrlRepository;

	public ScheduleInviteUrlService(IScheduleInviteUrlRepository shcScheduleInviteUrlRepository,
		ILoggerAdapter<ScheduleInviteUrlService> loggerAdapter)
	{
		_shcScheduleInviteUrlRepository = shcScheduleInviteUrlRepository;
		_loggerAdapter = loggerAdapter;
	}

	public Task<string> CreateNewUrlAsync()
	{
		throw new NotImplementedException();
	}
}
