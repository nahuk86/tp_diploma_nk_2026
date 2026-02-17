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
        /// <summary>
        /// Obtiene un permiso por su identificador
        /// </summary>
        /// <param name="id">El identificador del permiso a buscar</param>
        /// <returns>El permiso encontrado o null si no existe</returns>
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

        /// <summary>
        /// Obtiene un permiso por su código
        /// </summary>
        /// <param name="permissionCode">El código único del permiso a buscar</param>
        /// <returns>El permiso encontrado o null si no existe</returns>
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

        /// <summary>
        /// Obtiene la lista completa de permisos ordenados por módulo y nombre
        /// </summary>
        /// <returns>Lista de todos los permisos en el sistema</returns>
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

        /// <summary>
        /// Obtiene la lista de permisos activos ordenados por módulo y nombre
        /// </summary>
        /// <returns>Lista de permisos que están marcados como activos</returns>
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

        /// <summary>
        /// Obtiene los permisos filtrados por módulo
        /// </summary>
        /// <param name="module">El nombre del módulo para filtrar los permisos</param>
        /// <returns>Lista de permisos del módulo especificado ordenados por nombre</returns>
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

        /// <summary>
        /// Obtiene la lista de códigos de permisos de un usuario
        /// </summary>
        /// <param name="userId">El identificador del usuario</param>
        /// <returns>Lista de códigos de permisos activos asignados al usuario a través de sus roles</returns>
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

        /// <summary>
        /// Inserta un nuevo permiso en la base de datos
        /// </summary>
        /// <param name="entity">La entidad de permiso a insertar</param>
        /// <returns>El identificador del permiso recién creado</returns>
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

        /// <summary>
        /// Actualiza los datos de un permiso existente
        /// </summary>
        /// <param name="entity">La entidad de permiso con los datos actualizados</param>
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

        /// <summary>
        /// Elimina físicamente un permiso de la base de datos
        /// </summary>
        /// <param name="id">El identificador del permiso a eliminar</param>
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

        /// <summary>
        /// Realiza una eliminación lógica marcando el permiso como inactivo
        /// </summary>
        /// <param name="id">El identificador del permiso a desactivar</param>
        /// <param name="deletedBy">El identificador del usuario que realiza la eliminación</param>
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

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de permiso
        /// </summary>
        /// <param name="reader">El lector de datos SQL con la información del permiso</param>
        /// <returns>Una entidad de permiso con los datos mapeados</returns>
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
