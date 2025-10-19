using Ovr.ClientServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ovr.ClientServices.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseBase<List<User>>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<List<User>>>();
                    return result ?? new ResponseBase<List<User>> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<List<User>>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al obtener usuarios: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<User>>
                {
                    Code = 500,
                    Message = $"Excepción al obtener usuarios: {ex.Message}"
                };
            }
        }

        public async Task<ResponseBase<object>> CreateUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user", user);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<object>>();
                    return result ?? new ResponseBase<object> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.Conflict,
                        Message = $"El usuario '{user.Email}' ya existe."
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<object>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al crear el usuario: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<object>
                {
                    Code = 500,
                    Message = $"Excepción al crear usuario: {ex.Message}"
                };
            }
        }

        public async Task<ResponseBase<object>> UpdateUserAsync(long id, User user)
        {
            ResponseBase<object>? response = null;
            try
            {
                var httpResponse = await _httpClient.PutAsJsonAsync($"api/user/{id}", user);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new ResponseBase<object>((int)httpResponse.StatusCode, "No fue posible actualizar la información.");
                }

                response = await httpResponse.Content.ReadFromJsonAsync<ResponseBase<object>>();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new ResponseBase<object>
                    {
                        Code = response.Code,
                        Data = response.Data,
                        Message = response.Message
                    };
                }

            }
            catch
            {
                return response;
            }

        }

        public async Task<ResponseBase<bool>> DeleteUserAsync(long userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/user/{userId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new ResponseBase<bool>
                    {
                        Code = (int)response.StatusCode,
                        Data = false,
                        Message = "La API no devolvió una respuesta válida."
                    };
                }

                var parsedResponse = JsonSerializer.Deserialize<ResponseBase<JsonElement>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                bool dataBool = parsedResponse?.Data.ValueKind == JsonValueKind.True || parsedResponse?.Data.ValueKind == JsonValueKind.False
                    ? parsedResponse.Data.GetBoolean()
                    : false;

                return new ResponseBase<bool>
                {
                    Code = parsedResponse?.Code ?? (int)response.StatusCode,
                    Data = dataBool,
                    Message = parsedResponse?.Message ?? "Error al procesar la respuesta de la API."
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>
                {
                    Code = 500,
                    Data = false,
                    Message = $"Excepción al eliminar usuario: {ex.Message}"
                };
            }
        }
    }
}
