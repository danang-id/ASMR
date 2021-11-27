//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 4:19 PM
//
// CreateBeanRequestModel.cs
//

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class CreateBeanRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the bean name.")]
	[DataType(DataType.Text)]
	public string Name { get; set; }

	[Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the bean description.")]
	[DataType(DataType.Text)]
	public string Description { get; set; }
}