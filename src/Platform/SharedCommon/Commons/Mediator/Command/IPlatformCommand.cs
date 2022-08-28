using MediatR;

namespace SharedCommon.Commons.Mediator.Command;

public interface IPlatformCommand : IPlatformRequest
{
}

public interface IPlatformCommand<out TResult> : IPlatformCommand, IRequest<TResult>
{
}
