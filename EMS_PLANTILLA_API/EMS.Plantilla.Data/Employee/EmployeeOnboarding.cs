using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.API;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("employee_onboarding")]
    public class EmployeeOnboarding

    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("employee_id")]
        public int EmployeeID { get; set; }
        
        [Column("current_step_approver_role_ids")]
        public string CurrentStepApproverRoleIDs { get; set; }
        
        [Column("workflow_status")]
        public string WorkflowStatus { get; set; }
        
        [Column("current_step_description")]
        public string CurrentStepDescription { get; set; }
        
        [Column("current_step_code")]
        public string CurrentStepCode { get; set; }

        [Column("date_scheduled")]
        public DateTime? DateScheduled { get; set; }

        [Column("date_completed")]
        public DateTime? DateCompleted { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
    }
}