//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// Argon2.cs
//

using System;
using System.Linq;

namespace ASMR.Common.Cryptography;

public class Argon2
{
	public Argon2Options Options { get; set; }

	public Argon2()
	{
		Options = new Argon2Options();
	}

	public Argon2(Argon2Options options)
	{
		Options = options;
	}

	public byte[] GenerateSalt()
	{
		var buffer = new byte[Options.SaltLength];
		LibSodium.randombytes_buf(buffer, buffer.Length);
		return buffer;
	}

	public byte[] Hash(string plain)
	{
		var salt = GenerateSalt();
		return Hash(plain, salt);
	}

	public byte[] Hash(string plain, byte[] salt)
	{
		var buffer = new byte[Options.BufferLength];

		var result = LibSodium.crypto_pwhash(
			buffer,
			buffer.Length,
			Options.Encoding.GetBytes(plain),
			plain.Length,
			salt,
			Options.IterationLimit,
			Options.MemoryLimit,
			Options.Algorithm
		);

		if (result != 0)
		{
			throw new Argon2Exception($"Hashing failed with error code {result}");
		}

		var hashed = new byte[buffer.Length + salt.Length];
		Buffer.BlockCopy(buffer, 0, hashed, 0, buffer.Length);
		Buffer.BlockCopy(salt, 0, hashed, buffer.Length, salt.Length);

		return hashed;
	}

	public bool Verify(string plain, byte[] hashed)
	{
		var salt = new byte[Options.SaltLength];
		Buffer.BlockCopy(hashed, Options.BufferLength, salt, 0, salt.Length);
		return Verify(plain, hashed, salt);
	}

	public bool Verify(string plain, byte[] hashed, byte[] salt)
	{
		var againstHash = Hash(plain, salt);
		return hashed.SequenceEqual(againstHash);
	}
}