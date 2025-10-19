using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Ovr.Domain.Models;
using System.Security.Claims;

namespace Ovr.BlazorApp.Extensions
{
    public class AutenticacionExtension : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;
        private readonly ClaimsPrincipal _sinInformacion = new ClaimsPrincipal(new ClaimsIdentity());

        public AutenticacionExtension(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task ActualizarEstadoAutenticacion(UserInfo? sesionUsuario)
        {
            ClaimsPrincipal claimsPrincipal;

            if (sesionUsuario != null)
            {
                claimsPrincipal = ConstruirClaimsPrincipal(sesionUsuario);
                await _sessionStorage.GuardarStorage("sesionUsuario", sesionUsuario);
                UserSession.SetSession = sesionUsuario; // ✅ También lo asignamos aquí
            }
            else
            {
                claimsPrincipal = _sinInformacion;
                await _sessionStorage.RemoveItemAsync("sesionUsuario");
                UserSession.SetSession = null; // Limpieza segura
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionUsuario = await _sessionStorage.ObtenerStorage<UserInfo>("sesionUsuario");

            if (sesionUsuario == null)
                return new AuthenticationState(_sinInformacion);

            // ✅ Esta línea es la clave para recuperar sesión tras recarga
            UserSession.SetSession = sesionUsuario;

            var claimsPrincipal = ConstruirClaimsPrincipal(sesionUsuario);

            return new AuthenticationState(claimsPrincipal);
        }

        private ClaimsPrincipal ConstruirClaimsPrincipal(UserInfo usuario)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", usuario.UserId.ToString()),
                new Claim("ShortName", usuario.ShortName ?? string.Empty),
                new Claim("FullName", usuario.FullName ?? string.Empty),
                new Claim("Email", usuario.Email ?? string.Empty),
                new Claim("RoleName", usuario.RoleName ?? string.Empty),
                new Claim("PersonId", usuario.PersonId.ToString()),
                new Claim("BranchId", usuario.BranchId.ToString()),
                new Claim("BrancheName", usuario.BrancheName ?? string.Empty),
                new Claim("Username", usuario.UserName ?? string.Empty),
                new Claim("IsActive", usuario.IsActive.ToString())
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));
        }

        public async Task<long?> ObtenerUserIdAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;

            var userIdClaim = user.FindFirst("UserId")?.Value;

            return long.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        public async Task<string?> ObtenerFullNameAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            return authState.User.FindFirst("FullName")?.Value;
        }

        public async Task<long?> ObtenerBranchIdAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            var claim = authState.User.FindFirst("BranchId")?.Value;
            return long.TryParse(claim, out var branchId) ? branchId : null;
        }



    }
}
