using EFCore.Persistence.Filter;
using Identity.Domain.Common;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.Logger;

namespace Identity.Application.Features.Commands.ResetPassword;

public class ResetAccountCommandHandler : IRequestHandler<ResetAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IVerifiedUrlRepository _verifiedUrlRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerAdapter<ResetAccountCommandHandler> _loggerAdapter;

    public ResetAccountCommandHandler(IUserAccountRepository userAccountRepository,
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
        var predicateDefinition = new PredicateDefinition<VerifiedUrl>(q =>
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