using System.Linq.Expressions;
using MongoDB.Driver;

namespace Persistence.MongoDb.Abstract;

public interface IMongoRepository<TDocument> : IMongoReadOnlyRepository<TDocument> where TDocument : IDocument
{
	#region Upsert

	Task<TDocument> Upsert(
		Expression<Func<TDocument, bool>> predicate,
		TDocument document,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TDerived> Upsert<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		TDerived document,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<TDocument> Upsert(
		FilterDefinition<TDocument> predicate,
		TDocument document,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TDerived> Upsert<TDerived>(
		FilterDefinition<TDerived> predicate,
		TDerived document,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Insert

	Task InsertOneAsync(TDocument document, CancellationToken cancellationToken = default);

	Task InsertBatchAsync(IEnumerable<TDocument> documents, CancellationToken cancellationToken = default);

	Task InsertBatchAsync<TDerived>(IEnumerable<TDerived> documents, CancellationToken cancellationToken = default)
		where TDerived : TDocument;

	#endregion

	#region Find or insert

	Task<TProject> FindOrInsertAsync<TProject>(Expression<Func<TDocument, bool>> predicate,
		TDocument document,
		Expression<Func<TDocument, TProject>>? projectExpression = null,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TProject> FindOrInsertAsync<TDerived, TProject>(Expression<Func<TDerived, bool>> predicate,
		TDerived document,
		Expression<Func<TDerived, TProject>>? projectExpression = null,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<TProject> FindOrInsertAsync<TProject>(FilterDefinition<TDocument> predicate,
		TDocument document,
		Expression<Func<TDocument, TProject>>? projectExpression = null,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TProject> FindOrInsertAsync<TDerived, TProject>(FilterDefinition<TDerived> predicate,
		TDerived document,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Update

	Task<UpdateResult> UpdateOneAsync(
		string id,
		Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default);

	Task<UpdateResult> UpdateOneAsync<TDerived>(
		string id,
		Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<UpdateResult> UpdateOneAsync(
		Expression<Func<TDocument, bool>> predicate,
		Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default);

	Task<UpdateResult> UpdateOneAsync<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<UpdateResult> UpdateOneAsync(
		FilterDefinition<TDocument> predicate,
		Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default);

	Task<UpdateResult> UpdateOneAsync<TDerived>(
		FilterDefinition<TDerived> predicate,
		Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> updateFunc,
		UpdateOptions? updateOptions = null,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Find and update

	Task<TProject> FindAndUpdateAsync<TProject>(
		Expression<Func<TDocument, bool>> predicate,
		Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> updateFunc,
		Expression<Func<TDocument, TProject>> projectExpression,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TProject> FindAndUpdateAsync<TDerived, TProject>(
		Expression<Func<TDerived, bool>> predicate,
		Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> updateFunc,
		Expression<Func<TDerived, TProject>> projectExpression,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	Task<TProject> FindAndUpdateAsync<TProject>(
		FilterDefinition<TDocument> predicate,
		Func<UpdateDefinitionBuilder<TDocument>, UpdateDefinition<TDocument>> updateFunc,
		Expression<Func<TDocument, TProject>> projectExpression,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default);

	Task<TProject> FindAndUpdateAsync<TDerived, TProject>(
		FilterDefinition<TDerived> predicate,
		Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> updateFunc,
		Expression<Func<TDerived, TProject>> projectExpression,
		ReturnDocument returnDocument = ReturnDocument.After,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Delete

	Task<DeleteResult> DeleteOneAsync(string id, CancellationToken cancellationToken = default);

	Task<DeleteResult> DeleteOneAsync(Expression<Func<TDocument, bool>> predicate,
		CancellationToken cancellationToken = default);

	Task<DeleteResult> DeleteOneAsync<TDerived>(Expression<Func<TDerived, bool>> predicate,
		CancellationToken cancellationToken = default)
		where TDerived : TDocument;

	Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> predicate,
		CancellationToken cancellationToken = default);

	Task<DeleteResult> DeleteOneAsync<TDerived>(FilterDefinition<TDerived> predicate,
		CancellationToken cancellationToken = default) where TDerived : TDocument;

	#endregion

	#region Find and delete

	Task<TDocument> FindAndDeleteAsync(string id, CancellationToken cancellationToken = default);

	Task<TDocument> FindAndDeleteAsync(
		Expression<Func<TDocument, bool>> predicate,
		FindOneAndDeleteOptions<TDocument>? options = null,
		CancellationToken cancellationToken = default);

	Task<TDocument> FindAndDeleteAsync(
		FilterDefinition<TDocument> predicate,
		FindOneAndDeleteOptions<TDocument>? options = null,
		CancellationToken cancellationToken = default);

	Task<TDerived> FindAndDeleteAsync<TDerived>(
		Expression<Func<TDerived, bool>> predicate,
		FindOneAndDeleteOptions<TDerived>? options = null,
		CancellationToken cancellationToken = default);

	Task<TDerived> FindAndDeleteAsync<TDerived>(
		FilterDefinition<TDerived> predicate,
		FindOneAndDeleteOptions<TDerived>? options = null,
		CancellationToken cancellationToken = default);

	#endregion
}
