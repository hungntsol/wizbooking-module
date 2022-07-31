namespace Identity.Domain.Common;
public static class UserGenderTypes
{
    public const string MALE = "MALE";
    public const string FEMALE = "FEMALE";
    public const string OTHER = "OTHER";

    public const string DEFAULT = MALE;

    private static readonly List<string> _roles = new()
    {
        MALE, FEMALE, OTHER
    };

    public static bool IsDefined(string gender)
    {
        return _roles.Contains(gender);
    }
}
