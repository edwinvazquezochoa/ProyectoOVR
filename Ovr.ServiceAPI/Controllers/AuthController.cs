using GoNetPos.Ovr.ApiServices.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Core.Infrastructures.Utils;
using Ovr.Core.Infrastructures.Utils.Emails;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Requests;
using Ovr.Domain.Responses;
using System.Net;

namespace Ovr.ApiServices.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IEventLogger _eventLogger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly SecurityUtils _SecurityUtils;

        public AuthController(
            IEmailService emailService,
            IUserService userService,
            IAuthService authService,
            IEventLogger eventLogger,
            SecurityUtils securityUtils)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
            _SecurityUtils = securityUtils ?? throw new ArgumentNullException(nameof(securityUtils));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseBase<string>
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Solicitud inválida. Verifique los datos enviados.",
                    Data = null
                });
            }

            try
            {
                var userInfo = await _authService.VerifyUserCredentialsAsync(request.Email, request.Password);

                if (userInfo == null)
                {
                    return Unauthorized(new ResponseBase<UserInfo>
                    {
                        Code = (int)HttpStatusCode.Unauthorized,
                        Message = "Correo o contraseña incorrectos.",
                        Data = null
                    });

                }

                string jwtToken = _SecurityUtils.GenerateJwtToken(userInfo);

                return Ok(new ResponseBase<string>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "Inicio de sesión exitoso.",
                    Data = jwtToken
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "AuthController.Login");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseBase<string>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = "Ocurrió un error interno. Por favor, inténtelo más tarde.",
                    Data = null
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> forgotpassword([FromQuery] string email)
        {
            try
            {
                // Validar si el usuario existe
                User user = await _userService.EmailExists(email);

                if (user == null)
                {
                    return NotFound(new ResponseBase<string>
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "El correo no está registrado.",
                        Data = null
                    });
                }

                if (!user.IsPasswordTemp)
                {
                    return BadRequest(new ResponseBase<string>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "El usuario ya no tiene una contraseña temporal.",
                        Data = null
                    });
                }

                // Generar nueva contraseña
                string newTemporaryPassword = PasswordHelper.GenerateTemporaryPassword();
                user.PasswordHash = PasswordHelper.Hash256Password(newTemporaryPassword);
                user.IsPasswordTemp = true;

                await _userService.UpdatePasswordAndTempStatus(user.UserId, user.PasswordHash, user.IsPasswordTemp);

                // Preparar cuerpo del correo
                var emailBody = $@"
                    <p>Hola <strong>{user.FullName}</strong>,</p>
                    <p>Tu nueva contraseña temporal es:</p>
                    <p style='font-size: 20px; color: #333; font-weight: bold;'>{newTemporaryPassword}</p>
                    <p>Por favor, inicia sesión y cámbiala lo antes posible.</p>";

                await _emailService.SendEmailAsync(email, "Tu nueva contraseña temporal en OVR", emailBody);

                _eventLogger.Log("La nueva contraseña temporal ha sido enviada.", "AuthController.ForgotPassword ");

                return Ok(new ResponseBase<string>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "La nueva contraseña temporal ha sido enviada.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "AuthController.ForgotPassword ");

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseBase<string>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = "Ocurrió un error al reenviar la contraseña temporal.",
                    Data = null
                });
            }
        }
    }
}
