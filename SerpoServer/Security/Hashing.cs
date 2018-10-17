// ################################################
// ##@project SerpoCMS.Core
// ##@filename Hashing.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

using System;
using System.Text;

namespace SerpoServer.Security
{
    public static class Hashing
    {
        public static byte[] RandomBytes(int length = 8)
        {
            var result = new byte[length];
            new Random().NextBytes(result);
            return result;
        }

        public static string SHA512(string input, string salt = null)
        {
            var bytes = Encoding.UTF8.GetBytes(salt == null ? input : input + salt);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public static string SHA512(byte[] input)
        {
            return SHA512(input);
        }
    }
}