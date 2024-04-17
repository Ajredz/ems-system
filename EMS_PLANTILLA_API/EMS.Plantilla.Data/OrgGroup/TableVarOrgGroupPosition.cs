using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_position")]
    public class TableVarOrgGroupPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("position_description")]
        public string PositionDescription { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("active_count")]
        public int ActiveCount { get; set; }

        [Column("active_prob_count")]
        public int ActiveProbCount { get; set; }

        [Column("outgoing_count")]
        public int OutgoingCount { get; set; }

        [Column("total_active_count")]
        public int TotalActiveCount { get; set; }

        [Column("inactive_count")]
        public int InactiveCount { get; set; }

        [Column("is_head")]
        public bool IsHead { get; set; }

        [Column("reporting_position_id")]
        public int ReportingPositionID { get; set; }

        [Column("reporting_position_description")]
        public string ReportingPositionDescription { get; set; }

    }
}