using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BankSystem.Application.Common.Utils
{
    public static class EncryptDecryptAccountNumber
    {
        private static readonly IConfiguration _configuration;
        static EncryptDecryptAccountNumber()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static string Encrypt(string plainText)
        {
            var encryptKey = _configuration["AccountNumEncryption:Key"];
            using var aes = Aes.Create();
            var key = Encoding.UTF8.GetBytes(encryptKey.PadRight(32).Substring(0, 32));
            aes.Key = key;
            aes.IV = new byte[16];

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            var decryptKey = _configuration["AccountNumEncryption:Key"];
            using var aes = Aes.Create();
            var key = Encoding.UTF8.GetBytes(decryptKey.PadRight(32).Substring(0, 32));
            aes.Key = key;
            aes.IV = new byte[16];

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }
    }
}
