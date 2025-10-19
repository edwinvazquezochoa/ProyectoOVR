using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface ISalesNoteService
    {
        Task<long> Add(SaleNoteDetailDto model, long userId);
        Task<ResponseBase<SaleNoteDetailDto>> Update(SaleNoteDetailDto model, long userId);
        Task<SaleNoteDetailDto?> GetById(long saleNoteId);
    }
}
