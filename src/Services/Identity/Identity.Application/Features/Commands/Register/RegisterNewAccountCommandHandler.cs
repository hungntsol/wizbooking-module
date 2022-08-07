using System.Text.Json;
using EventBusMessage.Abstracts;
using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.JsonSerialization;
using SharedCommon.Commons.Mailing;
using SharedCommon.Commons.Mailing.Models;
using SharedEventBus.Events;

namespace Identity.Application.Features.Commands.Register;

internal sealed class
    RegisterNewAccountCommandHandler : IRequestHandler<RegisterNewAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUnitOfWork<IdentityDataContext> _unitOfWork;
    private readonly IMessageProducer _messageProducer;
    private readonly DomainClientAppSetting _domainClientAppSetting;

    public RegisterNewAccountCommandHandler(IUserAccountRepository userAccountRepository,
        IUnitOfWork<IdentityDataContext> unitOfWork,
        IMessageProducer messageProducer,
        IOptions<DomainClientAppSetting> domainClientAppSettingOption)
    {
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
        _messageProducer = messageProducer;
        _domainClientAppSetting = domainClientAppSettingOption.Value;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(RegisterNewAccountCommand request,
        CancellationToken cancellationToken)
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

        await using var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var addUserTask = _userAccountRepository.InsertAsync(newUser, cancellationToken);
            var produceEventMessageTask = ProduceConfirmAccountMailEvent(newUser);

            await Task.WhenAll(addUserTask, produceEventMessageTask);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw new InternalServerException();
        }

        return JsonHttpResponse<Unit>.Ok(Unit.Value, "Register successfully, please check your email");
    }

    private async Task ProduceConfirmAccountMailEvent(UserAccount newUser)
    {
        var eventMessage = CreateEventBusMessage(newUser);
        await _messageProducer.PublishAsync(eventMessage, "Worker.Mailing.Send");
    }

    private SendMailEventBusMessage CreateEventBusMessage(UserAccount newUser)
    {
        var eventModel = new ConfirmAccountMailModel()
        {
            ConfirmUrl = _domainClientAppSetting.Url() + "/confirm/me?code_id=" + Guid.NewGuid(),
            ResendUrl = _domainClientAppSetting.Url() + "/confirm/resend?account=" + newUser.Email
        };

        return new SendMailEventBusMessage()
        {
            To = newUser.Email,
            From = "WizBooking <noreply-auth@wizbooking.com>",
            Subject = "Verify Email",
            TemplateName = EmailTemplateConstants.ConfirmAccountMail,
            TemplateModel = JsonSerializer.Serialize(eventModel, PlatformJsonSerializerOptions.DefaultOptions),
            Attachments = default
        };
    }
}