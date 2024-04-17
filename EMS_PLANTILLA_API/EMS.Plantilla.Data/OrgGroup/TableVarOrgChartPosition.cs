using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_chart_position")]
    public class TableVarOrgChartPosition
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("active_count")]
        public int ActiveCount { get; set; }
        
        [Column("inactive_count")]
        public int InactiveCount { get; set; }

        [Column("is_head")]
        public bool IsHead { get; set; } 

    }
}