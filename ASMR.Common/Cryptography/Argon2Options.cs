//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:57 AM
//
// Argon2Options.cs
//

using System.Text;

namespace ASMR.Common.Cryptography;

public class Argon2Options
{
	public int Algorithm { get; } = LibSodium.crypto_pwhash_argon2id_ALG_ARGON2ID13;

	public int BufferLength { get; set; } = 16;

	public Encoding Encoding { get; set; } = Encoding.UTF8;

	public long IterationLimit { get; set; } = LibSodium.crypto_pwhash_argon2id_OPSLIMIT_SENSITIVE;

	public int MemoryLimit { get; set; } = LibSodium.crypto_pwhash_argon2id_MEMLIMIT_SENSITIVE;

	public int SaltLength { get; set; } = 16;
}