//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/11/2021 10:22 AM
//
// TransactionItem.cs
//

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class TransactionItem : DefaultAbstractEntity
{
	[Required] [JsonIgnore] public Transaction Transaction { get; set; }

	[Required] public string TransactionId { get; set; }

	[Required] [JsonIgnore] public Product Product { get; set; }

	[Required] public string ProductId { get; set; }

	[Required]
	[Column(TypeName = "decimal(8,3)")]
	public decimal Quantity { get; set; }

	[Required]
	[Column(TypeName = "decimal(22,2)")]
	public decimal Price { get; set; }
}