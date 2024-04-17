using EMS.FrontEnd.Pages.Shared.Reference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Plantilla.Reference
{
    public class IndexModel : _ReferenceMaintenance
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, new string[] { EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString() }, _plantillaBaseURL, env)
        { }
    }
}