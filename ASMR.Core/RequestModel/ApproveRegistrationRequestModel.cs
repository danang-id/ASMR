//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ApproveRegistrationRequestModel.cs
// 

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;

namespace ASMR.Core.RequestModel;

public class ApproveRegistrationRequestModel
{
	[Required(ErrorMessage = "Please assign minimal a role.")]
	public IEnumerable<Role> Roles { get; set; }
}