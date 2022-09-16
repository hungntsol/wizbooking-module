using Identity.Domain.Common;
using SharedCommon.Commons.Exceptions.StatusCodes._500;
using SharedCommon.Commons.PredicateBuilder;
using SharedCommon.Modules.LoggerAdapter;

namespace Identity.Application.Features.Queries.ConfirmAccount;

public class ConfirmAccountQueryHandler : IRequestHandler<ConfirmAccountQuery, JsonHttpResponse>
{
	private readonly IEfCoreAtomicWork _efCoreAtomicWork;
	private readonly ILoggerAdapter<ConfirmAccountQueryHandler> _logger;
	private readonly IUserAccountRepository _userAccountRepository;
	private readonly IVerifiedUrlRepository _verifiedUrlOnlyRepository;

	public ConfirmAccountQueryHandler(IVerifiedUrlRepository verifiedUrlRepository,
		ILoggerAdapter<ConfirmAccountQueryHandler> logger,
		IUserAccountRepository userAccountRepository,
		IEfCoreAtomicWork efCoreAtomicWork)
	{
		_verifiedUrlOnlyRepository = verifiedUrlRepository;
		_logger = logger;
		_userAccountRepository = userAccountRepository;
		_efCoreAtomicWork = efCoreAtomicWork;
	}

	public async Task<JsonHttpResponse> Handle(ConfirmAccountQuery request, CancellationToken cancellationToken)
	{
		// begin tx
		await using var transaction = await _efCoreAtomicWork.BeginTransactionAsync(cancellationToken);

		var predicateBuilder =
			new PredicateBuilder<VerifiedUrl>(q =>
				q.AppCode.Equals(request.AppCode) && q.Target.Equals(VerifiedUrlTargetConstant.ConfirmEmail));
		var verifyUrl =
			await _verifiedUrlOnlyRepository.FindAndDelete(predicateBuilder, cancellationToken: cancellationToken);

		ArgumentNullException.ThrowIfNull(verifyUrl);

		if (!verifyUrl.IsValid())
		{
			return JsonHttpResponse.Fail("Your link has been expired");
		}

		var account =
			await _userAccountRepository.FindOneAsync(q => q.Email.Equals(verifyUrl.Email), cancellationToken);

		ArgumentNullException.ThrowIfNull(account);

		account.Activate();
		await _userAccountRepository.UpdateAsync(account, cancellationToken: cancellationToken);

		_logger.LogInformation("Activate account {Email} at {Time}", account.Email, DateTime.UtcNow);

		try
		{
			await transaction.CommitAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{Message}", e.Message);
			await transaction.RollbackAsync(cancellationToken);
			throw new InternalServerException();
		}

		return JsonHttpResponse.Success(Unit.Value);
	}
}
