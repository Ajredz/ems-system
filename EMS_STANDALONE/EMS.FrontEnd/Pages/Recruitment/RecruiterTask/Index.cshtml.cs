using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Recruitment.Transfer.RecruiterTask;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.RecruiterTask
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/RECRUITMENT/RECRUITERTASK/ADD")).Count() > 0 ? "true" : "false";
            }

        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("RecruiterTask").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Recruiter=", param.Recruiter, "&",
                  "Applicant=", param.Applicant, "&",
                  "Description=", param.Description, "&",
                  "StatusDelimited=", param.StatusDelimited, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo, "&",
                  "DateModifiedFrom=", param.DateModifiedFrom, "&",
                  "DateModifiedTo=", param.DateModifiedTo);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);

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

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _recruitmentBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRecruiterAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApplicantAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicantAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRecruiterByID(int ID)
        {
            _resultView.Result = await new Common_Synced_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserBySyncID(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetApplicantByID(int ID)
        {
            _resultView.Result = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicantNameByID(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}