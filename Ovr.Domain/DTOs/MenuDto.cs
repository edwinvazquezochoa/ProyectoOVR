namespace Ovr.Domain.DTOs
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int? ParentMenuId { get; set; }
        public int OrderNumber { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public bool IsActive { get; set; }
    }
}
