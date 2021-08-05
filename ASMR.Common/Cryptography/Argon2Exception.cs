//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:57 AM
//
// Argon2Exception.cs
//
using System;

namespace ASMR.Common.Cryptography
{
	public class Argon2Exception : Exception
	{
		public Argon2Exception() { }

		public Argon2Exception(string message) : base(message) { }
	}
}
