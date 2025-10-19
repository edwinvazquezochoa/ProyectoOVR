using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class RoleDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;
        public static async Task<(long id, string ErrorCode)> Insert(Role entity)
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

                    using (SqlCommand command = CreateCommand("InsertRole", connection))
                    {
                        command.Parameters.Add("@RoleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@RoleName", entity.RoleName);
                        command.Parameters.AddWithValue("@IsActive", true); 

                        await command.ExecuteNonQueryAsync();

                        var id = Convert.ToInt64(command.Parameters["@LensId"].Value ?? 0);
                        return (id, null);  
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                return (0, "DUPLICATE"); 
            }
            catch (Exception)
            {
                return (0, "ERROR");  
            }
        }
        public static async Task<ResponseBase<Role>> Update(Role entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("UpdateRole", connection))
                {
                    // Parámetros de entrada
                    command.Parameters.AddWithValue("@RoleId", entity.RoleId);
                    command.Parameters.AddWithValue("@RoleName", entity.RoleName);
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

                    return new ResponseBase<Role>(resultCode, resultMessage, entity);
                }
            }
        }
        public static async Task<bool> Delete(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del Role debe ser mayor que 0.", nameof(id));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("DeleteRole", connection))
                    {
                        command.Parameters.AddWithValue("@RoleId", id);
                        var result = await command.ExecuteScalarAsync();
                        int affectedRows = Convert.ToInt32(result);
                        return affectedRows>0 ; // Devuelve el resultado real (0 o 1)
                    }
                }
            }
            catch (Exception ex)
            {
                return false; // Error inesperado
            }
        }
        public static async Task<List<Role?>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllRoles", connection))
                {
                    return await GetRow(command);
                }
            }
        }
        public static async Task<Role?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetRoleById", connection))
                {
                    command.Parameters.AddWithValue("@RoleId", id);
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
        private static async Task<List<Role?>> GetRow(SqlCommand command)
        {
            var entities = new List<Role?>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    entities.Add(new Role
                    {
                        RoleId = reader["RoleId"] == DBNull.Value ? 0 : (int)reader["RoleId"],
                        RoleName = reader["RoleName"] == DBNull.Value ? string.Empty : (string)reader["RoleName"],
                        IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) && reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    });
                }
            }
            return entities;
        }
    }
}
