using Blazored.SessionStorage;
using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IApiHelper _apiHelper;
        private string? _authToken;
        private readonly ISessionStorageService _sessionStorage;

        public AuthService(IApiHelper apiHelper, ISessionStorageService sessionStorage)
        {
            _apiHelper = apiHelper;
            _sessionStorage = sessionStorage;
        }

        public async Task<ResponseBase<string>> LoginAsync(string email, string password)
        {
            var result = await _apiHelper.LoginAsync(email, password);

            if (result != null && !string.IsNullOrEmpty(result.Data))
            {
                _authToken = result.Data;
                await _sessionStorage.SetItemAsync("authToken", _authToken);
            }

            return result;
        }

        public async Task<ResponseBase<string>> ResetPasswordAsync(string email)
        {
            return await _apiHelper.ForgotPasswordAsync(email);
        }

        public async Task LogoutAsync()
        {
            _authToken = null;
            var client = await _apiHelper.GetAuthorizedClientAsync();
            client.DefaultRequestHeaders.Authorization = null;
            await _sessionStorage.RemoveItemAsync("authToken");
        }

        public async Task<string?> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_authToken))
                return _authToken;

            _authToken = await _sessionStorage.GetItemAsync<string>("authToken");
            return _authToken;
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            return Task.FromResult(!string.IsNullOrEmpty(_authToken));
        }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
