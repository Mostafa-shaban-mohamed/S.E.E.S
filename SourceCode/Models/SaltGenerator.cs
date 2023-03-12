using System.Security.Cryptography;

namespace SourceCode.Models
{
    public static class SaltGenerator
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(32);
            return salt;
        }
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
    }
}