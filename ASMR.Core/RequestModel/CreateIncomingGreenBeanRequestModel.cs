//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// CreateIncomingGreenBeanRequestModel.cs
//

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class CreateIncomingGreenBeanRequestModel
{
	[Required(ErrorMessage = "Please specify the green bean weight to be added to inventory.")]
	public decimal Weight { get; set; }
}