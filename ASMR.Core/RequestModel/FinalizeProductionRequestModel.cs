using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
	public class FinalizeProductionRequestModel
	{
		[Required(ErrorMessage = "Please fill in the roasted bean weight.")]
		public decimal RoastedBeanWeight { get; set; }
	}
}