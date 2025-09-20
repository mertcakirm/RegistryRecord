using System.Security.Cryptography;
using System.Text;

namespace RegistryRecord.Helpers
{
    public static class CreatePassword
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                password = ""; 

            password = password.Trim();

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password); 
                var hashBytes = sha256.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;
            
            var hashOfInput = HashPassword(password);

            hashOfInput = hashOfInput.Trim();
            hashedPassword = hashedPassword.Trim();

            Console.WriteLine(hashOfInput);
            Console.WriteLine(hashedPassword);

            return hashOfInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}