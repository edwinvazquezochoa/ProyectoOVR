using Ovr.Domain.Models;

namespace Ovr.DaoServices.Interfaces
{
    public interface IMenuPermissionService
    {
        // Permisos por usuario
        Task<List<MenuPermission>> GetUserAsync(long userId);
        Task<List<MenuPermission>> InsertOrUpdateAsync(List<MenuPermission> permissions, long userId);

        // Permisos por rol
        Task<List<MenuPermission>> GetByRoleAsync(int roleId);
        Task<List<MenuPermission>> InsertOrUpdateByRoleAsync(List<MenuPermission> permissions, int roleId);
    }
}
