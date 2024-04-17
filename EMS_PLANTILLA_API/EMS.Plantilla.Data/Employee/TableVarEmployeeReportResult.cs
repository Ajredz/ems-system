using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;


namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_report_result")]
    public class TableVarEmployeeReportResult
    {

        [Key]

        [Column("employee")]
        public int Employee { get; set; }

        [Column("org")]
        public int Org { get; set; }

        [Column("active_employee_count")]
        public int ActiveEmployeeCount { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("count_percent")]
        public decimal CountPercent { get; set; }

    }
}
