using MediatR;

namespace SharedCommon.Modules.Mediator.Command;

public abstract class CqrsCommandHandler<TCommand, TResult> : ICqrsRequestHandler<TCommand>,
	IRequestHandler<TCommand, TResult>
	where TCommand : ICqrsCommand<TResult>
{
	protected readonly IMediator Mediator;

	protected CqrsCommandHandler(IMediator mediator)
	{
		Mediator = mediator;
	}

	/// <summary>
	/// Default handle business logic of MediaR lib
	/// Can override default behaviour
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public virtual async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
	{
		// TODO: check if transaction enable (FROM ATTRIBUTE)
		var result = await OnExecutingAsync(request, cancellationToken);

		// publish executed event
		await Mediator.Publish(
			new CqrsCommandNotification<TCommand>(request, PlatformInternalEventAction.Executed),
			cancellationToken);


		return result;
	}

	/// <summary>
	/// This method is use to trigger main business logic
	/// A notification will be published as ActionExecuting
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected virtual async Task<TResult> OnExecutingAsync(TCommand request, CancellationToken cancellationToken)
	{
		// publish executing event
		var notifyTask = Mediator.Publish(
			new CqrsCommandNotification<TCommand>(request, PlatformInternalEventAction.Executing),
			cancellationToken);

		var handleTask = InternalHandleAsync(request, cancellationToken);

		// wait handle and notify task
		await Task.WhenAll(handleTask, notifyTask);

		return handleTask.Result;
	}

	/// <summary>
	/// Contains main business logic of a command request
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected abstract Task<TResult> InternalHandleAsync(TCommand request, CancellationToken cancellationToken);
}
