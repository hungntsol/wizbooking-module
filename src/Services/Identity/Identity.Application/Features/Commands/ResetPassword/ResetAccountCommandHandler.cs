using Identity.Domain.Common;

namespace Identity.Application.Features.Commands.ResetPassword;

public class ResetAccountCommandHandler : IRequestHandler<ResetAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly ILoggerAdapter<ResetAccountCommandHandler> _loggerAdapter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAccountCoreRepository _userAccountRepository;
    private readonly IVerifiedUrlRepository _verifiedUrlRepository;

    public ResetAccountCommandHandler(IUserAccountCoreRepository userAccountRepository,
        IVerifiedUrlRepository verifiedUrlRepository,
        ILoggerAdapter<ResetAccountCommandHandler> loggerAdapter,
        IUnitOfWork unitOfWork)
    {
        _userAccountRepository = userAccountRepository;
        _verifiedUrlRepository = verifiedUrlRepository;
        _loggerAdapter = loggerAdapter;
        _unitOfWork = unitOfWork;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(ResetAccountCommand request,
        CancellationToken cancellationToken)
    {
        var predicateDefinition = new PredicateBuilder<VerifiedUrl>(q =>
            q.AppCode.Equals(request.AppCode) && q.Target.Equals(VerifiedUrlTargetConstant.ResetAccount));

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var verifyUrl = await _verifiedUrlRepository.FindAndDelete(predicateDefinition, cancellationToken);
        ArgumentNullException.ThrowIfNull(verifyUrl);

        var user = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(verifyUrl.Email), cancellationToken);
        ArgumentNullException.ThrowIfNull(user);


        user.SetPassword(request.NewPassword);
        var updatedAccount = await _userAccountRepository.UpdateAsync(user, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return JsonHttpResponse<Unit>.Ok(Unit.Value);
    }
}
