using Ovr.Domain.Models;

namespace Ovr.ClientServices.Intefaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetMenusAsync();
    }

}
