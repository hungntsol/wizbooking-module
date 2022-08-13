using System.Text.Json;
using EventBusMessage.Abstracts;
using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.JsonSerialization;
using SharedCommon.Commons.Mailing;
using SharedCommon.Commons.Mailing.Models;
using SharedEventBus.Events;

namespace Identity.Application.Features.Commands.Register;

internal sealed class
    RegisterNewAccountCommandHandler : IRequestHandler<RegisterNewAccountCommand, JsonHttpResponse<Unit>>
{
    private readonly DomainClientAppSetting _domainClientAppSetting;
    private readonly AuthAppSetting _authAppSetting;

    public RegisterNewAccountCommandHandler(IUserAccountCoreRepository userAccountRepository,
        IUnitOfWork<IdentityDataContext> unitOfWork,
        IMessageProducer messageProducer,
        IOptions<DomainClientAppSetting> domainClientAppSettingOption,
        IVerifiedUrlRepository verifiedUrlRepository,
        IOptions<AuthAppSetting> authAppSettingOptions)
    {
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
        _messageProducer = messageProducer;
        _verifiedUrlRepository = verifiedUrlRepository;
        _domainClientAppSetting = domainClientAppSettingOption.Value;
        _authAppSetting = authAppSettingOptions.Value;
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

        var newVerifyUrl = VerifiedUrl.New(newUser.Email,
            VerifiedUrlTargetConstant.ConfirmEmail,
            TimeSpan.FromMinutes(_authAppSetting.ConfirmLinkExpiredMinutes));

        await using var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var addAccount = await _userAccountRepository.InsertAsync(newUser, cancellationToken);
        var produceEventMessageTask = ProduceConfirmAccountMailEvent(newUser, newVerifyUrl.AppCode);
        var addVerifiedUrl = await _verifiedUrlRepository.InsertAsync(newVerifyUrl, cancellationToken);

        try
        {
            await produceEventMessageTask.WaitAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw new InternalServerException();
        }

        return JsonHttpResponse<Unit>.Ok(Unit.Value, "Register successfully, please check your email");
    }

    private async Task ProduceConfirmAccountMailEvent(UserAccount newUser, string verifyAppCode)
    {
        var eventMessage = CreateEventBusMessage(newUser, verifyAppCode);
        await _messageProducer.PublishAsync(eventMessage, "Worker.Mailing.Send");
    }

    private SendMailEventBusMessage CreateEventBusMessage(UserAccount newUser, string verifyAppCode)
    {
        var eventModel = new ConfirmAccountMailModel
        {
            ConfirmUrl = _domainClientAppSetting.Url() + "/auth/confirm/me?appCode=" + verifyAppCode,
            ResendUrl = _domainClientAppSetting.Url() + "/auth/confirm/resend?account=" + newUser.Email
        };

        return new SendMailEventBusMessage
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
