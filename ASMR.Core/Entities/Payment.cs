//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/11/2021 10:23 AM
//
// Payment.cs
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class Payment : DefaultAbstractEntity
    {
		[Required]
		public PaymentMethod Method { get; set; }

		[Required]
		public PaymentStatus Status { get; set; }

		[Required]
		[Column(TypeName = "decimal(22,2)")]
		public decimal Amount { get; set; }
	}
}
