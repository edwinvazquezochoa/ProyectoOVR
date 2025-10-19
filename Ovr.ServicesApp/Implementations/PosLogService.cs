using Ovr.Domain.Config;
using Ovr.ServicesApp.Interfaces;

namespace GoNetPos.Services
{
    public class PosLogService : IPosLogService
    {
        private string path = GlobalConfig.PathLog;

        public void Add(Exception? ex = null, string? sLog = null)
        {
            //********************************************************************************
            // Registrar en log systems
            //********************************************************************************
            string? message = sLog;

            if (ex != null)
            {
                // Qué ha sucedido
                var logMessage = "Fecha y Hora: " + DateTime.Now.ToString() + Environment.NewLine;
                logMessage = logMessage + "¿Qué ha sucedido? " + Environment.NewLine;
                logMessage = logMessage + "Error message: " + ex.Message;

                // Información sobre la excepción interna
                logMessage = logMessage + Environment.NewLine + " Información sobre la excepción interna: ";
                if (ex.InnerException != null)
                {
                    logMessage = logMessage + Environment.NewLine + " Inner exception: " + ex.InnerException.Message + Environment.NewLine;
                }
                else
                {
                    logMessage = logMessage + " No se encotró información." + Environment.NewLine;
                }
                // Dónde ha sucedido
                logMessage = logMessage + " Dónde ha sucedido: " + Environment.NewLine;
                logMessage = logMessage + " Stack trace: " + Environment.NewLine + ex.StackTrace + Environment.NewLine;
                // Registrar log
                message = logMessage;
            }




            CreateDirectory();
            string nombre = GetNameFile();
            string? cadena = message;

            //cadena += DateTime.Now + " - " + sLog + Environment.NewLine;

            StreamWriter sw = new StreamWriter(path + "/" + nombre, true);
            sw.WriteLine(cadena);
            sw.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            sw.Close();

        }

        #region HELPER
        private string GetNameFile()
        {
            string nombre = "";

            nombre = "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";

            return nombre;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion
    }
}
