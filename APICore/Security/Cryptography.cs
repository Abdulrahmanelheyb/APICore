using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace APICore.Security
{
    public static class Cryptography
    {
        public static bool Check(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = Hash(hashAlgorithm, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
        
        [UsedImplicitly]
        public static string Hash(HashAlgorithm hashAlgorithm, string value)
        {
            var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                _ = sBuilder.Append(t.ToString("X2"));
            }
            return sBuilder.ToString();
        }
    }
}