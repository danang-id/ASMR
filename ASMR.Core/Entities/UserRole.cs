//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/11/2021 10:28 AM
//
// UserRole.cs
//
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class UserRole : DefaultAbstractEntity
    {
        [Required]
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        public Role Role { get; set; }
    }
}
