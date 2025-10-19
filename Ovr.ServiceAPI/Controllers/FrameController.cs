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
    public class FrameController : ControllerBase
    {
        private readonly IEventLogger _eventLogger;
        private readonly IFrameService _frameService;

        public FrameController(IFrameService frameService, IEventLogger eventLogger)
        {
            _frameService = frameService ?? throw new ArgumentNullException(nameof(frameService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var frameList = await _frameService.GetAll();

                if (frameList == null || !frameList.Any())
                {
                    _eventLogger.Log("No se encontró información de frames.", "FrameController.GetAll");
                    return Ok(new ResponseBase<List<Frame>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de frames.",
                        Data = null
                    });
                }

                _eventLogger.Log("Lista de frames obtenida correctamente.", "FrameController.GetAll");
                return Ok(new ResponseBase<List<Frame>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Frames obtenidos correctamente.",
                    Data = frameList
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "FrameController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var frame = await _frameService.GetById(id);

                if (frame == null)
                {
                    _eventLogger.Log($"Frame con ID {id} no encontrado.", "FrameController.GetById");
                    return NotFound(new ResponseBase<Frame>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Frame con ID {id} no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Frame con ID {id} obtenido correctamente.", "FrameController.GetById");
                return Ok(new ResponseBase<Frame>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Frame obtenido correctamente.",
                    Data = frame
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "FrameController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Frame request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de frame.", "FrameController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var response = await _frameService.Add(request);

                if (response.Code == 409) // Ya existe
                {
                    _eventLogger.Log($"El frame '{request.FrameName}' ya existe.", "FrameController.Create");
                    return Conflict(new ResponseBase<object>
                    {
                        Code = 409,
                        Message = response.Message,
                        Data = null
                    });
                }

                if (response.Code == 500 || response.Data == null)
                {
                    _eventLogger.Log("Error interno al crear el frame.", "FrameController.Create");
                    return StatusCode(500, new ResponseBase<object>
                    {
                        Code = 500,
                        Message = "Error interno al crear el frame.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Frame creado correctamente con ID {response.Data.FrameId}.", "FrameController.Create");
                return CreatedAtAction(nameof(GetById), new { id = response.Data.FrameId }, new ResponseBase<Frame>
                {
                    Code = 201,
                    Message = response.Message,
                    Data = response.Data
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "FrameController.Create");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Excepción inesperada.",
                    Data = null
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Frame request)
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

                if (id != request.FrameId)
                {
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "El ID proporcionado no coincide con el ID del frame."
                    });
                }

                var response = await _frameService.Update(request);

                if (response.Code == 404)
                {
                    return NotFound(response);
                }
                if (response.Code == 409)
                {
                    return Conflict(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "FrameController.Update");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = "Error interno del servidor."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _frameService.Delete(id);

                if (!deleted)
                {
                    _eventLogger.Log($"Frame con ID {id} no encontrado para eliminación.", "FrameController.Delete");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "El frame ya está eliminado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Frame con ID {id} eliminado correctamente.", "FrameController.Delete");
                return Ok(new ResponseBase<object>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Frame eliminado correctamente.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "FrameController.Delete");
                throw;
            }
        }
    }
}