using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class MenuPermissionService : IMenuPermissionService
    {
        private readonly IApiHelper _apiHelper;

        public MenuPermissionService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<MenuPermission>> GetByUserIdAsync(long userId)
        {
            var response = await _apiHelper.GetTypedAsync<List<MenuPermission>>($"MenuPermission/GetUserPermissions?userId={userId}");
            return response.Data ?? new List<MenuPermission>();
        }

        public async Task<ResponseBase<List<MenuPermission>>> InsertOrUpdateAsync(List<MenuPermission> permissions, long userId)
        {
            return await _apiHelper.PostTypedAsync<List<MenuPermission>>($"MenuPermission/InsertOrUpdate?userId={userId}", permissions);
        }

        public async Task<List<MenuPermission>> GetByRoleIdAsync(long roleId)
        {
            var response = await _apiHelper.GetTypedAsync<List<MenuPermission>>($"MenuPermission/GetRolePermissions?roleId={roleId}");
            return response.Data ?? new List<MenuPermission>();
        }

        public async Task<ResponseBase<List<MenuPermission>>> InsertOrUpdateByRoleAsync(List<MenuPermission> permissions, long roleId)
        {
            return await _apiHelper.PostTypedAsync<List<MenuPermission>>($"MenuPermission/InsertOrUpdateByRole?roleId={roleId}", permissions);
        }
    }
}
