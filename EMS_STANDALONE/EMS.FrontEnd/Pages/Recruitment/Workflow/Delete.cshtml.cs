using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Recruitment.Workflow
{
    public class DeleteModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                   _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Workflow").GetSection("Delete").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}