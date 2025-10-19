using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class PersonsDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<(long PersonId, string ErrorCode)> Insert(Person entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = CreateCommand("SP_CRUD_Person", connection);
                command.Parameters.AddWithValue("@Accion", "Insertar");
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@MiddleName", (object?)entity.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@GenderId", entity.GenderId);
                command.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                command.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNumber", (object?)entity.PhoneNumber ?? DBNull.Value);

                // Nuevas columnas de dirección
                command.Parameters.AddWithValue("@AddressStreetNumber", (object?)entity.AddressStreetNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@AddressNeighborhood", (object?)entity.AddressNeighborhood ?? DBNull.Value);
                command.Parameters.AddWithValue("@AddressCity", (object?)entity.AddressCity ?? DBNull.Value);
                command.Parameters.AddWithValue("@AddressState", (object?)entity.AddressState ?? DBNull.Value);
                command.Parameters.AddWithValue("@AddressPostalCode", (object?)entity.AddressPostalCode ?? DBNull.Value);

                var personId = Convert.ToInt64(await command.ExecuteScalarAsync());
                return (personId, null);
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


        public static async Task<ResponseBase<Person>> Update(Person entity)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Person", connection);
            command.Parameters.AddWithValue("@Accion", "Actualizar");
            command.Parameters.AddWithValue("@PersonId", entity.PersonId);
            command.Parameters.AddWithValue("@FirstName", entity.FirstName);
            command.Parameters.AddWithValue("@LastName", entity.LastName);
            command.Parameters.AddWithValue("@MiddleName", (object?)entity.MiddleName ?? DBNull.Value);
            command.Parameters.AddWithValue("@GenderId", entity.GenderId);
            command.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)entity.PhoneNumber ?? DBNull.Value);

            // Nuevas columnas de dirección
            command.Parameters.AddWithValue("@AddressStreetNumber", (object?)entity.AddressStreetNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@AddressNeighborhood", (object?)entity.AddressNeighborhood ?? DBNull.Value);
            command.Parameters.AddWithValue("@AddressCity", (object?)entity.AddressCity ?? DBNull.Value);
            command.Parameters.AddWithValue("@AddressState", (object?)entity.AddressState ?? DBNull.Value);
            command.Parameters.AddWithValue("@AddressPostalCode", (object?)entity.AddressPostalCode ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();

            return new ResponseBase<Person>(200, "Persona actualizada correctamente.", entity);
        }

        public static async Task<List<Person>> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Person", connection);
            command.Parameters.AddWithValue("@Accion", "ObtenerTodo");

            return await GetRows(command);
        }

        public static async Task<Person?> GetById(long id)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_Person", connection);
            command.Parameters.AddWithValue("@Accion", "ObtenerPorId");
            command.Parameters.AddWithValue("@PersonId", id);

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

        private static async Task<List<Person>> GetRows(SqlCommand command)
        {
            var list = new List<Person>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Person
                {
                    PersonId = reader["PersonId"] == DBNull.Value ? 0 : Convert.ToInt64(reader["PersonId"]),
                    FirstName = reader["FirstName"]?.ToString() ?? "",
                    LastName = reader["LastName"]?.ToString() ?? "",
                    MiddleName = reader["MiddleName"]?.ToString(),
                    GenderId = Convert.ToInt32(reader["GenderId"]),
                    GenderName = reader["GenderName"]?.ToString() ?? "",
                    BirthDate = reader["BirthDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["BirthDate"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    Email = reader["Email"]?.ToString(),
                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                    ShortName = reader["ShortName"]?.ToString() ?? "",
                    FullName = reader["FullName"]?.ToString() ?? "",
                    AddressStreetNumber = reader["AddressStreetNumber"]?.ToString(),
                    AddressNeighborhood = reader["AddressNeighborhood"]?.ToString(),
                    AddressCity = reader["AddressCity"]?.ToString(),
                    AddressState = reader["AddressState"]?.ToString(),
                    AddressPostalCode = reader["AddressPostalCode"] == DBNull.Value ? null : reader["AddressPostalCode"].ToString()
                });
            }
            return list;
        }


    }
}
