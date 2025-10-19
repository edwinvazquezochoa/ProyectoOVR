using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.Models;
using System.Data;

namespace Ovr.DAO
{
    public static class MenuPermissionDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion(); //GlobalSettings.ConnectionString;

        public static async Task<List<MenuPermission>> GetUserAsync(long userId)
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            var listaPermisos = new List<MenuPermission>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SP_CRUD_MenusPermissions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Accion", "Get");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var permiso = new MenuPermission
                            {
                                MenuPermissionId = reader["MenuPermissionId"] != DBNull.Value ? Convert.ToInt32(reader["MenuPermissionId"]) : 0,
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt64(reader["UserId"]) : 0,
                                MenuId = reader["MenuId"] != DBNull.Value ? Convert.ToInt32(reader["MenuId"]) : 0,
                                MenusName = reader["MenusName"]?.ToString() ?? string.Empty,

                                CanView = Convert.ToBoolean(reader["CanView"]),
                                CanCreate = Convert.ToBoolean(reader["CanCreate"]),
                                CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                                CanDelete = Convert.ToBoolean(reader["CanDelete"]),
                                CanExport = Convert.ToBoolean(reader["CanExport"]),
                                CanPrint = Convert.ToBoolean(reader["CanPrint"]),
                                CanApprove = Convert.ToBoolean(reader["CanApprove"]),
                                CanCancel = Convert.ToBoolean(reader["CanCancel"]),
                                CanAuthorize = Convert.ToBoolean(reader["CanAuthorize"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };


                            listaPermisos.Add(permiso);
                        }
                    }
                }
            }

            return listaPermisos;
        }

        public static async Task<List<MenuPermission>> InsertOrUpdateAsync(List<MenuPermission> permissions, long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (permissions == null || permissions.Count == 0)
                throw new ArgumentException("La lista de permisos no puede estar vacía.", nameof(permissions));

            var result = new List<MenuPermission>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SP_CRUD_MenusPermissions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Crear el TVP
                    var table = new DataTable();
                    table.Columns.Add("UserId", typeof(long));
                    table.Columns.Add("MenuId", typeof(int));
                    table.Columns.Add("CanView", typeof(bool));
                    table.Columns.Add("CanCreate", typeof(bool));
                    table.Columns.Add("CanEdit", typeof(bool));
                    table.Columns.Add("CanDelete", typeof(bool));
                    table.Columns.Add("CanExport", typeof(bool));
                    table.Columns.Add("CanPrint", typeof(bool));
                    table.Columns.Add("CanApprove", typeof(bool));
                    table.Columns.Add("CanCancel", typeof(bool));
                    table.Columns.Add("CanAuthorize", typeof(bool));

                    foreach (var p in permissions)
                    {
                        table.Rows.Add(p.UserId, p.MenuId, p.CanView, p.CanCreate, p.CanEdit, p.CanDelete, p.CanExport, p.CanPrint, p.CanApprove, p.CanCancel, p.CanAuthorize);
                    }

                    // Parámetros
                    var tvpParam = new SqlParameter("@Permisos", SqlDbType.Structured)
                    {
                        TypeName = "dbo.TVP_MenuPermissions",
                        Value = table
                    };
                    command.Parameters.Add(tvpParam);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Accion", "InsertOrUpdate");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new MenuPermission
                            {
                                MenuPermissionId = reader.GetInt32(reader.GetOrdinal("MenuPermissionId")),
                                UserId = reader.GetInt64(reader.GetOrdinal("UserId")),
                                MenuId = reader.GetInt32(reader.GetOrdinal("MenuId")),
                                MenusName = reader["MenusName"]?.ToString() ?? string.Empty,
                                CanView = reader.GetBoolean(reader.GetOrdinal("CanView")),
                                CanCreate = reader.GetBoolean(reader.GetOrdinal("CanCreate")),
                                CanEdit = reader.GetBoolean(reader.GetOrdinal("CanEdit")),
                                CanDelete = reader.GetBoolean(reader.GetOrdinal("CanDelete")),
                                CanExport = reader.GetBoolean(reader.GetOrdinal("CanExport")),
                                CanPrint = reader.GetBoolean(reader.GetOrdinal("CanPrint")),
                                CanApprove = reader.GetBoolean(reader.GetOrdinal("CanApprove")),
                                CanCancel = reader.GetBoolean(reader.GetOrdinal("CanCancel")),
                                CanAuthorize = reader.GetBoolean(reader.GetOrdinal("CanAuthorize")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<List<MenuPermission>> GetByRoleAsync(int roleId)
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            var listaPermisos = new List<MenuPermission>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("SP_CRUD_RoleMenuPermissions", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RoleId", roleId);
            // @Accion se omite para consulta

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                listaPermisos.Add(new MenuPermission
                {
                    MenuId = reader.GetInt32(reader.GetOrdinal("MenuId")),
                    MenusName = reader["MenusName"]?.ToString() ?? string.Empty,
                    CanView = reader.GetBoolean(reader.GetOrdinal("CanView")),
                    CanCreate = reader.GetBoolean(reader.GetOrdinal("CanCreate")),
                    CanEdit = reader.GetBoolean(reader.GetOrdinal("CanEdit")),
                    CanDelete = reader.GetBoolean(reader.GetOrdinal("CanDelete")),
                    CanExport = reader.GetBoolean(reader.GetOrdinal("CanExport")),
                    CanPrint = reader.GetBoolean(reader.GetOrdinal("CanPrint")),
                    CanApprove = reader.GetBoolean(reader.GetOrdinal("CanApprove")),
                    CanCancel = reader.GetBoolean(reader.GetOrdinal("CanCancel")),
                    CanAuthorize = reader.GetBoolean(reader.GetOrdinal("CanAuthorize")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    RoleId = roleId
                });
            }

            return listaPermisos;
        }

        public static async Task<List<MenuPermission>> InsertOrUpdateByRoleAsync(List<MenuPermission> permissions, int roleId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (permissions == null || permissions.Count == 0)
                throw new ArgumentException("La lista de permisos no puede estar vacía.", nameof(permissions));

            var result = new List<MenuPermission>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("SP_CRUD_RoleMenuPermissions", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Crear el TVP
            var table = new DataTable();
            table.Columns.Add("RoleId", typeof(int));
            table.Columns.Add("MenuId", typeof(int));
            table.Columns.Add("CanView", typeof(bool));
            table.Columns.Add("CanCreate", typeof(bool));
            table.Columns.Add("CanEdit", typeof(bool));
            table.Columns.Add("CanDelete", typeof(bool));
            table.Columns.Add("CanExport", typeof(bool));
            table.Columns.Add("CanPrint", typeof(bool));
            table.Columns.Add("CanApprove", typeof(bool));
            table.Columns.Add("CanCancel", typeof(bool));
            table.Columns.Add("CanAuthorize", typeof(bool));
            table.Columns.Add("IsActive", typeof(bool)); // ← esta faltaba
            foreach (var p in permissions)
            {
                table.Rows.Add(
                    p.RoleId,
                    p.MenuId,
                    p.CanView,
                    p.CanCreate,
                    p.CanEdit,
                    p.CanDelete,
                    p.CanExport,
                    p.CanPrint,
                    p.CanApprove,
                    p.CanCancel,
                    p.CanAuthorize,
                    p.IsActive == null ? true : p.IsActive // asegúrate de que nunca sea null
                );
            }


            var tvpParam = new SqlParameter("@Permisos", SqlDbType.Structured)
            {
                TypeName = "dbo.TVP_RoleMenuPermissions",
                Value = table
            };

            command.Parameters.Add(tvpParam);
            command.Parameters.AddWithValue("@RoleId", roleId);
            command.Parameters.AddWithValue("@Accion", "InsertOrUpdate");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new MenuPermission
                {
                    RoleId = roleId,
                    MenuId = reader.GetInt32(reader.GetOrdinal("MenuId")),
                    MenusName = reader["MenusName"]?.ToString() ?? string.Empty,
                    CanView = reader.GetBoolean(reader.GetOrdinal("CanView")),
                    CanCreate = reader.GetBoolean(reader.GetOrdinal("CanCreate")),
                    CanEdit = reader.GetBoolean(reader.GetOrdinal("CanEdit")),
                    CanDelete = reader.GetBoolean(reader.GetOrdinal("CanDelete")),
                    CanExport = reader.GetBoolean(reader.GetOrdinal("CanExport")),
                    CanPrint = reader.GetBoolean(reader.GetOrdinal("CanPrint")),
                    CanApprove = reader.GetBoolean(reader.GetOrdinal("CanApprove")),
                    CanCancel = reader.GetBoolean(reader.GetOrdinal("CanCancel")),
                    CanAuthorize = reader.GetBoolean(reader.GetOrdinal("CanAuthorize")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))

                });
            }

            return result;
        }


    }
}
