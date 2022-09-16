using System.Text.Json;
using EventBusMessage.Abstracts;
using EventBusMessage.Events;
using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.SettingOptions;
using Microsoft.Extensions.Options;
using SharedCommon.Commons.Exceptions.StatusCodes._500;
using SharedCommon.MailingConstants;
using SharedCommon.MailingConstants.Models;
using JsonSerializerOptions = SharedCommon.Commons.JsonSerialization.JsonSerializerOptions;

namespace Identity.Application.Features.Commands.Register;

public sealed class
	RegisterNewAccountCommandHandler : IRequestHandler<RegisterNewAccountCommand, JsonHttpResponse>
{
	private readonly AuthAppSetting _authAppSetting;
	private readonly DomainClientAppSetting _domainClientAppSetting;
	private readonly IEfCoreAtomicWork _efCoreAtomicWork;
	private readonly IMessageProducer _messageProducer;
	private readonly IUserAccountRepository _userAccountRepository;
	private readonly IVerifiedUrlRepository _verifiedUrlOnlyRepository;

	public RegisterNewAccountCommandHandler(IUserAccountRepository userAccountRepository,
		IEfCoreAtomicWork<AppDbContext> efCoreAtomicWork,
		IMessageProducer messageProducer,
		IOptions<DomainClientAppSetting> domainClientAppSettingOption,
		IVerifiedUrlRepository verifiedUrlRepository,
		IOptions<AuthAppSetting> authAppSettingOptions)
	{
		_userAccountRepository = userAccountRepository;
		_efCoreAtomicWork = efCoreAtomicWork;
		_messageProducer = messageProducer;
		_verifiedUrlOnlyRepository = verifiedUrlRepository;
		_domainClientAppSetting = domainClientAppSettingOption.Value;
		_authAppSetting = authAppSettingOptions.Value;
	}

	public async Task<JsonHttpResponse> Handle(RegisterNewAccountCommand request,
		CancellationToken cancellationToken)
	{
		var existedUser = await _userAccountRepository.FindOneAsync(q => q.Email == request.Email, cancellationToken);
		if (existedUser is not null)
		{
			return JsonHttpResponse.Fail("Email has been used");
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

		// start tx
		await using var transaction = await _efCoreAtomicWork.BeginTransactionAsync(cancellationToken);

		await _userAccountRepository
			.InsertAsync(newUser, cancellationToken: cancellationToken);

		var produceEventMessageTask = ProduceConfirmAccountMailEvent(newUser, newVerifyUrl.AppCode);

		await _verifiedUrlOnlyRepository.InsertAsync(newVerifyUrl, cancellationToken: cancellationToken);

		try
		{
			await produceEventMessageTask.WaitAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			throw new InternalServerException();
		}

		return JsonHttpResponse.Success(Unit.Value, "Register successfully, please check your email");
	}

	/// <summary>
	/// Produce confirm account event
	/// </summary>
	/// <param name="newUser"></param>
	/// <param name="verifyAppCode"></param>
	/// <returns></returns>
	private async Task ProduceConfirmAccountMailEvent(UserAccount newUser, string verifyAppCode)
	{
		var eventMessage = CreateEventBusMessage(newUser, verifyAppCode);
		await _messageProducer.PublishAsync(eventMessage, "Worker.Mailing.Send");
	}

	/// <summary>
	/// Create new event for register action to confirm email address.
	/// </summary>
	/// <param name="newUser"></param>
	/// <param name="verifyAppCode"></param>
	/// <returns></returns>
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
