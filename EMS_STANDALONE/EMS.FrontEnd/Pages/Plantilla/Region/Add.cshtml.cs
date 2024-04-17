using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Region
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Region.Form Region { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            Region = new EMS.Plantilla.Transfer.Region.Form();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Region.CreatedBy = _globalCurrentUser.UserID;
            var URL = string.Concat(_plantillaBaseURL,
                    _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Region").GetSection("Add").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Region, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }
    }
}