using System.Text;

namespace Ovr.Core.Infrastructures.Configs
{
    public static class ObtenerValores
    {
        public static string SmtpServer() => "smtp.gmail.com";
        public static int SmtpPort() => 587;
        public static string SmtpUser() => "edwinjvo@gmail.com";

        /// <summary>
        /// Devuelve la cadena de conexión codificada para el entorno indicado.
        /// 1 = Dev, 2 = QA, 3 = Prod
        /// </summary>
        public static string Conexion()
        {
            var entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            return entorno switch
            {
                //"development" => Decode("RGF0YSBTb3VyY2U9LlxTUUxFWFBSRVNTO0luaXRpYWwgQ2F0YWxvZz1PdnJEYjtVc2VyIElkPXNhO1Bhc3N3b3JkPTEyMzQ1Njc4O1RydXN0U2VydmVyQ2VydGlmaWNhdGU9dHJ1ZTs="),
                "development" => "Data Source=.;Initial Catalog=OvrDb;User Id=sa;Password=12345678;TrustServerCertificate=true;",
                "qa" => Decode("RGF0YSBTb3VyY2U9LlxcU1FMRVhQUkVTUztJbml0aWFsIENhdGFsb2c9T3ZyRGI7VXNlciBJZD1zYTtQYXNzd29yZD0xMjM0NTY3O1RydXN0U2VydmVyQ2VydGlmaWNhdGU9dHJ1ZTs="),
                "production" => Decode("RGF0YSBTb3VyY2U9U1FMMTAwNC5zaXRlNG5vdy5uZXQ7SW5pdGlhbCBDYXRhbG9nPWRiX2FiYzAwNV9zYW92cjtVc2VyIElkPWRiX2FiYzAwNV9zYW92cl9hZG1pbjtQYXNzd29yZD1vdnJhZG1pbkAyMDI1JDtUcnVzdFNlcnZlckNlcnRpZmljYXRlPXRydWU7"),
                _ => throw new InvalidOperationException("No se detectó un entorno válido (dev, qa, prod).")
            };
        }

        /// <summary>
        /// Devuelve el ApiBaseUrl por entorno.
        /// </summary>
        public static string ApiBaseUrl()
        {
            var entorno = "development";//Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            return entorno switch
            {
                "development" => "https://localhost:7284",
                "qa" => "https://qa.api.ovrapp.com",
                "production" => "https://api.ovrapp.com",
                _ => throw new InvalidOperationException("No se detectó un entorno válido (dev, qa, prod).")
            };
        }

        /// <summary>
        /// Devuelve el ApiBaseOrigen por entorno.
        /// </summary>
        public static string ApiBaseOrigen()
        {
            var entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            return entorno switch
            {
                "development" => "https://localhost:7255",
                "qa" => "https://qa.ovrapp.com",
                "production" => "https://ovrapp.com",
                _ => throw new InvalidOperationException("No se detectó un entorno válido (dev, qa, prod).")
            };
        }

        /// <summary>
        /// Devuelve la contraseña del correo, protegida.
        /// </summary>
        public static string SmtpPassword()
        {
            return "pllm chms pesx lvoc"; //Decode("bmNwcSBpYmdjIGtoZnYgbWNjYg==");
        }

        /// <summary>
        /// Otros valores secretos pueden agregarse aquí
        /// </summary>
        public static string JwtSettingKey()
        {
            return Decode("QjFBOUMyQTEtQjhFQi00MTA2LThERjgtMkEwQUE3MDQzNTcx");
        }

        /// <summary>
        /// Devuelve el Issuer del token JWT (común para todos los entornos)
        /// </summary>
        public static string JwtIssuer()
        {
            return "OVR-API"; // o cámbialo por otro si prefieres
        }

        /// <summary>
        /// Devuelve el Audience del token JWT (común para todos los entornos)
        /// </summary>
        public static string JwtAudience()
        {
            return "OVR-CLIENT"; // o el que uses desde el frontend
        }

        /// <summary>
        /// Decodifica un valor en Base64.
        /// </summary>
        private static string Decode(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
    }
}
