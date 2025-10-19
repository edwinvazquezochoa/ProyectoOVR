using Microsoft.Data.SqlClient;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Config;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;
using System.Data;

namespace Ovr.DAO
{
    public static class SalesNoteDAO
    {
        private static readonly string? _connectionString = ObtenerValores.Conexion();

        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection)
        {
            return new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        public static async Task<long> Insert(SaleNoteDetailDto entity, long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_SaleNote", connection);

            command.Parameters.AddWithValue("@Accion", "Insertar");
            command.Parameters.Add("@SaleNoteId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            MapSaleNoteParameters(command, entity, userId);

            command.Parameters.AddWithValue("@AccessoriesTVP", CreateAccessoriesTVP(entity.Accessories));
            command.Parameters.AddWithValue("@PaymentsTVP", CreatePaymentsTVP(entity.Payments));

            await command.ExecuteNonQueryAsync();

            return Convert.ToInt64(command.Parameters["@SaleNoteId"].Value ?? 0);
        }

        public static async Task<ResponseBase<SaleNoteDetailDto>> Update(SaleNoteDetailDto entity, long userId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_SaleNote", connection);

            command.Parameters.AddWithValue("@Accion", "Actualizar");
            command.Parameters.AddWithValue("@SaleNoteId", entity.SaleNoteId);
            MapSaleNoteParameters(command, entity, userId);

            command.Parameters.AddWithValue("@AccessoriesTVP", CreateAccessoriesTVP(entity.Accessories));
            command.Parameters.AddWithValue("@PaymentsTVP", CreatePaymentsTVP(entity.Payments));

            await command.ExecuteNonQueryAsync();

            return new ResponseBase<SaleNoteDetailDto>(200, "Actualización exitosa", entity);
        }

        public static async Task<SaleNoteDetailDto?> GetById(long saleNoteId)
        {
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString), "Se requiere la conexión.");

            if (saleNoteId <= 0)
                throw new ArgumentException("El ID debe ser mayor que 0.", nameof(saleNoteId));

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = CreateCommand("SP_CRUD_SaleNote", connection);
            command.Parameters.AddWithValue("@Accion", "ObtenerPorId");
            command.Parameters.AddWithValue("@SaleNoteId", saleNoteId);

            using var reader = await command.ExecuteReaderAsync();

            SaleNoteDetailDto? entity = null;

            if (await reader.ReadAsync())
            {
                entity = new SaleNoteDetailDto
                {
                    SaleNoteId = saleNoteId,
                    OrderNumber = reader["OrderNumber"]?.ToString() ?? string.Empty,
                    SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                    PacienteId = reader.GetInt64(reader.GetOrdinal("PacienteId")),
                    ClienteNombre = reader["ClienteNombre"]?.ToString() ?? string.Empty,
                    ClienteTelefono = reader["ClienteTelefono"]?.ToString() ?? string.Empty,
                    ClienteEmail = reader["ClienteEmail"]?.ToString() ?? string.Empty,
                    AddressStreetNumber = reader["AddressStreetNumber"]?.ToString() ?? string.Empty,
                    AddressNeighborhood = reader["AddressNeighborhood"]?.ToString() ?? string.Empty,
                    AddressCity = reader["AddressCity"]?.ToString() ?? string.Empty,
                    AddressState = reader["AddressState"]?.ToString() ?? string.Empty,
                    AddressPostalCode = reader["AddressPostalCode"]?.ToString() ?? string.Empty,
                    BranchId = reader.GetInt64(reader.GetOrdinal("BranchId")),
                    BranchName = reader["BranchName"]?.ToString() ?? string.Empty,
                    StatusId = reader.GetInt32(reader.GetOrdinal("StatusId")),
                    StatusName = reader["StatusName"]?.ToString() ?? string.Empty,
                    CommitmentDate = reader.IsDBNull(reader.GetOrdinal("CommitmentDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CommitmentDate")),
                    Products_Services = reader["Products_Services"]?.ToString() ?? string.Empty,
                    IsPatientLenses = reader.GetBoolean(reader.GetOrdinal("IsPatientLenses")),
                    LensAdaptationNotes = reader["LensAdaptationNotes"]?.ToString() ?? string.Empty,
                    IsPatientFrame = reader.GetBoolean(reader.GetOrdinal("IsPatientFrame")),
                    FrameAdaptationNotes = reader["FrameAdaptationNotes"]?.ToString() ?? string.Empty,
                    ContactLensAdaptationNotes = reader["ContactLensAdaptationNotes"]?.ToString() ?? string.Empty,
                    IsPatientPrescription = reader.GetBoolean(reader.GetOrdinal("IsPatientPrescription")),
                    OD_Sphere = reader["OD_Sphere"]?.ToString() ?? string.Empty,
                    OI_Sphere = reader["OI_Sphere"]?.ToString() ?? string.Empty,
                    OD_Cylinder = reader["OD_Cylinder"]?.ToString() ?? string.Empty,
                    OI_Cylinder = reader["OI_Cylinder"]?.ToString() ?? string.Empty,
                    OD_Axis = reader["OD_Axis"]?.ToString() ?? string.Empty,
                    OI_Axis = reader["OI_Axis"]?.ToString() ?? string.Empty,
                    OD_Addition = reader["OD_Addition"]?.ToString() ?? string.Empty,
                    OI_Addition = reader["OI_Addition"]?.ToString() ?? string.Empty,
                    OD_Prism = reader["OD_Prism"]?.ToString() ?? string.Empty,
                    OI_Prism = reader["OI_Prism"]?.ToString() ?? string.Empty,
                    OD_DNP = reader["OD_DNP"]?.ToString() ?? string.Empty,
                    OI_DNP = reader["OI_DNP"]?.ToString() ?? string.Empty,
                    DIP_Far = reader["DIP_Far"]?.ToString() ?? string.Empty,
                    DIP_Near = reader["DIP_Near"]?.ToString() ?? string.Empty,
                    Height = reader["Height"]?.ToString() ?? string.Empty,
                    IsBifocal = reader.GetBoolean(reader.GetOrdinal("IsBifocal")),
                    IsProgressive = reader.GetBoolean(reader.GetOrdinal("IsProgressive")),
                    VertexDistance = reader["VertexDistance"]?.ToString() ?? string.Empty,
                    PantoscopicAngle = reader["PantoscopicAngle"]?.ToString() ?? string.Empty,
                    PanoramicAngle = reader["PanoramicAngle"]?.ToString() ?? string.Empty,
                    CreditMonths = reader.IsDBNull(reader.GetOrdinal("CreditMonths")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CreditMonths")),
                    BankOrigin = reader["BankOrigin"]?.ToString() ?? string.Empty,
                    BankDestination = reader["BankDestination"]?.ToString() ?? string.Empty,
                    Amount_Frame = reader.IsDBNull(reader.GetOrdinal("Amount_Frame")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Amount_Frame")),
                    Amount_Lens = reader.IsDBNull(reader.GetOrdinal("Amount_Lens")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Amount_Lens")),
                    Amount_Accessories = reader.IsDBNull(reader.GetOrdinal("Amount_Accessories")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Amount_Accessories")),
                    Amount_Service = reader.IsDBNull(reader.GetOrdinal("Amount_Service")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Amount_Service")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                    Discount = reader.IsDBNull(reader.GetOrdinal("Discount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Discount")),
                    Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                };
            }

            if (await reader.NextResultAsync() && entity != null)
            {
                while (await reader.ReadAsync())
                {
                    entity.Accessories.Add(new SaleNoteAccessoryDto
                    {
                        SaleAccessoryId = reader.GetInt64(reader.GetOrdinal("SaleAccessoryId")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        Description = reader["Description"]?.ToString() ?? string.Empty,
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                        SaleNoteId = saleNoteId
                    });
                }
            }

            if (await reader.NextResultAsync() && entity != null)
            {
                while (await reader.ReadAsync())
                {
                    entity.Payments.Add(new SaleNotePaymentDto
                    {
                        SalePaymentId = reader.GetInt64(reader.GetOrdinal("SalePaymentId")),
                        PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                        AmountPaid = reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                        RemainingBalance = reader.IsDBNull(reader.GetOrdinal("RemainingBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("RemainingBalance")),
                        Notes = reader["Notes"]?.ToString() ?? string.Empty,
                        SaleNoteId = saleNoteId
                    });
                }
            }

            return entity;
        }

        private static void MapSaleNoteParameters(SqlCommand command, SaleNoteDetailDto entity, long userId)
        {
            command.Parameters.AddWithValue("@OrderNumber", entity.OrderNumber);
            command.Parameters.AddWithValue("@SaleDate", entity.SaleDate);
            command.Parameters.AddWithValue("@PacienteId", entity.PacienteId);
            command.Parameters.AddWithValue("@BranchId", entity.BranchId);
            command.Parameters.AddWithValue("@StatusId", entity.StatusId);
            command.Parameters.AddWithValue("@Products_Services", entity.Products_Services ?? string.Empty);
            command.Parameters.AddWithValue("@IsPatientLenses", entity.IsPatientLenses);
            command.Parameters.AddWithValue("@LensAdaptationNotes", entity.LensAdaptationNotes ?? string.Empty);
            command.Parameters.AddWithValue("@IsPatientFrame", entity.IsPatientFrame);
            command.Parameters.AddWithValue("@FrameAdaptationNotes", entity.FrameAdaptationNotes ?? string.Empty);
            command.Parameters.AddWithValue("@ContactLensAdaptationNotes", entity.ContactLensAdaptationNotes ?? string.Empty);
            command.Parameters.AddWithValue("@IsPatientPrescription", entity.IsPatientPrescription);
            command.Parameters.AddWithValue("@OD_Sphere", entity.OD_Sphere ?? string.Empty);
            command.Parameters.AddWithValue("@OI_Sphere", entity.OI_Sphere ?? string.Empty);
            command.Parameters.AddWithValue("@OD_Cylinder", entity.OD_Cylinder ?? string.Empty);
            command.Parameters.AddWithValue("@OI_Cylinder", entity.OI_Cylinder ?? string.Empty);
            command.Parameters.AddWithValue("@OD_Axis", entity.OD_Axis ?? string.Empty);
            command.Parameters.AddWithValue("@OI_Axis", entity.OI_Axis ?? string.Empty);
            command.Parameters.AddWithValue("@OD_Addition", entity.OD_Addition ?? string.Empty);
            command.Parameters.AddWithValue("@OI_Addition", entity.OI_Addition ?? string.Empty);
            command.Parameters.AddWithValue("@OD_Prism", entity.OD_Prism ?? string.Empty);
            command.Parameters.AddWithValue("@OI_Prism", entity.OI_Prism ?? string.Empty);
            command.Parameters.AddWithValue("@OD_DNP", entity.OD_DNP ?? string.Empty);
            command.Parameters.AddWithValue("@OI_DNP", entity.OI_DNP ?? string.Empty);
            command.Parameters.AddWithValue("@DIP_Far", entity.DIP_Far ?? string.Empty);
            command.Parameters.AddWithValue("@DIP_Near", entity.DIP_Near ?? string.Empty);
            command.Parameters.AddWithValue("@Height", entity.Height ?? string.Empty);
            command.Parameters.AddWithValue("@IsBifocal", entity.IsBifocal);
            command.Parameters.AddWithValue("@IsProgressive", entity.IsProgressive);
            command.Parameters.AddWithValue("@VertexDistance", entity.VertexDistance ?? string.Empty);
            command.Parameters.AddWithValue("@PantoscopicAngle", entity.PantoscopicAngle ?? string.Empty);
            command.Parameters.AddWithValue("@PanoramicAngle", entity.PanoramicAngle ?? string.Empty);
            command.Parameters.AddWithValue("@PaymentMethod", entity.PaymentMethod ?? string.Empty); // ✅ NUEVO
            command.Parameters.AddWithValue("@CreditMonths", (object?)entity.CreditMonths ?? DBNull.Value);
            command.Parameters.AddWithValue("@BankOrigin", entity.BankOrigin ?? string.Empty);
            command.Parameters.AddWithValue("@BankDestination", entity.BankDestination ?? string.Empty);
            command.Parameters.AddWithValue("@Amount_Frame", (object?)entity.Amount_Frame ?? DBNull.Value);
            command.Parameters.AddWithValue("@Amount_Lens", (object?)entity.Amount_Lens ?? DBNull.Value);
            command.Parameters.AddWithValue("@Amount_Accessories", (object?)entity.Amount_Accessories ?? DBNull.Value);
            command.Parameters.AddWithValue("@Amount_Service", (object?)entity.Amount_Service ?? DBNull.Value);
            command.Parameters.AddWithValue("@Subtotal", entity.Subtotal);
            command.Parameters.AddWithValue("@Discount", (object?)entity.Discount ?? DBNull.Value);
            command.Parameters.AddWithValue("@Total", entity.Total);
            command.Parameters.AddWithValue("@CommitmentDate", (object?)entity.CommitmentDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@UserId", userId);
        }

        private static DataTable CreateAccessoriesTVP(List<SaleNoteAccessoryDto> accessories)
        {
            var table = new DataTable();
            table.Columns.Add("SaleNoteId", typeof(long));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("Total", typeof(decimal));

            foreach (var item in accessories)
            {
                table.Rows.Add(item.SaleNoteId, item.Quantity, item.Description, item.UnitPrice, item.Total);
            }

            return table;
        }


        private static DataTable CreatePaymentsTVP(List<SaleNotePaymentDto> payments)
        {
            var table = new DataTable();
            table.Columns.Add("PaymentDate", typeof(DateTime));
            table.Columns.Add("AmountPaid", typeof(decimal));
            table.Columns.Add("RemainingBalance", typeof(decimal));
            table.Columns.Add("Notes", typeof(string));

            foreach (var p in payments)
                table.Rows.Add(p.PaymentDate, p.AmountPaid, p.RemainingBalance ?? 0, p.Notes ?? string.Empty);

            return table;
        }

    }
}
