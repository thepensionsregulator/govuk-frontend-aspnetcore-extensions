using System;
using System.Security.Cryptography;

namespace GovUk.Frontend.AspNetCore.Extensions.Security
{
    public class NonceProvider : INonceProvider
    {
        private readonly string _nonce;
        public NonceProvider()
        {
            _nonce = GenerateNonce();
        }
        public string GetNonce()
        {
            return _nonce;
        }

        private string GenerateNonce()
        {
            var nonceBytes = RandomNumberGenerator.GetBytes(20);
            return Convert.ToBase64String(nonceBytes);
        }
    }
}
