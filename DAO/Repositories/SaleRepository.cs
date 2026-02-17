using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        /// <summary>
        /// Obtiene una venta por su identificador
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        /// <returns>La venta encontrada o null si no existe</returns>
        public Sale GetById(int saleId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.ClientId, s.SellerName,
                             s.TotalAmount, s.Notes, s.IsActive, s.CreatedAt, s.CreatedBy,
                             s.UpdatedAt, s.UpdatedBy,
                             c.Nombre + ' ' + c.Apellido AS ClientName
                             FROM Sales s
                             LEFT JOIN Clients c ON s.ClientId = c.ClientId
                             WHERE s.SaleId = @SaleId AND s.IsActive = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", saleId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapSale(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene una venta por su identificador incluyendo las líneas de detalle
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        /// <returns>La venta encontrada con sus líneas de detalle o null si no existe</returns>
        public Sale GetByIdWithLines(int saleId)
        {
            var sale = GetById(saleId);
            if (sale != null)
            {
                sale.SaleLines = GetSaleLines(saleId);
            }
            return sale;
        }

        /// <summary>
        /// Obtiene la lista completa de ventas activas ordenadas por fecha y número
        /// </summary>
        /// <returns>Lista de ventas activas</returns>
        public List<Sale> GetAll()
        {
            var sales = new List<Sale>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.ClientId, s.SellerName,
                             s.TotalAmount, s.Notes, s.IsActive, s.CreatedAt, s.CreatedBy,
                             s.UpdatedAt, s.UpdatedBy,
                             c.Nombre + ' ' + c.Apellido AS ClientName
                             FROM Sales s
                             LEFT JOIN Clients c ON s.ClientId = c.ClientId
                             WHERE s.IsActive = 1
                             ORDER BY s.SaleDate DESC, s.SaleNumber DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) sales.Add(MapSale(reader));
                    }
                }
            }
            return sales;
        }

        /// <summary>
        /// Obtiene la lista de ventas activas ordenadas por fecha y número
        /// </summary>
        /// <returns>Lista de ventas activas</returns>
        public List<Sale> GetAllActive()
        {
            // Same as GetAll() for Sales since GetAll() already filters by IsActive = 1
            return GetAll();
        }

        /// <summary>
        /// Obtiene la lista completa de ventas con sus líneas de detalle
        /// </summary>
        /// <returns>Lista de ventas con líneas de detalle cargadas</returns>
        public List<Sale> GetAllWithDetails()
        {
            var sales = GetAll();
            foreach (var sale in sales)
            {
                sale.SaleLines = GetSaleLines(sale.SaleId);
            }
            return sales;
        }

        /// <summary>
        /// Obtiene las ventas filtradas por nombre del vendedor
        /// </summary>
        /// <param name="sellerName">Nombre del vendedor a buscar</param>
        /// <returns>Lista de ventas del vendedor especificado</returns>
        public List<Sale> GetBySeller(string sellerName)
        {
            var sales = new List<Sale>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.ClientId, s.SellerName,
                             s.TotalAmount, s.Notes, s.IsActive, s.CreatedAt, s.CreatedBy,
                             s.UpdatedAt, s.UpdatedBy,
                             c.Nombre + ' ' + c.Apellido AS ClientName
                             FROM Sales s
                             LEFT JOIN Clients c ON s.ClientId = c.ClientId
                             WHERE s.SellerName LIKE @SellerName AND s.IsActive = 1
                             ORDER BY s.SaleDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SellerName", "%" + sellerName + "%"));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) sales.Add(MapSale(reader));
                    }
                }
            }
            return sales;
        }

        /// <summary>
        /// Obtiene las ventas de un cliente específico
        /// </summary>
        /// <param name="clientId">Identificador del cliente</param>
        /// <returns>Lista de ventas del cliente especificado</returns>
        public List<Sale> GetByClient(int clientId)
        {
            var sales = new List<Sale>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.ClientId, s.SellerName,
                             s.TotalAmount, s.Notes, s.IsActive, s.CreatedAt, s.CreatedBy,
                             s.UpdatedAt, s.UpdatedBy,
                             c.Nombre + ' ' + c.Apellido AS ClientName
                             FROM Sales s
                             LEFT JOIN Clients c ON s.ClientId = c.ClientId
                             WHERE s.ClientId = @ClientId AND s.IsActive = 1
                             ORDER BY s.SaleDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", clientId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) sales.Add(MapSale(reader));
                    }
                }
            }
            return sales;
        }

        /// <summary>
        /// Obtiene las ventas en un rango de fechas específico
        /// </summary>
        /// <param name="startDate">Fecha de inicio del rango</param>
        /// <param name="endDate">Fecha de fin del rango</param>
        /// <returns>Lista de ventas en el rango de fechas especificado</returns>
        public List<Sale> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var sales = new List<Sale>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.ClientId, s.SellerName,
                             s.TotalAmount, s.Notes, s.IsActive, s.CreatedAt, s.CreatedBy,
                             s.UpdatedAt, s.UpdatedBy,
                             c.Nombre + ' ' + c.Apellido AS ClientName
                             FROM Sales s
                             LEFT JOIN Clients c ON s.ClientId = c.ClientId
                             WHERE s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate AND s.IsActive = 1
                             ORDER BY s.SaleDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) sales.Add(MapSale(reader));
                    }
                }
            }
            return sales;
        }

        /// <summary>
        /// Crea una nueva venta en la base de datos
        /// </summary>
        /// <param name="sale">Entidad de venta a crear</param>
        /// <returns>Identificador de la venta creada</returns>
        public int Create(Sale sale)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Sales (SaleNumber, SaleDate, ClientId, SellerName, TotalAmount, Notes, IsActive, CreatedAt, CreatedBy)
                             VALUES (@SaleNumber, @SaleDate, @ClientId, @SellerName, @TotalAmount, @Notes, 1, GETDATE(), @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleNumber", sale.SaleNumber));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleDate", sale.SaleDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", (object)sale.ClientId ?? DBNull.Value));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SellerName", sale.SellerName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@TotalAmount", sale.TotalAmount));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Notes", (object)sale.Notes ?? DBNull.Value));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", (object)sale.CreatedBy ?? DBNull.Value));
                    
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Inserta una nueva venta en la base de datos (alias de Create)
        /// </summary>
        /// <param name="entity">Entidad de venta a insertar</param>
        /// <returns>Identificador de la venta insertada</returns>
        public int Insert(Sale entity)
        {
            // Alias for Create() to satisfy IRepository interface
            return Create(entity);
        }

        /// <summary>
        /// Crea una nueva venta con sus líneas de detalle en una transacción
        /// </summary>
        /// <param name="sale">Entidad de venta a crear</param>
        /// <param name="saleLines">Lista de líneas de detalle de la venta</param>
        /// <returns>Identificador de la venta creada</returns>
        public int CreateWithLines(Sale sale, List<SaleLine> saleLines)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert Sale
                        var saleQuery = @"INSERT INTO Sales (SaleNumber, SaleDate, ClientId, SellerName, TotalAmount, Notes, IsActive, CreatedAt, CreatedBy)
                                         VALUES (@SaleNumber, @SaleDate, @ClientId, @SellerName, @TotalAmount, @Notes, 1, GETDATE(), @CreatedBy);
                                         SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        int saleId;
                        using (var command = new SqlCommand(saleQuery, connection, transaction))
                        {
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleNumber", sale.SaleNumber));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleDate", sale.SaleDate));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", (object)sale.ClientId ?? DBNull.Value));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@SellerName", sale.SellerName));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@TotalAmount", sale.TotalAmount));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@Notes", (object)sale.Notes ?? DBNull.Value));
                            command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", (object)sale.CreatedBy ?? DBNull.Value));
                            
                            saleId = (int)command.ExecuteScalar();
                        }

                        // Insert Sale Lines
                        var lineQuery = @"INSERT INTO SaleLines (SaleId, ProductId, Quantity, UnitPrice, LineTotal)
                                         VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @LineTotal)";
                        foreach (var line in saleLines)
                        {
                            using (var command = new SqlCommand(lineQuery, connection, transaction))
                            {
                                command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", saleId));
                                command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", line.ProductId));
                                command.Parameters.Add(DatabaseHelper.CreateParameter("@Quantity", line.Quantity));
                                command.Parameters.Add(DatabaseHelper.CreateParameter("@UnitPrice", line.UnitPrice));
                                command.Parameters.Add(DatabaseHelper.CreateParameter("@LineTotal", line.LineTotal));
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return saleId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de una venta existente
        /// </summary>
        /// <param name="sale">Entidad de venta con los datos actualizados</param>
        public void Update(Sale sale)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Sales SET 
                             SaleDate = @SaleDate,
                             ClientId = @ClientId,
                             SellerName = @SellerName,
                             TotalAmount = @TotalAmount,
                             Notes = @Notes,
                             UpdatedAt = GETDATE(),
                             UpdatedBy = @UpdatedBy
                             WHERE SaleId = @SaleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", sale.SaleId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleDate", sale.SaleDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", (object)sale.ClientId ?? DBNull.Value));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SellerName", sale.SellerName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@TotalAmount", sale.TotalAmount));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Notes", (object)sale.Notes ?? DBNull.Value));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", (object)sale.UpdatedBy ?? DBNull.Value));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica de una venta
        /// </summary>
        /// <param name="saleId">Identificador de la venta a eliminar</param>
        public void Delete(int saleId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Sales SET IsActive = 0, UpdatedAt = GETDATE() WHERE SaleId = @SaleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", saleId));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica marcando la venta como inactiva
        /// </summary>
        /// <param name="id">Identificador de la venta a eliminar</param>
        /// <param name="deletedBy">Identificador del usuario que realiza la eliminación</param>
        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Sales SET IsActive = 0, UpdatedAt = GETDATE(), UpdatedBy = @UpdatedBy 
                             WHERE SaleId = @SaleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina todas las líneas de detalle de una venta
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        public void DeleteSaleLines(int saleId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM SaleLines WHERE SaleId = @SaleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", saleId));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Obtiene las líneas de detalle de una venta específica
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        /// <returns>Lista de líneas de detalle de la venta</returns>
        private List<SaleLine> GetSaleLines(int saleId)
        {
            var lines = new List<SaleLine>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sl.SaleLineId, sl.SaleId, sl.ProductId, sl.Quantity, sl.UnitPrice, sl.LineTotal,
                             p.Name AS ProductName, p.SKU
                             FROM SaleLines sl
                             INNER JOIN Products p ON sl.ProductId = p.ProductId
                             WHERE sl.SaleId = @SaleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SaleId", saleId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) lines.Add(MapSaleLine(reader));
                    }
                }
            }
            return lines;
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de venta
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de venta mapeada</returns>
        private Sale MapSale(SqlDataReader reader)
        {
            return new Sale
            {
                SaleId = reader.GetInt32(reader.GetOrdinal("SaleId")),
                SaleNumber = reader.GetString(reader.GetOrdinal("SaleNumber")),
                SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                ClientId = reader.IsDBNull(reader.GetOrdinal("ClientId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ClientId")),
                SellerName = reader.GetString(reader.GetOrdinal("SellerName")),
                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UpdatedBy"))
            };
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de línea de venta
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de línea de venta mapeada</returns>
        private SaleLine MapSaleLine(SqlDataReader reader)
        {
            return new SaleLine
            {
                SaleLineId = reader.GetInt32(reader.GetOrdinal("SaleLineId")),
                SaleId = reader.GetInt32(reader.GetOrdinal("SaleId")),
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                LineTotal = reader.GetDecimal(reader.GetOrdinal("LineTotal")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                SKU = reader.GetString(reader.GetOrdinal("SKU"))
            };
        }
    }
}
