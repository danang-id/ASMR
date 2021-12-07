//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// UserService.cs
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Flurl;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Web.Services;

public interface IUserService : IServiceBase
{
	public IEnumerable<User> GetAllUsers();

	public Task<User> GetUserById(string id);

	public Task<User> GetUserByEmailAddress(string emailAddress);

	public Task<User> GetUserByName(string userName);

	public Task<IdentityResult> CreateUser(User user, string password);

	public Task<IdentityResult> ModifyUser(string id, User user);

	public Task<IdentityResult> ModifyUserPassword(string id, string currentPassword, string newPassword);

	public Task<IdentityResult> ResetUserPassword(string id, string newPassword, string resetPasswordToken = null);

	public Task<IdentityResult> RemoveUser(string id);

	public Task<IEnumerable<UserRole>> GetUserRoles(User user);

	public Task<bool> HasRole(User user, Role role);

	public Task<IdentityResult> AssignRolesToUser(string id, IEnumerable<Role> roles);

	public Task<User> GetAuthenticatedUser(ClaimsPrincipal principal);

	public Task<IdentityResult> ApproveUser(string id);

	public Task<IdentityResult> DisapproveUser(string id);

	public Task<IdentityResult> AddUserToWaitingList(string id);

	public Task<Url> GenerateEmailAddressConfirmationUrl(User user, Url baseUrl);

	public Task<Url> GeneratePasswordResetUrl(User user, Url baseUrl);

	public Task<IdentityResult> ConfirmEmailAddress(User user, string emailAddressConfirmationToken);
}

public class UserService : ServiceBase, IUserService
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<UserRole> _roleManager;

	public UserService(ApplicationDbContext dbContext,
		UserManager<User> userManager,
		RoleManager<UserRole> roleManager) : base(dbContext)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public IEnumerable<User> GetAllUsers()
	{
		return _userManager.Users;
	}

	public Task<User> GetUserById(string id)
	{
		return _userManager.FindByIdAsync(id);
	}

	public Task<User> GetUserByEmailAddress(string emailAddress)
	{
		return _userManager.FindByEmailAsync(emailAddress);
	}

	public Task<User> GetUserByName(string userName)
	{
		return _userManager.FindByNameAsync(userName);
	}

	public async Task<IdentityResult> CreateUser(User user, string password)
	{
		var result = await _userManager.CreateAsync(user, password);
		if (result.Succeeded)
		{
			await PopulateAnalytics(user);
		}

		return result;
	}

	public async Task<IdentityResult> ModifyUser(string id, User user)
	{
		var entity = await _userManager.FindByIdAsync(id);
		if (entity is null)
		{
			return null;
		}

		if (!string.IsNullOrEmpty(user.FirstName))
		{
			entity.FirstName = user.FirstName;
		}

		if (!string.IsNullOrEmpty(user.LastName))
		{
			entity.LastName = user.LastName;
		}

		if (!string.IsNullOrEmpty(user.Email))
		{
			entity.Email = user.Email;
			entity.EmailConfirmed = entity.Email == user.Email && entity.EmailConfirmed;
		}

		if (!string.IsNullOrEmpty(user.UserName))
		{
			entity.UserName = user.UserName;
		}

		if (!string.IsNullOrEmpty(user.Image))
		{
			entity.Image = user.Image;
		}

		return await _userManager.UpdateAsync(entity);
	}

	public async Task<IdentityResult> ModifyUserPassword(string id, string currentPassword, string newPassword)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return null;
		}

		return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
	}

	public async Task<IdentityResult> ResetUserPassword(string id, string newPassword, string passwordResetToken = null)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return null;
		}

		if (string.IsNullOrEmpty(passwordResetToken))
		{
			passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
		}

		return await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
	}

	public async Task<IdentityResult> RemoveUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return null;
		}

		var result = await _userManager.DeleteAsync(user);
		if (result.Succeeded)
		{
			RemoveAnalytics(user);
		}

		return result;
	}

	public async Task<IEnumerable<UserRole>> GetUserRoles(User user)
	{
		var userRoles = new Collection<UserRole>();
		if (user is null)
		{
			return userRoles;
		}

		foreach (var role in await _userManager.GetRolesAsync(user))
		{
			var userRole = await _roleManager.FindByNameAsync(role);
			userRoles.Add(userRole);
		}

		return userRoles;
	}

	public async Task<bool> HasRole(User user, Role role)
	{
		if (user is null)
		{
			return false;
		}

		return await _userManager.IsInRoleAsync(user, role.ToString());
	}

	public async Task<IdentityResult> AssignRolesToUser(string id, IEnumerable<Role> roles)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return null;
		}

		var userRoles = await _userManager.GetRolesAsync(user);
		var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
		if (!removeResult.Succeeded || removeResult.Errors.Any())
		{
			return removeResult;
		}

		return await _userManager.AddToRolesAsync(user, roles.Select(role => role.ToString()));
	}

	public Task<User> GetAuthenticatedUser(ClaimsPrincipal principal)
	{
		return principal is null ? null : _userManager.GetUserAsync(principal);
	}

	public async Task<IdentityResult> ApproveUser(string id)
	{
		var entity = await _userManager.FindByIdAsync(id);
		if (entity is null)
		{
			return null;
		}

		entity.IsApproved = true;
		entity.IsWaitingForApproval = false;

		return await _userManager.UpdateAsync(entity);
	}

	public async Task<IdentityResult> DisapproveUser(string id)
	{
		var entity = await _userManager.FindByIdAsync(id);
		if (entity is null)
		{
			return null;
		}

		entity.IsApproved = false;
		entity.IsWaitingForApproval = false;

		return await _userManager.UpdateAsync(entity);
	}

	public async Task<IdentityResult> AddUserToWaitingList(string id)
	{
		var entity = await _userManager.FindByIdAsync(id);
		if (entity is null)
		{
			return null;
		}

		entity.IsApproved = false;
		entity.IsWaitingForApproval = true;

		return await _userManager.UpdateAsync(entity);
	}

	public async Task<Url> GenerateEmailAddressConfirmationUrl(User user, Url baseUrl)
	{
		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		return baseUrl.AppendPathSegments("authentication", "email-address", "confirm")
			.SetQueryParams(new
			{
				id = user.Id,
				emailAddress = user.Email,
				token
			});
	}

	public async Task<Url> GeneratePasswordResetUrl(User user, Url baseUrl)
	{
		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		return baseUrl.AppendPathSegments("authentication", "password", "reset")
			.SetQueryParams(new
			{
				id = user.Id,
				emailAddress = user.Email,
				token
			});
	}

	public Task<IdentityResult> ConfirmEmailAddress(User user, string emailAddressConfirmationToken)
	{
		return _userManager.ConfirmEmailAsync(user, emailAddressConfirmationToken);
	}
}