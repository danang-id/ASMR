//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// FinishProductionRequestModel.cs
//

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class FinishProductionRequestModel
{
	[Required(ErrorMessage = "Please fill in the roasted bean weight.")]
	public decimal RoastedBeanWeight { get; set; }
}