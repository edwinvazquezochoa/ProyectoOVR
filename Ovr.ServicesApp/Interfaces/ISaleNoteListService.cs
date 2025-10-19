using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface ISaleNoteListService
    {
        Task<PagedResult<SaleNoteListDto>> GetAllSaleNotesAsync(string buscar, int? statusId, DateTime? fechaInicio, DateTime? fechaFin, int page, int rows);
    }
}
