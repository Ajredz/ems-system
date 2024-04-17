using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages
{
    public class IndexModel : EMS.FrontEnd.SharedClasses.Security
    {

        public IndexModel(IConfiguration iconfiguration) : base(iconfiguration)
        {
        }

        public void OnGet()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    Response.Redirect("/Login");
            //}
        }

        public async Task<JsonResult> OnGetEmployeeDetails()
        {
            _resultView.Result = _globalCurrentUser;
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}