namespace SharedCommon.Exceptions.StatusCodes._500;
public class InternalServerException : HttpBaseException
{
    public InternalServerException() : base(500, "Server is downed")
    {

    }
}
