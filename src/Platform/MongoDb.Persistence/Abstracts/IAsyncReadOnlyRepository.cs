using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoDb.Persistence.Abstracts;

/// <summary>
/// This interface is use for MongoDb repository
/// All of method here is only use for READ data from MongoDb
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IAsyncReadOnlyRepository<TEntity> : IDisposable where TEntity : class
{
    #region Queryable & Aggregation

    IMongoQueryable<TEntity> Queryable(AggregateOptions options);

    IMongoQueryable<TDerived> Queryable<TDerived>(AggregateOptions options) where TDerived : TEntity;

    IAggregateFluent<TEntity> Aggregate(AggregateOptions? options = null);

    #endregion

    #region Count

    Task<long> CountAsync(FilterDefinition<TEntity> predicate, CancellationToken cancellationToken = default);

    Task<long> CountAsync(Expression<TEntity> predicate, CancellationToken cancellationToken = default);

    Task<long> CountAsync<TDerived>(FilterDefinition<TDerived> predicate, CancellationToken cancellationToken = default)
        where TDerived : TEntity;

    Task<long> CountAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    #endregion

    #region Find

    IFindFluent<TEntity, TEntity> FindFluent(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null) where TDerived : TEntity;

    IFindFluent<TEntity, TEntity> FindFluent(
        FilterDefinition<TEntity> predicate,
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        FilterDefinition<TDerived> predicate,
        FindOptions? findOptions = null) where TDerived : TEntity;

    IFindFluent<TEntity, TEntity> FindFluent(
        Expression<Func<TEntity, object>> property,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        Expression<Func<TDerived, object>> property,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null) where TDerived : TEntity;

    IFindFluent<TEntity, TEntity> FindFluent(
        FilterDefinition<TEntity> property,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        FilterDefinition<TDerived> property,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null) where TDerived : TEntity;

    IFindFluent<TEntity, TEntity> FindFluent(
        IEnumerable<Expression<Func<TEntity, object>>> properties,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        IEnumerable<Expression<Func<TDerived, object>>> properties,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null) where TDerived : TEntity;

    IFindFluent<TEntity, TEntity> FindFluent(
        IEnumerable<FilterDefinition<TEntity>> properties,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null);

    IFindFluent<TEntity, TDerived> FindFluent<TDerived>(
        IEnumerable<FilterDefinition<TDerived>> properties,
        string regexPattern,
        string regexOption = "i",
        FindOptions? findOptions = null) where TDerived : TEntity;

    Task<IAsyncCursor<TEntity>> FindAsync(
        FilterDefinition<TEntity> predicate,
        FindOptions<TEntity, TEntity>? findOptions = null,
        CancellationToken cancellationToken = default);

    Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
        FilterDefinition<TDerived> predicate,
        FindOptions<TEntity, TDerived>? findOptions = null,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    Task<IAsyncCursor<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null,
        CancellationToken cancellationToken = default);

    Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
        Expression<Func<TDerived, bool>> predicate,
        FindOptions? findOptions = null,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    Task<IAsyncCursor<TProject>> FindAsync<TProject>(
        FilterDefinition<TEntity> predicate,
        Expression<Func<TEntity, TProject>> project,
        FindOptions? findOptions = null,
        CancellationToken cancellationToken = default);

    Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
        FilterDefinition<TDerived> predicate,
        Expression<Func<TDerived, TProject>> project,
        FindOptions? findOptions = null,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    Task<IAsyncCursor<TProject>> FindAsync<TProject>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProject>> project,
        FindOptions? findOptions = null,
        CancellationToken cancellation = default);

    Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
        Expression<Func<TDerived, bool>> predicate,
        Expression<Func<TDerived, TProject>> project,
        FindOptions? findOptions = null,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TDerived?> FindOneAsync<TDerived>(
        Expression<Func<TDerived, bool>> predicate,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    Task<TProject?> FindOneAsync<TProject>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProject>> project,
        CancellationToken cancellationToken = default);

    Task<TProject?> FindOneAsync<TDerived, TProject>(
        Expression<Func<TDerived, bool>> predicate,
        Expression<Func<TDerived, TProject>> project,
        CancellationToken cancellationToken = default) where TDerived : TEntity;

    #endregion


    #region Get

    Task<TEntity> GetOneAsync(
        FilterDefinition<TEntity> predicate,
        FindOptions? findOptions = null);

    Task<TDerived> GetOneAsync<TDerived>(
        FilterDefinition<TDerived> predicate,
        FindOptions? findOptions = null) where TDerived : TEntity;

    Task<TProject> GetOneAsync<TProject>(
        FilterDefinition<TEntity> predicate,
        FindOptions? findOptions = null);

    Task<TProject> GetOneAsync<TDerived, TProject>(
        FilterDefinition<TDerived> predicate,
        FindOptions? findOptions = null) where TDerived : TEntity;

    Task<TEntity> GetOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null);

    Task<TDerived> GetOneAsync<TDerived>(
        Expression<Func<TDerived, bool>> predicate,
        FindOptions? findByOptions = null) where TDerived : TEntity;

    Task<TProject> GetOneAsync<TProject>(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null);

    Task<TProject> GetOneAsync<TDerived, TProject>(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions? findOptions = null) where TDerived : TEntity;

    #endregion
}