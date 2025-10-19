using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Interfaces
{
    public interface ISalesNoteService
    {
        Task<ResponseBase<SaleNoteDetailDto>> GetSaleNoteByIdAsync(long saleNoteId);
        Task<ResponseBase<long>> CreateSaleNoteAsync(SaleNoteDetailDto saleNote);
        Task<ResponseBase<SaleNoteDetailDto>> UpdateSaleNoteAsync(long saleNoteId, SaleNoteDetailDto saleNote);
    }
}
