﻿using Utilities.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Workflow.Data.Workflow
{
    // Table and Column names are all lower case to be mapped with MySQL 
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_workflow_step_approver")]
    public class TableVarWorkflowStepApprover
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("workflow_id")]
        public int WorkflowID { get; set; }

        [Column("workflow_code")]
        public string WorkflowCode { get; set; }

        [Column("step_code")]
        public string StepCode { get; set; }
        
        [Column("role_id")]
        public int RoleID { get; set; }

    }
}
