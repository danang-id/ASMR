//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 3:32 PM
//
// Production.cs
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class RoastedBeanProduction : DefaultAbstractEntity
    {
        [Required]
        [JsonIgnore]
        public Bean Bean { get; set; }

        [Required]
        public string BeanId { get; set; }

        [Required]
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,3)")]
        public decimal GreenBeanWeight { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(8,3)")]
        public decimal RoastedBeanWeight { get; set; }

        [Required]
        public bool IsCancelled { get; set; }
        
        [Required]
        public bool IsFinalized { get; set; }
    }
}
