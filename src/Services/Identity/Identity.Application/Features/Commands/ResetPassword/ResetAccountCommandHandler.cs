using Identity.Domain.Common;
using SharedCommon.Commons.PredicateBuilder;

namespace Identity.Application.Features.Commands.ResetPassword;

public class ResetAccountCommandHandler : IRequestHandler<ResetAccountCommand, JsonHttpResponse>
{
	private readonly IEfCoreAtomicWork _efCoreAtomicWork;
	private readonly IUserAccountRepository _userAccountRepository;
	private readonly IVerifiedUrlRepository _verifiedUrlOnlyRepository;

	public ResetAccountCommandHandler(IUserAccountRepository userAccountRepository,
		IVerifiedUrlRepository verifiedUrlRepository,
		IEfCoreAtomicWork efCoreAtomicWork)
	{
		_userAccountRepository = userAccountRepository;
		_verifiedUrlOnlyRepository = verifiedUrlRepository;
		_efCoreAtomicWork = efCoreAtomicWork;
	}

	public async Task<JsonHttpResponse> Handle(ResetAccountCommand request,
		CancellationToken cancellationToken)
	{
		var predicateDefinition = new PredicateBuilder<VerifiedUrl>(q =>
			q.AppCode.Equals(request.AppCode) && q.Target.Equals(VerifiedUrlTargetConstant.ResetAccount));

		// start transaction
		await using var transaction = await _efCoreAtomicWork.BeginTransactionAsync(cancellationToken);

		var verifyUrl = await FindAndDeleteUrlFromRepo(predicateDefinition, cancellationToken);
		var user = await FindAccountFromRepo(verifyUrl, cancellationToken);

		user.SetPassword(request.NewPassword);
		await _userAccountRepository.UpdateAsync(user, cancellationToken: cancellationToken);

		await _efCoreAtomicWork.CommitAsync(cancellationToken);

		return JsonHttpResponse.Success(Unit.Value);
	}

	private async Task<UserAccount> FindAccountFromRepo(VerifiedUrl verifyUrl,
		CancellationToken cancellationToken = default)
	{
		var user = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(verifyUrl.Email), cancellationToken);
		ArgumentNullException.ThrowIfNull(user);
		return user;
	}

	private async Task<VerifiedUrl> FindAndDeleteUrlFromRepo(PredicateBuilder<VerifiedUrl> predicateDefinition,
		CancellationToken cancellationToken = default)
	{
		var verifyUrl =
			await _verifiedUrlOnlyRepository.FindAndDelete(predicateDefinition, cancellationToken: cancellationToken);
		ArgumentNullException.ThrowIfNull(verifyUrl);
		return verifyUrl;
	}
}
