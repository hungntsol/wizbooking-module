using EventBusMessage.Abstracts;
using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using SharedCommon.Commons;
using SharedCommon.Exceptions.StatusCodes._500;
using SharedEventBus.Events;

namespace Identity.Application.Features.Commands.Register;
internal sealed class RegisterNewAccountCommandHandler : IRequestHandler<RegisterNewAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUnitOfWork<IdentityDataContext> _unitOfWork;
    private readonly IMessageProducer _messageProducer;

    public RegisterNewAccountCommandHandler(IUserAccountRepository userAccountRepository,
        IUnitOfWork<IdentityDataContext> unitOfWork,
        IMessageProducer messageProducer)
    {
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
        _messageProducer = messageProducer;
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

            var eventMessage = new SendMailEventBus()
            {
                To = newUser.Email,
                DisplayName = "test.com",
                Subject = "Verify Email",
                TemplateName = "Test",
                TemplateModel = default,
            };
            await _messageProducer.PublishAsync(eventMessage, "Worker.Mailing.Send");

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            throw new InternalServerException();
        }

        return JsonHttpResponse<Unit>.Ok(Unit.Value, "Register succsesfully");
    }
}
