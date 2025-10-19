namespace Ovr.Core.Infrastructures.Utils.Emails
{
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico.
        /// </summary>
        /// <param name="toEmail">Correo del destinatario.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del correo.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
