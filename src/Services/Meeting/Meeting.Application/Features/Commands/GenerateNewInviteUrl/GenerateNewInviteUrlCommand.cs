using MediatR;
using SharedCommon.Commons.HttpResponse;

namespace Meeting.Application.Features.Commands.GenerateNewInviteUrl;

public class GenerateNewInviteUrlCommand : IRequest<JsonHttpResponse<Unit>>
{
}
