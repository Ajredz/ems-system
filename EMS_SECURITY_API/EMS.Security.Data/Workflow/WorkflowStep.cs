using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("workflow_step")]
    public class WorkflowStep
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("step_id")]
        public string StepID { get; set; }
        
        [Column("step_description")]
        public string StepDescription { get; set; }

        [Column("previous_step_id")]
        public string PreviousStepID { get; set; }

        [Column("is_required")]
        public bool IsRequired { get; set; }

        [Column("tat_days")]
        public int TATDays { get; set; }

        [Column("allow_backflow")]
        public bool AllowBackflow { get; set; }

        [Column("result_type")]
        public string ResultType { get; set; }


    }
}