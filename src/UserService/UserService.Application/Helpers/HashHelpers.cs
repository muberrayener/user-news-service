using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace UserService.UserService.Application.Helpers
{
    public static class HashHelpers
    {
        private const string Key = "secret_key";  // todo: move to config
        public static string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Key));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashBytes = Convert.FromBase64String(storedHash);
            return computedHash.SequenceEqual(hashBytes);
        }
    }
}
