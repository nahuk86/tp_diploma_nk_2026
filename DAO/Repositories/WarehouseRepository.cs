using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        public Warehouse GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT WarehouseId, Code, Name, Address, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Warehouses WHERE WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", id));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapWarehouse(reader);
                    }
                }
            }
            return null;
        }

        public Warehouse GetByCode(string code)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT WarehouseId, Code, Name, Address, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Warehouses WHERE Code = @Code";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Code", code));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapWarehouse(reader);
                    }
                }
            }
            return null;
        }

        public List<Warehouse> GetAll()
        {
            var warehouses = new List<Warehouse>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT WarehouseId, Code, Name, Address, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Warehouses ORDER BY Code";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) warehouses.Add(MapWarehouse(reader));
                    }
                }
            }
            return warehouses;
        }

        public List<Warehouse> GetAllActive()
        {
            var warehouses = new List<Warehouse>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT WarehouseId, Code, Name, Address, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Warehouses WHERE IsActive = 1 ORDER BY Code";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) warehouses.Add(MapWarehouse(reader));
                    }
                }
            }
            return warehouses;
        }

        public bool CodeExists(string code, int? excludeWarehouseId = null)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = excludeWarehouseId.HasValue
                    ? "SELECT COUNT(1) FROM Warehouses WHERE Code = @Code AND WarehouseId != @ExcludeWarehouseId"
                    : "SELECT COUNT(1) FROM Warehouses WHERE Code = @Code";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Code", code));
                    if (excludeWarehouseId.HasValue)
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ExcludeWarehouseId", excludeWarehouseId.Value));
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public int Insert(Warehouse entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Warehouses (Code, Name, Address, IsActive, CreatedAt, CreatedBy) 
                             VALUES (@Code, @Name, @Address, @IsActive, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Code", entity.Code));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Name", entity.Name));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Address", entity.Address));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", entity.CreatedBy));
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void Update(Warehouse entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Warehouses SET Code = @Code, Name = @Name, Address = @Address, 
                             IsActive = @IsActive, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy 
                             WHERE WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", entity.WarehouseId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Code", entity.Code));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Name", entity.Name));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Address", entity.Address));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", entity.UpdatedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM Warehouses WHERE WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Warehouses SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy WHERE WarehouseId = @WarehouseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private Warehouse MapWarehouse(SqlDataReader reader)
        {
            return new Warehouse
            {
                WarehouseId = (int)reader["WarehouseId"],
                Code = reader["Code"].ToString(),
                Name = reader["Name"].ToString(),
                Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? (int?)null : (int)reader["CreatedBy"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["UpdatedAt"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"]
            };
        }
    }
}
