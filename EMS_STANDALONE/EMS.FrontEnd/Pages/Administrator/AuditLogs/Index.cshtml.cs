using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.AuditLog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;

namespace EMS.FrontEnd.Pages.Administrator.AuditLogs
{
    public class IndexModel : SharedClasses.Utilities
    {

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        { }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, false);

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

        public async Task<(List<GetListOutput>, bool, string)> GetExportData([FromQuery] GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_securityBaseURL,
                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("AuditLog").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "EventTypeDelimited=", param.EventTypeDelimited, "&",
                  "TableNameDelimited=", param.TableNameDelimited, "&",
                  "Remarks=", param.Remarks, "&",
                  "Name=", param.Name, "&",
                  "IPAddress=", param.IPAddress, "&",
                  "DateCreatedFrom=", param.DateCreatedFrom, "&",
                  "DateCreatedTo=", param.DateCreatedTo);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetEventTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                .GetEventTypeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetTableNameAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                .GetTableNameAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}