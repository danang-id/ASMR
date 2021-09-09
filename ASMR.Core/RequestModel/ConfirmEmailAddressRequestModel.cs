using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
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
}