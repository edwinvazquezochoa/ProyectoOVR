using Ovr.ClientServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ovr.ClientServices.Implementations
{
    public class FrameClientService : IFrameClientService
    {
        private readonly HttpClient _httpClient;

        public FrameClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseBase<List<Frame>>> GetFramesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/frame");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<List<Frame>>>();
                    return result ?? new ResponseBase<List<Frame>> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<List<Frame>>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al obtener frames: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<Frame>>
                {
                    Code = 500,
                    Message = $"Excepción al obtener frames: {ex.Message}"
                };
            }
        }

        public async Task<ResponseBase<Frame>> CreateFrameAsync(Frame frame)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/frame", frame);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseBase<Frame>>();
                    return result ?? new ResponseBase<Frame> { Code = 500, Message = "Error al procesar la respuesta" };
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ResponseBase<Frame>
                    {
                        Code = (int)HttpStatusCode.Conflict,
                        Message = $"El frame '{frame.FrameName}' ya existe."
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResponseBase<Frame>
                    {
                        Code = (int)response.StatusCode,
                        Message = $"Error al crear el frame: {errorContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<Frame>
                {
                    Code = 500,
                    Message = $"Excepción al crear frame: {ex.Message}"
                };
            }
        }

        public async Task<ResponseBase<Frame>> UpdateFrameAsync(int frameId, Frame frame)
        {
            ResponseBase<Frame>? response = null;
            try
            {


                var httpResponse = await _httpClient.PutAsJsonAsync($"api/frame/{frameId}", frame);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new ResponseBase<Frame>((int)httpResponse.StatusCode, "No fue posible actualizar la información.");
                }

                response = await httpResponse.Content.ReadFromJsonAsync<ResponseBase<Frame>>();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new ResponseBase<Frame>
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

        public async Task<ResponseBase<bool>> DeleteFrameAsync(int frameId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/frame/{frameId}");
                var responseContent = await response.Content.ReadAsStringAsync();

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
                    Message = $"Excepción al eliminar frame: {ex.Message}"
                };
            }
        }
    }
}
