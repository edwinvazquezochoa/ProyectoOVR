using Ovr.ClientServices.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net.Http.Json;

namespace Ovr.ClientServices.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Menu>> GetMenusAsync()
        {
            try
            {
                // Realizar solicitud al servidor
                var response = await _httpClient.GetAsync("api/menu");

                // Verificar si la respuesta es exitosa
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en la respuesta: {response.StatusCode}");
                    Console.WriteLine($"Contenido del error: {errorContent}");
                    return new List<Menu>();
                }

                // Leer y deserializar la respuesta
                var apiResponse = await response.Content.ReadFromJsonAsync<ResponseBase<List<Menu>>>();

                // Verificar si los datos están presentes
                if (apiResponse?.Data == null)
                {
                    Console.WriteLine("No se encontraron datos en la respuesta.");
                    return new List<Menu>();
                }

                var menus = apiResponse.Data;

                // Construir jerarquía del menú
                var menuDictionary = menus.ToDictionary(m => m.MenuId);
                foreach (var menu in menus)
                {
                    if (menu.ParentMenuId.HasValue && menuDictionary.ContainsKey(menu.ParentMenuId.Value))
                    {
                        menuDictionary[menu.ParentMenuId.Value].SubMenus.Add(menu);
                    }
                }

                return menus.Where(m => m.ParentMenuId == null).OrderBy(m => m.OrderNumber).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menus: {ex.Message}");
                return new List<Menu>();
            }
        }



    }

}
