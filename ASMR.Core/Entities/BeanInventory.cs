using ASMR.Core.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASMR.Core.Entities;

public class BeanInventory : DefaultAbstractEntity
{
	[JsonIgnore] public Bean Bean { get; set; }

	[Required]
	[Column(TypeName = "decimal(8,3)")]
	public decimal CurrentGreenBeanWeight { get; set; }

	[Required]
	[Column(TypeName = "decimal(8,3)")]
	public decimal CurrentRoastedBeanWeight { get; set; }
}