using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Responses;

namespace Ovr.ApiServices.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashBoardService;
        private readonly IEventLogger _eventLogger;

        public DashBoardController(IDashBoardService dashBoardService, IEventLogger eventLogger)
        {
            _dashBoardService = dashBoardService ?? throw new ArgumentNullException(nameof(dashBoardService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet("Resumen")]
        public async Task<IActionResult> GetSummary()
        {
            var response = new ResponseBase<DashBoardDTO>();

            try
            {
                var dashboard = new DashBoardDTO
                {
                    TotalVentas = await _dashBoardService.TotalVentasUltimaSemana(),
                    TotalIngresos = await _dashBoardService.TotalIngresosUltimaSemana(),
                    TotalProductos = await _dashBoardService.TotalProductos(),
                    VentasUltimaSemana = (await _dashBoardService.VentasUltimaSemana())
                        .Select(item => new VentaSemanaDTO
                        {
                            Fecha = item.Key,
                            Total = item.Value
                        }).ToList()
                };

                response = new ResponseBase<DashBoardDTO>
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = dashboard
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "DashBoardController.GetSummary");

                response = new ResponseBase<DashBoardDTO>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = $"Unexpected error: {ex.Message}",
                    Data = null
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
