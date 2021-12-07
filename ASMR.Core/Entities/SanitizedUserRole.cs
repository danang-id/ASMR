//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// SanitizedUserRole.cs
//
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;

namespace ASMR.Core.Entities;

public class SanitizedUserRole : DefaultAbstractEntity
{
	public Role Role { get; set; }
}