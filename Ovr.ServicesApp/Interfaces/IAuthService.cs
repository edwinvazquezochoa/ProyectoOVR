using Ovr.Domain.Models;

namespace Ovr.DaoServices.Interfaces
{
    public  interface IAuthService
    {
        Task<UserInfo?> VerifyUserCredentialsAsync(string email, string password);
    }
}
