using MediatR;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Mediator.Command;

namespace Meeting.Application.Features.Commands.GenerateNewInviteUrl;

public class GenerateNewInviteUrlCommand : PlatformCommand<JsonHttpResponse<Unit>>
{
}
