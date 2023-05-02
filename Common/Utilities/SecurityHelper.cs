using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public class SecurityHelper
    {
        public static string HashPasswordSHA256(string password)
        {
            using var sha = SHA256.Create();
            var byteValue=Encoding.UTF8.GetBytes(password);
            var byteHash = sha.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
