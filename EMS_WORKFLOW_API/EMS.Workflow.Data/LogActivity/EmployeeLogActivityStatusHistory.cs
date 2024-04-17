using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.LogActivity
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_log_activity_status_history")]
    public class EmployeeLogActivityStatusHistory
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        
        [Column("employee_log_activity_id")]
        public int EmployeeLogActivityID { get; set; }
        
        [Column("status")]
        public string Status { get; set; }
        
        [Column("remarks")]
        public string Remarks { get; set; }
        
        [Column("user_id")]
        public int UserID { get; set; }
        
        [Column("timestamp")]
        public DateTime? Timestamp { get; set; }

        [Column("is_pass")]
        public bool IsPass { get; set; }
    }
}
