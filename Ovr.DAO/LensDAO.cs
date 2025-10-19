using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class LensesDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<(long LensId, string ErrorCode)> Insert(Len entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = CreateCommand("InsertLens", connection))
                    {
                        command.Parameters.Add("@LensId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@LensName", entity.LensName);
                        //command.Parameters.AddWithValue("@IsActive", true); // Asignado directamente como activo

                        await command.ExecuteNonQueryAsync();

                        var lensId = Convert.ToInt64(command.Parameters["@LensId"].Value ?? 0);
                        return (lensId, null); // ✅ OK
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                return (0, "DUPLICATE"); // ⚠️ Nombre ya existe
            }
            catch (Exception)
            {
                return (0, "ERROR"); // ❌ Otro error
            }
        }


        public static async Task<ResponseBase<Len>> Update(Len entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = CreateCommand("UpdateLens", connection);
            command.Parameters.AddWithValue("@LensId", entity.LensId);
            command.Parameters.AddWithValue("@LensName", entity.LensName);
            //command.Parameters.AddWithValue("@IsActive", true);

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(resultCodeParam);
            command.Parameters.Add(resultMessageParam);

            await command.ExecuteNonQueryAsync();

            return new ResponseBase<Len>(
                (int)resultCodeParam.Value,
                (string)resultMessageParam.Value,
                entity
            );
        }

        public static async Task<bool> Delete(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del lente debe ser mayor que 0.", nameof(id));

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = CreateCommand("DeleteLens", connection);
                command.Parameters.AddWithValue("@LensId", id);
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [DAO] Error al eliminar el lente: {ex.Message}");
                return false;
            }
        }

        public static async Task<List<Len>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = CreateCommand("GetAllLenses", connection);
            return await GetRow(command);
        }

        public static async Task<Len?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = CreateCommand("GetLensById", connection);
            command.Parameters.AddWithValue("@LensId", id);
            var lenses = await GetRow(command);
            return lenses.FirstOrDefault();
        }

        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        private static async Task<List<Len>> GetRow(SqlCommand command)
        {
            var lenses = new List<Len>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lenses.Add(new Len
                {
                    LensId = reader["LensId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LensId"]),
                    LensName = reader["LensName"]?.ToString() ?? string.Empty
                });
            }
            return lenses;
        }
    }
}
