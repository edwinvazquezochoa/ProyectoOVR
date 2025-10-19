using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class AuthGuardService : IAuthGuardService
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly NavigationManager _navigation;

        public AuthGuardService(ISessionStorageService sessionStorage, NavigationManager navigation)
        {
            _sessionStorage = sessionStorage;
            _navigation = navigation;
        }

        public async Task<bool> RedirigirSiNoAutenticado(string? rutaRedireccion = "/")
        {
            var user = await _sessionStorage.GetItemAsync<UserInfo>("sesionUsuario");

            if (user == null)
            {
                _navigation.NavigateTo(rutaRedireccion ?? "/");
                return true; // Se redirigió
            }

            return false; // Autenticado
        }

        public async Task<bool> TieneRol(string rol)
        {
            var user = await _sessionStorage.GetItemAsync<UserInfo>("sesionUsuario");

            if (user == null)
                return false;

            return string.Equals(user.RoleName, rol, StringComparison.OrdinalIgnoreCase);
        }
    }
}
