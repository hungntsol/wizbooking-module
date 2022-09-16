using SharedCommon.Commons.HttpResponse;
using SharedCommon.Modules.Mediator.Command;

namespace Meeting.Application.Features.HostUserSupply.Commands;

public class CreateNewHostUserSupplyingCommand : CqrsCommand<JsonHttpResponse>
{
	public CreateNewHostUserSupplyingCommand(string name, string description, List<string> tags)
	{
		Name = name;
		Description = description;
		Tags = tags;
	}

	public string Name { get; init; }
	public string Description { get; init; }
	public List<string> Tags { get; init; }
}
