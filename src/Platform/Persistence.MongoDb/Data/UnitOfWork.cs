using Persistence.MongoDb.Abstract;

namespace Persistence.MongoDb.Internal;

public class UnitOfWork : IUnitOfWork
{
	private readonly IMongoDbContext _dbContext;


	public UnitOfWork(IMongoDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void Dispose()
	{
		_dbContext.Dispose();
	}

	public async Task<bool> Commit()
	{
		return await _dbContext.SaveChanges() > 0;
	}
}
