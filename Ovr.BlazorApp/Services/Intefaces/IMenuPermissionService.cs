using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IMenuPermissionService
    {
        // Permisos por usuario
        Task<List<MenuPermission>> GetByUserIdAsync(long userId);
        Task<ResponseBase<List<MenuPermission>>> InsertOrUpdateAsync(List<MenuPermission> permissions, long userId);

        // Permisos por rol
        Task<List<MenuPermission>> GetByRoleIdAsync(long roleId);
        Task<ResponseBase<List<MenuPermission>>> InsertOrUpdateByRoleAsync(List<MenuPermission> permissions, long roleId);
    }
}
