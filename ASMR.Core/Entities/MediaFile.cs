//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/27/2021 8:28 PM
//
// MediaFile.cs
//
using ASMR.Core.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASMR.Core.Entities
{
    public class MediaFile : DefaultAbstractEntity
    {
        [Required]
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string MimeType { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
