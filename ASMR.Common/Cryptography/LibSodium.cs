//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:56 AM
//
// LibSodium.cs
//

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ASMR.Common.Cryptography;

public static class LibSodium
{
	private const string Name = "libsodium";

	public const int crypto_pwhash_argon2id_ALG_ARGON2ID13 = 2;

	public const long crypto_pwhash_argon2id_OPSLIMIT_SENSITIVE = 4;

	public const int crypto_pwhash_argon2id_MEMLIMIT_SENSITIVE = 536870912;

	static LibSodium()
	{
		sodium_init();
	}

	[DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	private static extern void sodium_init();

	[DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	internal static extern void randombytes_buf(byte[] buffer, int size);

	[DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	internal static extern int crypto_pwhash(byte[] buffer, long bufferLen, byte[] password, long passwordLen,
		byte[] salt, long opsLimit, int memLimit, int alg);
}