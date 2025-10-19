using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class BranchDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<ResponseBase<Branch>> Create(Branch entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("InsertBranch", connection))
                {
                    // Parámetros
                    command.Parameters.Add("@NewBranchId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    command.Parameters.AddWithValue("@BrancheName", entity.BrancheName);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Phone", entity.Phone); // Agregado para el campo Phone
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    // Parámetros de salida
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

                    // Capturar valores de salida
                    int resultCode = (int)resultCodeParam.Value;
                    string resultMessage = (string)resultMessageParam.Value;

                    return new ResponseBase<Branch>(resultCode, resultMessage, entity);
                }
            }
        }

        public static async Task<ResponseBase<Branch>> Update(Branch entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("UpdateBranch", connection))
                {
                    // Parámetros de entrada
                    command.Parameters.AddWithValue("@BranchId", entity.BranchId);
                    command.Parameters.AddWithValue("@BrancheName", entity.BrancheName);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Phone", entity.Phone); // Agregado para el campo Phone
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);

                    // Parámetros de salida
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

                    // Capturar valores de salida
                    int resultCode = (int)resultCodeParam.Value;
                    string resultMessage = (string)resultMessageParam.Value;

                    return new ResponseBase<Branch>(resultCode, resultMessage, entity);
                }
            }
        }

        public static async Task<bool> Delete(long id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del Branch debe ser mayor que 0.", nameof(id));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    // Se actualiza el nombre del procedimiento a "DeleteBrach"
                    using (SqlCommand command = CreateCommand("DeleteBrach", connection))
                    {
                        command.Parameters.AddWithValue("@BranchId", id);

                        // Leer el resultado del procedimiento almacenado (0 = ya estaba inactivo, 1 = se desactivó)
                        var result = await command.ExecuteScalarAsync();

                        int affectedRows = Convert.ToInt32(result);
                        Console.WriteLine($"🔍 [DAO] Resultado de DeleteBrach: {affectedRows}");

                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [DAO] Error al eliminar la sucursal: {ex.Message}");
                return false;
            }
        }

        public static async Task<List<Branch?>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllBranch", connection))
                {
                    return await GetRow(command);
                }
            }
        }

        public static async Task<Branch?> GetById(long id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetByIdBranch", connection))
                {
                    command.Parameters.AddWithValue("@BranchId", id);
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

        private static async Task<List<Branch?>> GetRow(SqlCommand command)
        {
            var entities = new List<Branch?>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var branch = new Branch
                    {
                        BranchId = reader["BranchId"] == DBNull.Value ? 0 : (long)reader["BranchId"],
                        BrancheName = reader["BrancheName"] == DBNull.Value ? string.Empty : (string)reader["BrancheName"],
                        Address = reader["Address"] == DBNull.Value ? string.Empty : (string)reader["Address"],
                        Phone = reader["Phone"] == DBNull.Value ? string.Empty : (string)reader["Phone"],
                        IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) && reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    };

                    entities.Add(branch);
                }
            }
            return entities;
        }
    }
}
