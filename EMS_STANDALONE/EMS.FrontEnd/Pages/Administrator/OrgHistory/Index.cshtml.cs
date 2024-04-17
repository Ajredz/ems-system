using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.OrgHistory
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            //_IsAdminAccess = IsAdminAccess;
        }

        public virtual async Task OnGetAsync()
        {
            ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/ADMINISTRATOR/ORGHISTORY/ADD")).Count() > 0 ? "true" : "false";
        }

        public async Task<IActionResult> OnGetList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.OrgGroupHistoryListInput param) {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupHistoryList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "tdatefrom=", param.TDateFrom, "&",
                  "tdateto=", param.TDateTo, "&",
                  "islatest=", param.IsLatest
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.OrgGroup.GetOrgGroupHistoryListOutput>(), URL);
            
            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    param.pageNumber,
                    sort = 1,
                    records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
                    rows = Result
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<JsonResult> OnGetOrgGroupAutoComplete(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}
