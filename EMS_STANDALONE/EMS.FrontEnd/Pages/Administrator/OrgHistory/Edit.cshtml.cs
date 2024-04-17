using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Administrator.OrgHistory
{
    public class EditModel : SharedClasses.Utilities
    {
        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }
        public virtual async Task OnGetAsync([FromQuery] string TDate, [FromQuery] string IsLatest)
        {
            ViewData["TDate"] = TDate;
            ViewData["IsLatest"] = IsLatest;
            ViewData["Mode"] = "Edit";
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
                    tdate = Result.Select(x => x.TDate).FirstOrDefault(),
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
                        ParentDescriptionDisplay= x.ParentDescription
                    })
                };

                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }


        public async Task<JsonResult> OnPostAsync([FromQuery] string TDate, [FromQuery] bool IsLatest, [FromBody] List<AddOrgGroupHistoryInput> param)
        {
            //param.RemoveAll(x=>x==null);
            var URL = string.Concat(_plantillaBaseURL,
            _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("AddOrgGroupHistory").Value, "?",
                               "userid=", _globalCurrentUser.UserID,"&",
                               "TDate=", TDate, "&",
                               "IsLatest=", IsLatest);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(param, URL);
            _resultView.IsSuccess = IsSuccess;
            //_resultView.Message = Message;
            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EDIT.ToString(),
                        TableName = "OrgGroupHistory",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(_globalCurrentUser.Username, " Edit Branch Regroup (Date: ", TDate, ")"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(Message);
        }
    }
}
