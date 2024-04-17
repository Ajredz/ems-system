using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_approval_history")]
    public class TableVarMRFApprovalHistory
    {
        [Key]
        [Column("hierarchy_level")]
        public int HierarchyLevel { get; set; }
        [Column("position_id")]
        public int PositionID { get; set; }
        [Column("position_code")]
        public string PositionCode { get; set; }
        [Column("org_group_id")]
        public int OrgGroupID { get; set; }
        [Column("org_group_code")]
        public string OrgGroupCode { get; set; }
        [Column("approval_status")]
        public string ApprovalStatus { get; set; }
        [Column("alt_position_id")]
        public int AltPositionID { get; set; }
        [Column("alt_org_group_id")]
        public int AltOrgGroupID { get; set; }
        [Column("approval_status_code")]
        public string ApprovalStatusCode { get; set; }
        [Column("approved_date")]
        public string ApprovedDate { get; set; }
        [Column("approver_id")]
        public int ApproverID { get; set; }
        [Column("approval_remarks")]
        public string ApprovalRemarks { get; set; }
    }
}