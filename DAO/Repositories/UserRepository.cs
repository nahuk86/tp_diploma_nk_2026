using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using SERVICES;

namespace DAO.Repositories
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Obtiene un usuario por su identificador
        /// </summary>
        /// <param name="id">Identificador único del usuario</param>
        /// <returns>Usuario encontrado o null si no existe</returns>
        public User GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users WHERE UserId = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", id));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Usuario encontrado o null si no existe</returns>
        public User GetByUsername(string username)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users WHERE Username = @Username";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Username", username));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <returns>Usuario encontrado o null si no existe</returns>
        public User GetByEmail(string email)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users WHERE Email = @Email";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Email", email));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene la lista completa de usuarios ordenados por nombre de usuario
        /// </summary>
        /// <returns>Lista de todos los usuarios</returns>
        public List<User> GetAll()
        {
            var users = new List<User>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users ORDER BY Username";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            
            return users;
        }

        /// <summary>
        /// Obtiene la lista de usuarios activos ordenados por nombre de usuario
        /// </summary>
        /// <returns>Lista de usuarios activos</returns>
        public List<User> GetAllActive()
        {
            var users = new List<User>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users WHERE IsActive = 1 ORDER BY Username";
                
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            
            return users;
        }

        /// <summary>
        /// Busca usuarios por término de búsqueda en nombre de usuario, nombre completo o correo
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de usuarios que coinciden con la búsqueda</returns>
        public List<User> Search(string searchTerm)
        {
            var users = new List<User>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, 
                             CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LastLogin 
                             FROM Users 
                             WHERE (Username LIKE @SearchTerm OR FullName LIKE @SearchTerm OR Email LIKE @SearchTerm)
                             ORDER BY Username";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@SearchTerm", "%" + searchTerm + "%"));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            
            return users;
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="entity">Entidad de usuario a insertar</param>
        /// <returns>Identificador del usuario insertado</returns>
        public int Insert(User entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Email, IsActive, CreatedAt, CreatedBy) 
                             VALUES (@Username, @PasswordHash, @PasswordSalt, @FullName, @Email, @IsActive, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Username", entity.Username));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PasswordHash", entity.PasswordHash));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PasswordSalt", entity.PasswordSalt));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@FullName", entity.FullName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Email", entity.Email));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", entity.CreatedBy));
                    
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente
        /// </summary>
        /// <param name="entity">Entidad de usuario con los datos actualizados</param>
        public void Update(User entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Users SET 
                             Username = @Username, 
                             PasswordHash = @PasswordHash, 
                             PasswordSalt = @PasswordSalt, 
                             FullName = @FullName, 
                             Email = @Email, 
                             IsActive = @IsActive, 
                             UpdatedAt = @UpdatedAt, 
                             UpdatedBy = @UpdatedBy 
                             WHERE UserId = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", entity.UserId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Username", entity.Username));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PasswordHash", entity.PasswordHash));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PasswordSalt", entity.PasswordSalt));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@FullName", entity.FullName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Email", entity.Email));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", entity.UpdatedBy));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina físicamente un usuario de la base de datos
        /// </summary>
        /// <param name="id">Identificador del usuario a eliminar</param>
        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM Users WHERE UserId = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica marcando el usuario como inactivo
        /// </summary>
        /// <param name="id">Identificador del usuario a eliminar</param>
        /// <param name="deletedBy">Identificador del usuario que realiza la eliminación</param>
        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Users SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy 
                             WHERE UserId = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Obtiene los roles asignados a un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Lista de roles asignados al usuario</returns>
        public List<Role> GetUserRoles(int userId)
        {
            var roles = new List<Role>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT r.RoleId, r.RoleName, r.Description, r.IsActive, r.CreatedAt, r.CreatedBy, r.UpdatedAt, r.UpdatedBy 
                             FROM Roles r 
                             INNER JOIN UserRoles ur ON r.RoleId = ur.RoleId 
                             WHERE ur.UserId = @UserId AND r.IsActive = 1";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(MapRole(reader));
                        }
                    }
                }
            }
            
            return roles;
        }

        /// <summary>
        /// Asigna un rol a un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="roleId">Identificador del rol</param>
        /// <param name="assignedBy">Identificador del usuario que realiza la asignación</param>
        public void AssignRole(int userId, int roleId, int assignedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
                             INSERT INTO UserRoles (UserId, RoleId, AssignedAt, AssignedBy) 
                             VALUES (@UserId, @RoleId, @AssignedAt, @AssignedBy)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@AssignedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@AssignedBy", assignedBy));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Remueve un rol de un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="roleId">Identificador del rol</param>
        public void RemoveRole(int userId, int roleId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Asigna múltiples roles a un usuario reemplazando los existentes
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="roleIds">Lista de identificadores de roles a asignar</param>
        public void AssignRoles(int userId, List<int> roleIds)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, remove all existing roles for this user
                        var deleteQuery = "DELETE FROM UserRoles WHERE UserId = @UserId";
                        using (var deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            deleteCommand.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                            deleteCommand.ExecuteNonQuery();
                        }
                        
                        // Then, assign the new roles
                        if (roleIds != null && roleIds.Count > 0)
                        {
                            var insertQuery = @"INSERT INTO UserRoles (UserId, RoleId, AssignedAt, AssignedBy) 
                                               VALUES (@UserId, @RoleId, @AssignedAt, @AssignedBy)";
                            
                            var assignedAt = DateTime.Now;
                            var assignedBy = SessionContext.CurrentUserId;
                            
                            using (var insertCommand = new SqlCommand(insertQuery, connection, transaction))
                            {
                                insertCommand.Parameters.Add("@UserId", SqlDbType.Int);
                                insertCommand.Parameters.Add("@RoleId", SqlDbType.Int);
                                insertCommand.Parameters.Add("@AssignedAt", SqlDbType.DateTime);
                                insertCommand.Parameters.Add("@AssignedBy", SqlDbType.Int);
                                
                                foreach (var roleId in roleIds)
                                {
                                    insertCommand.Parameters["@UserId"].Value = userId;
                                    insertCommand.Parameters["@RoleId"].Value = roleId;
                                    insertCommand.Parameters["@AssignedAt"].Value = assignedAt;
                                    insertCommand.Parameters["@AssignedBy"].Value = assignedBy;
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        transaction.Commit();
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
        /// Actualiza la fecha y hora del último inicio de sesión del usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        public void UpdateLastLogin(int userId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Users SET LastLogin = @LastLogin WHERE UserId = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@LastLogin", DateTime.Now));
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de usuario
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de usuario mapeada</returns>
        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = (int)reader["UserId"],
                Username = reader["Username"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                PasswordSalt = reader["PasswordSalt"].ToString(),
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? (int?)null : (int)reader["CreatedBy"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["UpdatedAt"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"],
                LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["LastLogin"]
            };
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de rol
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de rol mapeada</returns>
        private Role MapRole(SqlDataReader reader)
        {
            return new Role
            {
                RoleId = (int)reader["RoleId"],
                RoleName = reader["RoleName"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedBy = reader["CreatedBy"] == DBNull.Value ? (int?)null : (int)reader["CreatedBy"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["UpdatedAt"],
                UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? (int?)null : (int)reader["UpdatedBy"]
            };
        }
    }
}
