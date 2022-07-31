namespace Identity.Infrastructure.Services.Abstracts;

public interface IAccountAccessorService
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
