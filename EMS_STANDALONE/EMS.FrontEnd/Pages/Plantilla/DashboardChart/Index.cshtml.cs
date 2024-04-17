using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses;
using EMS.Plantilla.Transfer.Dashboard;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using System.Collections.Generic;

namespace EMS.FrontEnd.Pages.Plantilla.DashboardChart
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {
        }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasTableFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/DASHBOARD/TABLE")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<JsonResult> OnGetPlantillaDashboard(string DashboardData, string OrgGroupID, string PositionID, string EmploymentStatus)
        {
            PlantillaDashboardInput input = new PlantillaDashboardInput();

            input.DashboardData = DashboardData;

            if (string.IsNullOrEmpty(OrgGroupID))
            {
                if (_systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/DASHBOARD/ADMIN")).Count() == 0)
                {
                    List<int> GetOrg = new List<int>();
                    if (_globalCurrentUser.OrgGroupDescendants != null)
                        GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
                    if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                        GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
                    if (GetOrg.Count() > 0)
                        input.OrgGroupID = GetOrg;
                }
            }
            else
                input.OrgGroupID = (OrgGroupID.Split(",")).Select(x=>int.Parse(x)).ToList();

            if(!string.IsNullOrEmpty(PositionID))
                input.PositionID = (PositionID.Split(",")).Select(x => int.Parse(x)).ToList();

            if (!string.IsNullOrEmpty(EmploymentStatus))
                input.EmploymentStatus = EmploymentStatus;

            var (Result, IsSuccess, Message) = (await new Common_Dashboard(_iconfiguration,_globalCurrentUser,_env)
                .GetPlantillaDashboard(input));

            var jsonData = new
            {
                Result = Result,
                Total = Result.Sum(x=>x.Count)
            };
            return new JsonResult(jsonData);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            List<int> GetOrg = new List<int>();
            GetOrg.Add(_globalCurrentUser.OrgGroupID);
            if (_globalCurrentUser.OrgGroupDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
            if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);

            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;

            _resultView.Result = result;
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/DASHBOARD/ADMIN")).Count() == 0)
            {
                _resultView.Result = result.Where(x => GetOrg.Contains(x.ID)).ToList();
            }

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
