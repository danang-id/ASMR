    //
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 3:51 PM
//
// Transaction.cs
//
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class Transaction : DefaultAbstractEntity
    {
        public IEnumerable<TransactionItem> Items { get; set; }

        [Required]
        public Payment Payment { get; set; }

        [Required]
        [JsonIgnore]
        public string PaymentId { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        public User Server { get; set; }

        [Required]
        [JsonIgnore]
        public string ServerId { get; set; }
    }
}
