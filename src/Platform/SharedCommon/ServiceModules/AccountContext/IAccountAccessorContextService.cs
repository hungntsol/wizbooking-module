namespace SharedCommon.ServiceModules.AccountContext;

public interface IAccountAccessorContextService
{
	/// <summary>
	/// Get account email from context
	/// </summary>
	/// <returns></returns>
	string GetEmail();

	/// <summary>
	/// Get account role from context
	/// </summary>
	/// <returns></returns>
	string Role();
}
