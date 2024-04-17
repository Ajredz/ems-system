using EMS.FrontEnd.Pages.Shared.Reference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EMS.FrontEnd.Pages.Workflow.Reference
{
    public class IndexModel : _ReferenceMaintenance
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, 
            new string[] { }, _workflowBaseURL, env)
        { }
    }
}