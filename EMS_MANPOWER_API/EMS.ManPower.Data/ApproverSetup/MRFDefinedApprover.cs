using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.ApproverSetup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("mrf_defined_approver")]
    public class MRFDefinedApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }
        [Column("requesting_position_id")]
        public int RequestingPositionID { get; set; }
        [Column("requesting_org_group_id")]
        public int RequestingOrgGroupID { get; set; }
        [Column("approver_position_id")]
        public int ApproverPositionID { get; set; }
        [Column("approver_org_group_id")]
        public int ApproverOrgGroupID { get; set; }

        [Column("alt_approver_position_id")]
        public int? AltApproverPositionID { get; set; }

        [Column("alt_approver_org_group_id")]
        public int? AltApproverOrgGroupID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
        
    }
}