using Ovr.Core.Infrastructures.Utils;
using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;

namespace Ovr.DaoServices.Implementations
{
    public class AuthService : IAuthService
    {
        public Task<UserInfo?> VerifyUserCredentialsAsync(string email, string password)
        {
            var hashedPassword = PasswordHelper.Hash256Password(password);
            return AuthDAO.VerifyUserCredentialsAsync(email, hashedPassword);
        }

    }
}
