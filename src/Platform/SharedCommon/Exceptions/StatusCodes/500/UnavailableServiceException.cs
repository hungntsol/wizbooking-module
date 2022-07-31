namespace SharedCommon.Exceptions.StatusCodes._500;
public class UnavailableServiceException : HttpBaseException
{
    public UnavailableServiceException() : base(503, "Some services is not available now")
    {

    }

    public UnavailableServiceException(string message) : base(503, message)
    {

    }
}
