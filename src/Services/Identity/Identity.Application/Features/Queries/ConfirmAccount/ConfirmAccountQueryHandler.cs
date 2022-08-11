using EFCore.Persistence.Filter;
using Identity.Domain.Common;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Logger;

namespace Identity.Application.Features.Queries.ConfirmAccount;

public class ConfirmAccountQueryHandler : IRequestHandler<ConfirmAccountQuery, JsonHttpResponse<Unit>>
{
    private readonly IVerifiedUrlRepository _verifiedUrlRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly ILoggerAdapter<ConfirmAccountQueryHandler> _loggerAdapter;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmAccountQueryHandler(IVerifiedUrlRepository verifiedUrlRepository,
        ILoggerAdapter<ConfirmAccountQueryHandler> loggerAdapter,
        IUserAccountRepository userAccountRepository,
        IUnitOfWork unitOfWork)
    {
        _verifiedUrlRepository = verifiedUrlRepository;
        _loggerAdapter = loggerAdapter;
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(ConfirmAccountQuery request, CancellationToken cancellationToken)
    {
        // begin tx
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var predicateBuilder =
            new PredicateDefinition<VerifiedUrl>(q =>
                q.AppCode.Equals(request.AppCode) && q.Target.Equals(VerifiedUrlTargetConstant.ConfirmEmail));
        var verifyUrl = await _verifiedUrlRepository.FindAndDelete(predicateBuilder, cancellationToken);

        ArgumentNullException.ThrowIfNull(verifyUrl);

        if (!verifyUrl.IsValid())
        {
            return JsonHttpResponse<Unit>.ErrorBadRequest("Your link has been expired");
        }

        var account =
            await _userAccountRepository.FindOneAsync(q => q.Email.Equals(verifyUrl.Email), cancellationToken);

        ArgumentNullException.ThrowIfNull(account);

        account.Activate();
        await _userAccountRepository.UpdateAsync(account, cancellationToken);

        _loggerAdapter.LogInformation("Activate account {Email} at {Time}", account.Email, DateTime.UtcNow);

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _loggerAdapter.LogError(e, "{Message}", e.Message);
            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException();
        }

        return JsonHttpResponse<Unit>.Ok(Unit.Value);
    }
}