using EFCore.Persistence.Filter;
using EFCore.Persistence.Filter.Extensions;
using System.Linq.Expressions;

namespace PlatfromTests.EFCoreFilter.Predicate;

[Collection("EFCore Predicate Filter")]
public class PredicateBuilderTest : IClassFixture<PredicateTestFixture>
{
    PredicateTestFixture fixture;

    public PredicateBuilderTest(PredicateTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_EmptyPredicateBuilder_ReturnAllOfItem()
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateBuilder<User>().Empty();

        // act
        var result = users.AsQueryable()
            .Where(predicate.Statement)
            .ToList();

        // assert
        Assert.Equal(users.Count, result.Count);
        Assert.Equal(users[0].Id, result[0].Id);
        Assert.Equal(users[1].Name, result[1].Name);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_FilterIdPredicate_ReturnUser(int id)
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateBuilder<User>().Where(q => q.Id == id);

        // act
        var result = users.AsQueryable()
            .Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id].Name, result!.Name);
    }

    [Theory]
    [InlineData(1, "test02")]
    [InlineData(2, "test03")]
    [InlineData(3, "test04")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_FilterIdAndName_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        var predicate = new PredicateBuilder<User>().Where(q => q.Id == id && q.Name == name);

        // act
        var result = users.AsQueryable()
            .Where(predicate.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }

    [Theory]
    [InlineData(1, "02")]
    [InlineData(3, "04")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_CombineAndAlsoTwoExpression_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        Expression<Func<User, bool>> filterId = q => q.Id == id;
        Expression<Func<User, bool>> filterName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

        var predicateAnd = new PredicateBuilder<User>().Where(filterId.AndAlso(filterName));

        // act
        var result = users.AsQueryable()
            .Where(predicateAnd.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }

    [Theory]
    [InlineData(1, "05")]
    [InlineData(3, "06")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_CombineAndAlsoTwoExpression_ReturnNull(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        Expression<Func<User, bool>> filterId = q => q.Id == id;
        Expression<Func<User, bool>> filterName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

        var predicateAnd = new PredicateBuilder<User>().Where(filterId.AndAlso(filterName));

        // act
        var result = users.AsQueryable()
            .Where(predicateAnd.Statement)
            .FirstOrDefault();

        // assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, "test03")]
    [InlineData(6, "test02")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_CombineOr2Expression_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        Expression<Func<User, bool>> filterId = q => q.Id == id;
        Expression<Func<User, bool>> filterName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

        var predicateOr = new PredicateBuilder<User>().Where(filterId.Or(filterName));

        // act
        var result = users.AsQueryable().Where(predicateOr.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(9, "test09")]
    [InlineData(11, "test015")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_CombineOr2Expression_ReturnNull(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        Expression<Func<User, bool>> filterId = q => q.Id == id;
        Expression<Func<User, bool>> filterName = q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

        var predicateOr = new PredicateBuilder<User>().Where(filterId.Or(filterName));

        // act
        var result = users.AsQueryable().Where(predicateOr.Statement)
            .FirstOrDefault();

        // assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, "test02")]
    [InlineData(5, "test06")]
    [Trait("Category", "EFCore PredicateBuilder")]
    public void Should_CombineComplexOrWithAndExpression_ReturnUser(int id, string name)
    {
        // arrange
        var users = fixture.Users;
        Expression<Func<User, bool>> exprFalse = q => q.Name.Length > 10; // alway false
        Expression<Func<User, bool>> exprId = q => q.Id == id;
        Expression<Func<User, bool>> exprName = q => q.Name == name;

        var predBuilder = new PredicateBuilder<User>().Where(exprFalse.Or(exprId.AndAlso(exprName)));

        // act
        var result = users.AsQueryable()
            .Where(predBuilder.Statement)
            .FirstOrDefault();

        // assert
        Assert.NotNull(result);
        Assert.Equal(users[id], result);
    }
}
