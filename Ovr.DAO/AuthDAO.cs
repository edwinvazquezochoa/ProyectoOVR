using Microsoft.Data.SqlClient;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using System.Data;
using Ovr.Core.Infrastructures.Configs;
namespace Ovr.DAO
{
    public static class AuthDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion();
        /// <summary>
        /// Verifica las credenciales del usuario y devuelve la información del usuario si son válidas.
        /// </summary>
        /// <param name="email">Correo del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Un objeto UserInfo con la información del usuario o null si las credenciales no son válidas.</returns>
        public static async Task<UserInfo?> VerifyUserCredentialsAsync(string email, string password)
        {
            try
            {
                if (_connectionString == null)
                    throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    await connection.OpenAsync();
                    using (SqlCommand command = CreateCommand("VerifyUserCredentials", connection))
                    {
                        // Parámetros
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new UserInfo
                                {
                                    UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0,
                                    PersonId = reader["PersonId"] != DBNull.Value ? (long)reader["PersonId"] : 0,
                                    UserName = reader["Username"]?.ToString() ?? string.Empty, // Added Username
                                    Email = reader["Email"]?.ToString() ?? string.Empty,
                                    ShortName = reader["ShortName"] as string,
                                    FullName = reader["FullName"]?.ToString() ?? string.Empty,
                                    RoleId = reader["RoleId"] != DBNull.Value ? (int)reader["RoleId"] : 0,
                                    RoleName = reader["RoleName"]?.ToString() ?? string.Empty,
                                    BranchId = reader["BranchId"] != DBNull.Value ? (long)reader["BranchId"] : 0,      // Added BranchId
                                    BrancheName = reader["BrancheName"]?.ToString() ?? string.Empty,  // Added BrancheName
                                    IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
            return null; // Si las credenciales no son válidas
        }

        /// <summary>
        /// Crea un comando SQL para ejecutar un procedimiento almacenado.
        /// </summary>
        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }
    }
}
