//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// Packaging.cs
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class Packaging : DefaultAbstractEntity
{
	[Required] [JsonIgnore] public Bean Bean { get; set; }

	[Required] public string BeanId { get; set; }

	[Required] [JsonIgnore] public User User { get; set; }

	[Required] public string UserId { get; set; }

	public IEnumerable<PackagingResult> Results { get; set; }
}