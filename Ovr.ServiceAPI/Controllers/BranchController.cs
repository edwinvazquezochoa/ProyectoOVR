using Azure;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.ClientServices.Intefaces;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ovr.ApiServices.Controllers
{
    [Authorize]  
    [Route("[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IEventLogger _eventLogger;
        private readonly IBranchService _BranchService;

        public BranchController(IBranchService BranchService, IEventLogger eventLogger)
        {
            _BranchService = BranchService ?? throw new ArgumentNullException(nameof(BranchService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var BranchList = await _BranchService.GetAll();

                if (BranchList == null || !BranchList.Any())
                {
                    _eventLogger.Log("No se encontró información de Branchs.", "BranchController.GetAll");
                    return Ok(new ResponseBase<List<Branch>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de Branchs.",
                        Data = null
                    });
                }

                _eventLogger.Log("Lista de Branchs obtenida correctamente.", "BranchController.GetAll");
                return Ok(new ResponseBase<List<Branch>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Branchs obtenidos correctamente.",
                    Data = BranchList
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "BranchController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Branch = await _BranchService.GetById(id);

                if (Branch == null)
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado.", "BranchController.GetById");
                    return NotFound(new ResponseBase<Branch>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Rol con ID {id} no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Rol con ID {id} obtenido correctamente.", "BranchController.GetById");
                return Ok(new ResponseBase<Branch>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Rol obtenido correctamente.",
                    Data = Branch
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "BranchController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Branch request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de rol.", "BranchController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var response = await _BranchService.Create(request);

                // Manejo de códigos de respuesta
                if (response.Code == 409) // El rol ya existe con otro ID
                {
                    _eventLogger.Log($"Error al crear branche: {response.Message}", "BranchController.Create");
                    return Conflict(response);
                }
                _eventLogger.Log($"Brache {request.BrancheName} actualizado correctamente.", "BranchController.Create");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "BranchController.Create");
                throw;
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Branch request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la actualización de rol.", "BranchController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                if (id != request.BranchId)
                {
                    _eventLogger.Log("El ID proporcionado no coincide con el ID del rol.", "BranchController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "El ID proporcionado no coincide con el ID del rol."));
                }

                // Llamar al servicio para actualizar el rol
                var response = await _BranchService.Update(request);

                // Manejo de códigos de respuesta
                if (response.Code == 409) // El rol ya existe con otro ID
                {
                    _eventLogger.Log($"Error al actualizar el rol: {response.Message}", "BranchController.Update");
                    return Conflict(response);
                }
                if (response.Code == 404) // Rol no encontrado
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado para actualización.", "BranchController.Update");
                    return NotFound(response);
                }

                _eventLogger.Log($"Rol con ID {id} actualizado correctamente.", "BranchController.Update");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "BranchController.Update");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno del servidor."));
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _BranchService.Delete(id);

                if (!deleted)
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado para eliminación.", "BranchController.Delete");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "El rol ya esta desactivado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Rol con ID {id} eliminado correctamente.", "BranchController.Delete");
                return Ok(new ResponseBase<object>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Rol deshabilitado correctamente.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "BranchController.Delete");
                throw;
            }
        }

    }
}