using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Obtiene un producto por su identificador
        /// </summary>
        /// <param name="id">Identificador del producto</param>
        /// <returns>El producto encontrado o null si no existe</returns>
        public Product GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products WHERE ProductId = @ProductId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", id));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapProduct(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene un producto por su código SKU
        /// </summary>
        /// <param name="sku">Código SKU del producto</param>
        /// <returns>El producto encontrado o null si no existe</returns>
        public Product GetBySKU(string sku)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products WHERE SKU = @SKU";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SKU", sku));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapProduct(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene la lista completa de productos ordenados por nombre
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public List<Product> GetAll()
        {
            var products = new List<Product>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products ORDER BY Name";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProduct(reader));
                        }
                    }
                }
            }
            
            return products;
        }

        /// <summary>
        /// Obtiene la lista de productos activos ordenados por nombre
        /// </summary>
        /// <returns>Lista de productos activos</returns>
        public List<Product> GetAllActive()
        {
            var products = new List<Product>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products WHERE IsActive = 1 ORDER BY Name";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProduct(reader));
                        }
                    }
                }
            }
            
            return products;
        }

        /// <summary>
        /// Busca productos por término de búsqueda en SKU, nombre o descripción
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        public List<Product> Search(string searchTerm)
        {
            var products = new List<Product>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products 
                             WHERE (SKU LIKE @SearchTerm OR Name LIKE @SearchTerm OR Description LIKE @SearchTerm)
                             ORDER BY Name";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SearchTerm", "%" + searchTerm + "%"));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProduct(reader));
                        }
                    }
                }
            }
            
            return products;
        }

        /// <summary>
        /// Obtiene la lista de productos filtrados por categoría
        /// </summary>
        /// <param name="category">Categoría del producto</param>
        /// <returns>Lista de productos de la categoría especificada</returns>
        public List<Product> GetByCategory(string category)
        {
            var products = new List<Product>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT ProductId, SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy 
                             FROM Products WHERE Category = @Category ORDER BY Name";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProduct(reader));
                        }
                    }
                }
            }
            
            return products;
        }

        /// <summary>
        /// Verifica si un código SKU ya existe en la base de datos
        /// </summary>
        /// <param name="sku">Código SKU a verificar</param>
        /// <param name="excludeProductId">Identificador de producto a excluir de la búsqueda (opcional)</param>
        /// <returns>True si el SKU existe, false en caso contrario</returns>
        public bool SKUExists(string sku, int? excludeProductId = null)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = excludeProductId.HasValue
                    ? "SELECT COUNT(1) FROM Products WHERE SKU = @SKU AND ProductId != @ExcludeProductId"
                    : "SELECT COUNT(1) FROM Products WHERE SKU = @SKU";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SKU", sku));
                    if (excludeProductId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ExcludeProductId", excludeProductId.Value));
                    }
                    
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo producto en la base de datos
        /// </summary>
        /// <param name="entity">Entidad de producto a insertar</param>
        /// <returns>Identificador del producto creado</returns>
        public int Insert(Product entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Products (SKU, Name, Description, Category, UnitPrice, MinStockLevel, IsActive, CreatedAt, CreatedBy) 
                             VALUES (@SKU, @Name, @Description, @Category, @UnitPrice, @MinStockLevel, @IsActive, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SKU", entity.SKU));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Name", entity.Name));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", entity.Category));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UnitPrice", entity.UnitPrice));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MinStockLevel", entity.MinStockLevel));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", entity.CreatedBy));
                    
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de un producto existente
        /// </summary>
        /// <param name="entity">Entidad de producto con los datos actualizados</param>
        public void Update(Product entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Products SET 
                             SKU = @SKU, 
                             Name = @Name, 
                             Description = @Description, 
                             Category = @Category, 
                             UnitPrice = @UnitPrice, 
                             MinStockLevel = @MinStockLevel, 
                             IsActive = @IsActive, 
                             UpdatedAt = @UpdatedAt, 
                             UpdatedBy = @UpdatedBy 
                             WHERE ProductId = @ProductId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", entity.ProductId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SKU", entity.SKU));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Name", entity.Name));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", entity.Category));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UnitPrice", entity.UnitPrice));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@MinStockLevel", entity.MinStockLevel));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", entity.UpdatedBy));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina físicamente un producto de la base de datos
        /// </summary>
        /// <param name="id">Identificador del producto a eliminar</param>
        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM Products WHERE ProductId = @ProductId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica marcando el producto como inactivo
        /// </summary>
        /// <param name="id">Identificador del producto a desactivar</param>
        /// <param name="deletedBy">Identificador del usuario que realiza la eliminación</param>
        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Products SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy 
                             WHERE ProductId = @ProductId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de producto
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de producto mapeada</returns>
        private Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = (int)reader["ProductId"],
                SKU = reader["SKU"].ToString(),
                Name = reader["Name"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Category = reader["Category"].ToString(),
                UnitPrice = (decimal)reader["UnitPrice"],
                MinStockLevel = (int)reader["MinStockLevel"],
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? (int?)null : (int)reader["CreatedBy"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["UpdatedAt"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"]
            };
        }
    }
}
