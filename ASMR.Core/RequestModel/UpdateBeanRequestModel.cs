//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// UpdateBeanRequestModel.cs
//

using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.RequestModel;

public class UpdateBeanRequestModel
{
	[DataType(DataType.Text)] public string Name { get; set; }

	[DataType(DataType.Text)] public string Description { get; set; }
}