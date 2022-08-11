using System.Text.Json;
using EventBusMessage.Abstracts;
using Identity.Domain.Common;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.JsonSerialization;
using SharedCommon.Commons.Logger;
using SharedCommon.Commons.Mailing;
using SharedCommon.Commons.Mailing.Models;
using SharedEventBus.Events;

namespace Identity.Application.Features.Queries.ResendConfirm;

public class
    ResendMailConfirmAccountQueryHandler : IRequestHandler<ResendMailConfirmAccountQuery, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IVerifiedUrlRepository _verifiedUrlRepository;
    private readonly ILoggerAdapter<ResendMailConfirmAccountQueryHandler> _loggerAdapter;
    private readonly DomainClientAppSetting _domainClientAppSetting;
    private readonly AuthAppSetting _authAppSetting;
    private readonly IMessageProducer _messageProducer;
    private readonly IUnitOfWork _unitOfWork;

    public ResendMailConfirmAccountQueryHandler(IUserAccountRepository userAccountRepository,
        IVerifiedUrlRepository verifiedUrlRepository,
        ILoggerAdapter<ResendMailConfirmAccountQueryHandler> loggerAdapter,
        IOptions<DomainClientAppSetting> domainClientAppSettingOptions,
        IOptions<AuthAppSetting> authAppSettingOptions,
        IMessageProducer messageProducer,
        IUnitOfWork unitOfWork)

    {
        _userAccountRepository = userAccountRepository;
        _verifiedUrlRepository = verifiedUrlRepository;
        _loggerAdapter = loggerAdapter;
        _messageProducer = messageProducer;
        _unitOfWork = unitOfWork;
        _domainClientAppSetting = domainClientAppSettingOptions.Value;
        _authAppSetting = authAppSettingOptions.Value;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(ResendMailConfirmAccountQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
        ArgumentNullException.ThrowIfNull(account);

        var newVerifyUrl = VerifiedUrl.New(
            request.Email,
            VerifiedUrlTargetConstant.ConfirmEmail,
            TimeSpan.FromMinutes(_authAppSetting.ConfirmLinkExpiredMinutes));

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var added = await _verifiedUrlRepository.InsertAsync(newVerifyUrl, cancellationToken);

        try
        {
            await ProduceConfirmAccountMailEvent(account, newVerifyUrl.AppCode);
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

    private async Task ProduceConfirmAccountMailEvent(UserAccount newUser, string verifyAppCode)
    {
        var eventMessage = CreateEventBusMessage(newUser, verifyAppCode);
        await _messageProducer.PublishAsync(eventMessage, "Worker.Mailing.Send");
    }

    private SendMailEventBusMessage CreateEventBusMessage(UserAccount newUser, string verifyAppCode)
    {
        var eventModel = new ConfirmAccountMailModel()
        {
            ConfirmUrl = _domainClientAppSetting.Url() + "/auth/confirm/me?appCode=" + verifyAppCode,
            ResendUrl = _domainClientAppSetting.Url() + "/auth/confirm/resend?account=" + newUser.Email
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