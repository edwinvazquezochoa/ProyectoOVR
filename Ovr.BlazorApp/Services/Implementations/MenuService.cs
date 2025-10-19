using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IApiHelper _apiHelper;

        public MenuService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<Menu>> GetMenusAsync()
        {
            try
            {
                var response = await _apiHelper.GetTypedAsync<List<Menu>>("/menu");
                return response.Data ?? new List<Menu>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetMenusAsync] Error: {ex.Message}");
                return new List<Menu>();
            }
        }

        public async Task<List<Menu>> GetMenusByUserIdAsync(long userId)
        {
            try
            {
 
                var response = await _apiHelper.GetTypedAsync<List<Menu>>($"/menu/ByUser/{userId}");
                return response.Data ?? new List<Menu>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetMenusByUserIdAsync] Error: {ex.Message}");
                return new List<Menu>();
            }
        }
    }
}
