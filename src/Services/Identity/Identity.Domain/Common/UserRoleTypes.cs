namespace Identity.Domain.Common;

public static class UserRoleTypes
{
    public const string NORMAL = "NORMAL";
    public const string MODERATOR = "MODERATOR";
    public const string ADMIN = "ADMIN";

    public const string DEFAULT = NORMAL;

    private static readonly List<string> _roles = new() { UserRoleTypes.NORMAL, UserRoleTypes.MODERATOR, UserRoleTypes.ADMIN };

    public static bool IsDefined(string role)
    {
        return _roles.Contains(role);
    }
}
