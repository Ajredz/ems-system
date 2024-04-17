using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.Plantilla.PositionLevel
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.PositionLevel.Form PositionLevel { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync(int ID)
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITIONLEVEL/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/POSITIONLEVEL/EDIT")).Count() > 0 ? "true" : "false"; 
            }

            PositionLevel = await new Common_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevel(ID);

        }

    }
}