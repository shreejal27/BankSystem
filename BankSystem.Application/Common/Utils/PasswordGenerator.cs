using System;
using System.Collections.Generic;

namespace BankSystem.Application.Common.Utils
{
    public class PasswordGenerator
    {
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?/";

        public static string GenerateRandomPassword(int length = 12)
        {
            if (length < 4)
                throw new ArgumentException("Password length must be at least 4 to include all character types.");

            var random = new Random();
            var passwordChars = new List<char>();

            passwordChars.Add(Lowercase[random.Next(Lowercase.Length)]);
            passwordChars.Add(Uppercase[random.Next(Uppercase.Length)]);
            passwordChars.Add(Numbers[random.Next(Numbers.Length)]);
            passwordChars.Add(SpecialChars[random.Next(SpecialChars.Length)]);

            string allChars = Lowercase + Uppercase + Numbers + SpecialChars;
            for (int i = passwordChars.Count; i < length; i++)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            return Shuffle(passwordChars, random);
        }

        private static string Shuffle(List<char> chars, Random random)
        {
            for (int i = chars.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }
            return new string(chars.ToArray());
        }
    }
}
