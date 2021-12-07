//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// Roasting.cs
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class Roasting : DefaultAbstractEntity
{
	[Required] [JsonIgnore] public Bean Bean { get; set; }

	[Required] public string BeanId { get; set; }

	[Required] [JsonIgnore] public User User { get; set; }

	[Required] public string UserId { get; set; }

	[Required]
	[Column(TypeName = "decimal(8,3)")]
	public decimal GreenBeanWeight { get; set; }

	[Required]
	[Column(TypeName = "decimal(8,3)")]
	public decimal RoastedBeanWeight { get; set; }

	public DateTimeOffset? CancelledAt { get; set; }

	[Required] public RoastingCancellationReason CancellationReason { get; set; }

	public DateTimeOffset? FinishedAt { get; set; }
}