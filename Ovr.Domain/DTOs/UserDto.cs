namespace Ovr.Domain.DTOs
{
    public class UserDto
    {
        public long UserId { get; set; }
        public long PersonId { get; set; }  
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        // Propiedades calculadas
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
    }
}
