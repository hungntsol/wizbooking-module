using System.Text.Json;
using EventBusMessage.Abstracts;
using EventBusMessage.Events;
using Identity.Domain.Common;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.Exceptions.StatusCodes._500;
using SharedCommon.MailingConstants;
using SharedCommon.MailingConstants.Models;
using SharedCommon.Modules.LoggerAdapter;
using JsonSerializerOptions = SharedCommon.Commons.JsonSerialization.JsonSerializerOptions;

namespace Identity.Application.Features.Commands.ForgetPassword;

public class ForgetPasswordCommandCommandHandler : IRequestHandler<ForgetPasswordCommand, JsonHttpResponse>
{
	private readonly AuthAppSetting _authAppSetting;
	private readonly DomainClientAppSetting _domainClientAppSetting;
	private readonly ILoggerAdapter<ForgetPasswordCommandCommandHandler> _logger;
	private readonly IMessageProducer _messageProducer;
	private readonly IUserAccountRepository _userAccountRepository;
	private readonly IVerifiedUrlRepository _verifiedUrlOnlyRepository;

	public ForgetPasswordCommandCommandHandler(IUserAccountRepository userAccountRepository,
		IVerifiedUrlRepository verifiedUrlRepository,
		ILoggerAdapter<ForgetPasswordCommandCommandHandler> logger,
		IMessageProducer messageProducer,
		IOptions<DomainClientAppSetting> domainClientAppSettingOptions,
		IOptions<AuthAppSetting> authAppSettingOption)
	{
		_userAccountRepository = userAccountRepository;
		_verifiedUrlOnlyRepository = verifiedUrlRepository;
		_logger = logger;
		_messageProducer = messageProducer;
		_domainClientAppSetting = domainClientAppSettingOptions.Value;
		_authAppSetting = authAppSettingOption.Value;
	}

	public async Task<JsonHttpResponse> Handle(ForgetPasswordCommand request,
		CancellationToken cancellationToken)
	{
		var existingAccount =
			await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
		ArgumentNullException.ThrowIfNull(existingAccount, "account is not existing");

		var newVerifyUrl = VerifiedUrl.New(request.Email,
			VerifiedUrlTargetConstant.ResetAccount,
			TimeSpan.FromMinutes(_authAppSetting.ResetLinkExpiredMinutes));

		try
		{
			var addVerifyUrl =
				await _verifiedUrlOnlyRepository.InsertAsync(newVerifyUrl, cancellationToken: cancellationToken);

			// send event to send email
			var resetAccountMailModel = CreateResetAccountMailModel(newVerifyUrl);
			var recoverPasswordEventBusMessage =
				CreateRecoverPasswordEventBusMessage(addVerifyUrl, resetAccountMailModel);

			await _messageProducer.PublishAsync(recoverPasswordEventBusMessage, "Worker.Mailing.Send");

			return JsonHttpResponse.Success(Unit.Value, "Please check your email for recover key");
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "{Message}", e.Message);
			throw new InternalServerException();
		}
	}

	/// <summary>
	/// Create reset account model for mail worker
	/// Contains reset url
	/// </summary>
	/// <param name="newVerifyUrl"></param>
	/// <returns></returns>
	private ResetAccountMailModel CreateResetAccountMailModel(VerifiedUrl newVerifyUrl)
	{
		var resetAccountMailModel =
			new ResetAccountMailModel(
				$"{_domainClientAppSetting.Url()}/auth/recover?appCode={newVerifyUrl.AppCode}");
		return resetAccountMailModel;
	}

	/// <summary>
	/// Create recover event for reset account action
	/// </summary>
	/// <param name="addVerifyUrl"></param>
	/// <param name="resetAccountMailModel"></param>
	/// <returns></returns>
	private static SendMailEventBusMessage CreateRecoverPasswordEventBusMessage(VerifiedUrl? addVerifyUrl,
		ResetAccountMailModel resetAccountMailModel)
	{
		var recoverPasswordEventBusMessage = new SendMailEventBusMessage
		{
			From = "WizBooking <noreply-auth@wizbooking.com>",
			To = addVerifyUrl!.Email,
			Subject = "Recovery password",
			TemplateName = EmailTemplateConstants.ResetAccountMail,
			TemplateModel =
				JsonSerializer.Serialize(resetAccountMailModel, JsonSerializerOptions.DefaultOptions),
			Attachments = default
		};
		return recoverPasswordEventBusMessage;
	}
}
