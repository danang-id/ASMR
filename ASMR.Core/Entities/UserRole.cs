//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// UserRole.cs
//
using System;
using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Core.Entities;

public class UserRole : IdentityRole
{
	public UserRole()
	{
		CreatedAt = DateTimeOffset.Now;
	}

	[Required] public DateTimeOffset CreatedAt { get; set; }

	public DateTimeOffset? LastUpdatedAt { get; set; }

	public SanitizedUserRole SanitizeUserRole()
	{
		return new SanitizedUserRole
		{
			Id = Id,
			Role = (Role)Enum.Parse(typeof(Role), Name),
			CreatedAt = CreatedAt,
			LastUpdatedAt = LastUpdatedAt
		};
	}
}