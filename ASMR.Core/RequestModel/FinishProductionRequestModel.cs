using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class FinishProductionRequestModel
{
	[Required(ErrorMessage = "Please fill in the roasted bean weight.")]
	public decimal RoastedBeanWeight { get; set; }
}