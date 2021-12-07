//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// EnumUtils.cs
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASMR.Common.DataStructure;

public static class EnumUtils
{
	public static IEnumerable<T> GetValues<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}
}