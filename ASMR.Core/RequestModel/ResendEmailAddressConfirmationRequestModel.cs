//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ResendEmailAddressConfirmationRequestModel.cs
//

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class ResendEmailAddressConfirmationRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Parameter username is not satisfied.")]
	[DataType(DataType.Text)]
	public string Username { get; set; }

	[Required(AllowEmptyStrings = false, ErrorMessage = "Parameter password is not satifisfied.")]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}