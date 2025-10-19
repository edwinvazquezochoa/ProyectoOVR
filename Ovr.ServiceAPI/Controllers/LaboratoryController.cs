using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;

namespace Ovr.ApiServices.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LaboratoryController : ControllerBase
    {
        private readonly ILaboratoryService _service;
        private readonly IEventLogger _logger;

        public LaboratoryController(ILaboratoryService service, IEventLogger logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _service.GetAll();
                if (list == null || !list.Any())
                {
                    _logger.Log("No se encontraron laboratorios.", "LaboratoryController.GetAll");
                    return Ok(new ResponseBase<List<Laboratory>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontraron laboratorios.",
                        Data = null
                    });
                }

                return Ok(new ResponseBase<List<Laboratory>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lista de laboratorios obtenida correctamente.",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "LaboratoryController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var lab = await _service.GetById(id);
                if (lab == null)
                {
                    _logger.Log($"Laboratorio con ID {id} no encontrado.", "LaboratoryController.GetById");
                    return NotFound(new ResponseBase<Laboratory>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Laboratorio no encontrado.",
                        Data = null
                    });
                }

                return Ok(new ResponseBase<Laboratory>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Laboratorio obtenido correctamente.",
                    Data = lab
                });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "LaboratoryController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Laboratory request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log("Modelo inválido al crear laboratorio.", "LaboratoryController.Create");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                var result = await _service.Add(request);
                if (result.Code == 409)
                {
                    return Conflict(result);
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Data?.LaboratoryId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "LaboratoryController.Create");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno al crear laboratorio."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Laboratory request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log("Modelo inválido para actualización de laboratorio.", "LaboratoryController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                if (id != request.LaboratoryId)
                {
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "El ID proporcionado no coincide con el laboratorio."));
                }

                var result = await _service.Update(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "LaboratoryController.Update");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno al actualizar laboratorio."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.Delete(id);
                if (!deleted)
                {
                    return NotFound(new ResponseBase<object>((int)HttpStatusCode.NotFound, "El laboratorio no fue encontrado o ya está desactivado."));
                }

                return Ok(new ResponseBase<object>((int)HttpStatusCode.OK, "Laboratorio eliminado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "LaboratoryController.Delete");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno al eliminar laboratorio."));
            }
        }
    }
}
