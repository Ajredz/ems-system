using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_transaction")]
    public class TableVarTransaction
    {
        [Key]
        [Column("order")]
        public int Order { get; set; }
        [Column("step")]
        public string Step { get; set; }
        [Column("step_code")]
        public string StepCode { get; set; }
        [Column("workflow_code")]
        public string WorkflowCode { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("status_code")]
        public string StatusCode { get; set; }
        [Column("timestamp")]
        public string Timestamp { get; set; }
        [Column("date_scheduled")]
        public string DateScheduled { get; set; }
        [Column("date_completed")]
        public string DateCompleted { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("result_type")]
        public string ResultType { get; set; }
        [Column("start_date_time")]
        public DateTime? StartDatetime { get; set; }
    }
}