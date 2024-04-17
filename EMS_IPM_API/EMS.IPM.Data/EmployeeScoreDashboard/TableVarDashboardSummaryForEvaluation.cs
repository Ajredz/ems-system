using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.IPM.Data.EmployeeScoreDashboard
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_dashboard_summary_for_evaluation")]
    public class TableVarDashboardSummaryForEvaluation
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("region")]
        public string Region { get; set; }

        [Column("with_complete_score")]
        public int WithCompleteScore { get; set; }

        [Column("with_missing_score")]
        public int WithMissingScore { get; set; }

        [Column("no_score")]
        public int NoScore { get; set; }

        [Column("on_going_evaluation")]
        public int OnGoingEvaluation { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }
    }
}