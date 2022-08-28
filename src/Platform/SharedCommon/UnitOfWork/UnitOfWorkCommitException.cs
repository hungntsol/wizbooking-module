namespace SharedCommon.UnitOfWork;

public class UnitOfWorkCommitException : Exception
{
	public UnitOfWorkCommitException(string msg) : base(msg)
	{
	}
}
