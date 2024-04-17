using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_rollup_position_dropdown")]
    public class TableVarOrgGroupRollupPositionDropdown
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        
        [Column("org_group")]
        public string OrgGroup { get; set; }
        
        [Column("position")]
        public string Position { get; set; }
        
        [Column("planned_count")]
        public int PlannedCount { get; set; }
        
        [Column("active_count")]
        public int ActiveCount { get; set; }
        
        [Column("inactive_count")]
        public int InactiveCount { get; set; }
        
        [Column("variance_count")]
        public int VarianceCount { get; set; }
    }
}
