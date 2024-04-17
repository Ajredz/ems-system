using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_log_activity_status_history")]
    public class TableVarEmployeeLogActivityStatusHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [Column("timestamp")]
        public string Timestamp { get; set; }

        [Column("is_pass")]
        public string IsPass { get; set; }
    }
}
