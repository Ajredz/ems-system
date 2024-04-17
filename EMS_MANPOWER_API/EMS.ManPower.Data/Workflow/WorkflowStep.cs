using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Manpower.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("workflow_step")]
    public class WorkflowStep
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("code")]
        public string Code { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Column("previous_step_code")]
        public string PreviousStepCode { get; set; }

        [Column("is_required")]
        public bool IsRequired { get; set; }

        [Column("tat_days")]
        public int TATDays { get; set; }

        [Column("allow_backflow")]
        public bool AllowBackflow { get; set; }

        [Column("result_type")]
        public string ResultType { get; set; }

        [Column("order")]
        public int Order { get; set; }
    }
}