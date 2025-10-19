using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using System.Data;
using System.Globalization;

namespace Ovr.DAO
{
    public static class DashBoardDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<int> TotalVentasUltimaSemana()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("GetTotalVentasUltimaSemana", connection))
                    {
                        return (int)(await command.ExecuteScalarAsync() ?? 0);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task<string> TotalIngresosUltimaSemana()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("GetTotalIngresosUltimaSemana", connection))
                    {
                        var resultado = await command.ExecuteScalarAsync();
                        return Convert.ToString(resultado, new CultureInfo("es-MX")) ?? "0";
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> TotalProductos()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("GetTotalProductos", connection))
                    {
                        return (int)(await command.ExecuteScalarAsync() ?? 0);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            var resultado = new Dictionary<string, int>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("GetVentasUltimaSemana", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string fecha = reader["Fecha"] == DBNull.Value ? string.Empty : (string)reader["Fecha"];
                                int total = reader["Total"] == DBNull.Value ? 0 : (int)reader["Total"];
                                resultado.Add(fecha, total);
                            }
                        }
                    }
                }
                return resultado;
            }
            catch
            {
                throw;
            }
        }

        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }
    }
}
