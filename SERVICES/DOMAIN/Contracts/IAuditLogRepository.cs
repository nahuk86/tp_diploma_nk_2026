using System.Collections.Generic;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DOMAIN.Contracts
{
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Registra un cambio en la auditoría del sistema
        /// </summary>
        void LogChange(string tableName, int recordId, AuditAction action, string fieldName, string oldValue, string newValue, int? changedBy);
        
        /// <summary>
        /// Obtiene los registros de auditoría de una tabla y registro específico
        /// </summary>
        List<AuditLog> GetByTable(string tableName, int recordId);
        
        /// <summary>
        /// Obtiene los registros de auditoría dentro de un rango de fechas
        /// </summary>
        List<AuditLog> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        
        /// <summary>
        /// Obtiene los registros de auditoría realizados por un usuario específico
        /// </summary>
        List<AuditLog> GetByUser(int userId);
    }
}
