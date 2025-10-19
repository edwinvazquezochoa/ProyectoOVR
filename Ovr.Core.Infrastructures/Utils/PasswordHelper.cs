using System.Security.Cryptography;
using System.Text;

namespace Ovr.Core.Infrastructures.Utils
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Genera un hash seguro para la contraseña.
        /// </summary>
        /// <param name="password">La contraseña en texto plano.</param>
        /// <returns>El hash de la contraseña.</returns>
        public static string HashBCryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static string Hash256Password(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder hex = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                {
                    hex.AppendFormat("{0:x2}", b); // Convert byte to hexadecimal
                }
                return hex.ToString();
            }
        }
        /// <summary>
        /// Verifica si una contraseña coincide con su hash.
        /// </summary>
        /// <param name="password">La contraseña en texto plano.</param>
        /// <param name="hashedPassword">El hash almacenado.</param>
        /// <returns>Verdadero si coincide, falso en caso contrario.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static string GenerateTemporaryPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz123456789@#$%"; // Excluye caracteres ambiguos como '0', 'O', 'l', 'I'
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
