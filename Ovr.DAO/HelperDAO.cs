// HelperDAO.cs
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using Ovr.Domain.Config;
using Ovr.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Ovr.DAO.Helpers
{
    public static class HelperDAO
    {
        private static readonly string? _connectionString = GlobalSettings.ConnectionString;
        public static ILogger? Logger { get; set; }

        public static async Task<ResponseBase<object?>> Crear(string storedProcedure, object model, Dictionary<string, SqlParameter>? parametrosExtra = null, int timeout = 30)
        {
            var mergedParams = MergeWithDefaultOutputs(parametrosExtra);
            return await EjecutarNonQueryConSalida(storedProcedure, model, mergedParams, timeout);
        }

        public static async Task<ResponseBase<T>> Crear<T>(string storedProcedure, T model, Dictionary<string, SqlParameter>? parametrosExtra = null, int timeout = 30)
        {
            var mergedParams = MergeWithDefaultOutputs(parametrosExtra);
            var response = await EjecutarNonQueryConSalida(storedProcedure, model!, mergedParams, timeout);
            return new ResponseBase<T>(response.Code, response.Message, model);
        }

        public static async Task<ResponseBase<T>> Actualizar<T>(string storedProcedure, T model, Dictionary<string, SqlParameter>? parametrosExtra = null, int timeout = 30)
        {
            var mergedParams = MergeWithDefaultOutputs(parametrosExtra);
            var response = await EjecutarNonQueryConSalida(storedProcedure, model!, mergedParams, timeout);
            return new ResponseBase<T>(response.Code, response.Message, model);
        }

        public static async Task<ResponseBase<bool>> Eliminar(string storedProcedure, object model, int timeout = 30)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(storedProcedure, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                AgregarParametros(cmd, model);

                await conn.OpenAsync();
                var result = await cmd.ExecuteNonQueryAsync();

                return new ResponseBase<bool>(0, "Registro eliminado correctamente", result > 0);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error al eliminar en SP {SP}", storedProcedure);
                return new ResponseBase<bool>(-1, $"Error al eliminar: {ex.Message}", false);
            }
        }

        public static async Task<ResponseBase<T?>> ObtenerPorId<T>(string storedProcedure, object model, Func<SqlDataReader, T> mapFunc, int timeout = 30)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(storedProcedure, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                AgregarParametros(cmd, model);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var result = mapFunc(reader);
                    return new ResponseBase<T?>(0, "Registro encontrado", result);
                }

                return new ResponseBase<T?>(1, "No se encontraron resultados", default);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error al consultar SP {SP}", storedProcedure);
                return new ResponseBase<T?>(-1, $"Error al consultar: {ex.Message}", default);
            }
        }

        public static async Task<ResponseBase<List<T>>> ObtenerLista<T>(string storedProcedure, object? model, Func<SqlDataReader, T> mapFunc, int timeout = 30)
        {
            var lista = new List<T>();

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(storedProcedure, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                if (model != null)
                    AgregarParametros(cmd, model);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(mapFunc(reader));
                }

                return new ResponseBase<List<T>>(0, "Consulta exitosa", lista);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error al consultar lista en SP {SP}", storedProcedure);
                return new ResponseBase<List<T>>(-1, $"Error al consultar lista: {ex.Message}", new());
            }
        }

        private static async Task<ResponseBase<object?>> EjecutarNonQueryConSalida(string storedProcedure, object model, Dictionary<string, SqlParameter>? parametrosExtra = null, int timeout = 30)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(storedProcedure, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                AgregarParametros(cmd, model);

                if (parametrosExtra != null)
                {
                    foreach (var kvp in parametrosExtra)
                    {
                        if (!cmd.Parameters.Contains(kvp.Key))
                            cmd.Parameters.Add(kvp.Value);
                    }
                }

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int resultCode = 0;
                string resultMessage = "Operación ejecutada correctamente";

                if (cmd.Parameters.Contains("@ResultCode"))
                    resultCode = Convert.ToInt32(cmd.Parameters["@ResultCode"].Value);
                if (cmd.Parameters.Contains("@ResultMessage"))
                    resultMessage = cmd.Parameters["@ResultMessage"].Value?.ToString() ?? resultMessage;

                return new ResponseBase<object?>(resultCode, resultMessage, null);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error en ejecución de SP {SP}", storedProcedure);
                return new ResponseBase<object?>(-1, $"Error: {ex.Message}", null);
            }
        }

        private static Dictionary<string, SqlParameter> MergeWithDefaultOutputs(Dictionary<string, SqlParameter>? extras)
        {
            var merged = new Dictionary<string, SqlParameter>
            {
                { "@ResultCode", new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output } },
                { "@ResultMessage", new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output } }
            };

            if (extras != null)
            {
                foreach (var kvp in extras)
                {
                    merged[kvp.Key] = kvp.Value; // Sobrescribe si ya existe
                }
            }

            return merged;
        }

        private static void AgregarParametros(SqlCommand cmd, object model)
        {
            foreach (PropertyInfo prop in model.GetType().GetProperties())
            {
                if (!EsTipoSoportado(prop.PropertyType)) continue;

                string nombre = "@" + prop.Name;
                object? valor = prop.GetValue(model) ?? DBNull.Value;

                if (!cmd.Parameters.Contains(nombre))
                    cmd.Parameters.AddWithValue(nombre, valor);
            }
        }

        private static bool EsTipoSoportado(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Guid) ||
                   type == typeof(decimal) ||
                   type == typeof(bool) ||
                   Nullable.GetUnderlyingType(type) != null;
        }
    }
}
