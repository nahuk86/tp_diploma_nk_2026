using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DAO.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        public StockMovement GetById(int movementId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sm.MovementId, sm.MovementNumber, sm.MovementType, sm.MovementDate,
                             sm.SourceWarehouseId, sm.DestinationWarehouseId, sm.Reason, sm.Notes,
                             sm.CreatedAt, sm.CreatedBy, u.Username AS CreatedByUsername,
                             sw.Name AS SourceWarehouseName, dw.Name AS DestinationWarehouseName
                             FROM StockMovements sm
                             LEFT JOIN Users u ON sm.CreatedBy = u.UserId
                             LEFT JOIN Warehouses sw ON sm.SourceWarehouseId = sw.WarehouseId
                             LEFT JOIN Warehouses dw ON sm.DestinationWarehouseId = dw.WarehouseId
                             WHERE sm.MovementId = @MovementId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementId", movementId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapStockMovement(reader);
                    }
                }
            }
            return null;
        }

        public List<StockMovement> GetAll()
        {
            var movements = new List<StockMovement>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sm.MovementId, sm.MovementNumber, sm.MovementType, sm.MovementDate,
                             sm.SourceWarehouseId, sm.DestinationWarehouseId, sm.Reason, sm.Notes,
                             sm.CreatedAt, sm.CreatedBy, u.Username AS CreatedByUsername,
                             sw.Name AS SourceWarehouseName, dw.Name AS DestinationWarehouseName
                             FROM StockMovements sm
                             LEFT JOIN Users u ON sm.CreatedBy = u.UserId
                             LEFT JOIN Warehouses sw ON sm.SourceWarehouseId = sw.WarehouseId
                             LEFT JOIN Warehouses dw ON sm.DestinationWarehouseId = dw.WarehouseId
                             ORDER BY sm.MovementDate DESC, sm.MovementNumber DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) movements.Add(MapStockMovement(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovement> GetByType(MovementType movementType)
        {
            var movements = new List<StockMovement>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sm.MovementId, sm.MovementNumber, sm.MovementType, sm.MovementDate,
                             sm.SourceWarehouseId, sm.DestinationWarehouseId, sm.Reason, sm.Notes,
                             sm.CreatedAt, sm.CreatedBy, u.Username AS CreatedByUsername,
                             sw.Name AS SourceWarehouseName, dw.Name AS DestinationWarehouseName
                             FROM StockMovements sm
                             LEFT JOIN Users u ON sm.CreatedBy = u.UserId
                             LEFT JOIN Warehouses sw ON sm.SourceWarehouseId = sw.WarehouseId
                             LEFT JOIN Warehouses dw ON sm.DestinationWarehouseId = dw.WarehouseId
                             WHERE sm.MovementType = @MovementType
                             ORDER BY sm.MovementDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementType", movementType.ToString()));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) movements.Add(MapStockMovement(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovement> GetByWarehouse(int warehouseId)
        {
            var movements = new List<StockMovement>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sm.MovementId, sm.MovementNumber, sm.MovementType, sm.MovementDate,
                             sm.SourceWarehouseId, sm.DestinationWarehouseId, sm.Reason, sm.Notes,
                             sm.CreatedAt, sm.CreatedBy, u.Username AS CreatedByUsername,
                             sw.Name AS SourceWarehouseName, dw.Name AS DestinationWarehouseName
                             FROM StockMovements sm
                             LEFT JOIN Users u ON sm.CreatedBy = u.UserId
                             LEFT JOIN Warehouses sw ON sm.SourceWarehouseId = sw.WarehouseId
                             LEFT JOIN Warehouses dw ON sm.DestinationWarehouseId = dw.WarehouseId
                             WHERE sm.SourceWarehouseId = @WarehouseId OR sm.DestinationWarehouseId = @WarehouseId
                             ORDER BY sm.MovementDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) movements.Add(MapStockMovement(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovement> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var movements = new List<StockMovement>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sm.MovementId, sm.MovementNumber, sm.MovementType, sm.MovementDate,
                             sm.SourceWarehouseId, sm.DestinationWarehouseId, sm.Reason, sm.Notes,
                             sm.CreatedAt, sm.CreatedBy, u.Username AS CreatedByUsername,
                             sw.Name AS SourceWarehouseName, dw.Name AS DestinationWarehouseName
                             FROM StockMovements sm
                             LEFT JOIN Users u ON sm.CreatedBy = u.UserId
                             LEFT JOIN Warehouses sw ON sm.SourceWarehouseId = sw.WarehouseId
                             LEFT JOIN Warehouses dw ON sm.DestinationWarehouseId = dw.WarehouseId
                             WHERE sm.MovementDate BETWEEN @StartDate AND @EndDate
                             ORDER BY sm.MovementDate DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) movements.Add(MapStockMovement(reader));
                    }
                }
            }
            return movements;
        }

        public int Insert(StockMovement movement)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO StockMovements (MovementNumber, MovementType, MovementDate, SourceWarehouseId, DestinationWarehouseId, Reason, Notes, CreatedAt, CreatedBy) 
                             VALUES (@MovementNumber, @MovementType, @MovementDate, @SourceWarehouseId, @DestinationWarehouseId, @Reason, @Notes, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementNumber", movement.MovementNumber));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementType", movement.MovementType.ToString()));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementDate", movement.MovementDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SourceWarehouseId", movement.SourceWarehouseId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@DestinationWarehouseId", movement.DestinationWarehouseId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Reason", movement.Reason));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Notes", movement.Notes));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", movement.CreatedBy));
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public string GenerateMovementNumber(MovementType movementType)
        {
            var prefix = movementType.ToString().Substring(0, Math.Min(3, movementType.ToString().Length)).ToUpper();
            var date = DateTime.Now.ToString("yyyyMMdd");
            var sequence = GetNextSequence(movementType, date);
            return $"{prefix}{date}{sequence:D4}";
        }

        private int GetNextSequence(MovementType movementType, string date)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ISNULL(MAX(CAST(RIGHT(MovementNumber, 4) AS INT)), 0) + 1 
                             FROM StockMovements 
                             WHERE MovementType = @MovementType AND CONVERT(VARCHAR(8), MovementDate, 112) = @Date";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementType", movementType.ToString()));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Date", date));
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public List<StockMovementLine> GetMovementLines(int movementId)
        {
            var lines = new List<StockMovementLine>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT sml.LineId, sml.MovementId, sml.ProductId, sml.Quantity, sml.UnitPrice,
                             p.Name AS ProductName, p.SKU AS ProductSKU
                             FROM StockMovementLines sml
                             INNER JOIN Products p ON sml.ProductId = p.ProductId
                             WHERE sml.MovementId = @MovementId
                             ORDER BY sml.LineId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementId", movementId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) lines.Add(MapStockMovementLine(reader));
                    }
                }
            }
            return lines;
        }

        public void InsertLine(StockMovementLine line)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO StockMovementLines (MovementId, ProductId, Quantity, UnitPrice) 
                             VALUES (@MovementId, @ProductId, @Quantity, @UnitPrice)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementId", line.MovementId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", line.ProductId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Quantity", line.Quantity));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UnitPrice", line.UnitPrice));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private StockMovement MapStockMovement(SqlDataReader reader)
        {
            return new StockMovement
            {
                MovementId = (int)reader["MovementId"],
                MovementNumber = reader["MovementNumber"].ToString(),
                MovementType = (MovementType)Enum.Parse(typeof(MovementType), reader["MovementType"].ToString()),
                MovementDate = (DateTime)reader["MovementDate"],
                SourceWarehouseId = reader["SourceWarehouseId"] == DBNull.Value ? (int?)null : (int)reader["SourceWarehouseId"],
                DestinationWarehouseId = reader["DestinationWarehouseId"] == DBNull.Value ? (int?)null : (int)reader["DestinationWarehouseId"],
                Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? 0 : (int)reader["CreatedBy"],
                CreatedByUsername = reader["CreatedByUsername"] == DBNull.Value ? null : reader["CreatedByUsername"].ToString(),
                SourceWarehouseName = reader["SourceWarehouseName"] == DBNull.Value ? null : reader["SourceWarehouseName"].ToString(),
                DestinationWarehouseName = reader["DestinationWarehouseName"] == DBNull.Value ? null : reader["DestinationWarehouseName"].ToString()
            };
        }

        private StockMovementLine MapStockMovementLine(SqlDataReader reader)
        {
            return new StockMovementLine
            {
                LineId = (int)reader["LineId"],
                MovementId = (int)reader["MovementId"],
                ProductId = (int)reader["ProductId"],
                Quantity = (int)reader["Quantity"],
                UnitPrice = reader["UnitPrice"] == DBNull.Value ? (decimal?)null : (decimal)reader["UnitPrice"],
                ProductName = reader["ProductName"].ToString(),
                ProductSKU = reader["ProductSKU"].ToString()
            };
        }
    }
}
