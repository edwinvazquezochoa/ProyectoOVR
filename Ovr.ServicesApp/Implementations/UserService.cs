using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Core.Infrastructures.Utils;
using Ovr.Core.Infrastructures.Utils.Emails;
using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using Ovr.DaoServices.Interfaces;
using System.Net;

namespace Ovr.DaoServices.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IEventLogger _eventLogger;

        public UserService(IEmailService emailService, IEventLogger eventLogger)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
        }

        /// <summary>
        /// Agrega un nuevo usuario y envía un correo de verificación.
        /// </summary>
        public async Task<ResponseBase<User>> Create(User model)
        {
            try
            {
                // Validaciones iniciales
                if (model == null || string.IsNullOrEmpty(model.Email))
                {
                    _eventLogger.Log("El modelo de usuario es inválido. El correo electrónico es obligatorio.", "UserService.Create");
                    return new ResponseBase<User>((int)HttpStatusCode.BadRequest, "El modelo de usuario es inválido. El correo electrónico es obligatorio.");
                }

                // Verificar si el correo ya existe
                User? emailExists = await UserDao.EmailUserExists(model.Email);
                if (emailExists != null)
                {
                    _eventLogger.Log($"El correo {model.Email} ya está registrado.", "UserService.Create");
                    return new ResponseBase<User>((int)HttpStatusCode.Conflict, $"El correo {model.Email} ya está registrado.");
                }

                // Obtener información de la persona asociada
                Person? personInfo = await PersonsDAO.GetById(model.PersonId);
                if (personInfo != null)
                {
                    model.ShortName = personInfo.ShortName;
                    model.FullName = personInfo.FullName;
                }

                // Obtener información del rol asociado
                Role? roleInfo = await RoleDAO.GetById(model.RoleId);
                if (roleInfo != null)
                {
                    model.RoleName = roleInfo.RoleName;
                }

                // Generar contraseña temporal
                string temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
                model.PasswordHash = PasswordHelper.HashBCryptPassword(temporaryPassword);

                // Generar token de verificación
                model.VerificationToken = Guid.NewGuid().ToString();
                model.TokenExpirationDate = DateTime.UtcNow.AddHours(24);

                // Insertar usuario en la base de datos
                long userId = await UserDao.Insert(model);
                if (userId > 0)
                {
                    model.UserId = userId;

                    // Generar cuerpo del correo
                    var emailBody = $@"Hola {model.FullName},<br/><br/>
            Tu contraseña temporal es: <strong>{temporaryPassword}</strong>.<br/><br/>
            Por favor verifica tu correo haciendo clic en el siguiente enlace:<br/>
            <a href='https://example.com/verificar?token={model.VerificationToken}' target='_blank'>Verificar correo</a><br/><br/>
            Este enlace expirará en 24 horas.";

                    // Enviar correo al usuario
                    await _emailService.SendEmailAsync(model.Email, "Bienvenido a OVR - Verifica tu correo", emailBody);

                    // Registro en logs
                    _eventLogger.Log($"Usuario con ID {userId} creado y correo de verificación enviado a {model.Email}.", "UserService.Create");

                    return new ResponseBase<User>((int)HttpStatusCode.Created, "Usuario creado correctamente.", model);
                }

                // Si no se insertó el usuario, devolver error
                return new ResponseBase<User>((int)HttpStatusCode.InternalServerError, "Error al crear el usuario. Intente nuevamente.");
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.Create");
                return new ResponseBase<User>((int)HttpStatusCode.InternalServerError, "Error interno del servidor.");
            }
        }


        /// <summary>
        /// Actualiza la información de un usuario.
        /// </summary>
        public async Task<ResponseBase<User>> Update(User model)
        {
            return await UserDao.Update(model);
        }

        public async Task UpdatePasswordAndTempStatus(long userId, string passwordHash, bool isPasswordTemp)
        {
            await UserDao.UpdatePasswordAndTempStatus(userId, passwordHash, isPasswordTemp);
        }

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        public async Task<bool> Delete(long id)
        {
            try
            {
                var deleted = await UserDao.Delete(id);
                if (!deleted) throw new InvalidOperationException($"No se pudo eliminar el usuario con ID {id}.");

                _eventLogger.Log($"Usuario con ID {id} eliminado correctamente.", "UserService.Delete");
                return deleted;
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.Delete");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        public async Task<User?> GetById(long id)
        {
            try
            {
                return await UserDao.GetById(id)
                       ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.GetById");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        public async Task<List<User?>?> GetAll()
        {
            try
            {
                var users = await UserDao.GetAll();
                return users ?? new List<User?>();
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.GetAll");
                throw;
            }
        }

        /// <summary>
        /// Verifica si un correo ya existe en el sistema.
        /// </summary>
        public async Task<User?> EmailExists(string email, long? userId = null)
        {
            try
            {
                return await UserDao.EmailUserExists(email, userId);
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.EmailExists");
                throw;
            }
        }

        /// <summary>
        /// Verifica el token de correo electrónico.
        /// </summary>
        public async Task<bool> VerifyEmailToken(string token)
        {
            try
            {
                var user = await UserDao.VerifyToken(token);

                if (user == null) return false;

                var isActivated = await UserDao.ActivateUserByToken(token);
                _eventLogger.Log($"Usuario con token {token} activado correctamente.", "UserService.VerifyEmailToken");

                return isActivated;
            }
            catch (Exception ex)
            {
                _eventLogger.LogException(ex, "UserService.VerifyEmailToken");
                throw;
            }
        }
    }
}
