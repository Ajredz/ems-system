using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("mrf")]
    public class MRF
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("mrf_transaction_id")]
        public string MRFTransactionID { get; set; }

        [Column("old_mrf_id")]
        public string OldMRFID { get; set; }

        [Column("org_group_id")]
        public int OrgGroupID { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("is_confidential")]
        public bool IsConfidential { get; set; }

        [Column("nature_of_employment")]
        public string NatureOfEmployment { get; set; }

        [Column("purpose")]
        public string Purpose { get; set; }

        [Column("vacancy")]
        public int Vacancy { get; set; }

        [Column("turnaround_time")]
        public int TurnaroundTime { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("reason_for_cancellation")]
        public string ReasonForCancellation { get; set; }

        [Column("status")]
        public string Status { get; set; }

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
        
        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [Column("company_id")]
        public short CompanyID { get; set; }
        
        [Column("approver_position_id")]
        public int ApproverPositionID { get; set; }
        
        [Column("approver_org_group_id")]
        public int ApproverOrgGroupID { get; set; }

        [Column("alt_approver_position_id")]
        public int AltApproverPositionID { get; set; }

        [Column("alt_approver_org_group_id")]
        public int AltApproverOrgGroupID { get; set; }

        [Column("temp_approver_id")]
        public int TempApproverID { get; set; }

        [Column("approval_level")]
        public int ApprovalLevel { get; set; }
        
        [Column("max_approval_level")]
        public int MaxApprovalLevel { get; set; }

        [Column("cancelled_by")]
        public int? CancelledBy { get; set; }

        [Column("OnlinePosition")]
        public string OnlinePosition { get; set; }

        [Column("OnlineLocation")]
        public string OnlineLocation { get; set; }

        [Column("OnlineJobDescription")]
        public string OnlineJobDescription { get; set; }

        [Column("OnlineJobQualification")]
        public string OnlineJobQualification { get; set; }

        [Column("IsAvailableOnline")]
        public bool IsAvailableOnline { get; set; }
    }
}