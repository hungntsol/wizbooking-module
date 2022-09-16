using MediatR;

namespace SharedCommon.Modules.Mediator.Query;

public interface ICqrsQuery : ICqrsRequest
{
}

public interface ICqrsQuery<out TResult> : ICqrsQuery, IRequest<TResult>
{
}

public abstract class CqrsQuery<TResult> : CqrsRequest, ICqrsQuery<TResult>
{
}
