using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SalesNoteController : ControllerBase
    {
        private readonly IEventLogger _eventLogger;
        private readonly ISalesNoteService _salesNoteService;

        public SalesNoteController(ISalesNoteService salesNoteService, IEventLogger eventLogger)
        {
            _salesNoteService = salesNoteService ?? throw new ArgumentNullException(nameof(salesNoteService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var salesNote = await _salesNoteService.GetById(id);

                if (salesNote == null)
                {
                    _eventLogger.Log($"Nota de venta con ID {id} no encontrada.", "SalesNoteController.GetById");
                    return NotFound(new ResponseBase<SaleNoteDetailDto>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Nota de venta con ID {id} no encontrada.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Nota de venta con ID {id} obtenida correctamente.", "SalesNoteController.GetById");
                return Ok(new ResponseBase<SaleNoteDetailDto>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Nota de venta obtenida correctamente.",
                    Data = salesNote
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "SalesNoteController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleNoteDetailDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de nota de venta.", "SalesNoteController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                // 👉 Obtén el UserId del token (ya que tienes [Authorize]):
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var branchId = User.Claims.FirstOrDefault(c => c.Type == "BranchId")?.Value;

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(branchId))
                    return Unauthorized();

                if (request.StatusId <= 0)
                    request.StatusId = 1; // o el ID válido por defecto

                request.OrderNumber = null;

                request.BranchId = Convert.ToInt64(branchId);

                var createdId = await _salesNoteService.Add(request, Convert.ToInt64(userId));

                _eventLogger.Log($"Nota de venta creada correctamente con ID {createdId}.", "SalesNoteController.Create");

                return CreatedAtAction(nameof(GetById), new { id = createdId }, new ResponseBase<long>
                {
                    Code = (int)HttpStatusCode.Created,
                    Message = "Nota de venta creada correctamente.",
                    Data = createdId
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "SalesNoteController.Create");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] SaleNoteDetailDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la actualización de nota de venta.", "SalesNoteController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                if (id != request.SaleNoteId)
                {
                    _eventLogger.Log("El ID proporcionado no coincide con el ID de la nota de venta.", "SalesNoteController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "El ID proporcionado no coincide con el ID de la nota de venta."));
                }

                // 👉 Obtén el UserId del token (ya que tienes [Authorize]):
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var branchId = User.Claims.FirstOrDefault(c => c.Type == "BranchId")?.Value;

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(branchId))
                    return Unauthorized();

                if (request.StatusId <= 0)
                    request.StatusId = 1; // o el ID válido por defecto


                request.BranchId = Convert.ToInt64(branchId);



                var response = await _salesNoteService.Update(request, Convert.ToInt64(userId));

                _eventLogger.Log($"Nota de venta con ID {id} actualizada correctamente.", "SalesNoteController.Update");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "SalesNoteController.Update");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno del servidor."));
            }
        }
    }
}
