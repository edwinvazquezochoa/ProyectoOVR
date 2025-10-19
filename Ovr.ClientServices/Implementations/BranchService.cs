using Ovr.ClientServices.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ovr.ClientServices.Implementations
{
    public class BranchService: IBranchService
    {
        private readonly HttpClient _httpClient;

        public BranchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseBase<List<Branch>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/branch");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<List<Branch>>>();
                    return result ?? new ResponseBase<List<Branch>> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<List<Branch>>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al obtener sucursales: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<Branch>>
                {
                    Code = 500,
                    Message = $"Excepción al obtener lista de sucursales: {ex.Message}"
                };
            }
        }
        public async Task<ResponseBase<object>> CreateAsync(Branch model)
        {
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("api/branch", model);

                if (httpResponse.IsSuccessStatusCode)
                {
                    // Configuramos la deserialización para que ignore mayúsculas/minúsculas.
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var result = await httpResponse.Content.ReadFromJsonAsync<ResponseBase<object>>(options);
                    return result ?? new ResponseBase<object>
                    {
                        Code = 500,
                        Message = "Error al procesar la respuesta del servidor."
                    };
                }
                else if (httpResponse.StatusCode == HttpStatusCode.Conflict) // Manejo de sucursal existente
                {
                    return new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.Conflict,
                        Message = $"La sucursal '{model.BrancheName}' ya existe."
                    };
                }
                else
                {
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    return new ResponseBase<object>
                    {
                        Code = (int)httpResponse.StatusCode,
                        Message = $"Error al crear la sucursal: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<object>
                {
                    Code = 500,
                    Message = $"Excepción al crear la sucursal: {ex.Message}"
                };
            }
        }



        public async Task<ResponseBase<object>> UpdateAsync(long id, Branch model)
        {
            ResponseBase<object>? response = null;
            try
            {
                var httpResponse = await _httpClient.PutAsJsonAsync($"api/branch/{id}", model);
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
        public async Task<ResponseBase<bool>> DeleteAsync(long id)
        {
            try
            {
                var httpResponse = await _httpClient.DeleteAsync($"api/branch/{id}");
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"🔍 [Blazor] Respuesta de la API: {responseContent}");
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return new ResponseBase<bool>
                    {
                        Code = (int)httpResponse.StatusCode,
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
                    Code = parsedResponse?.Code ?? (int)httpResponse.StatusCode,
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
                    Message = $"Eorror al eliminar información"
                };
            }
        }
    }
}