using Blazored.SessionStorage;
using Ovr.Domain.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Ovr.BlazorApp.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISessionStorageService _sessionStorage;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiHelper(IHttpClientFactory httpClientFactory,ISessionStorageService sessionStorage)
        {
            _httpClientFactory = httpClientFactory;
            _sessionStorage = sessionStorage;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<HttpClient> GetAuthorizedClientAsync()
        {
            var client = _httpClientFactory.CreateClient("OvrAPI");
            var token = await _sessionStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var client = await GetAuthorizedClientAsync();
            return await client.GetFromJsonAsync<T>(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var client = await GetAuthorizedClientAsync();
            return await client.PostAsJsonAsync(endpoint, data);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var client = await GetAuthorizedClientAsync();
            return await client.PutAsJsonAsync(endpoint, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var client = await GetAuthorizedClientAsync();
            return await client.DeleteAsync(endpoint);
        }

        public async Task<ResponseBase<T>> GetTypedAsync<T>(string endpoint)
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            return TryDeserialize<ResponseBase<T>>(content) ?? new ResponseBase<T>
            {
                Code = (int)response.StatusCode,
                Message = $"Error al obtener datos: {response.ReasonPhrase}"
            };
        }

        public async Task<ResponseBase<T>> PostTypedAsync<T>(string endpoint, object body)
        {
            var client = await GetAuthorizedClientAsync();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return TryDeserialize<ResponseBase<T>>(responseContent) ?? new ResponseBase<T>
            {
                Code = (int)response.StatusCode,
                Message = $"Error al deserializar respuesta: {response.ReasonPhrase}"
            };
        }

        public async Task<ResponseBase<T>> PutTypedAsync<T>(string endpoint, object body)
        {
            var client = await GetAuthorizedClientAsync();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return TryDeserialize<ResponseBase<T>>(responseContent) ?? new ResponseBase<T>
            {
                Code = (int)response.StatusCode,
                Message = $"Error al deserializar respuesta: {response.ReasonPhrase}"
            };
        }

        public async Task<ResponseBase<bool>> DeleteTypedAsync(string endpoint)
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.DeleteAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            return TryDeserialize<ResponseBase<bool>>(content) ?? new ResponseBase<bool>
            {
                Code = (int)response.StatusCode,
                Message = $"Error al deserializar respuesta: {response.ReasonPhrase}"
            };
        }

        public async Task<ResponseBase<string>> LoginAsync(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("OvrAPI");

            var body = new
            {
                Email = email,
                Password = password
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync("auth/login", content);
            }
            catch (Exception ex)
            {
                return new ResponseBase<string>
                {
                    Code = 0,
                    Message = $"No se pudo conectar con el servidor: {ex.Message}",
                    Data = null
                };
            }

            var statusCode = (int)response.StatusCode;
            var responseContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new ResponseBase<string>
                {
                    Code = statusCode,
                    Message = "El servidor respondió sin contenido.",
                    Data = null
                };
            }

            // 🎯 Aquí obtienes el ResponseBase<string> devuelto por la API
            var result = TryDeserialize<ResponseBase<string>>(responseContent);

            if (result == null)
            {
                return new ResponseBase<string>
                {
                    Code = statusCode,
                    Message = "No se pudo interpretar la respuesta del servidor.",
                    Data = null
                };
            }

            return result;
        }



        public async Task<ResponseBase<string>> ForgotPasswordAsync(string email)
        {
            var client = _httpClientFactory.CreateClient("OvrAPI");

            // Enviar solo el correo por query string, sin body
            var endpoint = $"/auth/forgot-password?email={Uri.EscapeDataString(email)}";

            var response = await client.PostAsync(endpoint, null);
            var responseContent = await response.Content.ReadAsStringAsync();

            return TryDeserialize<ResponseBase<string>>(responseContent) ?? new ResponseBase<string>
            {
                Code = (int)response.StatusCode,
                Message = $"Error al deserializar respuesta: {response.ReasonPhrase}"
            };
        }

        private T? TryDeserialize<T>(string json)
        {
            if(json!=string.Empty)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(json, _jsonOptions);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[❌ Deserialize Error] Tipo: {typeof(T).Name}, Error: {ex.Message}, JSON: {json}");
                    return default;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[❌ Error inesperado] Tipo: {typeof(T).Name}, Error: {ex.Message}");
                    return default;
                }
            }
            else
            {
                return default;
            }

        }

    }
}
