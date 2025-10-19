using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using System.Data;

namespace Ovr.DAO
{
    public static class MenuDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<long> Insert(Menu model)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("InsertMenu", connection))
                {
                    // Parámetros
                    command.Parameters.Add("@MenuId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.AddWithValue("@MenusName", model.MenuName);
                    command.Parameters.AddWithValue("@ParentMenuId", (object?)model.ParentMenuId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OrderNumber", model.OrderNumber);
                    command.Parameters.AddWithValue("@Controller", (object?)model.Controller ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Action", (object?)model.Action ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);

                    await command.ExecuteNonQueryAsync();
                    return Convert.ToInt64(command.Parameters["@MenuId"].Value ?? 0);
                }
            }
        }

        public static async Task<bool> Update(Menu model)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("UpdateMenu", connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@MenuId", model.MenuId);
                    command.Parameters.AddWithValue("@MenusName", model.MenuName);
                    command.Parameters.AddWithValue("@ParentMenuId", (object?)model.ParentMenuId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OrderNumber", model.OrderNumber);
                    command.Parameters.AddWithValue("@Controller", (object?)model.Controller ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Action", (object?)model.Action ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public static async Task<bool> Delete(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del menú debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("DeleteMenu", connection))
                {
                    command.Parameters.AddWithValue("@MenuId", id);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public static async Task<List<Menu>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllMenus", connection))
                {
                    return await GetRows(command);
                }
            }
        }

        public static async Task<Menu?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetMenuById", connection))
                {
                    command.Parameters.AddWithValue("@MenuId", id);
                    var entities = await GetRows(command);
                    return entities.FirstOrDefault();
                }
            }
        }

        public static async Task<List<Menu>?> GetMenusByUserId(long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la cadena de conexión.");

            if (userId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0.", nameof(userId));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("SP_GetMenusByUserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    return await GetRows(command);
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

        private static async Task<List<Menu>> GetRows(SqlCommand command)
        {
            var models = new List<Menu>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    models.Add(new Menu
                    {
                        MenuId = reader["MenuId"] == DBNull.Value ? 0 : (int)reader["MenuId"],
                        MenuName = reader["MenusName"] == DBNull.Value ? string.Empty : (string)reader["MenusName"],
                        ParentMenuId = reader["ParentMenuId"] == DBNull.Value ? (int?)null : (int)reader["ParentMenuId"],
                        OrderNumber = reader["OrderNumber"] == DBNull.Value ? 0 : (int)reader["OrderNumber"],
                        Controller = reader["Controller"] == DBNull.Value ? null : (string)reader["Controller"],
                        Action = reader["Action"] == DBNull.Value ? null : (string)reader["Action"],
                        IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"]
                    });
                }
            }
            return models;
        }
    }
}
