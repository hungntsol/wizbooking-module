﻿using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Persistence.MongoDb.Abstract;

namespace Persistence.MongoDb.Data;

public abstract class MongoRepositoryBase<TDocument> : IMongoRepository<TDocument> where TDocument : class, IDocument
{
	private readonly IMongoCollection<TDocument> _collection;
	private readonly IMongoDbContext _context;

	public MongoRepositoryBase(IMongoDbContext context)
	{
		_context = context;
		_collection = _context.GetCollection<TDocument>();
	}

	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}

	#region Get

	public virtual Task<TDocument> GetOneAsync(FilterDefinition<TDocument> predicate,
		FindOptions<TDocument>? findOptions = null)
	{
		return GetOneAsync<TDocument>(predicate, findOptions);
	}

	public virtual async Task<TDerived> GetOneAsync<TDerived>(FilterDefinition<TDerived> predicate,
		FindOptions<TDerived>? findOptions = null)
		where TDerived : TDocument
	{
		var cursor = await _collection.OfType<TDerived>().FindAsync(predicate, findOptions);
		var data = cursor.First();
		ArgumentNullException.ThrowIfNull(data);
		return data;
	}

	public virtual async Task<TProject> GetOneAsync<TProject>(FilterDefinition<TDocument> predicate,
		Func<TDocument, TProject> selector,
		FindOptions<TDocument>? findOptions = null)
	{
		var data = await GetOneAsync<TDocument>(predicate, findOptions);
		var result = selector(data);
		return result;
	}

	public virtual Task<TDocument> GetOneAsync(Expression<Func<TDocument, bool>> predicate,
		FindOptions<TDocument>? findOptions = null)
	{
		return GetOneAsync<TDocument>(predicate, findOptions);
	}

	public virtual async Task<TDerived> GetOneAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		FindOptions<TDerived>? findByOptions = null) where TDerived : TDocument
	{
		var data = await _collection.OfType<TDerived>()
			.FindAsync(predicate, findByOptions);

		ArgumentNullException.ThrowIfNull(data);
		return data.First();
	}

	#endregion

	#region Find

	public virtual Task<IAsyncCursor<TDocument>> FindAsync(FilterDefinition<TDocument> predicate,
		FindOptions<TDocument, TDocument>? findOptions = null,
		CancellationToken cancellationToken = default)
	{
		return FindAsync<TDocument>(predicate, findOptions, cancellationToken);
	}

	public virtual async Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(FilterDefinition<TDerived> predicate,
		FindOptions<TDerived, TDerived>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return await _collection.OfType<TDerived>().FindAsync(predicate, findOptions, cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<IAsyncCursor<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate,
		FindOptions<TDocument, TDocument>? findOptions = null,
		CancellationToken cancellationToken = default)
	{
		return FindAsync<TDocument>(predicate, findOptions, cancellationToken);
	}

	public virtual async Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		FindOptions<TDerived, TDerived>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return await _collection.OfType<TDerived>().FindAsync(predicate, findOptions, cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<IAsyncCursor<TProject>> FindAsync<TProject>(FilterDefinition<TDocument> predicate,
		Expression<Func<TDocument, TProject>> project,
		FindOptions<TDocument, TProject>? findOptions = null,
		CancellationToken cancellationToken = default)
	{
		return FindAsync<TDocument, TProject>(predicate, project, findOptions, cancellationToken);
	}

	public virtual async Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
		FilterDefinition<TDerived> predicate,
		Expression<Func<TDerived, TProject>> project,
		FindOptions<TDerived, TProject>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		var opt = findOptions ?? new FindOptions<TDerived, TProject>();
		opt.Projection = Builders<TDerived>.Projection.Expression(project);
		return await _collection.OfType<TDerived>().FindAsync(predicate, opt, cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<IAsyncCursor<TProject>> FindAsync<TProject>(Expression<Func<TDocument, bool>> predicate,
		Expression<Func<TDocument, TProject>> project,
		FindOptions<TDocument, TProject>? findOptions = null,
		CancellationToken cancellation = default)
	{
		return FindAsync<TDocument, TProject>(predicate, project, findOptions, cancellation);
	}

	public virtual async Task<IAsyncCursor<TProject>> FindAsync<TDerived, TProject>(
		Expression<Func<TDerived, bool>> predicate,
		Expression<Func<TDerived, TProject>> project,
		FindOptions<TDerived, TProject>? findOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		var opt = findOptions ?? new FindOptions<TDerived, TProject>();
		opt.Projection = Builders<TDerived>.Projection.Expression(project);
		return await _collection.OfType<TDerived>().FindAsync(predicate, opt, cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>> predicate,
		CancellationToken cancellationToken = default)
	{
		return FindOneAsync<TDocument>(predicate, cancellationToken);
	}

	public virtual async Task<TDerived?> FindOneAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return await (await _collection.OfType<TDerived>().FindAsync(predicate, cancellationToken: cancellationToken)
				.ConfigureAwait(false))
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<TProject?> FindOneAsync<TProject>(Expression<Func<TDocument, bool>> predicate,
		Expression<Func<TDocument, TProject>> project,
		CancellationToken cancellationToken = default)
	{
		return FindOneAsync<TDocument, TProject>(predicate, project, cancellationToken);
	}

	public virtual async Task<TProject?> FindOneAsync<TDerived, TProject>(Expression<Func<TDerived, bool>> predicate,
		Expression<Func<TDerived, TProject>> project,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return await (await _collection.OfType<TDerived>()
				.FindAsync(predicate,
					new FindOptions<TDerived, TProject>
					{
						Projection = Builders<TDerived>.Projection.Expression(project)
					}, cancellationToken)
				.ConfigureAwait(false))
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	#endregion

	#region Find fluent

	public virtual IFindFluent<TDocument, TDocument> FindFluent(Expression<Func<TDocument, bool>> predicate,
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(predicate, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(Expression<Func<TDerived, bool>> predicate,
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		return _collection.OfType<TDerived>().Find(predicate, findOptions);
	}

	public virtual IFindFluent<TDocument, TDocument> FindFluent(FilterDefinition<TDocument> predicate,
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(predicate, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(FilterDefinition<TDerived> predicate,
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		return _collection.OfType<TDerived>().Find(predicate, findOptions);
	}

	public virtual IFindFluent<TDocument, TDocument> FindFluent(Expression<Func<TDocument, object>> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(property, regexPattern, regexOption, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(Expression<Func<TDerived, object>> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		return _collection.OfType<TDerived>()
			.Find(Builders<TDerived>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOption)));
	}

	public virtual IFindFluent<TDocument, TDocument> FindFluent(FieldDefinition<TDocument> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(property, regexPattern, regexOption, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(FieldDefinition<TDerived> property,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		return _collection.OfType<TDerived>().Find(
			Builders<TDerived>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOption)),
			findOptions);
	}

	public virtual IFindFluent<TDocument, TDocument> FindFluent(
		IEnumerable<Expression<Func<TDocument, object>>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(properties, regexPattern, regexOption, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		IEnumerable<Expression<Func<TDerived, object>>> properties,
		string regexPattern,
		string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		var filters = properties.Select(p =>
			Builders<TDerived>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOption)));

		return _collection.OfType<TDerived>().Find(Builders<TDerived>.Filter.Or(filters), findOptions);
	}

	public virtual IFindFluent<TDocument, TDocument> FindFluent(IEnumerable<FieldDefinition<TDocument>> properties,
		string regexPattern, string regexOption = "i",
		FindOptions? findOptions = null)
	{
		return FindFluent<TDocument>(properties, regexPattern, regexOption, findOptions);
	}

	public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
		IEnumerable<FieldDefinition<TDerived>> properties,
		string regexPattern, string regexOption = "i",
		FindOptions? findOptions = null) where TDerived : TDocument
	{
		var filters = properties.Select(p =>
			Builders<TDerived>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOption)));

		return _collection.OfType<TDerived>().Find(Builders<TDerived>.Filter.Or(filters), findOptions);
	}

	#endregion

	#region Queryable

	public virtual IMongoQueryable<TDerived> Queryable<TDerived>(AggregateOptions options) where TDerived : TDocument
	{
		return _collection.OfType<TDerived>().AsQueryable(options);
	}

	public virtual IMongoQueryable<TDocument> Queryable(AggregateOptions options)
	{
		return _collection.AsQueryable(options);
	}

	public virtual IAggregateFluent<TDocument> Aggregate(AggregateOptions? options = null)
	{
		return _collection.Aggregate(options);
	}

	#endregion

	#region Count

	public virtual async Task<long> CountAsync(FilterDefinition<TDocument> predicate,
		CancellationToken cancellationToken = default)
	{
		return await _collection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<long> CountAsync(Expression<Func<TDocument, bool>> predicate,
		CancellationToken cancellationToken = default)
	{
		return CountAsync((FilterDefinition<TDocument>)predicate, cancellationToken);
	}

	public virtual async Task<long> CountAsync<TDerived>(FilterDefinition<TDerived> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return await _collection.OfType<TDerived>()
			.CountDocumentsAsync(predicate, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public virtual Task<long> CountAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument
	{
		return CountAsync((FilterDefinition<TDerived>)predicate, cancellationToken);
	}

	#endregion
}