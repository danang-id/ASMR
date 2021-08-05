using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
	public class CreateIncomingGreenBeanRequestModel
	{
        [Required(ErrorMessage = "Please specify the green bean weight to be added to inventory.")]
        public decimal Weight { get; set; }
	}
}