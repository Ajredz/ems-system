using EMS.Manpower.Transfer.ApproverSetup;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.Manpower.ApproverSetup
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                
            }
        }

        public async Task<IActionResult> OnGetList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, false);

            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    param.pageNumber,
                    sort = param.sidx,
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

        private async Task<(List<GetListOutput>, bool, string)> GetListData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_manpowerBaseURL,
                  _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("ApproverSetup").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", IsExport, "&",

                  "OrgGroup=", param.OrgGroup, "&",
                  "HasApprover=", param.HasApprover, "&",
                  "ModifiedDateFrom=", param.ModifiedDateFrom, "&",
                  "ModifiedDateTo=", param.ModifiedDateTo
                  );

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }


        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

    }
}