using Ovr.Core.Infrastructures.Configs;
using System.Net;
using System.Net.Mail;

namespace Ovr.Core.Infrastructures.Utils.Emails
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public EmailService()
        {
            _smtpServer = ObtenerValores.SmtpServer();
            _smtpPort = ObtenerValores.SmtpPort();
            _smtpUser = ObtenerValores.SmtpUser();
            _smtpPassword = ObtenerValores.SmtpPassword();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUser, "Sistema OVR"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpFailedRecipientException ex)
            {
                Console.WriteLine($"El correo {toEmail} no existe o no puede ser entregado. Detalles: {ex.Message}");
                throw new Exception($"El correo {toEmail} no existe o es inválido.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                throw new Exception("No se pudo enviar el correo.", ex);
            }
        }
    }
}
