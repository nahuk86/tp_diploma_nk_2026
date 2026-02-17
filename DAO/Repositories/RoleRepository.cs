using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;

namespace DAO.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// Obtiene un rol por su identificador
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        /// <returns>El rol encontrado o null si no existe</returns>
        public Role GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT RoleId, RoleName, Description, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Roles WHERE RoleId = @RoleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", id));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapRole(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene un rol por su nombre
        /// </summary>
        /// <param name="roleName">Nombre del rol</param>
        /// <returns>El rol encontrado o null si no existe</returns>
        public Role GetByName(string roleName)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT RoleId, RoleName, Description, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Roles WHERE RoleName = @RoleName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleName", roleName));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return MapRole(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene la lista completa de roles ordenados por nombre
        /// </summary>
        /// <returns>Lista de todos los roles</returns>
        public List<Role> GetAll()
        {
            var roles = new List<Role>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT RoleId, RoleName, Description, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Roles ORDER BY RoleName";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) roles.Add(MapRole(reader));
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// Obtiene la lista de roles activos ordenados por nombre
        /// </summary>
        /// <returns>Lista de roles activos</returns>
        public List<Role> GetAllActive()
        {
            var roles = new List<Role>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "SELECT RoleId, RoleName, Description, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy FROM Roles WHERE IsActive = 1 ORDER BY RoleName";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) roles.Add(MapRole(reader));
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// Obtiene los permisos asignados a un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <returns>Lista de permisos asignados al rol</returns>
        public List<Permission> GetRolePermissions(int roleId)
        {
            var permissions = new List<Permission>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT p.PermissionId, p.PermissionCode, p.PermissionName, p.Description, p.Module, p.IsActive, p.CreatedAt 
                             FROM Permissions p INNER JOIN RolePermissions rp ON p.PermissionId = rp.PermissionId 
                             WHERE rp.RoleId = @RoleId AND p.IsActive = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) permissions.Add(MapPermission(reader));
                    }
                }
            }
            return permissions;
        }

        /// <summary>
        /// Asigna un permiso a un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <param name="permissionId">Identificador del permiso</param>
        /// <param name="assignedBy">Identificador del usuario que realiza la asignación</param>
        public void AssignPermission(int roleId, int permissionId, int assignedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId)
                             INSERT INTO RolePermissions (RoleId, PermissionId, AssignedAt, AssignedBy) 
                             VALUES (@RoleId, @PermissionId, @AssignedAt, @AssignedBy)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", permissionId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@AssignedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@AssignedBy", assignedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Remueve un permiso de un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <param name="permissionId">Identificador del permiso</param>
        public void RemovePermission(int roleId, int permissionId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@PermissionId", permissionId));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina todos los permisos asignados a un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        public void ClearPermissions(int roleId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM RolePermissions WHERE RoleId = @RoleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", roleId));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo rol en la base de datos
        /// </summary>
        /// <param name="entity">Entidad de rol a insertar</param>
        /// <returns>El identificador del rol insertado</returns>
        public int Insert(Role entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO Roles (RoleName, Description, IsActive, CreatedAt, CreatedBy) 
                             VALUES (@RoleName, @Description, @IsActive, @CreatedAt, @CreatedBy);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleName", entity.RoleName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@CreatedBy", entity.CreatedBy));
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de un rol existente
        /// </summary>
        /// <param name="entity">Entidad de rol con los datos actualizados</param>
        public void Update(Role entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"UPDATE Roles SET RoleName = @RoleName, Description = @Description, IsActive = @IsActive, 
                             UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy WHERE RoleId = @RoleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", entity.RoleId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleName", entity.RoleName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Description", entity.Description));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@IsActive", entity.IsActive));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", entity.UpdatedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina físicamente un rol de la base de datos
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "DELETE FROM Roles WHERE RoleId = @RoleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica marcando el rol como inactivo
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        /// <param name="deletedBy">Identificador del usuario que realiza la eliminación</param>
        public void SoftDelete(int id, int deletedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = "UPDATE Roles SET IsActive = 0, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy WHERE RoleId = @RoleId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RoleId", id));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UpdatedBy", deletedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
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

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de permiso
        /// </summary>
        /// <param name="reader">Lector de datos SQL</param>
        /// <returns>Entidad de permiso mapeada</returns>
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
