using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRFSignatories
{
    [Table("tv_mrf_signatories")]
    public class TableVarMRFSignatories
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("workflow_step_id")]
        public int WorkflowStepID { get; set; }

        [Column("workflow_step_approver_id")]
        public int WorkflowStepApproverID { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [Column("position_description")]
        public string PositionDescription { get; set; }

        [Column("position_id")]
        public int PositionID { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("position_level_id")]
        public int PositionLevelID { get; set; }

        [Column("position_level_description")]
        public string PositionLevelDescription { get; set; }

        [Column("approver_name")]
        public string ApproverName { get; set; }

        [Column("approver_role_id")]
        public int ApproverRoleID { get; set; }

        [Column("approver_description")]
        public string ApproverDescription { get; set; }

        [Column("workflow_step_code")]
        public string WorkflowStepCode { get; set; }

        [Column("tat_days")]
        public int TATDays { get; set; }

        [Column("order")]
        public int Order { get; set; }
    }
}