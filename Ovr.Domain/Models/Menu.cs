namespace Ovr.Domain.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }
        public int OrderNumber { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public bool IsActive { get; set; }
        // Campo adicional para jerarquía
        public List<Menu>? SubMenus { get; set; } = new List<Menu>();
    }
}
