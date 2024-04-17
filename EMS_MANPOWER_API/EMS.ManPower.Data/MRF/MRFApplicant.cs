using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("mrf_applicant")]
    public class MRFApplicant
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("mrf_id")]
        public int MRFID { get; set; }

        [Column("applicant_id")]
        public int ApplicantID { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("current_step_code")]
        public string CurrentStepCode { get; set; }

        [Column("current_step_description")]
        public string CurrentStepDescription { get; set; }
        [Column("workflow_status")]
        public string WorkflowStatus { get; set; }

        [Column("current_step_approver_role_ids")]
        public string CurrentStepApproverRoleIDs { get; set; }

        [Column("date_scheduled")]
        public DateTime? DateScheduled { get; set; }

        [Column("date_completed")]
        public DateTime? DateCompleted { get; set; }

        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }

        [Column("result_type")]
        public string ResultType { get; set; }

        [Column("for_hiring")]
        public bool ForHiring { get; set; }

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