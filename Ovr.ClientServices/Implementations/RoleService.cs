using Ovr.ClientServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ovr.ClientServices.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;

        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener la lista de roles
        public async Task<ResponseBase<List<Role>>> GetRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/role");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<List<Role>>>();
                    return result ?? new ResponseBase<List<Role>> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<List<Role>>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al obtener roles: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<Role>>
                {
                    Code = 500,
                    Message = $"Excepción al obtener roles: {ex.Message}"
                };
            }
        }

        // Método para crear un nuevo rol
        public async Task<ResponseBase<Role>> CreateRoleAsync(Role role)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/role", role);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<Role>>();
                    return result ?? new ResponseBase<Role> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else if (response.StatusCode == HttpStatusCode.Conflict) // Manejo de rol existente
                {
                    return new ResponseBase<Role>
                    {
                        Code = (int)HttpStatusCode.Conflict,
                        Message = $"El rol '{role.RoleName}' ya existe."
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<Role>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al crear el rol: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<Role>
                {
                    Code = 500,
                    Message = $"Excepción al crear rol: {ex.Message}"
                };
            }
        }


        public async Task<ResponseBase<Role>> UpdateRoleAsync(int roleId, Role role)
        {
            ResponseBase<Role>? response= null;
            try
            {


                var httpResponse = await _httpClient.PutAsJsonAsync($"api/role/{roleId}", role);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new ResponseBase<Role>((int)httpResponse.StatusCode, "No fue posible actualizar la información.");
                }

                response = await httpResponse.Content.ReadFromJsonAsync<ResponseBase<Role>>();

                if(response!=null)
                {
                    return response;
                }
                else
                {
                    return new ResponseBase<Role>
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


        public async Task<ResponseBase<bool>> DeleteRoleAsync(int roleId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/role/{roleId}");

                // ✅ Leer la respuesta JSON si está disponible
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"🔍 [Blazor] Respuesta de la API: {responseContent}");

                // ✅ Si la API no devuelve contenido (204 No Content), manejamos el caso
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new ResponseBase<bool>
                    {
                        Code = (int)response.StatusCode,
                        Data = false,
                        Message = "La API no devolvió una respuesta válida."
                    };
                }

                // ✅ Deserializar JSON con `JsonElement` para manejar cualquier tipo de `data`
                var parsedResponse = JsonSerializer.Deserialize<ResponseBase<JsonElement>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // ✅ Convertir `data` a `bool` si es posible, si no, asignar `false`
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
                    Message = $"Excepción al eliminar rol: {ex.Message}"
                };
            }
        }

    }
}
