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

namespace Identity.Application.Features.Queries.ResendConfirm;

public class
	ResendMailConfirmAccountQueryHandler : IRequestHandler<ResendMailConfirmAccountQuery, JsonHttpResponse>
{
	private readonly AuthAppSetting _authAppSetting;
	private readonly DomainClientAppSetting _domainClientAppSetting;
	private readonly IEfCoreAtomicWork _efCoreAtomicWork;
	private readonly ILoggerAdapter<ResendMailConfirmAccountQueryHandler> _logger;
	private readonly IMessageProducer _messageProducer;
	private readonly IUserAccountRepository _userAccountRepository;
	private readonly IVerifiedUrlRepository _verifiedUrlOnlyRepository;

	public ResendMailConfirmAccountQueryHandler(IUserAccountRepository userAccountRepository,
		IVerifiedUrlRepository verifiedUrlRepository,
		ILoggerAdapter<ResendMailConfirmAccountQueryHandler> logger,
		IOptions<DomainClientAppSetting> domainClientAppSettingOptions,
		IOptions<AuthAppSetting> authAppSettingOptions,
		IMessageProducer messageProducer,
		IEfCoreAtomicWork efCoreAtomicWork)

	{
		_userAccountRepository = userAccountRepository;
		_verifiedUrlOnlyRepository = verifiedUrlRepository;
		_logger = logger;
		_messageProducer = messageProducer;
		_efCoreAtomicWork = efCoreAtomicWork;
		_domainClientAppSetting = domainClientAppSettingOptions.Value;
		_authAppSetting = authAppSettingOptions.Value;
	}

	public async Task<JsonHttpResponse> Handle(ResendMailConfirmAccountQuery request,
		CancellationToken cancellationToken)
	{
		var account = await _userAccountRepository.FindOneAsync(q => q.Email.Equals(request.Email), cancellationToken);
		ArgumentNullException.ThrowIfNull(account);

		var newVerifyUrl = VerifiedUrl.New(
			request.Email,
			VerifiedUrlTargetConstant.ConfirmEmail,
			TimeSpan.FromMinutes(_authAppSetting.ConfirmLinkExpiredMinutes));

		await using var transaction = await _efCoreAtomicWork.BeginTransactionAsync(cancellationToken);
		var added = await _verifiedUrlOnlyRepository.InsertAsync(newVerifyUrl, cancellationToken: cancellationToken);

		try
		{
			await ProduceConfirmAccountMailEvent(account, newVerifyUrl.AppCode);
			await transaction.CommitAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{Message}", e.Message);
			await transaction.RollbackAsync(cancellationToken);

			throw new InternalServerException();
		}

		return JsonHttpResponse.Success(Unit.Value);
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
			TemplateModel = JsonSerializer.Serialize(eventModel, JsonSerializerOptions.DefaultOptions),
			Attachments = default
		};
	}
}
