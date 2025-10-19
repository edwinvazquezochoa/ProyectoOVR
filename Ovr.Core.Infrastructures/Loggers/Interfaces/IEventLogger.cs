namespace Ovr.Core.Infrastructures.Loggers.Interfaces
{
    public interface IEventLogger
    {
        /// <summary>
        /// Registra una excepción en los logs.
        /// </summary>
        /// <param name="ex">Excepción a registrar.</param>
        /// <param name="context">Contexto adicional para el log.</param>
        void LogException(Exception ex, string context);

        /// <summary>
        /// Registra un mensaje simple en los logs.
        /// </summary>
        /// <param name="message">Mensaje a registrar.</param>
        /// <param name="context">Contexto adicional para el log.</param>
        void Log(string message, string context);
    }
}
