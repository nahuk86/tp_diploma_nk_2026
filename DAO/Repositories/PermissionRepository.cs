using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public Permission GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT PermissionId, PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt FROM Permissions WHERE PermissionId = @PermissionId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", id));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapPermission(reader);
                        }
                    }
                }
            }
            return null;
        }

        public Permission GetByCode(string permissionCode)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT PermissionId, PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt FROM Permissions WHERE PermissionCode = @PermissionCode";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionCode", permissionCode));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapPermission(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<Permission> GetAll()
        {
            var permissions = new List<Permission>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT PermissionId, PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt FROM Permissions ORDER BY Module, PermissionName";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissions.Add(MapPermission(reader));
                        }
                    }
                }
            }
            
            return permissions;
        }

        public List<Permission> GetAllActive()
        {
            var permissions = new List<Permission>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT PermissionId, PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt FROM Permissions WHERE IsActive = 1 ORDER BY Module, PermissionName";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissions.Add(MapPermission(reader));
                        }
                    }
                }
            }
            
            return permissions;
        }

        public List<Permission> GetByModule(string module)
        {
            var permissions = new List<Permission>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT PermissionId, PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt FROM Permissions WHERE Module = @Module ORDER BY PermissionName";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Module", module));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissions.Add(MapPermission(reader));
                        }
                    }
                }
            }
            
            return permissions;
        }

        public List<string> GetUserPermissions(int userId)
        {
            var permissionCodes = new List<string>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT DISTINCT p.PermissionCode 
                             FROM Permissions p 
                             INNER JOIN RolePermissions rp ON p.PermissionId = rp.PermissionId 
                             INNER JOIN UserRoles ur ON rp.RoleId = ur.RoleId 
                             WHERE ur.UserId = @UserId AND p.IsActive = 1";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissionCodes.Add(reader["PermissionCode"].ToString());
                        }
                    }
                }
            }
            
            return permissionCodes;
        }

        public int Insert(Permission entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Permissions (PermissionCode, PermissionName, Description, Module, IsActive, CreatedAt) 
                             VALUES (@PermissionCode, @PermissionName, @Description, @Module, @IsActive, @CreatedAt);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionCode", entity.PermissionCode));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionName", entity.PermissionName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Module", entity.Module));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void Update(Permission entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Permissions SET 
                             PermissionCode = @PermissionCode, 
                             PermissionName = @PermissionName, 
                             Description = @Description, 
                             Module = @Module, 
                             IsActive = @IsActive 
                             WHERE PermissionId = @PermissionId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", entity.PermissionId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionCode", entity.PermissionCode));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionName", entity.PermissionName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Module", entity.Module));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM Permissions WHERE PermissionId = @PermissionId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Permissions SET IsActive = 0 WHERE PermissionId = @PermissionId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private Permission MapPermission(SqlDataReader reader)
        {
            return new Permission
            {
                PermissionId = (int)reader["PermissionId"],
                PermissionCode = reader["PermissionCode"].ToString(),
                PermissionName = reader["PermissionName"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Module = reader["Module"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
    }
}
