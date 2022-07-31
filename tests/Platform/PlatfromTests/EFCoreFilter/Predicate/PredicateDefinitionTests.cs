using EFCore.Persistence.Filter;
using System.Linq.Expressions;

namespace PlatfromTests.EFCoreFilter.Predicate;

[Collection("EFCore Predicate Filter")]
public class PredicateDefinitionTests : IClassFixture<PredicateTestFixture>
{
    PredicateTestFixture fixture;

    public PredicateDefinitionTests(PredicateTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_PredicateDefinitionEmpty_ReturnAllUser()
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateDefinition<User>();

        // act
        var result = users.AsQueryable().Where(predicate.Statement)
            .ToList();

        // assert
        Assert.Equal(users.Count, result.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_PredicateDefinition_ReturnUser(int id)
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateDefinition<User>(q => q.Id == id);

        // act
        var result = users.AsQueryable().Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(-1)]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_PredicateDefinition_ReturnNull(int id)
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateDefinition<User>(q => q.Id == id);

        // act
        var result = users.AsQueryable().Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, "02")]
    [InlineData(2, "03")]
    [InlineData(4, "05")]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_CombinePredicateDefinitionAndExpression_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        var defiId = new PredicateDefinition<User>(q => q.Id == id);
        Expression<Func<User, bool>> defiName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
        var predicate = defiId.And(defiName);

        // act
        var result = users.AsQueryable().Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }


    [Theory]
    [InlineData(1, "04")]
    [InlineData(10, "02")]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_CombinePredicateDefinitionOrExpression_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        var defiId = new PredicateDefinition<User>(q => q.Id == id);
        Expression<Func<User, bool>> defiName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
        var predicate = defiId.Or(defiName);

        // act
        var result = users.AsQueryable().Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(1, "test02")]
    [InlineData(3, "test04")]
    [InlineData(5, "test06")]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_CombineBuilderAndDefinitionPredicate_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        var predDefinition = new PredicateDefinition<User>();
        var predBuilderId = new PredicateBuilder<User>().Where(q => q.Id == id);
        var predBuildeName = new PredicateBuilder<User>().Where(q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase));


        predDefinition.And(predBuilderId)
            .And(predBuildeName);

        // act
        var result = users.AsQueryable().Where(predDefinition.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }

    [Theory]
    [InlineData(1, "test07")]
    [InlineData(11, "test04")]
    [InlineData(4, "test06")]
    [Trait("Category", "EFCore PredicateDefinition")]
    public void Should_CombineBuilderOrDefinitionPredicate_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        var predBuilderId = new PredicateBuilder<User>().Where(q => q.Id == id);
        var predBuildeName = new PredicateBuilder<User>().Where(q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        var predDefinition = new PredicateDefinition<User>(predBuilderId);
        predDefinition.Or(predBuildeName);

        // act
        var result = users.AsQueryable().Where(predDefinition.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
    }
}
