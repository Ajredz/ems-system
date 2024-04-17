using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.OrgGroup;
using EMS.Plantilla.Transfer.Position;
using EMS.Plantilla.Transfer.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Count
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task OnGet()
        {
            var orgTypes = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                      EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGGROUPTYPE.ToString());

            ViewData["OrgTypeSelectList"] = orgTypes.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            ViewData["OrgListFilter"] = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.ORGLIST_FILTER.ToString())).FirstOrDefault().Value;
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetPlantillaCountInput param)
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

        private async Task<(List<GetPlantillaCountOutput>, bool, string)> GetExportData(GetPlantillaCountInput param, bool IsExport)
        {
            param.AdminAccess = new AdminAccess
            {
                CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                IsAdminAccess = _IsAdminAccess,
                OrgGroupDescendantsDelimited =
                _globalCurrentUser.OrgGroupRovingDescendants?.Count > 0 ?
                 string.Join(",", _globalCurrentUser.OrgGroupDescendants
                    .Union(_globalCurrentUser.OrgGroupRovingDescendants))
                : string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
            };

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetExportCountByOrgTypeData").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "OrgType=", param.OrgType /*THIS IS REQUIRED*/, "&",
                  "OrgGroupID=", param.OrgGroupID, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "ScopeOrgType=", param.ScopeOrgType, "&",
                  "ScopeOrgGroupDelimited=", param.ScopeOrgGroupDelimited, "&",
                  "PositionDelimited=", param.PositionDelimited, "&",
                  "PlannedMin=", param.PlannedMin, "&",
                  "PlannedMax=", param.PlannedMax, "&",
                  "ActiveMin=", param.ActiveMin, "&",
                  "ActiveMax=", param.ActiveMax, "&",
                  "InactiveMin=", param.InactiveMin, "&",
                  "InactiveMax=", param.InactiveMax, "&",
                  "VarianceMin=", param.VarianceMin, "&",
                  "VarianceMax=", param.VarianceMax, "&",
                  "IsExport=", IsExport /*IsExport*/, "&",
                  "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                  "AdminAccess.IsAdminAccess=", param.AdminAccess.IsAdminAccess
                  );

            return await SharedUtilities.GetFromAPI(new List<GetPlantillaCountOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportCountByOrgType([FromQuery] GetPlantillaCountInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            if (IsSuccess)
            {
                if (Result.Count > 0)
                {
                    _resultView.IsSuccess = true;
                }
                else
                {
                    _resultView.IsSuccess = false;
                    _resultView.Result = MessageUtilities.ERRMSG_NO_RECORDS;
                }
                

                return new JsonResult(_resultView);
            }
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<IActionResult> OnGetDownloadExportCountByOrgType([FromQuery] GetPlantillaCountInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = string.Concat("PlantillaCount_", param.OrgType, ".xlsx");
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Manpower Count by Org Type");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Filter By Region");
                    row.CreateCell(1).SetCellValue("Org Group");
                    row.CreateCell(2).SetCellValue("Position");
                    row.CreateCell(3).SetCellValue("Inactive ("+Result.Select(x=>x.TotalInactive).FirstOrDefault()+")");
                    row.CreateCell(4).SetCellValue("Planned (" + Result.Select(x => x.TotalPlanned).FirstOrDefault() + ")");
                    row.CreateCell(5).SetCellValue("Active-REG (" + Result.Select(x => x.TotalActiveReg).FirstOrDefault() + ")");
                    row.CreateCell(6).SetCellValue("Active-PROB (" + Result.Select(x => x.TotalActiveProb).FirstOrDefault() + ")");
                    row.CreateCell(7).SetCellValue("Active-OUT (" + Result.Select(x => x.TotalOutgoing).FirstOrDefault() + ")");
                    row.CreateCell(8).SetCellValue("Total Active (" + Result.Select(x => x.TotalActive).FirstOrDefault() + ")");
                    row.CreateCell(9).SetCellValue("Variance (" + Result.Select(x => x.TotalVariance).FirstOrDefault() + ")");

                    excelSheet.SetColumnWidth(0, 7300);
                    excelSheet.SetColumnWidth(1, 7300);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 3300);
                    excelSheet.SetColumnWidth(4, 3300);
                    excelSheet.SetColumnWidth(5, 3300);
                    excelSheet.SetColumnWidth(6, 3300);
                    excelSheet.SetColumnWidth(7, 3300);
                    excelSheet.SetColumnWidth(8, 3300);
                    excelSheet.SetColumnWidth(9, 3300);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignRight = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle negativeVariace = (XSSFCellStyle)workbook.CreateCellStyle();
                    var colHeaderFont = workbook.CreateFont();
                    var negativeVariaceFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    alignRight.Alignment = HorizontalAlignment.Right;
                    negativeVariaceFont.Color = HSSFColor.Red.Index;
                    negativeVariace.SetFont(negativeVariaceFont);
                    negativeVariace.Alignment = HorizontalAlignment.Right;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    row.Cells[9].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.ScopeOrgGroup);
                        row.CreateCell(1).SetCellValue(item.OrgGroup);
                        row.CreateCell(2).SetCellValue(item.Position);
                        row.CreateCell(3).SetCellValue(item.InactiveCount > 0 ? item.InactiveCount : 0);
                        row.CreateCell(4).SetCellValue(item.PlannedCount > 0 ? item.PlannedCount : 0);
                        row.CreateCell(5).SetCellValue(item.ActiveCount > 0 ? item.ActiveCount : 0);
                        row.CreateCell(6).SetCellValue(item.ActiveProbCount > 0 ? item.ActiveProbCount : 0);
                        row.CreateCell(7).SetCellValue(item.OutgoingCount > 0 ? item.OutgoingCount : 0);
                        row.CreateCell(8).SetCellValue(item.TotalActiveCount > 0 ? item.TotalActiveCount : 0);
                        row.CreateCell(9).SetCellValue(item.ActiveCount > 0 | item.InactiveCount > 0 | item.PlannedCount > 0 ? ((item.ActiveCount + item.ActiveProbCount) - item.PlannedCount) : 0);

                        row.Cells[3].CellStyle = alignRight;
                        row.Cells[4].CellStyle = alignRight;
                        row.Cells[5].CellStyle = alignRight;
                        row.Cells[6].CellStyle = alignRight;
                        row.Cells[7].CellStyle = alignRight;
                        row.Cells[8].CellStyle = alignRight;
                        row.Cells[9].CellStyle = ((item.ActiveCount + item.ActiveProbCount) - item.PlannedCount) != 0 ? negativeVariace : alignRight;

                        irow++;
                    }
                    #endregion

                    workbook.Write(fs);
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.EXPORT.ToString(),
                        TableName = "PlantillaCount",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Plantilla Count exported"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<JsonResult> OnGetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByOrgTypeAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoComplete(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_Position(_iconfiguration, _globalCurrentUser, _env)
                .GetPositionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }


    }
}