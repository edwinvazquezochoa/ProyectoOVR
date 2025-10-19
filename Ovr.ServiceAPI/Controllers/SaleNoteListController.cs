using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.ApiServices.Helpers;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;
using System.Net;

namespace Ovr.ApiServices.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SaleNoteListController : ControllerBase
    {
        private readonly ISaleNoteListService _saleNoteListService;
        private readonly IEventLogger _eventLogger;

        public SaleNoteListController(ISaleNoteListService saleNoteListService, IEventLogger eventLogger)
        {
            _saleNoteListService = saleNoteListService;
            _eventLogger = eventLogger;
        }

        [HttpGet("GetAllSaleNotes")]
        public async Task<IActionResult> GetAllSaleNotes(string? buscar, int? statusId, DateTime? fechaInicio, DateTime? fechaFin, int page = 1, int rows = 10)
        {
            try
            {
                var result = await _saleNoteListService.GetAllSaleNotesAsync(buscar ?? string.Empty, statusId, fechaInicio, fechaFin, page, rows);

                if (result.Items == null || !result.Items.Any())
                {
                    _eventLogger.Log("No se encontró información de notas de venta.", "SaleNoteListController.GetAllSaleNotes");

                    return ApiResponseHelper.Empty<PagedResult<SaleNoteListDto>>("No se encontró información de notas de venta.");
                }

                return ApiResponseHelper.Success(result, "Notas de venta obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Error(_eventLogger, ex, "SaleNoteListController.GetAllSaleNotes");
            }
        }

    }
}
