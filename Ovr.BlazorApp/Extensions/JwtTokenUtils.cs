using System.IdentityModel.Tokens.Jwt;
using Ovr.Domain.Models; // Asegúrate de tener este using
using System.Linq; // Para usar FirstOrDefault

namespace Ovr.BlazorApp.Extensions
{
    public class JwtTokenUtils
    {
        public UserInfo DecodeJwtToken(string? jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) // Validación inicial
            {
                return null; // O podrías lanzar una excepción más específica
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.ReadJwtToken(jwtToken);

                // Imprimir claims (solo para depuración, eliminar en producción)
                // Imprime los tipos y valores de los claims para verificar que coinciden con los del backend
                Console.WriteLine("Claims en el token:");
                foreach (var claim in token.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                var userInfo = new UserInfo
                {
                    UserId = GetClaimValueAsLong(token, "UserId"),
                    PersonId = GetClaimValueAsLong(token, "PersonId"),
                    UserName = GetClaimValueAsString(token, "Username"),
                    Email = GetClaimValueAsString(token, "Email"), // "Email" o "email", revisa tu backend
                    ShortName = GetClaimValueAsString(token, "ShortName"),
                    FullName = GetClaimValueAsString(token, "FullName"), // "FullName" o "unique_name", revisa tu backend
                    RoleId = GetClaimValueAsInt(token, "RoleId"),
                    RoleName = GetClaimValueAsString(token, "RoleName"), // "RoleName" o "role", revisa tu backend
                    BranchId = GetClaimValueAsInt(token, "BranchId"),
                    BrancheName = GetClaimValueAsString(token, "BrancheName"),
                    IsActive = GetClaimValueAsBool(token, "IsActive")
                };

                return userInfo;
            }
            catch (Exception ex) // Captura excepciones de lectura de token
            {
                Console.Error.WriteLine($"Error al decodificar token: {ex.Message}"); // Loguea el error
                return null; // O lanza una excepción específica
            }
        }

        // Métodos genéricos y reutilizables para obtener claims y convertirlos
        private static string GetClaimValueAsString(JwtSecurityToken token, string claimType) =>
            token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;

        private static long GetClaimValueAsLong(JwtSecurityToken token, string claimType) =>
            long.TryParse(token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value, out long result) ? result : 0;

        private static int GetClaimValueAsInt(JwtSecurityToken token, string claimType) =>
            int.TryParse(token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value, out int result) ? result : 0;

        private static bool GetClaimValueAsBool(JwtSecurityToken token, string claimType) =>
            bool.TryParse(token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value, out bool result) && result;
    }
}