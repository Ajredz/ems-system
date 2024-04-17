using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_export_count_by_org_id")]
    public class TableVarOrgGroupExportCountByOrgID
    {
        [Key]
        [Column("selected_org_group_code")]
        public string SelectedOrgGroupCode { get; set; }
        [Column("selected_org_group_description")]
        public string SelectedOrgGroupDescription { get; set; }
        [Column("selected_org_type")]
        public string SelectedOrgType { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
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
    }
}