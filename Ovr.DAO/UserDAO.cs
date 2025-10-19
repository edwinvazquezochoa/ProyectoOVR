using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class UserDao
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString; 

        public static async Task<long> Insert(User entity)
        {
            if(_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("InsertUser", connection))
                {
                    // Parámetros
                    command.Parameters.Add("@UserId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    command.Parameters.AddWithValue("@PersonId", entity.PersonId);
                    command.Parameters.AddWithValue("@Username", entity.Username);
                    command.Parameters.AddWithValue("@Email", entity.Email);
                    command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                    command.Parameters.AddWithValue("@RoleId", entity.RoleId);
                    command.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    command.Parameters.AddWithValue("@VerificationToken", entity.VerificationToken);
                    command.Parameters.AddWithValue("@TokenExpirationDate", entity.TokenExpirationDate);
                    command.Parameters.AddWithValue("@BranchId", entity.BranchId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsGlobal", entity.IsGlobal);
                    await command.ExecuteNonQueryAsync();
                    return Convert.ToInt64(command.Parameters["@UserId"].Value ?? 0);
                }
            }
        }

        public static async Task<ResponseBase<User>?> Update(User entity, bool returnJson = false)
        {
            // Validaciones previas
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = CreateCommand("UpdateUser", connection))
                    {
                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@UserId", entity.UserId);
                        command.Parameters.AddWithValue("@PersonId", entity.PersonId);
                        command.Parameters.AddWithValue("@Username", entity.Username);
                        command.Parameters.AddWithValue("@Email", entity.Email);
                        command.Parameters.AddWithValue("@RoleId", entity.RoleId);
                        command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                        command.Parameters.AddWithValue("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ReturnJson", returnJson ? 1 : 0);
                        command.Parameters.AddWithValue("@BranchId", entity.BranchId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsGlobal", entity.IsGlobal);

                        // Parámetros de salida
                        var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        command.Parameters.Add(resultCodeParam);

                        var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        command.Parameters.Add(resultMessageParam);

                        var jsonResponseParam = new SqlParameter("@JsonResponse", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                        command.Parameters.Add(jsonResponseParam);

                        // Ejecutar SP
                        await command.ExecuteNonQueryAsync();

                        // Obtener valores de salida
                        int resultCode = Convert.ToInt32(resultCodeParam.Value);
                        string resultMessage = Convert.ToString(resultMessageParam.Value) ?? "Sin mensaje";

                        if (returnJson && jsonResponseParam.Value != DBNull.Value)
                        {
                            string? jsonResponse = jsonResponseParam.Value.ToString();
                            var response = JsonConvert.DeserializeObject<ResponseBase<User>>(jsonResponse);
                            return response;
                        }
                        else
                        {
                            return new ResponseBase<User>(resultCode, resultMessage, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<User>(500, $"Error interno: {ex.Message}", null);
            }
        }

        public static async Task UpdatePasswordAndTempStatus(long userId, string passwordHash, bool isPasswordTemp)
        {
            if (userId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0.", nameof(userId));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El hash de la contraseña no puede estar vacío.", nameof(passwordHash));

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdatePasswordAndTempStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@IsPasswordTemp", isPasswordTemp);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }




        public static async Task<bool> Delete(long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (userId <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor que 0.", nameof(userId));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("DeleteUser", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public static async Task<List<User?>?> GetAll()
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetAllUsers", connection))
                {
                    return await GetRow(command);
                }
            }
        }

        public static async Task<User?> GetById(long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (userId <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(userId));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("GetUserById", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var entities = await GetRow(command);
                    return entities.FirstOrDefault();
                }
            }
        }

        public static async Task<User?> EmailUserExists(string email, long? userId = null)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("La cadena de conexión no está configurada.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido y no puede estar vacío o nulo.", nameof(email));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("GetEmailUserExists", connection))
                    {
                        // Agregar el parámetro del email
                        command.Parameters.AddWithValue("@Email", email);

                        // Agregar el parámetro opcional del UserId
                        if (userId.HasValue)
                        {
                            command.Parameters.AddWithValue("@UserId", userId.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@UserId", DBNull.Value);
                        }
                        var entities = await GetRow(command);

                        return entities.FirstOrDefault();
                    }
                }

            }
            catch (SqlException sqlEx)
            {
                // Registrar el error para trazabilidad (si tienes un logger, puedes usarlo aquí)
                Console.WriteLine($"Error al verificar el email '{email}': {sqlEx.Message}");
                throw new Exception("Error al verificar la existencia del email en la base de datos.", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo genérico de excepciones
                Console.WriteLine($"Error inesperado al verificar el email '{email}': {ex.Message}");
                throw;
            }
        }

        public static async Task SaveVerificationToken(long userId, string token)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = CreateCommand("SaveVerificationToken", connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@ExpirationDate", DateTime.UtcNow.AddHours(24));

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<bool> VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("El token no puede estar vacío o nulo.", nameof(token));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("VerifyEmailToken", connection))
                {
                    command.Parameters.AddWithValue("@Token", token);

                    var result = await command.ExecuteScalarAsync();
                    return result != null && Convert.ToBoolean(result);
                }
            }
        }

        public static async Task<bool> ActivateUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("El token no puede estar vacío o nulo.", nameof(token));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = CreateCommand("ActivateUserByToken", connection))
                {
                    command.Parameters.AddWithValue("@Token", token);
                    var result = await command.ExecuteScalarAsync();
                    return result != null && Convert.ToBoolean(result);
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

        private static async Task<List<User?>?> GetRow(SqlCommand command)
        {
            var entities = new List<User?>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    entities.Add(new User
                    {
                        UserId = reader["UserId"] == DBNull.Value ? 0 : (long)reader["UserId"],
                        PersonId = reader["PersonId"] == DBNull.Value ? 0 : (long)reader["PersonId"],
                        Username = reader["Username"] == DBNull.Value ? string.Empty : (string)reader["Username"],
                        Email = reader["Email"] == DBNull.Value ? string.Empty : (string)reader["Email"],
                        RoleId = reader["RoleId"] == DBNull.Value ? 0 : (int)reader["RoleId"],
                        IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"],
                        IsPasswordTemp = reader["IsPasswordTemp"] != DBNull.Value && (bool)reader["IsPasswordTemp"],
                        ShortName = reader["ShortName"] == DBNull.Value ? string.Empty : (string)reader["ShortName"],
                        FullName = reader["FullName"] == DBNull.Value ? string.Empty : (string)reader["FullName"],
                        RoleName = reader["RoleName"] == DBNull.Value ? string.Empty : (string)reader["RoleName"],
                        BranchId = reader["BranchId"] == DBNull.Value ? null : (long?)reader["BranchId"],
                        IsGlobal = reader["IsGlobal"] == DBNull.Value ? false : (bool)reader["IsGlobal"],
                        BrancheName = reader["BrancheName"] == DBNull.Value ? string.Empty : (string)reader["BrancheName"]
                    });
                }
            }
            return entities;
        }
    }
}

