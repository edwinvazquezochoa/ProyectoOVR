namespace Ovr.Domain.DTOs
{
    public class UserBranchDto
    {
        public long UserBranchId { get; set; }
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsGlobal { get; set; }
    }
}
