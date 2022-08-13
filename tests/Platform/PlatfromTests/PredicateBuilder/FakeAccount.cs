namespace PlatformTests.PredicateBuilder;

public class FakeAccount
{
    public FakeAccount(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public string Name { get; set; }
    public int Id { get; set; }
}
