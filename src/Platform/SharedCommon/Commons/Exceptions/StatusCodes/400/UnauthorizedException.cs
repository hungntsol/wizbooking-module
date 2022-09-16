namespace SharedCommon.Commons.Exceptions.StatusCodes._400;

public class UnauthorizedException : HttpBaseException
{
	public UnauthorizedException() : base(401, "You must authorized to perform this action")
	{
	}
}
