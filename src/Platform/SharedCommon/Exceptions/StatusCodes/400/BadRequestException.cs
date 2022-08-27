namespace SharedCommon.Exceptions.StatusCodes._400;

public class BadRequestException : HttpBaseException
{
	public BadRequestException(string message) : base(400, message)
	{
	}

	public BadRequestException() : base(400, "Bad request")
	{
	}
}
