//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ConfirmEmailAddressRequestModel.cs
// 

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class ConfirmEmailAddressRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Parameter user identifier is not satisfied.")]
	[DataType(DataType.Text)]
	public string Id { get; set; }

	[Required(AllowEmptyStrings = false, ErrorMessage = "Parameter email address is not satisfied.")]
	[EmailAddress(ErrorMessage = "The email address you provided is not a valid email address.")]
	[DataType(DataType.Text)]
	public string EmailAddress { get; set; }

	[Required(AllowEmptyStrings = false, ErrorMessage = "Parameter confirmation token is not satisfied.")]
	[DataType(DataType.Text)]
	public string Token { get; set; }
}