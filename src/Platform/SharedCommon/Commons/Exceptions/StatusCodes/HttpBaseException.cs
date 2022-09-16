namespace SharedCommon.Commons.Exceptions.StatusCodes;

public class HttpBaseException : Exception
{
	public int HttpCode;

	public HttpBaseException(int code, string message) : base(message)
	{
		HttpCode = code;
	}
}
