using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Maintenance;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Recruitment.Transfer.Applicant;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.ApplicationApproval
{
    public class UpdateStatusModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.AddWorkflowTransaction Form { get; set; }

        public UpdateStatusModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet(int ID, int WorkflowID ,bool HasApproval)
        {
            
            Form = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetTransactionByRecordID(new GetTransactionByRecordIDInput { WorkflowID = WorkflowID, RecordID = ID });

            if (Form.History.Where(x => x.Status.Equals("In-Progress")).Count() > 0)
            {
                var attachmentList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                       .GetReferenceValueByRefCode(Form.History.Where(x => x.Status.Equals("In-Progress")).FirstOrDefault().ResultType);
                ViewData["ResultTypeSelectList"] =
                    attachmentList.Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description,
                    }).ToList();

                ViewData["HasApproval"] = HasApproval ? "true" : "false";
            }
            else
            { 
                ViewData["HasApproval"] = "false";
            }


        }

        //public async Task<JsonResult> OnPostAsync()
        //{
        //    var (IsSuccess, Message) = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
        //        .GetTransactionByRecordID(Form);
        //    _resultView.IsSuccess = IsSuccess;
        //    _resultView.Result = Message;

        //    return new JsonResult(_resultView);
        //}

    }
}