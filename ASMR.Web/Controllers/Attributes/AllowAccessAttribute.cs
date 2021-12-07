//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// AllowAccessAttribute.cs
//

using System.Linq;
using ASMR.Core.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace ASMR.Web.Controllers.Attributes;

public class AllowAccessAttribute : AuthorizeAttribute
{
	public AllowAccessAttribute()
	{
	}

	public AllowAccessAttribute(params Role[] allowedRoles)
	{
		Roles = string.Join(",", allowedRoles.Select(role => role.ToString()));
	}

	public AllowAccessAttribute(string policy) : base(policy)
	{
	}
}