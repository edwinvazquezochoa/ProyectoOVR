namespace Ovr.Domain.DTOs
{
    public class SaleNoteListDto
    {
        public long SaleNoteId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public DateTime? CommitmentDate { get; set; }
        public decimal Total { get; set; }
        public int StatusId { get; set; }
        public DateTime? UltimoPagoDate { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int TotalRows { get; set; }
    }
}
