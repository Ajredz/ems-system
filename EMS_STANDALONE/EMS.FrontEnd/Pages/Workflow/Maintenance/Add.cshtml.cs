using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace EMS.FrontEnd.Pages.Workflow.Maintenance
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.Form Workflow { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_globalCurrentUser != null)
            {
                Workflow = new EMS.Workflow.Transfer.Workflow.Form();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Workflow.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Workflow, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "Workflow",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Workflow.Code, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (Workflow.WorkflowStepList != null && Workflow.WorkflowStepList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "WorkflowStep",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Workflow.Code, " Workflow Step added"),
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