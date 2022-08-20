namespace Persistence.EfCore.Data;

/// <summary>
///     Support publish notify for EfCore repository
/// </summary>
public abstract class EfCoreSupportEvent
{
	private bool _isAllowPublish;

	public void EnableEvent(bool allow)
	{
		_isAllowPublish = allow;
	}

	/// <summary>
	///     Decide whether publish notify or not. After checking the state will be reset
	/// </summary>
	/// <returns></returns>
	public bool CanPublish()
	{
		if (!_isAllowPublish)
		{
			return false;
		}

		_isAllowPublish = false;
		return true;
	}
}
