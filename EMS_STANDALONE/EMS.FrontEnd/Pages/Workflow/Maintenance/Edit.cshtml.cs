using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using System.Text;

namespace EMS.FrontEnd.Pages.Workflow.Maintenance
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.Form Workflow { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            Workflow = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(ID);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Workflow.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_Workflow(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(Workflow.ID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Workflow, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder Remarks = new StringBuilder();

                if (Workflow.Module != oldValue.Module)
                {
                    Remarks.Append(string.Concat("Module changed from ", oldValue.Module, " to ", Workflow.Module, ". "));
                }

                if (Workflow.Code != oldValue.Code)
                {
                    Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", Workflow.Code, ". "));
                }

                if (Workflow.Description != oldValue.Description)
                {
                    Remarks.Append(string.Concat("Description changed from ", oldValue.Description, " to ", Workflow.Description, ". "));
                }

                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "Workflow",
                            TableID = Workflow.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (Workflow.WorkflowStepList != null && Workflow.WorkflowStepList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "WorkflowStep",
                            TableID = Workflow.ID,
                            Remarks = string.Concat(Workflow.Code, " Workflow Step updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetResultTypeDropDown()
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env).GetReferenceValueByRefCode(ReferenceCodes_Workflow.RESULT_TYPE.ToString());
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApproverRoleDropDown()
        {
            _resultView.Result = await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleDropDown();
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }
    }
}