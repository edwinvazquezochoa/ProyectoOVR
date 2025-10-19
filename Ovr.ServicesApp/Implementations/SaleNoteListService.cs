using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Implementations
{
    public class SaleNoteListService : ISaleNoteListService
    {
        public async Task<PagedResult<SaleNoteListDto>> GetAllSaleNotesAsync(string buscar, int? statusId, DateTime? fechaInicio, DateTime? fechaFin, int page, int rows)
        {
            // Llamamos al DAO → devuelve la lista de SaleNoteListDto
            var saleNotes = await SaleNoteListDAO.GetAll(buscar, statusId, fechaInicio, fechaFin, page, rows);

            // Armamos el PagedResult
            var result = new PagedResult<SaleNoteListDto>
            {
                Items = saleNotes,
                TotalRows = saleNotes.FirstOrDefault()?.TotalRows ?? 0
            };

            // Limpieza (opcional) → quitar TotalRows de los Items individuales si quieres
            result.Items.ForEach(x => x.TotalRows = 0);

            return result;
        }
    }
}
