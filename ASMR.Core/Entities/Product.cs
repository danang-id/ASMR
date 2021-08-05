//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 3:37 PM
//
// Product.cs
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class Product : DefaultAbstractEntity
    {
        [Required]
        [JsonIgnore]
        public Bean Bean { get; set; }

        [Required]
        public string BeanId { get; set; }

        [Required]
        public int CurrentInventoryQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(22,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,3)")]
        public decimal WeightPerPackaging { get; set; }
    }
}
