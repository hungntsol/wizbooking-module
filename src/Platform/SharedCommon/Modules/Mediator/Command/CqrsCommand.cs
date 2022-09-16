using MediatR;

namespace SharedCommon.Modules.Mediator.Command;

public interface ICqrsCommand : ICqrsRequest
{
}

public interface ICqrsCommand<out TResult> : ICqrsCommand, IRequest<TResult>
{
}

public abstract class CqrsCommand<TResult> : CqrsRequest, ICqrsCommand<TResult>
{
}
