using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel
{
	public class StartProductionRequestModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please choose a bean.")]
		[DataType(DataType.Text)]
		public string BeanId { get; set; }
		
        [Required(ErrorMessage = "Please fill in the green bean weight.")]
        public decimal GreenBeanWeight { get; set; }
	}
}