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
    public class MenuController : Controller
    {
        private readonly IEventLogger _eventLogger;
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService, IEventLogger eventLogger)
        {
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var menus = await _menuService.GetAll();

                if (menus == null || !menus.Any())
                {
                    _eventLogger.Log("No se encontró información de menu.", "MenuController.GetAll");
                    return Ok(new ResponseBase<List<Menu>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontró información de menu.",
                        Data = null
                    });
                }

                _eventLogger.Log("Lista de menu obtenida correctamente.", "MenuController.GetAll");
                return Ok(new ResponseBase<List<Menu>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Menus obtenidos correctamente.",
                    Data = menus
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.GetAll");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var menu = await _menuService.GetById(id);

                if (menu == null)
                {
                    _eventLogger.Log($"Menu con ID {id} no encontrado.", "MenuController.GetById");
                    return NotFound(new ResponseBase<Role>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Menu con ID {id} no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Menu con ID {id} obtenido correctamente.", "MenuController.GetById");
                return Ok(new ResponseBase<Menu>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Menu obtenido correctamente.",
                    Data = menu
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.GetById");
                throw;
            }
        }
 
        [HttpGet("ByUser/{userId}")]
        public async Task<IActionResult> ByUser(long userId)
        {
            try
            {
                var menus = await _menuService.GetMenusPermissionsByUserId(userId);

                if (menus == null || !menus.Any())
                {
                    _eventLogger.Log($"No se encontraron menús para el usuario ID {userId}.", "MenuController.GetMenusPermissionsByUserId");
                    return NotFound(new ResponseBase<List<Menu>>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"No se encontraron menús para el usuario ID {userId}.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Menús del usuario ID {userId} obtenidos correctamente.", "MenuController.GetMenusPermissionsByUserId");
                return Ok(new ResponseBase<List<Menu>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Menús y permisos obtenidos correctamente.",
                    Data = menus
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.GetMenusPermissionsByUserId");
                return StatusCode(500, new ResponseBase<List<Menu>>
                {
                    Code = 500,
                    Message = "Ocurrió un error al obtener los menús.",
                    Data = null
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Menu request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de menu.", "MenuController.Create");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                var id = await _menuService.Add(request);

                _eventLogger.Log($"Menu creado correctamente con ID {id}.", "MenuController.Create");
                return CreatedAtAction(nameof(GetById), new { id = id }, new ResponseBase<long>
                {
                    Code = (int)HttpStatusCode.Created,
                    Message = "Menu creado correctamente.",
                    Data = id
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.Create");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Menu request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la actualización de menu.", "MenuController.Update");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Datos inválidos.",
                        Data = ModelState
                    });
                }

                if (id != request.MenuId)
                {
                    _eventLogger.Log("El ID proporcionado no coincide con el ID del menu.", "MenuController.Update");
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "El ID proporcionado no coincide con el ID del menu.",
                        Data = null
                    });
                }

                var updated = await _menuService.Update(request);

                if (!updated)
                {
                    _eventLogger.Log($"Menu con ID {id} no encontrado para actualización.", "MenuController.Update");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Menu no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Menu con ID {id} actualizado correctamente.", "MenuController.Update");
                return NoContent();
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.Update");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _menuService.Delete(id);

                if (!deleted)
                {
                    _eventLogger.Log($"Menu con ID {id} no encontrado para eliminación.", "MenuController.Delete");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Menu no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Menu con ID {id} eliminado correctamente.", "MenuController.Delete");
                return NoContent();
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "MenuController.Delete");
                throw;
            }
        }
    }
}