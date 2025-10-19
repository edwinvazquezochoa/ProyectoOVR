namespace Ovr.Domain.DTOs
{
    public class MenuPermissionDto
    {
        public long UserId { get; set; }
        public int MenuId { get; set; }

        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
        public bool CanApprove { get; set; }
        public bool CanCancel { get; set; }
        public bool CanAuthorize { get; set; }
    }
}
