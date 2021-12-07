//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// Argon2PasswordHashingService.cs
//

using System;
using ASMR.Common.Cryptography;
using ASMR.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Web.Services;

public interface IHashingService
{
	public byte[] Hash(string plain);

	public string HashBase64(string plain);

	public bool Verify(string plain, byte[] hashed);

	public bool VerifyBase64(string plain, string hashed);
}

public class Argon2PasswordHashingService : IHashingService, IPasswordHasher<User>
{
	private readonly Argon2 _argon2;

	public Argon2PasswordHashingService()
	{
		_argon2 = new Argon2();
	}

	public byte[] Hash(string plain)
	{
		return _argon2.Hash(plain);
	}

	public string HashBase64(string plain)
	{
		return Convert.ToBase64String(Hash(plain));
	}

	public bool Verify(string plain, byte[] hashed)
	{
		return _argon2.Verify(plain, hashed);
	}

	public bool VerifyBase64(string plain, string hashed)
	{
		return Verify(plain, Convert.FromBase64String(hashed));
	}

	public string HashPassword(User user, string password)
	{
		return HashBase64(password);
	}

	public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
	{
		return VerifyBase64(providedPassword, hashedPassword)
			? PasswordVerificationResult.Success
			: PasswordVerificationResult.Failed;
	}
}