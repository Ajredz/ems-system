using System.Collections.Generic;
using Utilities.API;

namespace EMS.Recruitment.Transfer.Workflow
{
    public class GetListInput : JQGridFilter
    {
        public int? ID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class Form
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public List<WorkflowStep> WorkflowStepList { get; set; }
    }

    public class WorkflowStep
    {
        public string StepCode { get; set; }
        public string StepDescription { get; set; }
        public bool IsRequired { get; set; }
        public int TATDays { get; set; }
        public bool AllowBackflow { get; set; }
        public string ResultType { get; set; }
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
}
