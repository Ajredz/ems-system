using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Recruitment.Transfer.Applicant;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.ApplicationApproval
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Recruitment.Transfer.Applicant.GetListInput param)
        {
            List<int> roles =
                (await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleByUserID()).Select(x => x.RoleID).ToList();

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("ApprovalList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "RoleDelimited=", string.Join(",", roles), "&",
                  "ID=", param.ID, "&",
                  //"ApplicantName=", param.ApplicantName, "&",
                  "ApplicationSourceDelimited=", param.ApplicationSourceDelimited, "&",
                  //"CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  //"WorkflowDelimited=", param.WorkflowDelimited, "&",
                  //"OrgGroupRemarks=", param.OrgGroupRemarks, "&",
                  //"OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionRemarks=", param.PositionRemarks, "&",
                  //"PositionDelimited=", param.PositionDelimited, "&",
                  "Course=", param.Course, "&",
                  "CurrentPositionTitle=", param.CurrentPositionTitle, "&",
                  "ExpectedSalaryFrom=", param.ExpectedSalaryFrom, "&",
                  "ExpectedSalaryTo=", param.ExpectedSalaryTo, "&",
                  "DateAppliedFrom=", param.DateAppliedFrom, "&",
                  "DateAppliedTo=", param.DateAppliedTo, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApprovalListOutput>(), URL);

            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    param.pageNumber,
                    sort = param.sidx,
                    records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
                    rows = Result
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetCurrentStepAutoCompleteAsync(GetWorkflowStepAutoCompleteInput param)
        {
            _resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflowStepAutoComplete(param);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        
        public async Task<JsonResult> OnGetReferredBy(string Term, int TopResults)
        {
            var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}