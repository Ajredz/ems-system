using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class CancelModel : SharedClasses.Utilities
    {
        public CancelModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task<JsonResult> OnPostAsync(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("Cancel").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}