namespace Ovr.Domain.DTOs
{
    public class BranchDto
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
