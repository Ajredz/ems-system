using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Security.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("workflow_step_approver")]
    public class WorkflowStepApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("step_id")]
        public string StepID { get; set; }
        
        [Column("role_id")]
        public string RoleID { get; set; }

        
    }
}