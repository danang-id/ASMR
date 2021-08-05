using ASMR.Core.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASMR.Core.Entities
{
    public class IncomingGreenBean : DefaultAbstractEntity
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
        public decimal WeightAdded { get; set; }
    }
}
