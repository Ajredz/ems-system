using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScoreDashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_branches_with_position")]
    public class TableVarDashboardBranchesWithPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("branch")]
        public string Branch { get; set; }
        
        [Column("position")]
        public string Position { get; set; }

        [Column("kra_group")]
        public string KRAGroup { get; set; }
        
        [Column("kpi")]
        public string KPI { get; set; }

        [Column("ee")]
        public int EE { get; set; }

        [Column("me")]
        public int ME { get; set; }

        [Column("sbe")]
        public int SBE { get; set; }

        [Column("be")]
        public int BE { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}