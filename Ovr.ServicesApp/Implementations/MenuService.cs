using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.DaoServices.Interfaces;

namespace Ovr.DaoServices.Implementations
{
    public class MenuService : IMenuService
    {
        public async Task<long> Add(Menu model)
        {
            return await MenuDAO.Insert(model);
        }

        public async Task<bool> Update(Menu model)
        {
            return await MenuDAO.Update(model);
        }

        public async Task<bool> Delete(int id)
        {
            return await MenuDAO.Delete(id);
        }

        public async Task<Menu?> GetById(int id)
        {
            return await MenuDAO.GetById(id);
        }

        public async Task<List<Menu?>?> GetAll()
        {
            var flatMenus = await MenuDAO.GetAll(); // Devuelve lista plana
            var jerarquia = ConstruirArbolDeMenus(flatMenus);
            return jerarquia;
        }

        public async Task<List<Menu>?> GetMenusPermissionsByUserId(long userId)
        {
            var flatMenus = await MenuDAO.GetMenusByUserId(userId); // Devuelve lista plana
            var jerarquia = ConstruirArbolDeMenus(flatMenus);
            return jerarquia;
        }

        private List<Menu> ConstruirArbolDeMenus(List<Menu> flatMenus, int? parentId = null)
        {
            return flatMenus
                .Where(m => m.ParentMenuId == parentId)
                .OrderBy(m => m.OrderNumber)
                .Select(m => new Menu
                {
                    MenuId = m.MenuId,
                    MenuName = m.MenuName,
                    ParentMenuId = m.ParentMenuId,
                    OrderNumber = m.OrderNumber,
                    Controller = m.Controller,
                    Action = m.Action,
                    IsActive = m.IsActive,
                    SubMenus = ConstruirArbolDeMenus(flatMenus, m.MenuId)
                }).ToList();
        }


    }
}
