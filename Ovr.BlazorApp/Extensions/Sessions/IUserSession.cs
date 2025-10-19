using Microsoft.AspNetCore.Http;
using Ovr.Domain.Models;

namespace Ovr.BlazorApp.Extensions.Sessions
{
    public interface IUserSession
    {
        UserInfo? SetSession (UserInfo userInfo);
        UserInfo GetSession (UserInfo userInfo);
    }
}
