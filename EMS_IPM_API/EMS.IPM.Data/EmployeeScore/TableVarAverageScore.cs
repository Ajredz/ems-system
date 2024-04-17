using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScore
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_average_score")]
    public class TableVarAverageScore
    {
        [Key]
        [Column("org_group_id")]
        public int OrgGroup { get; set; }

        [Column("kpi_id")]
        public int KPI { get; set; }

        [Column("average")]
        public decimal Average { get; set; }
    }
}