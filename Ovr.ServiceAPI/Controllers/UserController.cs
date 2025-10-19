using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Core.Infrastructures.Utils;
using Ovr.Core.Infrastructures.Utils.Emails;
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
    public class UserController : Controller
    {
        private readonly IEventLogger _eventLogger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserController(IEmailService emailService, IUserService userService, IEventLogger eventLogger)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _eventLogger = eventLogger;
        }

        // GET: user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<User?>? users = await _userService.GetAll();

                if (users == null || !users.Any())
                {
                    return Ok(new ResponseBase<List<User>>
                    {
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "No se encontraron usuarios.",
                        Data = null
                    });
                }

                //_eventLogger.Log($"Se obtuvieron {users.Count} usuarios.","UserController.GetAll");
                return Ok(new ResponseBase<List<User>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Usuarios obtenidos correctamente.",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.GetAll");
                throw; // Manejo global del middleware
            }
        }

        // GET: user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _userService.GetById(id);

                if (user == null)
                {
                    _eventLogger.Log($"Usuario con ID {id} no encontrado.", "UserController.GetUserById");
                    return NotFound(new ResponseBase<User>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = $"Usuario con ID {id} no encontrado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Usuario con ID {id} obtenido correctamente.", "UserController.GetUserById");
                return Ok(new ResponseBase<User>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Usuario obtenido correctamente.",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.GetUserById");
                throw; // Manejo global del middleware
            }
        }

        // POST: user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la creación de usuario.", "UserController.Create");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                // Verificar si el correo ya existe antes de proceder
                User emailExists = await _userService.EmailExists(user.Email);
                if (emailExists != null)
                {
                    _eventLogger.Log($"El correo {user.Email} ya está registrado.", "UserController.Create");
                    return Conflict(new ResponseBase<object>((int)HttpStatusCode.Conflict, $"El correo {user.Email} ya está registrado."));
                }

                // Llamar al servicio para crear el usuario
                var response = await _userService.Create(user);

                if (response.Code == 500 || response.Data == null)
                {
                    _eventLogger.Log("Error interno al crear el usuario.", "UserController.Create");
                    return StatusCode(500, new ResponseBase<object>
                    {
                        Code = 500,
                        Message = "Error interno al crear el usuario.",
                        Data = null
                    });
                }
                // Manejo de códigos de respuesta
                if (response.Code == 409) // Conflicto, usuario ya existente
                {
                    _eventLogger.Log($"Error al crear el usuario: {response.Message}", "UserController.Create");
                    return Conflict(response);
                }
                if (response.Code == 400) // Error en la solicitud
                {
                    _eventLogger.Log($"Error en la solicitud al crear usuario: {response.Message}", "UserController.Create");
                    return BadRequest(response);
                }


                _eventLogger.Log($"Usuario creado con ID {response.Data}.", "UserController.Create");
                return CreatedAtAction(nameof(GetUserById), new { id = response.Data }, response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.Create");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno del servidor."));
            }
        }



        // PUT: user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] User request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _eventLogger.Log("Modelo inválido para la actualización de rol.", "UserController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "Datos inválidos.", ModelState));
                }

                if (id != request.UserId)
                {
                    _eventLogger.Log("El ID proporcionado no coincide con el ID del usuario.", "UserController.Update");
                    return BadRequest(new ResponseBase<object>((int)HttpStatusCode.BadRequest, "El ID proporcionado no coincide con el ID del usuario."));
                }

                // Llamar al servicio para actualizar el rol
                var response = await _userService.Update(request);

                // Manejo de códigos de respuesta
                if (response.Code == 409) // El rol ya existe con otro ID
                {
                    _eventLogger.Log($"Error al actualizar el usurio: {response.Message}", "UserController.Update");
                    return Conflict(response);
                }
                if (response.Code == 404) // Rol no encontrado
                {
                    _eventLogger.Log($"Usurio con ID {id} no encontrado para actualización.", "UserController.Update");
                    return NotFound(response);
                }

                _eventLogger.Log($"Usuario con ID {id} actualizado correctamente.", "UserController.Update");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.Update");
                return StatusCode(500, new ResponseBase<object>((int)HttpStatusCode.InternalServerError, "Error interno del servidor."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                var deleted = await _userService.Delete(id);

                if (!deleted)
                {
                    _eventLogger.Log($"Usuario con ID {id} no encontrado.", "UserController.DeleteUser");
                    return NotFound(new ResponseBase<object>
                    {
                        Code = 404,
                        Message = "El usuario no existe o ya estaba eliminado.",
                        Data = null
                    });
                }

                _eventLogger.Log($"Usuario con ID {id} eliminado correctamente.", "UserController.DeleteUser");

                return Ok(new ResponseBase<object> // ✅ Devuelve 200 OK
                {
                    Code = 200,
                    Message = "Usuario eliminado correctamente.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.DeleteUser");

                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Error interno del servidor.",
                    Data = ex.Message
                });
            }
        }


        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            try
            {
                bool isVerified = await _userService.VerifyEmailToken(token);
                if (isVerified)
                {
                    return Ok(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.OK,
                        Message = "Correo verificado con éxito.",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseBase<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Token de verificación inválido o expirado.",
                        Data = null
                    });
                }

            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserController.VerifyEmail");
                throw;
            }
        }

        [HttpPost("resend-password")]
        public async Task<IActionResult> ResendTemporaryPassword(string email)
        {
            try
            {
                // Validar si el usuario existe
                User user = await _userService.EmailExists(email);

                if (user == null)
                {
                    return NotFound("El correo no está registrado.");
                }

                if (!user.IsPasswordTemp)
                {
                    return BadRequest("El usuario ya no tiene una contraseña temporal.");
                }

                // Generar una nueva contraseña temporal
                string newTemporaryPassword = PasswordHelper.GenerateTemporaryPassword();

                // Actualizar el hash de la contraseña en la base de datos
                user.PasswordHash = PasswordHelper.Hash256Password(newTemporaryPassword);
                user.IsPasswordTemp = true;

                await _userService.UpdatePasswordAndTempStatus(user.UserId, user.PasswordHash, user.IsPasswordTemp);

                // Preparar el correo con la nueva contraseña
                var emailBody = $@"
            <p>Hola <strong>{user.FullName}</strong>,</p>
            <p>Tu nueva contraseña temporal es:</p>
            <p style='font-size: 20px; color: #333; font-weight: bold;'>{newTemporaryPassword}</p>
            <p>Por favor, inicia sesión y cámbiala lo antes posible.</p>";

                // Enviar el correo
                await _emailService.SendEmailAsync(email, "Tu nueva contraseña temporal en OVR", emailBody);
                _eventLogger.Log($"La nueva contraseña temporal ha sido enviada..", "UserController.ResendTemporaryPassword");
                return Ok("La nueva contraseña temporal ha sido enviada.");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _eventLogger.LogException(ex, "UserController.ResendTemporaryPassword");
                throw; // Manejo global del middleware



            }
        }



    }
}
