using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utilities.API;

namespace EMS.Workflow.Transfer.Workflow
{
    public class GetListOutput : JQGridResult
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int WorkflowID { get; set; }
    }

    public class GetIDByAutoCompleteOutput
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class GetCodeByAutoCompleteOutput
    {
        public string ID { get; set; }
        public string Description { get; set; }
    }

    public class GetTransactionByRecordIDOutput
    {
        public int Order { get; set; }
        public string WorkflowCode { get; set; }
        public string StepCode { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string DateScheduled { get; set; }
        public string DateCompleted { get; set; }
        public string Timestamp { get; set; }
        public string Remarks { get; set; }
        public string ResultType { get; set; }
        public DateTime? StartDatetime { get; set; }
    }

    public class CurrentWorkflowStep 
    {
        public string StepCode { get; set; }
        public string StepDescription { get; set; }
        public string ApproverRoleIDs { get; set; }
        public string WorkflowStatus { get; set; }
        public string ResultType { get; set; }
    }

    public class GetWorkflowStepByWorkflowIDAndCodeOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ResultType { get; set; }
    }

    public class GetWorkflowStepByWorkflowCodeOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class GetNextWorkflowStepOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class GetWorkflowStepByRoleOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class GetAllWorkflowStepOutput
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class GetRolesByWorkflowStepCodeOutput
    {
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public string WorkflowCode { get; set; }
        public string StepCode { get; set; }
        public int RoleID { get; set; }
    }

    public class TableVarTransactionLastUpdateOutput
    {
        public int ID { get; set; }
        public string RecordID { get; set; }
        public string EndDateTime { get; set; }
        public int ApprovedBy { get; set; }
    }
}
