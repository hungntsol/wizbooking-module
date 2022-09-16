namespace SharedCommon.Commons.Exceptions.StatusCodes._400;

public class ForbiddenException : HttpBaseException
{
	public ForbiddenException() : base(403, "You have no access")
	{
	}
}
