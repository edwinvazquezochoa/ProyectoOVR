using System.Net;
using System.Net.Mail;

namespace Ovr.Core.Infrastructures.Utils
{
    public static class EmailControls
    {
        /// <summary>
        /// Envía un correo electrónico usando Gmail.
        /// </summary>
        /// <param name="toEmail">Dirección del destinatario.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del correo.</param>
        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Configuración del cliente SMTP
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("edwinjvo@gmail.com", "eejwtulixiqyvyrp"); // Tu correo y contraseña de aplicación
                    smtpClient.EnableSsl = true;

                    // Configuración del mensaje
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("edwinjvo@gmail.com", "Sistema OVR"), // Dirección fija del remitente
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    // Enviar correo
                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpFailedRecipientException ex)
            {
                // Manejo específico para correos inexistentes
                Console.WriteLine($"El correo {toEmail} no existe o no puede ser entregado. Detalles: {ex.Message}");
                throw new Exception($"El correo {toEmail} no existe o es inválido.", ex);
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                throw new Exception("No se pudo enviar el correo.", ex);
            }
        }
    }
}
