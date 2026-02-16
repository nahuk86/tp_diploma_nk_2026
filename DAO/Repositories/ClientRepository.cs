using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public Client GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT ClientId, Nombre, Apellido, Correo, DNI, Telefono, Direccion, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Clients WHERE ClientId = @ClientId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", id));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapClient(reader);
                    }
                }
            }
            return null;
        }

        public Client GetByDNI(string dni)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT ClientId, Nombre, Apellido, Correo, DNI, Telefono, Direccion, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Clients WHERE DNI = @DNI";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@DNI", dni));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapClient(reader);
                    }
                }
            }
            return null;
        }

        public List<Client> GetAll()
        {
            var clients = new List<Client>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT ClientId, Nombre, Apellido, Correo, DNI, Telefono, Direccion, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Clients ORDER BY Apellido, Nombre";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) clients.Add(MapClient(reader));
                    }
                }
            }
            return clients;
        }

        public List<Client> GetAllActive()
        {
            var clients = new List<Client>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT ClientId, Nombre, Apellido, Correo, DNI, Telefono, Direccion, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Clients WHERE IsActive = 1 ORDER BY Apellido, Nombre";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) clients.Add(MapClient(reader));
                    }
                }
            }
            return clients;
        }

        public bool DNIExists(string dni, int? excludeClientId = null)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = excludeClientId.HasValue
                    ? "SELECT COUNT(1) FROM Clients WHERE DNI = @DNI AND ClientId != @ExcludeClientId"
                    : "SELECT COUNT(1) FROM Clients WHERE DNI = @DNI";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@DNI", dni));
                    if (excludeClientId.HasValue)
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ExcludeClientId", excludeClientId.Value));
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public int Insert(Client entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Clients (Nombre, Apellido, Correo, DNI, Telefono, Direccion, IsActive, CreatedAt, CreatedBy) 
                             VALUES (@Nombre, @Apellido, @Correo, @DNI, @Telefono, @Direccion, @IsActive, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Nombre", entity.Nombre));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Apellido", entity.Apellido));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Correo", entity.Correo));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@DNI", entity.DNI));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Telefono", entity.Telefono));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Direccion", entity.Direccion));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", entity.CreatedBy));
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void Update(Client entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Clients SET Nombre = @Nombre, Apellido = @Apellido, Correo = @Correo, 
                             DNI = @DNI, Telefono = @Telefono, Direccion = @Direccion, 
                             IsActive = @IsActive, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy 
                             WHERE ClientId = @ClientId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", entity.ClientId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Nombre", entity.Nombre));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Apellido", entity.Apellido));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Correo", entity.Correo));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@DNI", entity.DNI));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Telefono", entity.Telefono));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Direccion", entity.Direccion));
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
                var query = "DELETE FROM Clients WHERE ClientId = @ClientId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Clients SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy WHERE ClientId = @ClientId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private Client MapClient(SqlDataReader reader)
        {
            return new Client
            {
                ClientId = (int)reader["ClientId"],
                Nombre = reader["Nombre"].ToString(),
                Apellido = reader["Apellido"].ToString(),
                Correo = reader["Correo"] == DBNull.Value ? null : reader["Correo"].ToString(),
                DNI = reader["DNI"].ToString(),
                Telefono = reader["Telefono"] == DBNull.Value ? null : reader["Telefono"].ToString(),
                Direccion = reader["Direccion"] == DBNull.Value ? null : reader["Direccion"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? (int?)null : (int)reader["CreatedBy"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["UpdatedAt"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"]
            };
        }
    }
}
