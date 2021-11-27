//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:04 AM
//
// User.cs
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Core.Entities;

public class User : IdentityUser
{
	public User()
	{
		CreatedAt = DateTimeOffset.Now;
	}

	[Required] public override string UserName { get; set; }

	[Required] public string FirstName { get; set; }

	[Required] public string LastName { get; set; }

	[Required] public string Image { get; set; }

	[Required] public bool IsApproved { get; set; }

	[Required] public bool IsWaitingForApproval { get; set; }

	[Required] public DateTimeOffset CreatedAt { get; set; }

	public DateTimeOffset? LastUpdatedAt { get; set; }

	public SanitizedUser SanitizeUser()
	{
		return new SanitizedUser
		{
			Id = Id,
			FirstName = FirstName,
			LastName = LastName,
			EmailAddress = Email,
			Username = UserName,
			Image = Image,
			IsEmailConfirmed = EmailConfirmed,
			IsApproved = IsApproved,
			IsWaitingForApproval = IsWaitingForApproval,
			CreatedAt = CreatedAt,
			LastUpdatedAt = LastUpdatedAt
		};
	}

	public SanitizedUser SanitizeUser(IEnumerable<UserRole> roles)
	{
		return new SanitizedUser
		{
			Id = Id,
			FirstName = FirstName,
			LastName = LastName,
			EmailAddress = Email,
			Username = UserName,
			Image = Image,
			IsEmailConfirmed = EmailConfirmed,
			IsApproved = IsApproved,
			IsWaitingForApproval = IsWaitingForApproval,
			Roles = roles.Select(role => role.SanitizeUserRole()),
			CreatedAt = CreatedAt,
			LastUpdatedAt = LastUpdatedAt
		};
	}

	public SanitizedUserWithToken SanitizeUserWithToken(string token)
	{
		return new SanitizedUserWithToken
		{
			Id = Id,
			FirstName = FirstName,
			LastName = LastName,
			EmailAddress = Email,
			Username = UserName,
			Image = Image,
			IsEmailConfirmed = EmailConfirmed,
			IsApproved = IsApproved,
			IsWaitingForApproval = IsWaitingForApproval,
			Token = token,
			CreatedAt = CreatedAt,
			LastUpdatedAt = LastUpdatedAt
		};
	}

	public SanitizedUserWithToken SanitizeUserWithToken(string token, IEnumerable<UserRole> roles)
	{
		return new SanitizedUserWithToken
		{
			Id = Id,
			FirstName = FirstName,
			LastName = LastName,
			EmailAddress = Email,
			Username = UserName,
			Image = Image,
			IsEmailConfirmed = EmailConfirmed,
			IsApproved = IsApproved,
			IsWaitingForApproval = IsWaitingForApproval,
			Roles = roles.Select(role => role.SanitizeUserRole()),
			Token = token,
			CreatedAt = CreatedAt,
			LastUpdatedAt = LastUpdatedAt
		};
	}
}