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

        public static string Sha1Encrypt(string value)
        {
            var hashFunc = new MD5CryptoServiceProvider();
            var key = hashFunc.ComputeHash(Encoding.ASCII.GetBytes("/%!#=#!%\\"));
            var iv = new byte[8];
            var shaFunc = new SHA1CryptoServiceProvider();
            var temp = shaFunc.ComputeHash(Encoding.ASCII.GetBytes("/%!#=#!%\\"));
            for (var i = 0; i < 8; i++)
            {
                iv[i] = temp[i];
            }
            var toenc = Encoding.UTF8.GetBytes(value);
            var des = new TripleDESCryptoServiceProvider();
            des.KeySize = 192;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            cs.Write(toenc, 0, toenc.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(ms.ToArray())));
        }

        public static string Sha1Decrypt(string value)
        {
            var hashFunc = new MD5CryptoServiceProvider();
            var key = hashFunc.ComputeHash(Encoding.ASCII.GetBytes("/%!#=#!%\\"));
            var iv = new byte[8];
            var shaFunc = new SHA1CryptoServiceProvider();
            var temp = shaFunc.ComputeHash(Encoding.ASCII.GetBytes("/%!#=#!%\\"));
            for (var i = 0; i < 8; i++)
            {
                iv[i] = temp[i];
            }
            var todec = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(value)));
            var des = new TripleDESCryptoServiceProvider();
            des.KeySize = 192;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            cs.Write(todec, 0, todec.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}