namespace SharedCommon.Commons.Mediator.Command;

public abstract class PlatformCommand<TResult> : PlatformRequest, IPlatformCommand<TResult>
{
}
