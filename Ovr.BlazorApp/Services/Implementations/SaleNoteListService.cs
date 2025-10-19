using Ovr.Client.Services.Interfaces;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;
using Ovr.BlazorApp.Helpers;

namespace Ovr.Client.Services
{
    public class SaleNoteListService : ISaleNoteListService
    {
        private readonly IApiHelper _apiHelper;

        public SaleNoteListService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<PagedResult<SaleNoteListDto>> GetAllSaleNotesAsync(
            string buscar, int? statusId, DateTime? fechaInicio, DateTime? fechaFin, int page, int rows)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(buscar))
                queryParams.Add($"buscar={Uri.EscapeDataString(buscar)}");

            if (statusId.HasValue)
                queryParams.Add($"statusId={statusId}");

            if (fechaInicio.HasValue && fechaInicio.Value.Year >= 1900)
                queryParams.Add($"fechaInicio={fechaInicio:yyyy-MM-dd}");

            if (fechaFin.HasValue && fechaFin.Value.Year >= 1900)
                queryParams.Add($"fechaFin={fechaFin:yyyy-MM-dd}");

            queryParams.Add($"page={page}");
            queryParams.Add($"rows={rows}");

            var url = $"SaleNoteList/GetAllSaleNotes?{string.Join("&", queryParams)}";

            var result = await _apiHelper.GetTypedAsync<PagedResult<SaleNoteListDto>>(url);

            return result.Data ?? new PagedResult<SaleNoteListDto>();
        }

    }
}
