using MapsterMapper;
using SharedCommon.Commons.HttpResponse;

namespace Identity.Application.Features.Commands.UpdateProfile;

internal class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IAccountAccessorService _accountAccessorService;
    private readonly IMapper _mapper;

    public UpdateProfileCommandHandler(IUserAccountRepository userAccountRepository,
        IAccountAccessorService accountAccessorService, IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _accountAccessorService = accountAccessorService;
        _mapper = mapper;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(_accountAccessorService.GetEmail()),
            cancellationToken);
        if (account is null)
        {
            throw new Exception();
        }

        _mapper.Map(request, account);

        try
        {
            var updated = await _userAccountRepository.UpdateAsync(account, cancellationToken);
            return JsonHttpResponse<Unit>.Ok(Unit.Value);
        }
        catch (Exception)
        {
            throw new InternalServerException();
        }
    }
}