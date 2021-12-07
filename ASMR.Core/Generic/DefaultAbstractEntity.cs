//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// DefaultAbstractEntity.cs
//

using System;
using System.ComponentModel.DataAnnotations;

namespace ASMR.Core.Generic;

public abstract class DefaultAbstractEntity
{
	public DefaultAbstractEntity()
	{
		Id = Guid.Empty.ToString();
		CreatedAt = DateTimeOffset.Now;
	}

	[Required] [Key] public string Id { get; set; }

	[Required] public DateTimeOffset CreatedAt { get; set; }

	public DateTimeOffset? LastUpdatedAt { get; set; }
}