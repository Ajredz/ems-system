using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_current_workflow_step")]
    public class TableVarCurrentWorkflowStep
    {
        [Key]
        [Column("step_code")]
        public string StepCode { get; set; }
        [Column("step_description")]
        public string StepDescription { get; set; }
        [Column("approver_role_ids")]
        public string ApproverRoleIDs { get; set; }
        [Column("workflow_status")]
        public string WorkflowStatus { get; set; }

    }
}