//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ForgetPasswordRequestModel
//
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class ForgetPasswordRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in your email address.")]
	[EmailAddress(ErrorMessage = "The email address you provided is not a valid email address.")]
	[DataType(DataType.Text)]
	public string EmailAddress { get; set; }
}