using Ovr.Domain.Models;

namespace Ovr.DaoServices.Interfaces
{
    public interface IMenuService
    {
        Task<long> Add(Menu model);
        Task<bool> Update(Menu model);
        Task<bool> Delete(int id);
        Task<Menu?> GetById(int id);
        Task<List<Menu?>?> GetAll();
        Task<List<Menu>?> GetMenusPermissionsByUserId(long userId);
    }
}