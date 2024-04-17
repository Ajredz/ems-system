using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_trans_employee_score_summary_id")]
    public class TableVarTransEmployeeScoreSummaryID
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
    }
}