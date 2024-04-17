using EMS.FrontEnd.Pages.Shared.Reference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Recruitment.Reference
{
    public class IndexModel : _ReferenceMaintenance
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, 
            new string[] { ReferenceCodes_Recruitment.NATURE_OF_EMPLOYMENT.ToString(), ReferenceCodes_Recruitment.APPLICATION_SOURCE.ToString(),
        ReferenceCodes_Recruitment.GEOGRAPHICAL_REGION.ToString(), ReferenceCodes_Recruitment.ATTACHMENT_TYPE.ToString() }, _recruitmentBaseURL, env)
        { }
    }
}