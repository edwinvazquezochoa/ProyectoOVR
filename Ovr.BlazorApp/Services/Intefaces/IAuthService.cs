using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Realiza el inicio de sesión del usuario y guarda el token en memoria.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Retorna true si el login fue exitoso, de lo contrario false.</returns>
        Task<ResponseBase<string>> LoginAsync(string email, string password);

        /// <summary>
        /// Realiza el cierre de sesión del usuario eliminando los datos de autenticación.
        /// </summary>
        /// <returns>Task completada cuando el proceso termina.</returns>
        Task LogoutAsync();

        /// <summary>
        /// Obtiene el token de autenticación almacenado en memoria.
        /// </summary>
        /// <returns>El token como string, o null si no hay token almacenado.</returns>
        Task<string?> GetTokenAsync();

        /// <summary>
        /// Verifica si el usuario está autenticado.
        /// </summary>
        /// <returns>True si hay un token válido almacenado en memoria, de lo contrario false.</returns>
        Task<bool> IsAuthenticatedAsync();
        Task<ResponseBase<string>> ResetPasswordAsync(string email);

    }
}
