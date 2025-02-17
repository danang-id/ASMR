﻿//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// Configuration.cs
//

using System.ComponentModel.DataAnnotations;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class Configuration : DefaultAbstractEntity
{
	[Required] public ConfigurationKey Key { get; set; }

	[Required] public string Value { get; set; }
}