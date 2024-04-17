using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Security.Data.AuditLog
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_audit_logs")]
    public class TableVarAuditLogs
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("event_type")]
        public string Type { get; set; }

        [Column("table_name")]
        public string TableName { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
        
        [Column("name")]
        public string Name { get; set; }

        [Column("ip_address")]
        public string IPAddress { get; set; }

        [Column("date_created")]
        public string DateCreated { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}
