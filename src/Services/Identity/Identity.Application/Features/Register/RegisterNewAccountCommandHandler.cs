using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using SharedCommon.Exceptions.StatusCodes._500;

namespace Identity.Application.Features.Register;
internal sealed class RegisterNewAccountCommandHandler : IRequestHandler<RegisterNewAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUnitOfWork<IdentityDataContext> _unitOfWork;

    public RegisterNewAccountCommandHandler(IUserAccountRepository userAccountRepository,
        IUnitOfWork<IdentityDataContext> unitOfWork)
    {
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(RegisterNewAccountCommand request, CancellationToken cancellationToken)
    {
        var existedUser = await _userAccountRepository.FindOneAsync(q => q.Email == request.Email, cancellationToken);
        if (existedUser is not null)
        {
            return JsonHttpResponse<Unit>.ErrorBadRequest("Email has been used");
        }

        var newUser = UserAccount.New(request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.Gender,
            UserRoleTypes.NORMAL);

        using var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var added = await _userAccountRepository.InsertAsync(newUser, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            throw new InternalServerException();
        }

        return JsonHttpResponse<Unit>.Ok(Unit.Value, "Register succsesfully");
    }
}
