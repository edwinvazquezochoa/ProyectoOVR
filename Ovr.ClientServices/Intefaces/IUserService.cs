using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Interfaces
{
    public interface IUserService
    {
        Task<ResponseBase<List<User>>> GetUsersAsync();
        Task<ResponseBase<object>> CreateUserAsync(User user);
        Task<ResponseBase<object>> UpdateUserAsync(long userId, User user);
        Task<ResponseBase<bool>> DeleteUserAsync(long userId);
    }
}
