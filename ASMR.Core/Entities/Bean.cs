//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/11/2021 10:20 AM
//
// Bean.cs
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class Bean : DefaultAbstractEntity
{
	[Required] public string Name { get; set; }

	[Required] public string Description { get; set; }

	[Required] public string Image { get; set; }

	[Required] public BeanInventory Inventory { get; set; }

	[Required] [JsonIgnore] public string InventoryId { get; set; }

	public IEnumerable<IncomingGreenBean> IncomingGreenBeans { get; set; }

	public IEnumerable<Roasting> RoastingSessions { get; set; }

	public IEnumerable<Product> Products { get; set; }
}