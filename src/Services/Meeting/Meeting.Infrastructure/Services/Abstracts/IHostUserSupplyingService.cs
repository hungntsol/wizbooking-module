namespace Meeting.Infrastructure.Services.Abstracts;

public interface IHostUserSupplyingService
{
	Task<bool> RegisterNew(string name, string description, IList<string> tags);
}
