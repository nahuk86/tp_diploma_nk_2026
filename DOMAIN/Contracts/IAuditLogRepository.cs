using System.Collections.Generic;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DOMAIN.Contracts
{
    public interface IAuditLogRepository
    {
        void LogChange(string tableName, int recordId, AuditAction action, string fieldName, string oldValue, string newValue, int? changedBy);
        List<AuditLog> GetByTable(string tableName, int recordId);
        List<AuditLog> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        List<AuditLog> GetByUser(int userId);
    }
}
