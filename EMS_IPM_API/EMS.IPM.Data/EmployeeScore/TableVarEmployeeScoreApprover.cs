using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_score_approver")]
    public class TableVarEmployeeScoreApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("approver_id")]
        public int ApproverID { get; set; }

        [Column("employee_id")]
        public int EmployeeID { get; set; }

        [Column("status")]
        public string Status { get; set; }
        
        [Column("tdate_from")]
        public string TDateFrom { get; set; }

        [Column("tdate_to")]
        public string TDateTo { get; set; }

    }
}