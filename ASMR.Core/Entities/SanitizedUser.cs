//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// SanitizedUSer.cs
//
using System.Collections.Generic;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class SanitizedUser : DefaultAbstractEntity
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string EmailAddress { get; set; }

	public string Username { get; set; }

	public string Image { get; set; }

	public bool IsEmailConfirmed { get; set; }

	public bool IsApproved { get; set; }

	public bool IsWaitingForApproval { get; set; }

	public IEnumerable<SanitizedUserRole> Roles { get; set; }
}