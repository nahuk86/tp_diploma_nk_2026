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
