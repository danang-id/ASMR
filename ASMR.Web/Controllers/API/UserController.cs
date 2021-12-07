//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// UserController.cs
//

using System;
using System.Collections.ObjectModel;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable ReplaceWithSingleCallToAny
namespace ASMR.Web.Controllers.API;

[AllowAccess(Role.Administrator)]
[ClientPlatform(ClientPlatform.Web)]
public class UserController : DefaultAbstractApiController<UserController>
{
	private readonly IEmailService _emailService;
	private readonly IMediaFileService _mediaFileService;
	private readonly IUserService _userService;
	private readonly SignInManager<User> _signInManager;

	public UserController(
		ILogger<UserController> logger,
		IEmailService emailService,
		IMediaFileService mediaFileService,
		IUserService userService,
		SignInManager<User> signInManager) : base(logger)
	{
		_emailService = emailService;
		_mediaFileService = mediaFileService;
		_userService = userService;
		_signInManager = signInManager;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var users = new Collection<SanitizedUser>();
		foreach (var user in _userService.GetAllUsers())
		{
			var userRoles = await _userService.GetUserRoles(user);
			users.Add(user.SanitizeUser(userRoles));
		}

		return Ok(new UsersResponseModel(users));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a user");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user you are looking for is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var userRoles = await _userService.GetUserRoles(user);

		return Ok(new UserResponseModel(user.SanitizeUser(userRoles)));
	}

	[HttpPost, DisableRequestSizeLimit]
	public async Task<IActionResult> Create([FromForm] CreateUserRequestModel model)
	{
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
			return BadRequest(new UserResponseModel(errorModel));
		}

		var existingUserByEmailAddress = await _userService.GetUserByEmailAddress(model.EmailAddress);
		if (existingUserByEmailAddress is not null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				$"User with email address '{model.EmailAddress}' is already exist.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (model.Roles is null || !model.Roles.Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please assign minimal a role.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (model.Roles.Where(role => role == Role.Administrator).Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Assigning Administrator role is not allowed.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var formCollection = await Request.ReadFormAsync();
		var formFile = formCollection.Files.GetFile("image");
		if (formFile is null || formFile.Length <= 0)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please choose an image for the user.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (formFile.Length > 2 * 1024 * 1024)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The user image file size must be under 2 MB.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (!formFile.IsImage(Logger))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"The file you uploaded is not an image.");
			return BadRequest(new UserResponseModel(errorModel));
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
			IsWaitingForApproval = false,
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
			return BadRequest(new UserResponseModel(errorModels));
		}

		var assignRolesResult = await _userService.AssignRolesToUser(user.Id, model.Roles);
		if (!assignRolesResult.Succeeded && assignRolesResult.Errors.Any())
		{
			var errorModels = assignRolesResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		user = await _userService.GetUserById(userId);
		var userRoles = (await _userService.GetUserRoles(user))
			.ToList();

		var emailAddressConfirmationUrl = await _userService
			.GenerateEmailAddressConfirmationUrl(user, Request.GetBaseUrl());
		var mailError = await _emailService.SendEmailAddressConfirmationMailAsync(user, emailAddressConfirmationUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new UserResponseModel(errorModel));
		}

		var upsertContactError = await _emailService.UpsertContactAsync(user);
		if (upsertContactError is not null)
		{
			Logger.LogError(upsertContactError.SendGridErrorMessage);
		}

		await _userService.CommitAsync();

		return Created(Request.Path, new UserResponseModel(user.SanitizeUser(userRoles))
		{
			Message = $"Successfully registered '{user.FirstName} {user.LastName}' as new user."
		});
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> Modify(string id, [FromForm] UpdateUserRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a user.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user you are trying to modify is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (!string.IsNullOrEmpty(model.Username) && user.UserName != model.Username)
		{
			var existingUser = await _userService.GetUserByName(model.Username);
			if (existingUser is not null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					$"User with username '{model.Username}' is already exist.");
				return BadRequest(new UserResponseModel(errorModel));
			}
		}

		if (!string.IsNullOrEmpty(model.EmailAddress) && user.Email != model.EmailAddress)
		{
			var existingUser = await _userService.GetUserByEmailAddress(model.EmailAddress);
			if (existingUser is not null)
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					$"User with email address '{model.EmailAddress}' is already exist.");
				return BadRequest(new UserResponseModel(errorModel));
			}
		}

		var modelHasRoles = model.Roles is not null && model.Roles.Any();
		var modelRoleIncludesAdministrator =
			modelHasRoles && model.Roles.Where(role => role == Role.Administrator).Any();
		var userIsAdministrator = await _userService.HasRole(user, Role.Administrator);

		if (!modelHasRoles)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please assign minimal a role.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (modelRoleIncludesAdministrator && !userIsAdministrator)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Assigning Administrator role is not allowed.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		MediaFile mediaFile = null;
		var formCollection = await Request.ReadFormAsync();
		var formFile = formCollection.Files.GetFile("image");
		if (formFile is not null)
		{
			switch (formFile.Length)
			{
				case <= 0:
				{
					var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
						"Please choose an image for the user.");
					return BadRequest(new ProductResponseModel(errorModel));
				}
				case > 2 * 1024 * 1024:
				{
					var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
						"The user image file size must be under 2 MB.");
					return BadRequest(new ProductResponseModel(errorModel));
				}
			}

			if (!formFile.IsImage(Logger))
			{
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					"The file you uploaded is not an image.");
				return BadRequest(new ProductResponseModel(errorModel));
			}

			mediaFile = await _mediaFileService.CreateMediaFile(user.Id, formFile);
		}

		var oldEmailAddress = user.Email;
		var modelHasEmailAddress = !string.IsNullOrEmpty(model.EmailAddress);
		var emailAddressModified = modelHasEmailAddress && model.EmailAddress != oldEmailAddress;
		var oldMediaFileId = user.Image.Split("/")
			.LastOrDefault();

		var modifyUserResult = await _userService.ModifyUser(id, new User
		{
			FirstName = model.FirstName,
			LastName = model.LastName,
			Email = model.EmailAddress,
			UserName = model.Username,
			Image = mediaFile is null ? null : $"/api/mediafile/{mediaFile.Id}"
		});
		if (!modifyUserResult.Succeeded && modifyUserResult.Errors.Any())
		{
			var errorModels = modifyUserResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		var assignRolesResult = await _userService.AssignRolesToUser(id, model.Roles);
		if (!assignRolesResult.Succeeded && assignRolesResult.Errors.Any())
		{
			var errorModels = assignRolesResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		if (emailAddressModified)
		{
			var emailAddressChangedError = await _emailService.SendEmailAddressChangedMailAsync(user, oldEmailAddress);
			if (emailAddressChangedError is not null)
			{
				Logger.LogError(emailAddressChangedError.SendGridErrorMessage);
				var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
					"This operation cannot be done at the moment. Please try again later.");
				return StatusCode(500, new UserResponseModel(errorModel));
			}

			var emailAddressConfirmationUrl = await _userService
				.GenerateEmailAddressConfirmationUrl(user, Request.GetBaseUrl());
			var emailAddressConfirmationError = await _emailService.SendEmailAddressConfirmationMailAsync(user,
				emailAddressConfirmationUrl);
			if (emailAddressConfirmationError is not null)
			{
				Logger.LogError(emailAddressConfirmationError.SendGridErrorMessage);
				var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
					"This operation cannot be done at the moment. Please try again later.");
				return StatusCode(500, new UserResponseModel(errorModel));
			}

			var deleteContactError = await _emailService.DeleteContactAsync(oldEmailAddress);
			if (deleteContactError is not null)
			{
				Logger.LogError(deleteContactError.SendGridErrorMessage);
			}

			var upsertContactError = await _emailService.UpsertContactAsync(user);
			if (upsertContactError is not null)
			{
				Logger.LogError(upsertContactError.SendGridErrorMessage);
			}
		}

		user = await _userService.GetUserById(id);

		var authenticatedUser = await _userService.GetAuthenticatedUser(User);
		if (user is not null && user.Id == authenticatedUser.Id)
		{
			await _signInManager.SignOutAsync();
			await _signInManager.SignInAsync(user, true);
		}

		if (mediaFile is not null && string.IsNullOrEmpty(oldMediaFileId))
		{
			await _mediaFileService.RemoveMediaFile(oldMediaFileId);
		}

		var userRoles = await _userService.GetUserRoles(user);

		await _userService.CommitAsync();
		return Ok(new UserResponseModel(user?.SanitizeUser(userRoles))
		{
			Message = $"Successfully saved profile information of '{user?.FirstName} {user?.LastName}'."
		});
	}

	[HttpPost("{id}/password-reset")]
	public async Task<IActionResult> ResetPassword(string id)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a user.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user you are trying to password reset is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (!user.EmailConfirmed)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.EmailAddressWaitingConfirmation,
				"Failed to send password reset instruction because the email address " +
				"has not been confirmed.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var passwordResetUrl = await _userService.GeneratePasswordResetUrl(user, Request.GetBaseUrl());
		var mailError = await _emailService.SendPasswordResetMailAsync(user, passwordResetUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new UserResponseModel(errorModel));
		}

		return Ok(new UserResponseModel()
		{
			Message = $"An instruction to reset account password has been sent to '{user.Email}'."
		});
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Remove(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select a user.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The user you are trying to remove is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var removeUserResult = await _userService.RemoveUser(id);
		if (!removeUserResult.Succeeded && removeUserResult.Errors.Any())
		{
			var errorModels = removeUserResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		var mediaFileId = user.Image.Split("/").LastOrDefault();
		if (mediaFileId is not null)
		{
			await _mediaFileService.RemoveMediaFile(mediaFileId);
		}

		var userRoles = await _userService.GetUserRoles(user);

		var deleteContactError = await _emailService.DeleteContactAsync(user.Email);
		if (deleteContactError is not null)
		{
			Logger.LogError(deleteContactError.SendGridErrorMessage);
		}

		await _userService.CommitAsync();
		return Ok(new UserResponseModel(user.SanitizeUser(userRoles))
		{
			Message = $"Successfully removed user '{user.FirstName} {user.LastName}'."
		});
	}

	[HttpPost("{id}/approve")]
	public async Task<IActionResult> Approve(string id, [FromBody] ApproveRegistrationRequestModel model)
	{
		var validationActionResult = GetValidationActionResult();
		if (validationActionResult is not null)
		{
			return validationActionResult;
		}

		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select account registration to be approved.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (model.Roles is null || !model.Roles.Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Please assign minimal a role.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		if (model.Roles.Where(role => role == Role.Administrator).Any())
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed,
				"Assigning Administrator role is not allowed.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The account registration you are trying to approve is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var assignRolesResult = await _userService.AssignRolesToUser(user.Id, model.Roles);
		if (!assignRolesResult.Succeeded && assignRolesResult.Errors.Any())
		{
			var errorModels = assignRolesResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		var modifyUserPasswordResult = await _userService.ApproveUser(id);
		if (!modifyUserPasswordResult.Succeeded && modifyUserPasswordResult.Errors.Any())
		{
			var errorModels = modifyUserPasswordResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		user = await _userService.GetUserById(id);
		var userRoles = (await _userService.GetUserRoles(user))
			.ToList();

		var role = string.Join(" and ", userRoles.Select(userRole => userRole.Name));
		var signInUrl = Request.GetBaseUrl().AppendPathSegments("dashboard");
		var mailError = await _emailService.SendWelcomeMailAsync(user, role, signInUrl);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new UserResponseModel(errorModel));
		}

		await _userService.CommitAsync();
		return Ok(new UserResponseModel(user.SanitizeUser(userRoles))
		{
			Message = $"Registration of user '{user.FirstName} {user.LastName}' has been approved."
		});
	}

	[HttpPost("{id}/reject")]
	public async Task<IActionResult> Reject(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please select account registration to be rejected.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var user = await _userService.GetUserById(id);
		if (user is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The account registration you are trying to reject is not found.");
			return BadRequest(new UserResponseModel(errorModel));
		}

		var modifyUserPasswordResult = await _userService.DisapproveUser(id);
		if (!modifyUserPasswordResult.Succeeded && modifyUserPasswordResult.Errors.Any())
		{
			var errorModels = modifyUserPasswordResult.Errors
				.Select(error => new ResponseError(ErrorCodeConstants.ModelValidationFailed,
					error.Description));
			return BadRequest(new UserResponseModel(errorModels));
		}

		user = await _userService.GetUserById(id);
		var userRoles = await _userService.GetUserRoles(user);

		var mailError = await _emailService.SendRegistrationRejectedMailAsync(user);
		if (mailError is not null)
		{
			Logger.LogError(mailError.SendGridErrorMessage);
			var errorModel = new ResponseError(ErrorCodeConstants.EmailSendingFailure,
				"This operation cannot be done at the moment. Please try again later.");
			return StatusCode(500, new UserResponseModel(errorModel));
		}

		await _userService.CommitAsync();
		return Ok(new UserResponseModel(user.SanitizeUser(userRoles))
		{
			Message = $"Registration of user '{user.FirstName} {user.LastName}' has been rejected."
		});
	}
}