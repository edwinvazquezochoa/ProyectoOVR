using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface IUserService
    {
        Task<ResponseBase<User>> Create(User model);
        Task<ResponseBase<User>> Update(User  model); 
        Task<bool> Delete(long id);
        Task<User?> GetById(long id);
        Task<List<User?>?> GetAll();
        Task<User> EmailExists(string email, long? userId = null);
        Task<bool> VerifyEmailToken(string token);
        Task UpdatePasswordAndTempStatus(long userId, string passwordHash, bool isPasswordTemp);
    }
}
