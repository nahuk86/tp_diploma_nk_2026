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
        /// <summary>
        /// Obtiene el stock de un producto en un almacén específico
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="warehouseId">Identificador del almacén</param>
        /// <returns>El stock del producto en el almacén, o null si no existe</returns>
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

        /// <summary>
        /// Obtiene el stock de un producto en todos los almacenes
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Lista de stock del producto en todos los almacenes</returns>
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

        /// <summary>
        /// Obtiene el stock de todos los productos en un almacén específico
        /// </summary>
        /// <param name="warehouseId">Identificador del almacén</param>
        /// <returns>Lista de stock de todos los productos en el almacén</returns>
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

        /// <summary>
        /// Obtiene la lista completa de stock de todos los productos en todos los almacenes
        /// </summary>
        /// <returns>Lista completa de todo el stock</returns>
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

        /// <summary>
        /// Obtiene la lista de productos con stock bajo o menor al nivel mínimo
        /// </summary>
        /// <returns>Lista de productos con stock bajo</returns>
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

        /// <summary>
        /// Actualiza o inserta el stock de un producto en un almacén
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="warehouseId">Identificador del almacén</param>
        /// <param name="quantity">Cantidad de stock</param>
        /// <param name="updatedBy">Identificador del usuario que realiza la actualización</param>
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

        /// <summary>
        /// Obtiene la cantidad actual de stock de un producto en un almacén
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="warehouseId">Identificador del almacén</param>
        /// <returns>Cantidad actual de stock, o 0 si no existe</returns>
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

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de stock
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de stock con los datos mapeados</returns>
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
