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
    public class MenuPermissionController : Controller
    {
        private readonly IMenuPermissionService _service;
        private readonly IEventLogger _eventLogger;

        public MenuPermissionController(IMenuPermissionService service, IEventLogger eventLogger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet("GetUserPermissions")]
        public async Task<IActionResult> GetUserAsync([FromQuery] long userId)
        {
            try
            {
                var permissions = await _service.GetUserAsync(userId);

                if (permissions == null || !permissions.Any())
                {
                    _eventLogger.Log($"No se encontró información de permisos de menú para el usuario {userId}.", "MenuPermissionController.GetUserAsync");
                    return Ok(new ResponseBase<List<MenuPermission>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de permisos de menú.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Permisos de menú obtenidos correctamente para el usuario {userId}.", "MenuPermissionController.GetUserAsync");
                return Ok(new ResponseBase<List<MenuPermission>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Permisos de menú obtenidos correctamente.",
                    Data = permissions
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuPermissionController.GetUserAsync");
                return StatusCode(500, new ResponseBase<string>
                {
                    Code = 500,
                    Message = "Error interno al obtener los permisos.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("InsertOrUpdate")]
        public async Task<IActionResult> InsertOrUpdate([FromBody] List<MenuPermission> permissions, [FromQuery] long userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para insertar o actualizar permisos de menú.", "MenuPermissionController.InsertOrUpdate");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var result = await _service.InsertOrUpdateAsync(permissions, userId);

                _eventLogger.Log($"Permisos de menú insertados o actualizados correctamente para el usuario {userId}.", "MenuPermissionController.InsertOrUpdate");
                return Ok(new ResponseBase<List<MenuPermission>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Permisos de menú insertados o actualizados correctamente.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuPermissionController.InsertOrUpdate");
                return StatusCode(500, new ResponseBase<string>
                {
                    Code = 500,
                    Message = "Error interno al guardar los permisos.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetRolePermissions")]
        public async Task<IActionResult> GetRoleAsync([FromQuery] int roleId)
        {
            try
            {
                var permissions = await _service.GetByRoleAsync(roleId);

                if (permissions == null || !permissions.Any())
                {
                    _eventLogger.Log($"No se encontró información de permisos de menú para el rol {roleId}.", "MenuPermissionController.GetRoleAsync");
                    return Ok(new ResponseBase<List<MenuPermission>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de permisos de menú.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Permisos de menú obtenidos correctamente para el rol {roleId}.", "MenuPermissionController.GetRoleAsync");
                return Ok(new ResponseBase<List<MenuPermission>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Permisos de menú obtenidos correctamente.",
                    Data = permissions
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuPermissionController.GetRoleAsync");
                return StatusCode(500, new ResponseBase<string>
                {
                    Code = 500,
                    Message = "Error interno al obtener los permisos por rol.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("InsertOrUpdateByRole")]
        public async Task<IActionResult> InsertOrUpdateByRole([FromBody] List<MenuPermission> permissions, [FromQuery] int roleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para insertar o actualizar permisos de menú por rol.", "MenuPermissionController.InsertOrUpdateByRole");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var result = await _service.InsertOrUpdateByRoleAsync(permissions, roleId);

                _eventLogger.Log($"Permisos de menú insertados o actualizados correctamente para el rol {roleId}.", "MenuPermissionController.InsertOrUpdateByRole");
                return Ok(new ResponseBase<List<MenuPermission>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Permisos de menú por rol insertados o actualizados correctamente.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuPermissionController.InsertOrUpdateByRole");
                return StatusCode(500, new ResponseBase<string>
                {
                    Code = 500,
                    Message = "Error interno al guardar los permisos por rol.",
                    Data = ex.Message
                });
            }
        }

    }
}
