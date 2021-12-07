//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// UpdateUserRequestModel.cs
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;

namespace ASMR.Core.RequestModel;

public class UpdateUserRequestModel
{
	[DataType(DataType.Text)] public string FirstName { get; set; }

	[DataType(DataType.Text)] public string LastName { get; set; }

	[DataType(DataType.EmailAddress)]
	[EmailAddress(ErrorMessage = "The email address you provided is not a valid email address.")]
	public string EmailAddress { get; set; }

	[DataType(DataType.Text)]
	[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
	public string Username { get; set; }

	public IEnumerable<Role> Roles { get; set; }
}