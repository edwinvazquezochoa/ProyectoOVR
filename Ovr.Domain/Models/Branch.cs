namespace Ovr.Domain.Models
{
    public class Branch
    {
        public long BranchId { get; set; }
        public  string BrancheName { get; set; }
        public  string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

    }
}
