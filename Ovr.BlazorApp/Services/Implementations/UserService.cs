using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IApiHelper _apiHelper;

        public UserService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<User>>> GetUsersAsync()
        {
            return await _apiHelper.GetTypedAsync<List<User>>("user");
        }

        public async Task<ResponseBase<object>> CreateUserAsync(User user)
        {
            return await _apiHelper.PostTypedAsync<object>("user", user);
        }

        public async Task<ResponseBase<object>> UpdateUserAsync(long id, User user)
        {
            return await _apiHelper.PutTypedAsync<object>($"user/{id}", user);
        }

        public async Task<ResponseBase<bool>> DeleteUserAsync(long userId)
        {
            return await _apiHelper.DeleteTypedAsync($"user/{userId}");
        }
    }
}
