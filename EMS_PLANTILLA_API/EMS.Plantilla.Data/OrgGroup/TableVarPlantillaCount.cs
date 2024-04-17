using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_plantilla_count")]
    public class TableVarPlantillaCount
    {
        [Key]

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }
        
        [Column("scope_org_group_id")]
        public int ScopeOrgGroupID { get; set; }
        
        [Column("scope_org_group")]
        public string ScopeOrgGroup { get; set; }
        
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        
        [Column("org_group")]
        public string OrgGroup { get; set; }
        
        [Column("position_id")]
        public int PositionID { get; set; }
        
        [Column("position")]
        public string Position { get; set; }
        
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

        [Column("variance_count")]
        public int VarianceCount { get; set; }

        [Column("total_planned")]
        public int TotalPlanned { get; set; }

        [Column("total_active_reg")]
        public int TotalActiveReg { get; set; }

        [Column("total_active_prob")]
        public int TotalActiveProb { get; set; }

        [Column("total_active")]
        public int TotalActive { get; set; }

        [Column("total_inactive")]
        public int TotalInactive { get; set; }

        [Column("total_outgoing")]
        public int TotalOutgoing { get; set; }

        [Column("total_variance")]
        public int TotalVariance { get; set; }

    }
}