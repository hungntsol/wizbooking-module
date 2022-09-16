namespace SharedCommon.Commons.Exceptions.StatusCodes._400;

public class DataNotFoundException : HttpBaseException
{
	public DataNotFoundException() : base(400, "Data not found")
	{
	}

	public DataNotFoundException(string message) : base(400, message)
	{
	}
}
