using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Persistence.MongoDb.Abstract;

/// <summary>
///     This interface is use for MongoDb repository
///     All of method here is only use for READ data from MongoDb
/// </summary>
/// <typeparam name="TDocument"></typeparam>
public interface IMongoReadOnlyRepository<TDocument> : IDisposable where TDocument : IDocument
{
	#region Queryable & Aggregation

	IMongoQueryable<TDocument> Queryable(AggregateOptions options);

	IMongoQueryable<TDerived> Queryable<TDerived>(AggregateOptions options) where TDerived : TDocument;

	IAggregateFluent<TDocument> Aggregate(AggregateOptions? options = null);

	#endregion

	#region Count

	Task<long> CountAsync(FilterDefinition<TDocument> predicate, CancellationToken cancellationToken = default);

	Task<long> CountAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);

	Task<long> CountAsync<TDerived>(FilterDefinition<TDerived> predicate, CancellationToken cancellationToken = default)
		where TDerived : TDocument;

	Task<long> CountAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<bool> AnyAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);

	#endregion

	#region Find

	IFindFluent<TDocument, TDocument> FindFluent(
		Expression<Func<TDocument, bool>> predicate,
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		FindOptions? findOptions = null) where TDerived : TDocument;

	IFindFluent<TDocument, TDocument> FindFluent(
		FilterDefinition<TDocument> predicate,
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		FilterDefinition<TDerived> predicate,
		FindOptions? findOptions = null) where TDerived : TDocument;

	IFindFluent<TDocument, TDocument> FindFluent(
		Expression<Func<TDocument, object>> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		Expression<Func<TDerived, object>> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument;

	IFindFluent<TDocument, TDocument> FindFluent(
		FieldDefinition<TDocument> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		FieldDefinition<TDerived> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument;

	IFindFluent<TDocument, TDocument> FindFluent(
		IEnumerable<Expression<Func<TDocument, object>>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		IEnumerable<Expression<Func<TDerived, object>>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument;

	IFindFluent<TDocument, TDocument> FindFluent(
		IEnumerable<FieldDefinition<TDocument>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null);

	IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		IEnumerable<FieldDefinition<TDerived>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument;

	Task<IAsyncCursor<TDocument>> FindAsync(
		FilterDefinition<TDocument> predicate,
		FindOptions<TDocument, TDocument>? findOptions = null,
		CancellationToken cancellationToken = default);

	Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
		FilterDefinition<TDerived> predicate,
		FindOptions<TDerived, TDerived>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<IAsyncCursor<TDocument>> FindAsync(
		Expression<Func<TDocument, bool>> predicate,
		FindOptions<TDocument, TDocument>? findOptions = null,
		CancellationToken cancellationToken = default);

	Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		FindOptions<TDerived, TDerived>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<IAsyncCursor<TProject>> FindAsync<TProject>(
		FilterDefinition<TDocument> predicate,
		Expression<Func<TDocument, TProject>> project,
		FindOptions<TDocument, TProject>? findOptions = null,
		CancellationToken cancellationToken = default);

	Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
		FilterDefinition<TDerived> predicate,
		Expression<Func<TDerived, TProject>> project,
		FindOptions<TDerived, TProject>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<IAsyncCursor<TProject>> FindAsync<TProject>(
		Expression<Func<TDocument, bool>> predicate,
		Expression<Func<TDocument, TProject>> project,
		FindOptions<TDocument, TProject>? findOptions = null,
		CancellationToken cancellation = default);

	Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
		Expression<Func<TDerived, bool>> predicate,
		Expression<Func<TDerived, TProject>> project,
		FindOptions<TDerived, TProject>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<TDocument?> FindOneAsync(
		Expression<Func<TDocument, bool>> predicate,
		CancellationToken cancellationToken = default);

	Task<TDerived?> FindOneAsync<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<TProject?> FindOneAsync<TProject>(
		Expression<Func<TDocument, bool>> predicate,
		Expression<Func<TDocument, TProject>> project,
		CancellationToken cancellationToken = default);

	Task<TProject?> FindOneAsync<TDerived, TProject>(
		Expression<Func<TDerived, bool>> predicate,
		Expression<Func<TDerived, TProject>> project,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Get

	Task<TDocument> GetOneAsync(
		FilterDefinition<TDocument> predicate,
		FindOptions<TDocument>? findOptions = null);

	Task<TDerived> GetOneAsync<TDerived>(
		FilterDefinition<TDerived> predicate,
		FindOptions<TDerived>? findOptions = null) where TDerived : TDocument;

	Task<TProject> GetOneAsync<TProject>(
		FilterDefinition<TDocument> predicate,
		Func<TDocument, TProject> selector,
		FindOptions<TDocument>? findOptions = null);

	Task<TDocument> GetOneAsync(
		Expression<Func<TDocument, bool>> predicate,
		FindOptions<TDocument>? findOptions = null);

	Task<TDerived> GetOneAsync<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		FindOptions<TDerived>? findByOptions = null) where TDerived : TDocument;

	#endregion
}
