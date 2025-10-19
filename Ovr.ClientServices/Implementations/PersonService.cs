using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Ovr.ClientServices.Intefaces;

namespace Ovr.ClientServices.Implementations
{
    public class PersonService: IPersonService
    {
        private readonly HttpClient _httpClient;

        public PersonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseBase<List<Person>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/person");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<List<Person>>>();
                    return result ?? new ResponseBase<List<Person>> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<List<Person>>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al obtener peronas: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<Person>>
                {
                    Code = 500,
                    Message = $"Excepción al obtener lista de personas: {ex.Message}"
                };
            }
        }
        public async Task<ResponseBase<Person>> CreateAsync(Person person)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/person", person);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<Person>>();
                    return result ?? new ResponseBase<Person> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else if (response.StatusCode == HttpStatusCode.Conflict) // Manejo de rol existente
                {
                    return new ResponseBase<Person>
                    {
                        Code = (int)HttpStatusCode.Conflict,
                        Message = $"La persona '{person.FullName}' ya existe."
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<Person>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al crear información de persona: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<Person>
                {
                    Code = 500,
                    Message = $"Error interno al crear informacion de persona"
                };
            }
        }
        public async Task<ResponseBase<Person>> UpdateAsync(int id, Person person)
        {
            ResponseBase<Person>? response = null;
            try
            {


                var httpResponse = await _httpClient.PutAsJsonAsync($"api/persona/{id}", person);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new ResponseBase<Person>((int)httpResponse.StatusCode, "No fue posible actualizar la información.");
                }

                response = await httpResponse.Content.ReadFromJsonAsync<ResponseBase<Person>>();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new ResponseBase<Person>
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
        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/person/{id}");

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
                    Message = $"Eorror al eliminar información"
                };
            }
        }

    }
}