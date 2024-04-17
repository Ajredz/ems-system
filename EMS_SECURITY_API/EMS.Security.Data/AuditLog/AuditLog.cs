using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data.AuditLog
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("audit_log")]
    public class AuditLog
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        
        [Column("event_type")]
        public string EventType { get; set; }
        
        [Column("table_name")]
        public string TableName { get; set; }
        
        [Column("table_id")]
        public long TableID { get; set; }
        
        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("ip_address")]
        public string IPAddress { get; set; }

        [Column("is_success")]
        public bool IsSuccess { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}