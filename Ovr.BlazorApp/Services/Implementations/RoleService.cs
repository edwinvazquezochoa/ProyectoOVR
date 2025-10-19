using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IApiHelper _apiHelper;

        public RoleService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Role>>> GetRolesAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Role>>("role");
        }

        public async Task<ResponseBase<Role>> CreateRoleAsync(Role role)
        {
            return await _apiHelper.PostTypedAsync<Role>("role", role);
        }

        public async Task<ResponseBase<Role>> UpdateRoleAsync(int roleId, Role role)
        {
            return await _apiHelper.PutTypedAsync<Role>($"role/{roleId}", role);
        }

        public async Task<ResponseBase<bool>> DeleteRoleAsync(int roleId)
        {
            return await _apiHelper.DeleteTypedAsync($"role/{roleId}");
        }
    }
}
