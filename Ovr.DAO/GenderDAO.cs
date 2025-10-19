using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using System.Data;

namespace Ovr.DAO
{
    public static class GenderDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<long> Insert(Gender model)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("InsertGender", connection))
                {
                    // Parámetros
                    command.Parameters.Add("@GenderId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.AddWithValue("@GenderName", model.GenderName);
                    await command.ExecuteNonQueryAsync();
                    return Convert.ToInt64(command.Parameters["@GenderId"].Value ?? 0);
                }
            }
        }

        public static async Task<bool> Update(Gender model)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("UpdateGender", connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@GenderId", model.GenderId);
                    command.Parameters.AddWithValue("@GenderName", model.GenderName);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public static async Task<bool> Delete(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID del Role debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("DeleteGender", connection))
                {
                    command.Parameters.AddWithValue("@GenderId", id);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public static async Task<List<Gender?>?> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllGenders", connection))
                {
                    return await GetRow(command);
                }
            }
        }

        public static async Task<Gender?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(id));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetGenders", connection))
                {
                    command.Parameters.AddWithValue("@GenderId", id);
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

        private static async Task<List<Gender?>> GetRow(SqlCommand command)
        {
            var models = new List<Gender?>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    models.Add(new Gender
                    {
                        GenderId = reader["GenderId"] == DBNull.Value ? 0 : (int)reader["GenderId"],
                        GenderName = reader["GenderName"] == DBNull.Value ? string.Empty : (string)reader["GenderName"],
                    });
                }
            }
            return models;
        }
    }
}
