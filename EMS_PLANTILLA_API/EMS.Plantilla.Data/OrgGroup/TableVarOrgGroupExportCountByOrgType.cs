using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_export_count_by_org_type")]
    public class TableVarOrgGroupExportCountByOrgType
    {
        [Key]
        [Column("org_type")]
        public string OrgType { get; set; }
        [Column("org_type_id")]
        public int OrgTypeID { get; set; }
        [Column("org_type_code")]
        public string OrgTypeCode { get; set; }
        [Column("org_type_description")]
        public string OrgTypeDescription { get; set; }
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