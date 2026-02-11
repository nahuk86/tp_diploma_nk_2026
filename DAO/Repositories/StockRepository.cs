using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class StockRepository : IStockRepository
    {
        public Stock GetByProductAndWarehouse(int productId, int warehouseId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.StockId, s.ProductId, s.WarehouseId, s.Quantity, s.LastUpdated, s.UpdatedBy,
                             p.Name AS ProductName, p.SKU AS ProductSKU, w.Name AS WarehouseName
                             FROM Stock s
                             INNER JOIN Products p ON s.ProductId = p.ProductId
                             INNER JOIN Warehouses w ON s.WarehouseId = w.WarehouseId
                             WHERE s.ProductId = @ProductId AND s.WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapStock(reader);
                    }
                }
            }
            return null;
        }

        public List<Stock> GetByProduct(int productId)
        {
            var stocks = new List<Stock>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.StockId, s.ProductId, s.WarehouseId, s.Quantity, s.LastUpdated, s.UpdatedBy,
                             p.Name AS ProductName, p.SKU AS ProductSKU, w.Name AS WarehouseName
                             FROM Stock s
                             INNER JOIN Products p ON s.ProductId = p.ProductId
                             INNER JOIN Warehouses w ON s.WarehouseId = w.WarehouseId
                             WHERE s.ProductId = @ProductId ORDER BY w.Name";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) stocks.Add(MapStock(reader));
                    }
                }
            }
            return stocks;
        }

        public List<Stock> GetByWarehouse(int warehouseId)
        {
            var stocks = new List<Stock>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.StockId, s.ProductId, s.WarehouseId, s.Quantity, s.LastUpdated, s.UpdatedBy,
                             p.Name AS ProductName, p.SKU AS ProductSKU, w.Name AS WarehouseName
                             FROM Stock s
                             INNER JOIN Products p ON s.ProductId = p.ProductId
                             INNER JOIN Warehouses w ON s.WarehouseId = w.WarehouseId
                             WHERE s.WarehouseId = @WarehouseId ORDER BY p.Name";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) stocks.Add(MapStock(reader));
                    }
                }
            }
            return stocks;
        }

        public List<Stock> GetAll()
        {
            var stocks = new List<Stock>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.StockId, s.ProductId, s.WarehouseId, s.Quantity, s.LastUpdated, s.UpdatedBy,
                             p.Name AS ProductName, p.SKU AS ProductSKU, w.Name AS WarehouseName
                             FROM Stock s
                             INNER JOIN Products p ON s.ProductId = p.ProductId
                             INNER JOIN Warehouses w ON s.WarehouseId = w.WarehouseId
                             ORDER BY p.Name, w.Name";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) stocks.Add(MapStock(reader));
                    }
                }
            }
            return stocks;
        }

        public List<Stock> GetLowStock()
        {
            var stocks = new List<Stock>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT s.StockId, s.ProductId, s.WarehouseId, s.Quantity, s.LastUpdated, s.UpdatedBy,
                             p.Name AS ProductName, p.SKU AS ProductSKU, w.Name AS WarehouseName
                             FROM Stock s
                             INNER JOIN Products p ON s.ProductId = p.ProductId
                             INNER JOIN Warehouses w ON s.WarehouseId = w.WarehouseId
                             WHERE s.Quantity <= p.MinStockLevel
                             ORDER BY p.Name, w.Name";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) stocks.Add(MapStock(reader));
                    }
                }
            }
            return stocks;
        }

        public void UpdateStock(int productId, int warehouseId, int quantity, int updatedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"IF EXISTS (SELECT 1 FROM Stock WHERE ProductId = @ProductId AND WarehouseId = @WarehouseId)
                             UPDATE Stock SET Quantity = @Quantity, LastUpdated = @LastUpdated, UpdatedBy = @UpdatedBy 
                             WHERE ProductId = @ProductId AND WarehouseId = @WarehouseId
                             ELSE
                             INSERT INTO Stock (ProductId, WarehouseId, Quantity, LastUpdated, UpdatedBy) 
                             VALUES (@ProductId, @WarehouseId, @Quantity, @LastUpdated, @UpdatedBy)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Quantity", quantity));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@LastUpdated", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", updatedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetCurrentStock(int productId, int warehouseId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT ISNULL(Quantity, 0) FROM Stock WHERE ProductId = @ProductId AND WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId));
                    connection.Open();
                    var result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : (int)result;
                }
            }
        }

        private Stock MapStock(SqlDataReader reader)
        {
            return new Stock
            {
                StockId = (int)reader["StockId"],
                ProductId = (int)reader["ProductId"],
                WarehouseId = (int)reader["WarehouseId"],
                Quantity = (int)reader["Quantity"],
                LastUpdated = (DateTime)reader["LastUpdated"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"],
                ProductName = reader["ProductName"].ToString(),
                ProductSKU = reader["ProductSKU"].ToString(),
                WarehouseName = reader["WarehouseName"].ToString()
            };
        }
    }
}
