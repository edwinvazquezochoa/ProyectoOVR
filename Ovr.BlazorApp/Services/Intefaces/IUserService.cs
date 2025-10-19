using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IUserService
    {
        Task<ResponseBase<List<User>>> GetUsersAsync();
        Task<ResponseBase<object>> CreateUserAsync(User user);
        Task<ResponseBase<object>> UpdateUserAsync(long userId, User user);
        Task<ResponseBase<bool>> DeleteUserAsync(long userId);
    }
}
