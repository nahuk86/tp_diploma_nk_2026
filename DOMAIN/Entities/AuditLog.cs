using System;

namespace DOMAIN.Entities
{
    public class AuditLog
    {
        public int AuditId { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Action { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedAt { get; set; }
        public int? ChangedBy { get; set; }
        
        // Navigation property
        public string ChangedByUsername { get; set; }
    }
}
