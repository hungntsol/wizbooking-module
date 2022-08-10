﻿using EventBusMessage.Abstracts;
using Identity.Domain.Common;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.JsonSerialization;
using SharedCommon.Commons.Logger;
using SharedCommon.Commons.Mailing;
using SharedCommon.Commons.Mailing.Models;
using SharedEventBus.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Identity.Application.Features.Commands.ForgetPassword;

public class ForgetPasswordCommandCommandHandler : IRequestHandler<ForgetPasswordCommand, JsonHttpResponse<Unit>>
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IVerifiedUrlRepository _verifiedUrlRepository;
    private readonly ILoggerAdapter<ForgetPasswordCommandCommandHandler> _loggerAdapter;
    private readonly IMessageProducer _messageProducer;
    private readonly DomainClientAppSetting _domainClientAppSetting;

    private const ushort TimeLifeOfUrlFromMinutes = 3;

    public ForgetPasswordCommandCommandHandler(IUserAccountRepository userAccountRepository,
        IVerifiedUrlRepository verifiedUrlRepository,
        ILoggerAdapter<ForgetPasswordCommandCommandHandler> loggerAdapter,
        IMessageProducer messageProducer,
        IOptions<DomainClientAppSetting> domainClientAppSettingOptions)
    {
        _userAccountRepository = userAccountRepository;
        _verifiedUrlRepository = verifiedUrlRepository;
        _loggerAdapter = loggerAdapter;
        _messageProducer = messageProducer;
        _domainClientAppSetting = domainClientAppSettingOptions.Value;
    }

    public async Task<JsonHttpResponse<Unit>> Handle(ForgetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var existingAccount =
            await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
        ArgumentNullException.ThrowIfNull(existingAccount, "account is not existing");

        var newVerifyUrl = VerifiedUrl.New(request.Email,
            VerifiedUrlTargetConstant.ResetAccount,
            TimeSpan.FromMinutes(TimeLifeOfUrlFromMinutes));

        try
        {
            var addVerifyUrl = await _verifiedUrlRepository.InsertAsync(newVerifyUrl, cancellationToken);

            // send event to send email
            var resetAccountMailModel = NewResetAccountMailModel(newVerifyUrl);
            var recoverPasswordEventBusMessage = NewRecoverPasswordEventBusMessage(addVerifyUrl, resetAccountMailModel);
            await _messageProducer.PublishAsync(recoverPasswordEventBusMessage, "Worker.Mailing.Send");

            return JsonHttpResponse<Unit>.Ok(Unit.Value, "Please check your email for recover key");
        }
        catch (Exception e)
        {
            _loggerAdapter.LogCritical(e, "{Message}", e.Message);
            throw new InternalServerException();
        }
    }

    private ResetAccountMailModel NewResetAccountMailModel(VerifiedUrl newVerifyUrl)
    {
        var resetAccountMailModel =
            new ResetAccountMailModel(
                $"{_domainClientAppSetting.Url()}/auth/recover?appCode={newVerifyUrl.AppCode}");
        return resetAccountMailModel;
    }

    private static SendMailEventBusMessage NewRecoverPasswordEventBusMessage(VerifiedUrl? addVerifyUrl,
        ResetAccountMailModel resetAccountMailModel)
    {
        var recoverPasswordEventBusMessage = new SendMailEventBusMessage()
        {
            From = "WizBooking <noreply-auth@wizbooking.com>",
            To = addVerifyUrl!.Email,
            Subject = "Recovery password",
            TemplateName = EmailTemplateConstants.ResetAccountMail,
            TemplateModel =
                JsonSerializer.Serialize(resetAccountMailModel, PlatformJsonSerializerOptions.DefaultOptions),
            Attachments = default
        };
        return recoverPasswordEventBusMessage;
    }
}