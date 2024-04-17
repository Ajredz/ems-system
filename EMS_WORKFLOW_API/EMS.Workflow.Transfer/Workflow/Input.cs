using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Workflow.Transfer.Workflow
{
    public class AddWorkflowTransaction
    {
        public int RecordID { get; set; }

        public int WorkflowID { get; set; }
        public string WorkflowCode { get; set; }
        
        public List<int> BatchUpdateRecordIDs { get; set; }
        public List<int> BatchUpdateApplicantIDs { get; set; }

        public string CurrentStepCode { get; set; }

        public string Result { get; set; }
        public string Remarks { get; set; }

        public string DateScheduled { get; set; }

        public string DateCompleted { get; set; }
        public DateTime? StartDatetime { get; set; }

        public List<GetTransactionByRecordIDOutput> History { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsExport { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public string Module { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public List<WorkflowStep> WorkflowStepList { get; set; }
    }

    public class WorkflowStep
    {
        public string StepCode { get; set; }
        public string StepDescription { get; set; }
        public string StatusColor { get; set; }
        public bool IsRequired { get; set; }
        public int TATDays { get; set; }
        public bool AllowBackflow { get; set; }
        public string ResultType { get; set; }
        public bool SendEmailToRequester { get; set; }
        public bool SendEmailToApprover { get; set; }
        public int Order { get; set; }
        public List<WorkflowStepApprover> WorkflowStepApproverList { get; set; }
    }

    public class WorkflowStepApprover
    {
        public string StepCode { get; set; }
        public int RoleID { get; set; }
    }

    public class GetAutoCompleteInput
    {
        public string Term { get; set; }

        public int TopResults { get; set; }
    } 
    
    public class GetWorkflowStepAutoCompleteInput
    {
        public string WorkflowCode { get; set; }

        public string Term { get; set; }

        public int TopResults { get; set; }
    }

    public class GetTransactionByRecordIDInput
    {
        public string WorkflowCode { get; set; }
        public int WorkflowID { get; set; }
        public int RecordID { get; set; }
    }

    public class GetWorkflowStepByWorkflowIDAndCodeInput
    {
        public string WorkflowCode { get; set; }
        public string Code { get; set; }
    }

    public class GetNextWorkflowStepInput
    {
        public string WorkflowCode { get; set; }
        public string CurrentStepCode { get; set; }
        public string RoleIDDelimited { get; set; }
    }

    public class GetWorkflowStepByRoleInput
    {
        public string WorkflowCode { get; set; }
        public string RoleIDDelimited { get; set; }
    }

    public class GetAllWorkflowStepInput
    {
        public string WorkflowCode { get; set; }
    }

    public class GetRolesByWorkflowStepCodeInput
    {
        public string WorkflowCode { get; set; }
        public string StepCode { get; set; }
    }

}
