using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Transfer.Dashboard;

namespace EMS.FrontEnd.Pages.LogActivity.Dashboard
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {
        }

        public void OnGet()
        {

        }

        public async Task<JsonResult> OnGetAccountabilityDashboard(GetAccountabilityDashboardInput param)
        {
            List<int> GetOrg = new List<int>();
            var GetClearingDept = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupHierarchy(_globalCurrentUser.OrgGroupID)).Where(x => x.OrgType.Equals("DEPT") || x.OrgType.Equals("SECTION")).ToList();
            GetOrg.AddRange(GetClearingDept.Select(x=>x.ID).ToList());
            GetOrg.Add(_globalCurrentUser.OrgGroupID);
            if (_globalCurrentUser.OrgGroupDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
            if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
            param.AccessOrg = string.Join(",",GetOrg.Distinct().ToList());

            var (Result, IsSuccess, Message) = (await new Common_Accountability(_iconfiguration, _globalCurrentUser, _env)
                .GetAccountabilityDashboard(param));

            var jsonData = new
            {
                Result = Result
            };

            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetReferenceValuePlantilla(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}
