//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:04 AM
//
// User.cs
//
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
    public class User : DefaultAbstractEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [JsonIgnore]
        public string HashedPassword { get; set; }

        public IEnumerable<UserRole> Roles { get; set; } 
    }
}
