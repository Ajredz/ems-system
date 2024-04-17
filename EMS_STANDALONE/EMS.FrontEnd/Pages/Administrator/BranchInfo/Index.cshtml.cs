using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Security.Transfer.SystemUser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.Plantilla.Transfer.Shared;
using EMS.Plantilla.Transfer.OrgGroup;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Drawing;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;

namespace EMS.FrontEnd.Pages.Administrator.BranchInfo
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.GetBranchInfoListInput param)
        {
            param.AdminAccess = new AdminAccess
            {
                CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                IsAdminAccess = _IsAdminAccess,
                OrgGroupDescendantsDelimited = string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
            };

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("ListBranchInfo").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "Description=", param.Description, "&",
                  "Category=", param.Category, "&",
                  "Email=", param.Email, "&",
                  "Number=", param.Number, "&",
                  "Address=", param.Address, "&",
                  "OrgTypeDelimited=", param.OrgTypeDelimited, "&",
                  "ParentOrgDescription=", param.ParentOrgDescription, "&",
                  "IsBranchActive=", param.IsBranchActive, "&",
                  "ServiceBayCountMin=", param.ServiceBayCountMin, "&",
                  "ServiceBayCountMax=", param.ServiceBayCountMax, "&",
                  "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                  "AdminAccess.IsAdminAccess=", true
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.OrgGroup.GetBranchListListOutput>(), URL);

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


        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        private async Task<(List<GetBranchListListOutput>, bool, string)> GetExportData(GetBranchInfoListInput param)
        {
            param.AdminAccess = new AdminAccess
            {
                CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                IsAdminAccess = _IsAdminAccess,
                OrgGroupDescendantsDelimited = string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
            };

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("ListBranchInfo").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "Description=", param.Description, "&",
                  "Category=", param.Category, "&",
                  "Email=", param.Email, "&",
                  "Number=", param.Number, "&",
                  "Address=", param.Address, "&",
                  "OrgTypeDelimited=", param.OrgTypeDelimited, "&",
                  "ParentOrgDescription=", param.ParentOrgDescription, "&",
                  "IsBranchActive=", param.IsBranchActive, "&",
                  "ServiceBayCountMin=", param.ServiceBayCountMin, "&",
                  "ServiceBayCountMax=", param.ServiceBayCountMax, "&",
                  "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                  "AdminAccess.IsAdminAccess=", true
                  );

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.OrgGroup.GetBranchListListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckOrgGroupExportList([FromQuery] GetBranchInfoListInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param);

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
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        public async Task<IActionResult> OnGetDownloadOrgGroupExportList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.GetBranchInfoListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetExportData(param);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "OrganizationalGroup.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Org Group with Position");
                    int irow = 0;

                    IDataValidation getDropdownValidation(CellRangeAddressList addressList, string[] stringArray, ISheet excelSheet)
                    {
                        IDataValidationHelper validationHelper = new XSSFDataValidationHelper((XSSFSheet)excelSheet);
                        IDataValidationConstraint constraint = validationHelper.CreateExplicitListConstraint(stringArray);
                        IDataValidation dataValidation = validationHelper.CreateValidation(constraint, addressList);
                        dataValidation.SuppressDropDownArrow = true;
                        dataValidation.ShowErrorBox = true;
                        dataValidation.CreateErrorBox("", "Please select from Dropdown values.");
                        return dataValidation;
                    }

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Branch Code");
                    row.CreateCell(1).SetCellValue("Branch Description");
                    row.CreateCell(2).SetCellValue("Branch Category");
                    row.CreateCell(3).SetCellValue("Branch Email");
                    row.CreateCell(4).SetCellValue("Branch Number");
                    row.CreateCell(5).SetCellValue("Address");
                    row.CreateCell(6).SetCellValue("Parent Organization");
                    row.CreateCell(7).SetCellValue("Is Branch Active?");
                    row.CreateCell(8).SetCellValue("Service Bay Count");

                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 5000);
                    excelSheet.SetColumnWidth(2, 5000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 5000);
                    excelSheet.SetColumnWidth(7, 5000);
                    excelSheet.SetColumnWidth(8, 5000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;

                    row.Cells[0].CellStyle = colHeaderStyle;
                    row.Cells[1].CellStyle = colHeaderStyle;
                    row.Cells[2].CellStyle = colHeaderStyle;
                    row.Cells[3].CellStyle = colHeaderStyle;
                    row.Cells[4].CellStyle = colHeaderStyle;
                    row.Cells[5].CellStyle = colHeaderStyle;
                    row.Cells[6].CellStyle = colHeaderStyle;
                    row.Cells[7].CellStyle = colHeaderStyle;
                    row.Cells[8].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.Code);
                        row.CreateCell(1).SetCellValue(item.Description);
                        row.CreateCell(2).SetCellValue(item.Category);
                        row.CreateCell(3).SetCellValue(item.Email);
                        row.CreateCell(4).SetCellValue(item.Number);
                        row.CreateCell(5).SetCellValue(item.Address);
                        row.CreateCell(6).SetCellValue(item.ParentOrgDescription);
                        row.CreateCell(7).SetCellValue(item.IsBranchActive);
                        row.CreateCell(8).SetCellValue(item.ServiceBayCount.ToString());

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
                        TableName = "OrgGroup",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Organizational Group exported"),
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

        public async Task<JsonResult> OnGetOrgGroupHierarchy(int ID)
        {
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupHierarchy(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetPSGCAutoComplete(string term, int TopResults)
        {
            var result = await new SharedClasses.Common_Plantilla.Common_PSGCPantillla(_iconfiguration, _globalCurrentUser, _env).GetPSGCAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
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