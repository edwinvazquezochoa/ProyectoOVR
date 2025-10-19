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
    public class LensesController : ControllerBase
    {
        private readonly ILensesService _lensService;
        private readonly IEventLogger _eventLogger;

        public LensesController(ILensesService lensService, IEventLogger eventLogger)
        {
            _lensService = lensService;
            _eventLogger = eventLogger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _lensService.GetAll();
                if (list == null || !list.Any())
                {
                    _eventLogger.Log("No se encontró información de lentes.", "LensesController.GetAll");
                    return Ok(new ResponseBase<List<Len>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de lentes.",
                        Data = null
                    });
                }

                return Ok(new ResponseBase<List<Len>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lentes obtenidos correctamente.",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "LensesController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var lens = await _lensService.GetById(id);
                if (lens == null)
                {
                    return NotFound(new ResponseBase<Len>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Lente con ID {id} no encontrado.",
                        Data = null
                    });
                }

                return Ok(new ResponseBase<Len>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lente obtenido correctamente.",
                    Data = lens
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "LensesController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Len request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de lente.", "LensesController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var response = await _lensService.Add(request);

                if (response.Code == 409)
                {
                    _eventLogger.Log($"El lente '{request.LensName}' ya existe.", "LensesController.Create");
                    return Conflict(new ResponseBase<object>
                    {
                        Code = 409,
                        Message = response.Message,
                        Data = null
                    });
                }

                if (response.Code == 500 || response.Data == null)
                {
                    _eventLogger.Log("Error interno al crear el lente.", "LensesController.Create");
                    return StatusCode(500, new ResponseBase<object>
                    {
                        Code = 500,
                        Message = "Error interno al crear el lente.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Lente creado correctamente con ID {response.Data.LensId}.", "LensesController.Create");
                return CreatedAtAction(nameof(GetById), new { id = response.Data.LensId }, new ResponseBase<Len>
                {
                    Code = 201,
                    Message = response.Message,
                    Data = response.Data
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "LensesController.Create");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Excepción inesperada.",
                    Data = null
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Len request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                if (id != request.LensId)
                {
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "El ID proporcionado no coincide con el ID del lente."
                    });
                }

                var response = await _lensService.Update(request);

                if (response.Code == 404)
                    return NotFound(response);

                if (response.Code == 409)
                    return Conflict(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "LensesController.Update");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Error interno al actualizar el lente."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _lensService.Delete(id);
                if (!deleted)
                {
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "El lente ya está eliminado o no existe.",
                        Data = null
                    });
                }

                return Ok(new ResponseBase<object>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lente eliminado correctamente.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "LensesController.Delete");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Error interno al eliminar el lente."
                });
            }
        }
    }
}
