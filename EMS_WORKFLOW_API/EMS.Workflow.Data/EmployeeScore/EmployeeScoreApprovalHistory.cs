using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Workflow.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_score_approval_history")]
    public class EmployeeScoreApprovalHistory
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("t_id")]
        public int TID { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("approver_id")]
        public int ApproverID { get; set; }

        [Column("timestamp")]
        public DateTime? Timestamp { get; set; }

    }
}