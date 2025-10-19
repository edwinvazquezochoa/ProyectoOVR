using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;

namespace Ovr.DaoServices.Implementations
{
    public class MenuPermissionService : IMenuPermissionService
    {
        // Permisos por usuario
        public async Task<List<MenuPermission>> GetUserAsync(long userId)
        {
            return await MenuPermissionDAO.GetUserAsync(userId);
        }

        public async Task<List<MenuPermission>> InsertOrUpdateAsync(List<MenuPermission> permissions, long userId)
        {
            return await MenuPermissionDAO.InsertOrUpdateAsync(permissions, userId);
        }

        // Permisos por rol
        public async Task<List<MenuPermission>> GetByRoleAsync(int roleId)
        {
            return await MenuPermissionDAO.GetByRoleAsync(roleId);
        }

        public async Task<List<MenuPermission>> InsertOrUpdateByRoleAsync(List<MenuPermission> permissions, int roleId)
        {
            return await MenuPermissionDAO.InsertOrUpdateByRoleAsync(permissions, roleId);
        }
    }
}
