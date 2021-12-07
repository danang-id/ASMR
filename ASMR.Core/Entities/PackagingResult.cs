//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// PackagingResult.cs
//
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class PackagingResult : DefaultAbstractEntity
{
	[Required] [JsonIgnore] public Packaging Packaging { get; set; }

	[Required] [JsonIgnore] public string PackagingId { get; set; }

	[Required] public Product Product { get; set; }

	[Required] [JsonIgnore] public string ProductId { get; set; }

	[Required] public int Quantity { get; set; }
}