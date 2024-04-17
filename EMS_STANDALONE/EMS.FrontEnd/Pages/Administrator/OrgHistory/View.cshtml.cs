using EMS.FrontEnd.SharedClasses;
using EMS.Plantilla.Transfer.OrgGroup;
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
    public class ViewModel : SharedClasses.Utilities
    {
        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }
        public virtual async Task OnGetAsync([FromQuery] string TDate, [FromQuery] string IsLatest)
        {
            ViewData["TDate"] = TDate;
            ViewData["IsLatest"] = IsLatest;
            ViewData["Mode"] = "View";
        }

        public async Task<IActionResult> OnGetList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.OrgGroupHistoryByDateInput param)
        {
            var AllowEdit = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_SystemAccess.ORG_REGROUP_INPUT.ToString())).Select(x => x.Value).FirstOrDefault();
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetOrgGroupHistoryByDate").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "tdate=", param.TDate, "&",
                  "islatest=", param.IsLatest
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.OrgGroup.GetOrgGroupHistoryByDateOutput>(), URL);

            if (IsSuccess)
            {
                Result = Result.Where(x => AllowEdit.Contains(x.OrgType)).ToList();
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    param.pageNumber,
                    sort = 1,
                    records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
                    rows = Result.Select(x=> new GetOrgGroupHistoryByDateOutput()
                    {
                        ID = x.ID,
                        TDate = x.TDate,
                        IsLatest = x.IsLatest,
                        IsLatestDescription = x.IsLatestDescription,
                        OrgType = x.OrgType,
                        Code = x.Code,
                        Description = x.Description,
                        ParentOrgId = x.ParentOrgId,
                        ParentDescription = x.ParentDescription,
                        CodeDescription = x.Code + " - " + x.Description,
                        ParentCodeDescription = x.ParentDescription,
                        ParentDescriptionDisplay = x.ParentDescription
                    })
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }
    }
}
