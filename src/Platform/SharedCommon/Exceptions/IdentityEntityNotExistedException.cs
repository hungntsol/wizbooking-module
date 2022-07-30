namespace SharedCommon.Exceptions;

public class IdentityEntityNotExistedException : Exception
{
    public IdentityEntityNotExistedException() : base("Not found Id column of table") { }
}
