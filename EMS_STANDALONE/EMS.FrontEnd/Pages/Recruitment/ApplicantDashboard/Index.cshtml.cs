using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Recruitment.Transfer.ApplicantDashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.ApplicantDashboard
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetApplicantCountByOrgGroup()
        {

            var URL = string.Concat(_recruitmentBaseURL,
                           _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("ApplicantDashboard").GetSection("List").Value, "?",
                            "UserID=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApplicantOrgGroupOutput>(), URL);

            if (IsSuccess)
            {
                _resultView.IsSuccess = true;
                _resultView.Result = Result;

                return new JsonResult(_resultView);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }
    }
}