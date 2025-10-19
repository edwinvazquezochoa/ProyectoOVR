using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ovr.DAO
{
    public static class FramesDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<(long FrameId, string ErrorCode)> Insert(Frame entity)
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

                    using (SqlCommand command = CreateCommand("InsertFrame", connection))
                    {
                        command.Parameters.Add("@FrameId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@FrameName", entity.FrameName);
                        command.Parameters.AddWithValue("@IsActive", entity.IsActive);

                        await command.ExecuteNonQueryAsync();

                        var frameId = Convert.ToInt64(command.Parameters["@FrameId"].Value ?? 0);
                        return (frameId, null); // ✅ OK
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




        public static async Task<ResponseBase<Frame>> Update(Frame entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("UpdateFrame", connection))
                {
                    command.Parameters.AddWithValue("@FrameId", entity.FrameId);
                    command.Parameters.AddWithValue("@FrameName", entity.FrameName);
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);

                    var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultCodeParam);

                    var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultMessageParam);

                    await command.ExecuteNonQueryAsync();

                    int resultCode = (int)resultCodeParam.Value;
                    string resultMessage = (string)resultMessageParam.Value;

                    return new ResponseBase<Frame>(resultCode, resultMessage, entity);
                }
            }
        }

        public static async Task<bool> Delete(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del Frame debe ser mayor que 0.", nameof(id));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("DeleteFrame", connection))
                    {
                        command.Parameters.AddWithValue("@FrameId", id);
                        var result = await command.ExecuteScalarAsync();
                        int affectedRows = Convert.ToInt32(result);
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [DAO] Error al eliminar el frame: {ex.Message}");
                return false;
            }
        }

        public static async Task<List<Frame?>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllFrames", connection))
                {
                    return await GetRow(command);
                }
            }
        }

        public static async Task<Frame?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetFrameById", connection))
                {
                    command.Parameters.AddWithValue("@FrameId", id);
                    var entities = await GetRow(command);
                    return entities.FirstOrDefault();
                }
            }
        }

        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        private static async Task<List<Frame?>> GetRow(SqlCommand command)
        {
            var entities = new List<Frame?>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    entities.Add(new Frame
                    {
                        FrameId = reader["FrameId"] == DBNull.Value ? 0 : (int)reader["FrameId"],
                        FrameName = reader["FrameName"] == DBNull.Value ? string.Empty : (string)reader["FrameName"],
                        IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) && reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    });
                }
            }
            return entities;
        }
    }
}
