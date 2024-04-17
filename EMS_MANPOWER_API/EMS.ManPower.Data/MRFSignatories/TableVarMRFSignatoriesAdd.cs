using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRFSignatories
{
    [Table("tv_mrf_signatories_add")]
    public class TableVarMRFSignatoriesAdd
    {
        [Key]
        [Column("requester_id")]
        public int RequesterID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("approver_description")]
        public string ApproverDescription { get; set; }

        [Column("approver_role_id")]
        public int ApproverRoleID { get; set; }

        [Column("approver_name")]
        public string ApproverName { get; set; }

        [Column("approval_tat")]
        public int ApprovalTAT { get; set; }

        [Column("approval_status")]
        public string ApprovalStatus { get; set; }  
        
        [Column("approval_status_code")]
        public string ApprovalStatusCode { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("approved_date")]
        public string ApprovedDate { get; set; }

    }
}