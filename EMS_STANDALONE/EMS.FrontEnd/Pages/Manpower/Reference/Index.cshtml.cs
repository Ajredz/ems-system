using EMS.FrontEnd.Pages.Shared.Reference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Manpower.Reference
{
    public class IndexModel : _ReferenceMaintenance
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, 
            new string[] { ReferenceCodes_Manpower.NATURE_OF_EMPLOYMENT.ToString(), ReferenceCodes_Manpower.MRF_PURPOSE.ToString() }, _manpowerBaseURL, env)
        { }
    }
}