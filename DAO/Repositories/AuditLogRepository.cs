using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DAO.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        /// <summary>
        /// Registra un cambio en el log de auditoría
        /// </summary>
        /// <param name="tableName">Nombre de la tabla donde se realizó el cambio</param>
        /// <param name="recordId">ID del registro que fue modificado</param>
        /// <param name="action">Acción realizada (INSERT, UPDATE, DELETE)</param>
        /// <param name="fieldName">Nombre del campo modificado</param>
        /// <param name="oldValue">Valor anterior del campo</param>
        /// <param name="newValue">Valor nuevo del campo</param>
        /// <param name="changedBy">ID del usuario que realizó el cambio</param>
        public void LogChange(string tableName, int recordId, AuditAction action, string fieldName, string oldValue, string newValue, int? changedBy)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"INSERT INTO AuditLog (TableName, RecordId, Action, FieldName, OldValue, NewValue, ChangedAt, ChangedBy) 
                             VALUES (@TableName, @RecordId, @Action, @FieldName, @OldValue, @NewValue, @ChangedAt, @ChangedBy)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@TableName", tableName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RecordId", recordId));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@Action", action.ToString().ToUpper()));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@FieldName", fieldName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@OldValue", oldValue));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@NewValue", newValue));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ChangedAt", DateTime.Now));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ChangedBy", changedBy));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Obtiene los registros de auditoría de una tabla y registro específico
        /// </summary>
        /// <param name="tableName">Nombre de la tabla a consultar</param>
        /// <param name="recordId">ID del registro a consultar</param>
        /// <returns>Lista de registros de auditoría ordenados por fecha descendente</returns>
        public List<AuditLog> GetByTable(string tableName, int recordId)
        {
            var logs = new List<AuditLog>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT a.AuditId, a.TableName, a.RecordId, a.Action, a.FieldName, a.OldValue, a.NewValue, a.ChangedAt, a.ChangedBy, u.Username AS ChangedByUsername
                             FROM AuditLog a
                             LEFT JOIN Users u ON a.ChangedBy = u.UserId
                             WHERE a.TableName = @TableName AND a.RecordId = @RecordId
                             ORDER BY a.ChangedAt DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@TableName", tableName));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@RecordId", recordId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) logs.Add(MapAuditLog(reader));
                    }
                }
            }
            return logs;
        }

        /// <summary>
        /// Obtiene los registros de auditoría en un rango de fechas específico
        /// </summary>
        /// <param name="startDate">Fecha inicial del rango de búsqueda</param>
        /// <param name="endDate">Fecha final del rango de búsqueda</param>
        /// <returns>Lista de registros de auditoría dentro del rango de fechas ordenados por fecha descendente</returns>
        public List<AuditLog> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var logs = new List<AuditLog>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT a.AuditId, a.TableName, a.RecordId, a.Action, a.FieldName, a.OldValue, a.NewValue, a.ChangedAt, a.ChangedBy, u.Username AS ChangedByUsername
                             FROM AuditLog a
                             LEFT JOIN Users u ON a.ChangedBy = u.UserId
                             WHERE a.ChangedAt BETWEEN @StartDate AND @EndDate
                             ORDER BY a.ChangedAt DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate));
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) logs.Add(MapAuditLog(reader));
                    }
                }
            }
            return logs;
        }

        /// <summary>
        /// Obtiene los registros de auditoría realizados por un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario que realizó los cambios</param>
        /// <returns>Lista de registros de auditoría del usuario ordenados por fecha descendente</returns>
        public List<AuditLog> GetByUser(int userId)
        {
            var logs = new List<AuditLog>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"SELECT a.AuditId, a.TableName, a.RecordId, a.Action, a.FieldName, a.OldValue, a.NewValue, a.ChangedAt, a.ChangedBy, u.Username AS ChangedByUsername
                             FROM AuditLog a
                             LEFT JOIN Users u ON a.ChangedBy = u.UserId
                             WHERE a.ChangedBy = @UserId
                             ORDER BY a.ChangedAt DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@UserId", userId));
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) logs.Add(MapAuditLog(reader));
                    }
                }
            }
            return logs;
        }

        /// <summary>
        /// Mapea los datos del lector SQL a una entidad de log de auditoría
        /// </summary>
        /// <param name="reader">Lector de datos SQL con la información del registro</param>
        /// <returns>Objeto AuditLog con los datos mapeados</returns>
        private AuditLog MapAuditLog(SqlDataReader reader)
        {
            return new AuditLog
            {
                AuditId = (int)reader["AuditId"],
                TableName = reader["TableName"].ToString(),
                RecordId = (int)reader["RecordId"],
                Action = reader["Action"].ToString(),
                FieldName = reader["FieldName"] == DBNull.Value ? null : reader["FieldName"].ToString(),
                OldValue = reader["OldValue"] == DBNull.Value ? null : reader["OldValue"].ToString(),
                NewValue = reader["NewValue"] == DBNull.Value ? null : reader["NewValue"].ToString(),
                ChangedAt = (DateTime)reader["ChangedAt"],
                ChangedBy = reader["ChangedBy"] == DBNull.Value ? (int?)null : (int)reader["ChangedBy"],
                ChangedByUsername = reader["ChangedByUsername"] == DBNull.Value ? null : reader["ChangedByUsername"].ToString()
            };
        }
    }
}
