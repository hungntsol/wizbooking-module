using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.Abstract;
using SharedCommon.Commons.LoggerAdapter;
using SharedCommon.Exceptions.StatusCodes._400;

namespace Persistence.MongoDb.Attribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MongoTransactionalAttribute : System.Attribute, IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var dbContext = context.HttpContext.RequestServices.GetRequiredService<IMongoDbContext>();
		var loggerAdapter = context.HttpContext.RequestServices
			.GetRequiredService<ILoggerAdapter<MongoTransactionalAttribute>>();

		using var session = dbContext.GetSessionHandle();
		session.StartTransaction();

		try
		{
			await next();
			await session.CommitTransactionAsync();
		}
		catch (Exception e)
		{
			loggerAdapter.LogError(e, "{Message}", e.Message);
			await session.AbortTransactionAsync();
			throw new BadRequestException();
		}
	}
}
