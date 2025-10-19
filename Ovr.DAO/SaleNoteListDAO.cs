using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.DTOs;
using System.Data;

namespace Ovr.DAO
{
    public static class SaleNoteListDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion();

        public static async Task<List<SaleNoteListDto>> GetAll(string buscar, int? statusId, DateTime? fechaInicio, DateTime? fechaFin, int page, int rows)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = CreateCommand("SP_GetAll_SaleNotes", connection);

            command.Parameters.AddWithValue("@Buscar", buscar ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@StatusId", statusId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FechaInicio", fechaInicio ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@FechaFin", fechaFin ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@Rows", rows);

            return await GetRow(command);
        }

        private static async Task<List<SaleNoteListDto>> GetRow(SqlCommand command)
        {
            var saleNotes = new List<SaleNoteListDto>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                saleNotes.Add(new SaleNoteListDto
                {
                    SaleNoteId = reader["SaleNoteId"] == DBNull.Value ? 0 : Convert.ToInt64(reader["SaleNoteId"]),
                    OrderNumber = reader["OrderNumber"]?.ToString() ?? string.Empty,
                    ClienteNombre = reader["ClienteNombre"]?.ToString() ?? string.Empty,
                    SaleDate = reader["SaleDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SaleDate"]),
                    CommitmentDate = reader["CommitmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CommitmentDate"]),
                    Total = reader["Total"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Total"]),
                    StatusId = reader["StatusId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StatusId"]),
                    UltimoPagoDate = reader["UltimoPagoDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UltimoPagoDate"]),
                    BranchName = reader["BranchName"]?.ToString() ?? string.Empty,
                    TotalRows = reader["TotalRows"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalRows"]) // ✅ AGREGADO
                });
            }
            return saleNotes;
        }


        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }
    }
}
