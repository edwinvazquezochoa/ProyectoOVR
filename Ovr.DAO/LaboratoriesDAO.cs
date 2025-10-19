using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class LaboratoriesDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<(int LaboratoryId, string ErrorCode)> Insert(Laboratory entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = CreateCommand("SP_CRUD_Laboratory", connection);
                command.Parameters.AddWithValue("@Accion", "Insertar");
                command.Parameters.AddWithValue("@LaboratoryName", entity.LaboratoryName);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);

                var id = Convert.ToInt32(await command.ExecuteScalarAsync());
                return (id, null);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                return (0, "DUPLICATE");
            }
            catch
            {
                return (0, "ERROR");
            }
        }

        public static async Task<ResponseBase<Laboratory>> Update(Laboratory entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Laboratory", connection);
            command.Parameters.AddWithValue("@Accion", "Actualizar");
            command.Parameters.AddWithValue("@LaboratoryId", entity.LaboratoryId);
            command.Parameters.AddWithValue("@LaboratoryName", entity.LaboratoryName);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);

            await command.ExecuteNonQueryAsync();

            return new ResponseBase<Laboratory>(200, "Laboratorio actualizado correctamente.", entity);
        }

        public static async Task<List<Laboratory>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Laboratory", connection);
            command.Parameters.AddWithValue("@Accion", "ObtenerTodo");

            return await GetRows(command);
        }

        public static async Task<Laboratory?> GetById(int id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Laboratory", connection);
            command.Parameters.AddWithValue("@Accion", "ObtenerPorId");
            command.Parameters.AddWithValue("@LaboratoryId", id);

            var result = await GetRows(command);
            return result.FirstOrDefault();
        }

        private static SqlCommand CreateCommand(string sp, SqlConnection conn)
        {
            return new SqlCommand(sp, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        private static async Task<List<Laboratory>> GetRows(SqlCommand command)
        {
            var list = new List<Laboratory>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Laboratory
                {
                    LaboratoryId = Convert.ToInt32(reader["LaboratoryId"]),
                    LaboratoryName = reader["LaboratoryName"]?.ToString() ?? "",
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }
            return list;
        }
    }
}
