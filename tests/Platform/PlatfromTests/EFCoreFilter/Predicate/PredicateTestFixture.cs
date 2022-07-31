namespace PlatfromTests.EFCoreFilter.Predicate;

public class PredicateTestFixture : IDisposable
{
    public IList<User> Users = new List<User>()
    {
        new User("test01", 0),
        new User("test02", 1),
        new User("test03", 2),
        new User("test04", 3),
        new User("test05", 4),
        new User("test06", 5)
    };

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
