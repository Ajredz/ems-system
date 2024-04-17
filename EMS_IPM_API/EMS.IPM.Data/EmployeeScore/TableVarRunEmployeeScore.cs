using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_run_employee_score")]
    public class TableVarRunEmployeeScore
    {
        [Column("tdate_from")]
        public DateTime TDateFrom { get; set; }

        [Column("tdate_to")]
        public DateTime TDateTo { get; set; }

        [Key]
        [Column("employee_id")]
        public int Employee { get; set; }

        [Column("org_group_id")]
        public int OrgGroup { get; set; }

        [Column("position_id")]
        public int Position { get; set; }

        [Column("kpi_id")]
        public int KPI { get; set; }        

        [Column("kpi_weight")]
        public decimal KPIWeight { get; set; }

        [Column("kpi_score")]
        public decimal? KPIScore { get; set; }

        [Column("pdate_from")]
        public DateTime PDateFrom { get; set; }

        [Column("pdate_to")]
        public DateTime PDateTo { get; set; }
    }
}