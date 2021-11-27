//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 01:51 PM
//
// GateController.cs
//

using System;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Web.Controllers.Attributes;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Extensions;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASMR.Web.Controllers.API;

public class GateController : DefaultAbstractApiController<GateController>
{
	private readonly ICaptchaService _captchaService;
	private readonly IEmailService _emailService;
	private readonly IMediaFileService _mediaFileService;
	private readonly ITokenService _tokenService;
	private readonly IUserService _userService;
	private readonly SignInManager<User> _signInManager;

	public GateController(ILogger<GateController> logger,
		ICaptchaService captchaService,
		IEmailService emailService,
		IMediaFileService mediaFileService,
		ITokenService tokenService,
		IUserService userService,
		SignInManager<User> signInManager) : base(logger)
	{
		_captchaService = captchaService;
		_emailService = emailService;
		_mediaFileService = mediaFileService;
		_tokenService = tokenService;
		_userService = userService;
		_signInManager = signInManager;
	}

	[ClientPlatform(ClientPlatform.Web)]
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromForm] RegistrationRequestModel model,
		[FromQuery] string captchaResponseToken)
	{
		var clientPlatform = Request.GetClientPlatform();
		if (clientPlatform == ClientPlatform.Web)
		{
			if (string.IsNullOrEmpty(captchaResponseToken))
			{
				var errorModel = new ResponseError(ErrorCodeConstants.CaptchaResponseTokenNotProvided,
					"Please verify that you are not a robot.");
				return BadRequest(new AuthenticationResponseModel(errorModel));
			}

			var captchaVerified = await _captchaService.VerifyCaptcha(captchaResponseToken);
			if (!captchaVerified)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.CaptchaVerificationFailed,
					"CAPTCHA verification failed. Are you a robot?");
				return BadRequest(new AuthenticationResponseModel(errorModel));
			}
		}

		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var existingUserByName = await _userService.GetUserByName(model.Username);
		if (existingUserByName is not null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				$"User with username '{model.Username}' is already exist.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var existingUserByEmailAddress = await _userService.GetUserByEmailAddress(model.EmailAddress);
		if (existingUserByEmailAddress is not null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				$"User with email address '{model.EmailAddress}' is already exist.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var formCollection = await Request.ReadFormAsync();
		var formFile = formCollection.Files.GetFile("image");
		if (formFile is null || formFile.Length <= 0)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please choose a user image.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		if (formFile.Length > 2 * 1024 * 1024)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The user image file size must be under 2 MB.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		if (!formFile.IsImage(Logger))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The file you uploaded is not an image.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var userId = Guid.NewGuid().ToString();
		var mediaFile = await _mediaFileService.CreateMediaFile(userId, formFile);

		var user = new User
		{
			Id = userId,
			FirstName = model.FirstName,
			LastName = model.LastName,
			Email = model.EmailAddress,
			EmailConfirmed = false,
			IsWaitingForApproval = true,
			IsApproved = true,
			UserName = model.Username,
			Image = $"/api/mediafile/{mediaFile.Id}"
		};
		var createUserResult = await _userService.CreateUser(user, model.Password);
		if (!createUserResult.Succeeded && createUserResult.Errors.Any())
		{
			var errorModels = createUserResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new AuthenticationResponseModel(errorModels));
		}

		user = await _userService.GetUserById(userId);
		var emailAddressConfirmationUrl = await _userService
			.GenerateEmailAddressConfirmationUrl(user, Request.GetBaseUrl());
		var mailError = await _emailService.SendEmailAddressConfirmationMailAsync(user, emailAddressConfirmationUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new AuthenticationResponseModel(errorModel));
		}

		var upsertContactError = await _emailService.UpsertContactAsync(user);
		if (upsertContactError is not null)
		{
			Logger.LogError(upsertContactError.SendGridErrorMessage);
		}

		await _userService.CommitAsync();

		return Ok(new AuthenticationResponseModel()
		{
			Message = "Thank you! Please check your email inbox and follow the instruction we sent you."
		});
	}

	[HttpPost("email-address/resend-confirmation")]
	public async Task<IActionResult> ResendEmailAddressConfirmation(
		[FromBody] ResendEmailAddressConfirmationRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var user = await _userService.GetUserByName(model.Username);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
				"Operation failed, please check your username and password.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var checkSignInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
		if (!checkSignInResult.Succeeded)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
				"Operation failed, please check your username and password.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var emailAddressConfirmationUrl = await _userService
			.GenerateEmailAddressConfirmationUrl(user, Request.GetBaseUrl());
		var mailError = await _emailService.SendEmailAddressConfirmationMailAsync(user, emailAddressConfirmationUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new AuthenticationResponseModel(errorModel));
		}

		return Ok(new AuthenticationResponseModel
		{
			Message = $"A confirmation email has been sent to '{user.Email}'. " +
			          "Please check your email inbox."
		});
	}

	[ClientPlatform(ClientPlatform.Web)]
	[HttpPost("email-address/confirm")]
	public async Task<IActionResult> ConfirmEmailAddress([FromBody] ConfirmEmailAddressRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var user = await _userService.GetUserById(model.Id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user which email address you are trying to confirm is not found.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		if (user.Email != model.EmailAddress)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Your email address confirmation link is invalid.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var confirmEmailResult = await _userService.ConfirmEmailAddress(user, model.Token);
		if (!confirmEmailResult.Succeeded)
		{
			var errorModels = confirmEmailResult.Errors
				.Select(error =>
				{
					if (error.Description == "Invalid token.")
					{
						error.Description = "Your email address confirmation link is invalid.";
					}

					return error;
				})
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new AuthenticationResponseModel(errorModels));
		}

		if (user.IsWaitingForApproval)
		{
			var mailError = await _emailService.SendRegistrationPendingApprovalMailAsync(user);
			if (mailError is not null)
			{
				Logger.LogError(mailError.SendGridErrorMessage);
			}
		}

		return Ok(new AuthenticationResponseModel()
		{
			Message = "Your email address has been confirmed."
		});
	}

	[HttpPost("authenticate")]
	public async Task<IActionResult> Authenticate([FromBody] SignInRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var user = await _userService.GetUserByName(model.Username);
		if (user is null)
		{
			var userNotFoundErrorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
				"Sign in failed, please check your username and password.");
			return BadRequest(new AuthenticationResponseModel(userNotFoundErrorModel));
		}

		var userRoles = (await _userService.GetUserRoles(user))
			.ToList();

		var checkSignInResult = await _signInManager
			.CheckPasswordSignInAsync(user, model.Password, false);
		if (checkSignInResult.Succeeded)
		{
			if (!user.EmailConfirmed)
			{
				var waitingEmailConfirmationErrorModel = new ResponseError(
					ErrorCodeConstants.EmailAddressWaitingConfirmation,
					"Your account is not active because you have not confirm your email address.");
				return BadRequest(new AuthenticationResponseModel(waitingEmailConfirmationErrorModel));
			}

			if (user.IsWaitingForApproval)
			{
				var waitingForApprovalErrorModel = new ResponseError(ErrorCodeConstants.AccountWaitingForApproval,
					"Your account registration is waiting for approval by our administrator.");
				return BadRequest(new AuthenticationResponseModel(waitingForApprovalErrorModel));
			}

			if (!user.IsApproved)
			{
				var inactiveAccountErrorModel = new ResponseError(ErrorCodeConstants.AccountWasNotApproved,
					"Your account registration was rejected by administrator. Please contact our administrator.");
				return BadRequest(new AuthenticationResponseModel(inactiveAccountErrorModel));
			}

			var signInResult = await _signInManager
				.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
			if (!signInResult.Succeeded)
			{
				var signInErrorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
					"Sign in failed, please check your username and password.");
				return BadRequest(new AuthenticationResponseModel(signInErrorModel));
			}

			var normalizedUser = _tokenService.BuildToken(user, userRoles);
			return Ok(new AuthenticationResponseModel(normalizedUser));
		}

		if (checkSignInResult.IsLockedOut)
		{
			var lockedOutErrorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
				"We are sorry but your account is being locked because we detected some " +
				"suspicious sign inactivity on your account.");
			return BadRequest(new AuthenticationResponseModel(lockedOutErrorModel));
		}

		if (checkSignInResult.IsNotAllowed)
		{
			var notAllowedErrorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
				"Your account has not been confirmed. Please confirm your account to be able to sign in.");
			return BadRequest(new AuthenticationResponseModel(notAllowedErrorModel));
		}

		if (checkSignInResult.RequiresTwoFactor)
		{
			var requiresTwoFactorErrorModel = new ResponseError(ErrorCodeConstants.RequiresTwoFactor,
				"Two Factor authentication required.");
			return BadRequest(new AuthenticationResponseModel(requiresTwoFactorErrorModel));
		}

		var errorModel = new ResponseError(ErrorCodeConstants.AuthenticationFailed,
			"Sign in failed, please check your username and password.");
		return BadRequest(new AuthenticationResponseModel(errorModel));
	}

	[HttpPost("exit")]
	public async Task<IActionResult> ClearSession()
	{
		await _signInManager.SignOutAsync();

		return Ok(new AuthenticationResponseModel());
	}

	[HttpPost("password/forget")]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestModel model,
		[FromQuery] string captchaResponseToken)
	{
		var clientPlatform = Request.GetClientPlatform();
		if (clientPlatform == ClientPlatform.Web)
		{
			if (string.IsNullOrEmpty(captchaResponseToken))
			{
				var errorModel = new ResponseError(ErrorCodeConstants.CaptchaResponseTokenNotProvided,
					"Please verify that you are not a robot.");
				return BadRequest(new AuthenticationResponseModel(errorModel));
			}

			var captchaVerified = await _captchaService.VerifyCaptcha(captchaResponseToken);
			if (!captchaVerified)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.CaptchaVerificationFailed,
					"CAPTCHA verification failed. Are you a robot?");
				return BadRequest(new AuthenticationResponseModel(errorModel));
			}
		}

		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var user = await _userService.GetUserByEmailAddress(model.EmailAddress);
		if (user is null)
		{
			return Ok(new AuthenticationResponseModel());
		}

		if (!user.EmailConfirmed)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.EmailAddressWaitingConfirmation,
				"Your account is not active because you have not confirm your email address.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var passwordResetUrl = await _userService.GeneratePasswordResetUrl(user, Request.GetBaseUrl());
		var mailError = await _emailService.SendPasswordResetMailAsync(user, passwordResetUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new AuthenticationResponseModel(errorModel));
		}

		return Ok(new AuthenticationResponseModel());
	}

	[ClientPlatform(ClientPlatform.Web)]
	[HttpPost("password/reset")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		var user = await _userService.GetUserById(model.Id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user you are trying to password reset is not found.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		if (user.Email != model.EmailAddress)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Your reset password link is invalid.");
			return BadRequest(new AuthenticationResponseModel(errorModel));
		}

		var resetPasswordResult = await _userService.ResetUserPassword(user.Id, model.Password, model.Token);
		if (resetPasswordResult.Succeeded)
		{
			var errorModels = resetPasswordResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new AuthenticationResponseModel(errorModels));
		}

		return Ok(new AuthenticationResponseModel()
		{
			Message = "Your password has been reset."
		});
	}

	[Authorize]
	[HttpGet("passport")]
	public async Task<IActionResult> GetUserPassport()
	{
		var user = await _userService.GetAuthenticatedUser(User);
		var userRoles = await _userService.GetUserRoles(user);

		return Ok(new AuthenticationResponseModel(user.SanitizeUserWithToken(string.Empty, userRoles)));
	}
}