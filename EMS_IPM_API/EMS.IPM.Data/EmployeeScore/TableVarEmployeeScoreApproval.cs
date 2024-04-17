using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employeescoreapproval")]
    public class TableVarEmployeeScoreApproval
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("approver_id")]
        public int ApproverID { get; set; }

        [Column("transaction_id")]
        public int TID { get; set; }

        [Column("status")]
        public string Position { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }
    }
}