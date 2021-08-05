//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 8:04 AM
//
// HashingService.cs
//
using System;
using ASMR.Common.Cryptography;

namespace ASMR.Web.Services
{
    public interface IHashingService
    {
        public byte[] Hash(string plain);

        public string HashBase64(string plain);

        public bool Verify(string plain, byte[] hashed);

        public bool VerifyBase64(string plain, string hashed);

    }

    public class HashingService : IHashingService
    {
        private readonly Argon2 _argon2;

        public HashingService()
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
    }
}
