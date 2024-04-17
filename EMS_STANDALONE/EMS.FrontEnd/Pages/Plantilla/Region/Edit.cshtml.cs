using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Region
{
    public class EditModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Region.Form Region { get; set; }

        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/REGION/DELETE")).Count() > 0 ? "true" : "false";
            }

            Region = await new Common_Region(_iconfiguration, _globalCurrentUser, _env).GetRegion(ID);
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Region.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Region").GetSection("Edit").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Region, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
           
            return new JsonResult(_resultView);
        }
    }
}