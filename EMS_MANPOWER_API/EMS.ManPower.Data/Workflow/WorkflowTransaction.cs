using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("workflow_transaction")]
    public class WorkflowTransaction
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("workflow_code")]
        public string WorkflowCode { get; set; }

        [Column("request_type")]
        public string RequestType { get; set; }

        [Column("step_code")]
        public string StepCode { get; set; }

        [Column("step_order")]
        public int StepOrder { get; set; }

        [Column("max_step_order")]
        public int MaxStepOrder { get; set; }

        [Column("record_id")]
        public string RecordID { get; set; }

        [Column("result")]
        public string Result { get; set; }

        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }

        [Column("start_date_time")]
        public DateTime? StartDateTime { get; set; }

        [Column("end_date_time")]
        public DateTime? EndDateTime { get; set; }
    }
}