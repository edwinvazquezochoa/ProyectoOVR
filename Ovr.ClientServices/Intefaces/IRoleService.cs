using System.IO;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseBase<List<Role>>> GetRolesAsync();
        Task<ResponseBase<Role>> CreateRoleAsync(Role role);
        Task<ResponseBase<Role>> UpdateRoleAsync(int roleId, Role role);
        Task<ResponseBase<bool>> DeleteRoleAsync(int roleId);
    }
}
