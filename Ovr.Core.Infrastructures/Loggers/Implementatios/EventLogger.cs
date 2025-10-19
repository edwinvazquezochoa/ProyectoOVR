using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Domain.Config;
using System.Text;

namespace Ovr.Core.Infrastructures.Loggers.Implementations
{
    public class EventLogger : IEventLogger
    {
        private readonly string? _path = GlobalSettings.PathLog;

        /// <summary>
        /// Registra una excepción en el log.
        /// </summary>
        /// <param name="ex">Excepción a registrar.</param>
        /// <param name="context">Contexto adicional para el log.</param>
        public void LogException(Exception ex, string context)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex), "La excepción no puede ser nula.");

            if (string.IsNullOrWhiteSpace(context))
                throw new ArgumentException("El contexto no puede ser nulo o vacío.", nameof(context));

            string message = BuildExceptionLog(ex, context);
            WriteLogToFile(message);
        }

        /// <summary>
        /// Registra un mensaje simple en el log.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        /// <param name="context">Contexto adicional para el log.</param>
        public void Log(string message, string context)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("El mensaje de log no puede estar vacío o ser nulo.", nameof(message));

            if (string.IsNullOrWhiteSpace(context))
                throw new ArgumentException("El contexto no puede ser nulo o vacío.", nameof(context));

            string logMessage = BuildSimpleLog(message, context);
            WriteLogToFile(logMessage);
        }

        #region Helper Methods

        private string BuildExceptionLog(Exception ex, string context)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Registro ID: {Guid.NewGuid()}");
            logMessage.AppendLine($"Fecha y Hora: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            logMessage.AppendLine($"Contexto: {context}");
            logMessage.AppendLine($"Mensaje de Error: {ex.Message}");

            if (ex.InnerException != null)
            {
                logMessage.AppendLine("Información sobre la excepción interna:");
                logMessage.AppendLine($"Mensaje Interno: {ex.InnerException.Message}");
            }

            logMessage.AppendLine("Traza del Error:");
            logMessage.AppendLine(ex.StackTrace);

            return logMessage.ToString();
        }

        private string BuildSimpleLog(string message, string context)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Registro ID: {Guid.NewGuid()}");
            logMessage.AppendLine($"Fecha y Hora: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            logMessage.AppendLine($"Contexto: {context}");
            logMessage.AppendLine($"Mensaje: {message}");

            return logMessage.ToString();
        }

        private void WriteLogToFile(string message)
        {
            if (string.IsNullOrWhiteSpace(_path))
                throw new InvalidOperationException("El directorio de logs no está configurado.");

            CreateDirectoryIfNotExists();

            string fileName = $"log_{DateTime.UtcNow:yyyy_MM_dd}.txt";
            string filePath = Path.Combine(_path, fileName);

            try
            {
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(message);
                    sw.WriteLine(new string('-', 120));
                }
            }
            catch (Exception fileEx)
            {
                // Manejar errores al escribir en el archivo
                Console.WriteLine($"Error al escribir en el archivo de log: {fileEx.Message}");
            }
        }

        private void CreateDirectoryIfNotExists()
        {
            if (!Directory.Exists(_path))
            {
                try
                {
                    Directory.CreateDirectory(_path);
                }
                catch (Exception ex)
                {
                    // Manejar errores al crear el directorio
                    Console.WriteLine($"Error al crear el directorio de logs: {ex.Message}");
                }
            }
        }

        #endregion
    }
}
