using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public enum HashingType
    {
        Strong = 1 << 15,
        Weak = 1 << 10
    }

    public class HashingService
    {
        private const int SaltLengthInBits = 128;

        private static readonly Random Rnd = new Random();

        public static Task<string> GetHashAsync(string token, string salt, HashingType type)
        {
            return Task.Run(() =>
            {
                byte[] saltInBytes = Encoding.UTF8.GetBytes(salt);
                using (Rfc2898DeriveBytes derBytes = new Rfc2898DeriveBytes(token, saltInBytes, (int)type))
                {
                    byte[] hashedPasswordInBytes = derBytes.GetBytes(64);
                    string hashedPassword = Convert.ToBase64String(hashedPasswordInBytes);
                    return hashedPassword;
                }
            });
        }

        public static Task<string> GetSaltAsync()
        {
            return GetSaltAsync(SaltLengthInBits);
        }

        public static Task<string> GetSaltAsync(int bits)
        {
            return Task.Run(() =>
            {
                if (bits < 8)
                {
                    throw new ArgumentException("Argument 'bits' must be at least 8.");
                }

                if ((bits & (bits - 1)) != 0)
                {
                    throw new ArgumentException("Argument 'bits' must be a power of 2.");
                }

                int numberOfBytes = bits / 8;

                byte[] saltInBytes = new byte[numberOfBytes];
                Rnd.NextBytes(saltInBytes);
                string salt = Convert.ToBase64String(saltInBytes);
                return salt;
            });
        }
    }
}
