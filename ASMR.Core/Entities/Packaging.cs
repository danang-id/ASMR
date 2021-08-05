//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 8/5/2021 8:55 AM
// 
// Packaging.cs
//
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities
{
	public class Packaging : DefaultAbstractEntity
	{
		[Required]
		[JsonIgnore]
		public Bean Bean { get; set; }
		
		[Required]
		[JsonIgnore]
		public string BeanId { get; set; }
		
		[Required]
		public User Roaster { get; set; }

		[Required]
		[JsonIgnore]
		public string RoasterId { get; set; }

		public IEnumerable<PackagingResult> Results { get; set; }
	}
}