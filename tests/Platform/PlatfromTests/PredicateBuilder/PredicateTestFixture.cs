namespace PlatformTests.PredicateBuilder;

public class PredicateTestFixture : IDisposable
{
    public IList<FakeAccount> Accounts = new List<FakeAccount>
    {
        new("test01", 0),
        new("test02", 1),
        new("test03", 2),
        new("test04", 3),
        new("test05", 4),
        new("test06", 5)
    };

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
