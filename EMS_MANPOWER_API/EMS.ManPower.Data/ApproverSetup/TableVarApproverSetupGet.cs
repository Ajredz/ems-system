using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.ApproverSetup
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_approver_setup_get")]
    public class TableVarApproverSetupGet
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("org_group")]
        public string OrgGroup { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }
        [Column("approver_position_id")]
        public int ApproverPositionID { get; set; }
        [Column("approver_position")]
        public string ApproverPosition { get; set; }
        [Column("approver_org_group_id")]
        public int ApproverOrgGroupID { get; set; }
        [Column("approver_org_group")]
        public string ApproverOrgGroup { get; set; }
        [Column("alt_approver_position_id")]
        public int AltApproverPositionID { get; set; }
        [Column("alt_approver_position")]
        public string AltApproverPosition { get; set; }
        [Column("alt_approver_org_group_id")]
        public int AltApproverOrgGroupID { get; set; }
        [Column("alt_approver_org_group")]
        public string AltApproverOrgGroup { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}