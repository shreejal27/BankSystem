using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Application.Common.Utils
{
    public class EncryptDecryptAccountNumber
    {
        private static readonly string EncryptionKey = "1234";

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            var key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32).Substring(0, 32));
            aes.Key = key;
            aes.IV = new byte[16]; // 16-byte zero IV for simplicity (better: random IV stored with ciphertext)

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }
}