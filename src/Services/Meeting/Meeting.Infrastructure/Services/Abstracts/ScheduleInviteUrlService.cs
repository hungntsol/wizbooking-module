namespace Meeting.Infrastructure.Services.Abstracts;

public interface IScheduleInviteUrlService
{
	Task<string> CreateNewUrl();
}
