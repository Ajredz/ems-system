using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("workflow_transaction")]
    public class WorkflowTransaction
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("step_id")]
        public string StepID { get; set; }

        [Column("record_id")]
        public string RecordID { get; set; }

        [Column("approver_remarks")]
        public string ApproverRemarks { get; set; }

        [Column("start_date_time")]
        public DateTime StartDateTime { get; set; }

        [Column("end_date_time")]
        public DateTime EndDateTime { get; set; }
    }
}