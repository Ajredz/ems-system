using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Plantilla.Data.OrgGroup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_org_group_export_list")]
    public class TableVarOrgGroupExportList
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("parent_org_group")]
        public string ParentOrgGroup { get; set; }

        [Column("org_group_code")]
        public string OrgGroupCode { get; set; }

        [Column("org_group_desc")]
        public string OrgGroupDesc { get; set; }

        [Column("org_type")]
        public string OrgType { get; set; }
        
        [Column("address")]
        public string Address { get; set; }
        
        [Column("is_branch_active")]
        public bool IsBranchActive { get; set; }
        [Column("service_bay_count")]
        public int ServiceBayCount { get; set; }

        [Column("company_tag")]
        public string CompanyTag { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }
        
        [Column("reporting_position_code")]
        public string ReportingPositionCode { get; set; }

        [Column("planned_count")]
        public int PlannedCount { get; set; }

        [Column("active_count")]
        public int ActiveCount { get; set; }

        [Column("inactive_count")]
        public int InactiveCount { get; set; }
        
        [Column("is_head")]
        public string IsHead { get; set; }
    }
}
