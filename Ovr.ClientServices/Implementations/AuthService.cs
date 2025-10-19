using Ovr.ClientServices.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Requests;
using Ovr.Domain.Responses;
using System.Net.Http.Json;

namespace Ovr.ClientServices.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private string? _authToken; // Token almacenado en memoria

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseBase<string>> LoginAsync(string email, string password)
        {
            var loginModel = new LoginRequest { Email = email, Password = password };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    // Supone que el token viene en un JSON con una propiedad "token"
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<string>>();

                    if (result != null)
                    {
                        return result; // Indica autenticación exitosa
                    }
                }
                else
                {
                    if (response.Content != null)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Content: {errorContent}");
                    }
                    else
                    {
                        Console.WriteLine("No hay contenido en la respuesta.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud de login: {ex.Message}");
            }

            return null; // Indica autenticación fallida
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/auth/resendpassword?email={email}", null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Solicitud de restablecimiento enviada correctamente.");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al enviar la solicitud de restablecimiento: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud de restablecimiento: {ex.Message}");
            }

            return false;
        }

        public Task LogoutAsync()
        {
            // Limpia el token de la memoria
            _authToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null; // Limpia los encabezados
            return Task.CompletedTask;
        }

        public Task<string?> GetTokenAsync()
        {
            // Devuelve el token almacenado en memoria
            return Task.FromResult(_authToken);
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            // Comprueba si hay un token válido almacenado en memoria
            return Task.FromResult(!string.IsNullOrEmpty(_authToken));
        }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
