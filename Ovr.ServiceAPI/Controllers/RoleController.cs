using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using Ovr.DaoServices.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Ovr.ApiServices.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IEventLogger _eventLogger;
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService, IEventLogger eventLogger)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roleList = await _roleService.GetAll();

                if (roleList == null || !roleList.Any())
                {
                    _eventLogger.Log("No se encontró información de roles.", "RoleController.GetAll");
                    return Ok(new ResponseBase<List<Role>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de roles.",
                        Data = null
                    });
                }

                _eventLogger.Log("Lista de roles obtenida correctamente.", "RoleController.GetAll");
                return Ok(new ResponseBase<List<Role>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Roles obtenidos correctamente.",
                    Data = roleList
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "RoleController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var role = await _roleService.GetById(id);

                if (role == null)
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado.", "RoleController.GetById");
                    return NotFound(new ResponseBase<Role>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Rol con ID {id} no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Rol con ID {id} obtenido correctamente.", "RoleController.GetById");
                return Ok(new ResponseBase<Role>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Rol obtenido correctamente.",
                    Data = role
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "RoleController.GetById");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de lente.", "RoleController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var response = await _roleService.Add(request);

                if (response.Code == 409)
                {
                    _eventLogger.Log($"El lente '{request.RoleName}' ya existe.", "RoleController.Create");
                    return Conflict(new ResponseBase<object>
                    {
                        Code = 409,
                        Message = response.Message,
                        Data = null
                    });
                }

                if (response.Code == 500 || response.Data == null)
                {
                    _eventLogger.Log("Error interno al crear el lente.", "RoleController.Create");
                    return StatusCode(500, new ResponseBase<object>
                    {
                        Code = 500,
                        Message = "Error interno al crear el lente.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Role creado correctamente con ID {response.Data.RoleId}.", "RoleController.Create");
                return CreatedAtAction(nameof(GetById), new { id = response.Data.RoleId }, new ResponseBase<Role>
                {
                    Code = 201,
                    Message = response.Message,
                    Data = response.Data
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "RoleController.Create");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Excepción inesperada.",
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Role request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la actualización de rol.", "RoleController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                if (id != request.RoleId)
                {
                    _eventLogger.Log("El ID proporcionado no coincide con el ID del rol.", "RoleController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "El ID proporcionado no coincide con el ID del rol."));
                }

                // Llamar al servicio para actualizar el rol
                var response = await _roleService.Update(request);

                // Manejo de códigos de respuesta
                if (response.Code == 409) // El rol ya existe con otro ID
                {
                    _eventLogger.Log($"Error al actualizar el rol: {response.Message}", "RoleController.Update");
                    return Conflict(response);
                }
                if (response.Code == 404) // Rol no encontrado
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado para actualización.", "RoleController.Update");
                    return NotFound(response);
                }

                _eventLogger.Log($"Rol con ID {id} actualizado correctamente.", "RoleController.Update");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "RoleController.Update");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno del servidor."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _roleService.Delete(id);

                if (!deleted)
                {
                    _eventLogger.Log($"Rol con ID {id} no encontrado para eliminación.", "RoleController.Delete");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "El rol ya esta desactivado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Rol con ID {id} eliminado correctamente.", "RoleController.Delete");
                return Ok(new ResponseBase<object>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Rol deshabilitado correctamente.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "RoleController.Delete");
                throw;
            }
        }

    }
}
