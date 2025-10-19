namespace Ovr.Domain.Models
{
    public class UserInfo
    {
        public long UserId { get; set; }
        public long PersonId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public long BranchId { get; set; } 
        public string BrancheName { get; set; } = string.Empty; 
        public bool IsActive { get; set; }
    }
}
