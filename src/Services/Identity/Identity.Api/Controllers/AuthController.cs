﻿using Identity.Application.Features.Commands.ChangePassword;
using Identity.Application.Features.Commands.ForgetPassword;
using Identity.Application.Features.Commands.Login;
using Identity.Application.Features.Commands.Register;
using Identity.Application.Features.Commands.ResetPassword;
using Identity.Application.Features.Queries.ConfirmAccount;
using Identity.Application.Features.Queries.ResendConfirm;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class AuthController : ApiV1ControllerBase
{
	public AuthController(IMediator mediator) : base(mediator)
	{
	}

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register([FromBody] RegisterNewAccountCommand command)
	{
		return JsonResponse(await Mediator.Send(command));
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginCommand command)
	{
		return JsonResponse(await Mediator.Send(command));
	}

	[HttpPost("update-password")]
	[Authorize]
	public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
	{
		return JsonResponse(await Mediator.Send(command));
	}

	[HttpPost("forget-password")]
	[AllowAnonymous]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command)
	{
		return JsonResponse(await Mediator.Send(command));
	}

	[HttpPost("reset-password")]
	[AllowAnonymous]
	public async Task<IActionResult> ResetPassword([FromBody] ResetAccountCommand command)
	{
		return JsonResponse(await Mediator.Send(command));
	}

	[HttpGet("/auth/confirm/me")]
	[AllowAnonymous]
	public async Task<IActionResult> ConfirmAccount([FromQuery] ConfirmAccountQuery query)
	{
		return JsonResponse(await Mediator.Send(query));
	}

	[HttpGet("/auth/confirm/resend")]
	[AllowAnonymous]
	public async Task<IActionResult> ResendConfirmMail([FromQuery] string account)
	{
		return JsonResponse(await Mediator.Send(new ResendMailConfirmAccountQuery(account)));
	}
}
