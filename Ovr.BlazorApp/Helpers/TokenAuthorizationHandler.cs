using Blazored.SessionStorage;
using System.Net.Http.Headers;

namespace Ovr.BlazorApp.Helpers
{
    public class TokenAuthorizationHandler : DelegatingHandler
    {
        private readonly ISessionStorageService _sessionStorage;

        public TokenAuthorizationHandler(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _sessionStorage.GetItemAsync<string>("authToken");
            Console.WriteLine($"Token desde SessionStorage: {token}");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Authorization Header agregado: Bearer {token}");
            }
            else
            {
                Console.WriteLine("No se encontró token");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
