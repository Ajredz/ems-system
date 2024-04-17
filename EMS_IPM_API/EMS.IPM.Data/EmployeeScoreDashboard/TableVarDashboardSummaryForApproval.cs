using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScoreDashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_dashboard_summary_for_approval")]
    public class TableVarDashboardSummaryForApproval
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("region")]
        public string Region { get; set; }

        [Column("no_key_in")]
        public int NoKeyIn { get; set; }

        [Column("for_approval")]
        public int ForApproval { get; set; }

        [Column("finalized")]
        public int Finalized { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}