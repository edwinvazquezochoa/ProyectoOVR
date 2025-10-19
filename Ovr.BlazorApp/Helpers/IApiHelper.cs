using System.Net.Http;
using System.Threading.Tasks;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Helpers
{
    public interface IApiHelper
    {
        Task<HttpClient> GetAuthorizedClientAsync();

        // Métodos básicos
        Task<T?> GetAsync<T>(string endpoint);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data);
        Task<HttpResponseMessage> DeleteAsync(string endpoint);

        // Métodos REST tipados con token
        Task<ResponseBase<T>> GetTypedAsync<T>(string endpoint);
        Task<ResponseBase<T>> PostTypedAsync<T>(string endpoint, object body);
        Task<ResponseBase<T>> PutTypedAsync<T>(string endpoint, object body);
        Task<ResponseBase<bool>> DeleteTypedAsync(string endpoint);

        // Métodos públicos sin token
        Task<ResponseBase<string>> LoginAsync(string email, string password);
        Task<ResponseBase<string>> ForgotPasswordAsync(string email);
    }
}
