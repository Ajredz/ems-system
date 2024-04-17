using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.MRF
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_mrf_existing_applicant")]
    public class TableVarMRFExistingApplicant
    {
        [Key]
        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("mrf_applicant_id")]
        public long MRFApplicantID { get; set; }

        [Column("applicant_name")]
        public string ApplicantName { get; set; }

        [Column("current_step")]
        public string CurrentStep { get; set; }

        [Column("status")]
        public string Status { get; set; }
        
        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("current_step_code")]
        public string CurrentStepCode { get; set; }
        
        [Column("current_result")]
        public string CurrentResult { get; set; }
        
        [Column("result_type")]
        public string ResultType { get; set; }

        [Column("date_scheduled")]
        public string DateScheduled { get; set; }

        [Column("date_completed")]
        public string DateCompleted { get; set; }

        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }

        [Column("points")]
        public int Points { get; set; }

        [Column("total_points")]
        public int TotalPoints { get; set; }

        [Column("flag")]
        public int Flag { get; set; }
    }
}