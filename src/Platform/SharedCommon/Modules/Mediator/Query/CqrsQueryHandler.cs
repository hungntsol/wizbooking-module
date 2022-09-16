using MediatR;

namespace SharedCommon.Modules.Mediator.Query;

public abstract class CqrsQueryHandler<TQuery, TResult> : ICqrsRequestHandler<TQuery>,
	IRequestHandler<TQuery, TResult>
	where TQuery : ICqrsQuery<TResult>
{
	public virtual async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
	{
		var result = await InternalHandleAsync(request, cancellationToken);
		return result;
	}

	protected abstract Task<TResult> InternalHandleAsync(TQuery request, CancellationToken cancellationToken = default);
}
