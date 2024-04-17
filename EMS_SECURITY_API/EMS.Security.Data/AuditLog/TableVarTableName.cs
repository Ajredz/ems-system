using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Security.Data.AuditLog
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_table_name")]
    public class TableVarTableName
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("table_name")]
        public string TableName { get; set; }
    }
}
