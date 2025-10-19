using Ovr.Domain.Models;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetMenusAsync();
        Task<List<Menu>> GetMenusByUserIdAsync(long userId);
    }

}
