using EMS.FrontEnd.SharedClasses.Common_Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.SystemUser
{
    public class DeleteModel : SharedClasses.Utilities
    {
        public DeleteModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            var URL = string.Concat(_securityBaseURL,
                   _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("Delete").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

    }
}