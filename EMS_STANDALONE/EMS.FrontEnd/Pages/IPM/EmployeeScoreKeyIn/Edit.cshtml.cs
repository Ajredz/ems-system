using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.Text;
using EMS.IPM.Transfer.EmployeeScore;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScoreKeyIn
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.Form EmployeeScore { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(GetByIDInput param)
        {

            List<SelectListItem> roles = new List<SelectListItem>();

            if (_globalCurrentUser != null)
            {
                if (param.ID > 0)
                {
                    roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

                    param.RoleIDs = string.Join(",", roles.Select(x => x.Value).ToArray());

                    EmployeeScore =
                                await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployeeScore(param);
                }
            }

           
            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
                {
                    WorkflowCode = "IPM",
                    CurrentStepCode = EmployeeScore.Status,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).ToList();

            ViewData["StatusSelectList"] =
            status.OrderBy(x => x.Order).Select(x => new SelectListItem
            {
                Value = x.Code,
                Text = x.Description,
                Selected = x.Code.Equals(EmployeeScore.Status)
            }).ToList();

            var description = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                    {
                        WorkflowCode = "IPM",
                        Code = EmployeeScore.Status
                    });

            ViewData["CurrentStatusDescription"] = description.Description;

            var maxValue = await new Common_Reference(_iconfiguration, _globalCurrentUser, _ipmBaseURL, _env)
                .GetRefValueByRefCode("KPI_SCORE_MAX_VALUE");

            EmployeeScore.MaxValue = maxValue.Value;
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var URL = string.Concat(_ipmBaseURL,
                   _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScoreKeyInApproval").GetSection("Edit").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

                var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

                var oldValue = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                    .GetEmployeeScore(new GetByIDInput
                    {
                        ID = EmployeeScore.TID, 
                        RoleIDs = string.Join(",", roles.Select(x => x.Value).ToArray())
                    });

                var approverRoles = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
               .GetRolesByWorkflowStepCode(new GetRolesByWorkflowStepCodeInput
               {
                   WorkflowCode = "IPM",
                   StepCode = EmployeeScore.Status // CurrentStep
               })).ToList();
                EmployeeScore.NextApproverRoleIDs = string.Join(",", approverRoles.Select(x => x.RoleID).Distinct().ToArray());

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(EmployeeScore, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {
                    StringBuilder Remarks = new StringBuilder();

                    if (EmployeeScore.TotalScore != oldValue.TotalScore)
                    {
                        Remarks.Append(string.Concat("Total Score of ", EmployeeScore.Employee, " changed from ", oldValue.TotalScore, " to ", EmployeeScore.TotalScore, ". "));
                    }

                    if (EmployeeScore.Status != oldValue.Status)
                    {
                        Remarks.Append(string.Concat("Status of Employee Score of ", EmployeeScore.Employee, " changed from ", oldValue.Status, " to ", EmployeeScore.Status, ". "));
                    }

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "Employee Score",
                                TableID = EmployeeScore.TID,
                                Remarks = Remarks.ToString(),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            }); ;
                    }

                    var workflowURL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("AddEmployeeScoreStatusHistory").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess1, Message1) = await SharedUtilities.PostFromAPI(EmployeeScore, workflowURL);
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = string.Concat(MessageUtilities.PRE_ERRMSG_REC_SAVE,
                string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()));
            }

            return new JsonResult(_resultView);
        }

    }
}