//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// CreatePackagingRequestModel.cs
// 

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class PackagingResultRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Please choose the resulting product.")]
	[DataType(DataType.Text)]
	public string ProductId { get; set; }

	[Required(ErrorMessage = "Please specify how many product you have packaged.")]
	public int Quantity { get; set; }
}

public class CreatePackagingRequestModel
{
	[Required(AllowEmptyStrings = false, ErrorMessage = "Please choose a bean.")]
	[DataType(DataType.Text)]
	public string BeanId { get; set; }

	[Required(ErrorMessage = "Please submit the packaging results.")]
	public IEnumerable<PackagingResultRequestModel> Results { get; set; }
}