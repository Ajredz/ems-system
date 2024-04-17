using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.Plantilla.Transfer.Shared;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
using Utilities.API.ReferenceMaintenance;
using EMS.FrontEnd.SharedClasses.Common_Security;
using NPOI.SS.Util;

namespace EMS.FrontEnd.Pages.Plantilla.OrgGroup
{
    public class ListModel : SharedClasses.Utilities
    {
        public ListModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public virtual void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadInsertFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/UPLOADINSERT")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/ORGGROUP/UPLOADEDIT")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.GetListInput param)
        {
            param.AdminAccess = new AdminAccess
            {
                CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                IsAdminAccess = _IsAdminAccess,
                OrgGroupDescendantsDelimited = string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
            };

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "Code=", param.Code, "&",
                  "Description=", param.Description, "&",
                  "OrgTypeDelimited=", param.OrgTypeDelimited, "&",
                  "ParentOrgDescription=", param.ParentOrgDescription, "&",
                  "IsBranchActive=", param.IsBranchActive,"&",
                  "ServiceBayCountMin=", param.ServiceBayCountMin,"&",
                  "ServiceBayCountMax=", param.ServiceBayCountMax, "&",
                  "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                  "AdminAccess.IsAdminAccess=", param.AdminAccess.IsAdminAccess
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.OrgGroup.GetListOutput>(), URL);

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

        public async Task<JsonResult> OnPostDeleteAsync(int ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("Delete").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term)
        {
            var result = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueAutoComplete("ORGGROUPLEVEL", term);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgGroupTagsByOrgGroupType(string RefCode)
        {
            var refCodes = await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);

            List<OrgGroupTagDynamicFieldOutput> result = new List<OrgGroupTagDynamicFieldOutput>();

            foreach (var item in refCodes)
            {
                 List<ReferenceValue> referenceValues = 
                    await new SharedClasses.Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(item.Value);
                result.Add(new OrgGroupTagDynamicFieldOutput
                {
                    RefCode = item.RefCode,
                    Value = item.Value,
                    Description = item.Description,
                    DropDownOptions = referenceValues.Select(x =>
                   new GetDropDownOutput
                   {
                       Value = x.Value,
                       Description = x.Description
                   }).ToList()
                });
            }

            _resultView.IsSuccess = true;
            _resultView.Result = result.Where(x=>!x.Value.Equals("REGION_TAG"));
            return new JsonResult(_resultView);
        }

        private async Task<(List<GetExportListOutput>, bool, string)> GetExportData(GetListInput param)
        {
            param.AdminAccess = new AdminAccess
            {
                CurrentUserOrgGroupID = _globalCurrentUser.OrgGroupID,
                IsAdminAccess = _IsAdminAccess,
                OrgGroupDescendantsDelimited = string.Join(",", _globalCurrentUser.OrgGroupDescendants ?? new List<int>())
            };

            var URL = string.Concat(_plantillaBaseURL,
                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetExportList").Value, "?",
                   "userid=", _globalCurrentUser.UserID, "&",
				   "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                   "ID=", param.ID, "&",
                   "Code=", param.Code, "&",
                   "Description=", param.Description, "&",
                   "OrgTypeDelimited=", param.OrgTypeDelimited, "&",
                   "ParentOrgDescription=", param.ParentOrgDescription, "&",
                   "IsBranchActive=", param.IsBranchActive, "&",
                   "ServiceBayCountMin=", param.ServiceBayCountMin, "&",
                   "ServiceBayCountMax=", param.ServiceBayCountMax, "&",
                   "AdminAccess.OrgGroupDescendantsDelimited=", param.AdminAccess.OrgGroupDescendantsDelimited, "&",
                   "AdminAccess.IsAdminAccess=", param.AdminAccess.IsAdminAccess);

            return await SharedUtilities.GetFromAPI(new List<GetExportListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckOrgGroupExportList([FromQuery] GetListInput param)
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

        public async Task<IActionResult> OnGetDownloadOrgGroupExportList([FromQuery] EMS.Plantilla.Transfer.OrgGroup.GetListInput param)
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

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 100, 10, 10),
                       new string[] { "YES", "NO" }, excelSheet));

                    row.CreateCell(0).SetCellValue("Parent Org Group");
                    row.CreateCell(1).SetCellValue("Org Group Code");
                    row.CreateCell(2).SetCellValue("Org Group Description");
                    row.CreateCell(3).SetCellValue("Org Type");
                    row.CreateCell(3).SetCellValue("Org Type");
                    row.CreateCell(4).SetCellValue("Company Tag");
                    row.CreateCell(5).SetCellValue("Position Code");
                    row.CreateCell(6).SetCellValue("Reporting Position Code");
                    row.CreateCell(7).SetCellValue("Planned");
                    row.CreateCell(8).SetCellValue("Active");
                    row.CreateCell(9).SetCellValue("Inactive");
                    row.CreateCell(10).SetCellValue("Address");
                    row.CreateCell(11).SetCellValue("Is Branch Active?");
                    row.CreateCell(12).SetCellValue("Service Bay Count");
                    row.CreateCell(13).SetCellValue("Is Head");

                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 5000);
                    excelSheet.SetColumnWidth(2, 9000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 5000);
                    excelSheet.SetColumnWidth(6, 8000);
                    excelSheet.SetColumnWidth(7, 3300);
                    excelSheet.SetColumnWidth(8, 3300);
                    excelSheet.SetColumnWidth(9, 3300);
                    excelSheet.SetColumnWidth(10, 12000);
                    excelSheet.SetColumnWidth(11, 6300);
                    excelSheet.SetColumnWidth(12, 7300);
                    excelSheet.SetColumnWidth(13, 7300);

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
                    row.Cells[9].CellStyle = colHeaderStyle;
                    row.Cells[10].CellStyle = colHeaderStyle;
                    row.Cells[11].CellStyle = colHeaderStyle;
                    row.Cells[12].CellStyle = colHeaderStyle;
                    row.Cells[13].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.ParentOrgGroup);
                        row.CreateCell(1).SetCellValue(item.OrgGroupCode);
                        row.CreateCell(2).SetCellValue(item.OrgGroupDesc);
                        row.CreateCell(3).SetCellValue(item.OrgType);
                        row.CreateCell(4).SetCellValue(item.CompanyTag);
                        row.CreateCell(5).SetCellValue(item.PositionCode);
                        row.CreateCell(6).SetCellValue(item.ReportingPositionCode);
                        row.CreateCell(7).SetCellValue((item.PlannedCount ?? 0) > 0 ? item.PlannedCount.Value.ToString() : "");
                        row.CreateCell(8).SetCellValue((item.ActiveCount ?? 0) > 0 ? item.ActiveCount.Value.ToString() : "");
                        row.CreateCell(9).SetCellValue((item.InactiveCount ?? 0) > 0 ? item.InactiveCount.Value.ToString() : "");
                        row.CreateCell(10).SetCellValue(item.Address);
                        row.CreateCell(11).SetCellValue(item.IsBranchActive ? "YES" : "NO");
                        row.CreateCell(12).SetCellValue(item.ServiceBayCount.ToString());
                        row.CreateCell(13).SetCellValue(item.IsHead);

                        row.Cells[7].CellStyle = alignCenter;
                        row.Cells[8].CellStyle = alignCenter;
                        row.Cells[9].CellStyle = alignCenter;
                        row.Cells[11].CellStyle = alignCenter;

                        irow++;
                    }
                    #endregion
                    void OrgGroup()
                    {
                        ISheet excelSheet = workbook.CreateSheet("Org Group Only");
                        int irow = 0;

                        #region Column Headers
                        IRow row = excelSheet.CreateRow(irow); irow++;

                        excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 100, 10, 10),
                           new string[] { "YES", "NO" }, excelSheet));

                        row.CreateCell(0).SetCellValue("Parent Org Group");
                        row.CreateCell(1).SetCellValue("Org Group Code");
                        row.CreateCell(2).SetCellValue("Org Group Description");
                        row.CreateCell(3).SetCellValue("Org Type");
                        row.CreateCell(4).SetCellValue("Is Branch Active?");
                        row.CreateCell(5).SetCellValue("Service Bay Count");
                        //row.CreateCell(8).SetCellValue("Is Head");

                        excelSheet.SetColumnWidth(0, 5000);
                        excelSheet.SetColumnWidth(1, 5000);
                        excelSheet.SetColumnWidth(2, 9000);
                        excelSheet.SetColumnWidth(3, 5000);
                        excelSheet.SetColumnWidth(4, 5000);
                        excelSheet.SetColumnWidth(5, 5000);


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

                        #endregion

                        #region Column Details
                        foreach (var item in Result)
                        {
                            row = excelSheet.CreateRow(Convert.ToInt32(irow));
                            row.CreateCell(0).SetCellValue(item.ParentOrgGroup);
                            row.CreateCell(1).SetCellValue(item.OrgGroupCode);
                            row.CreateCell(2).SetCellValue(item.OrgGroupDesc);
                            row.CreateCell(3).SetCellValue(item.OrgType);
                            row.CreateCell(4).SetCellValue(item.IsBranchActive ? "YES" : "NO");
                            row.CreateCell(5).SetCellValue(item.ServiceBayCount.ToString());
                            //row.CreateCell(8).SetCellValue(item.IsHead);

                            
                            irow++;
                        }
                        #endregion
                       
                    }
                    OrgGroup();
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

        public async Task<JsonResult> OnGetOrgGroupAutoComplete(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionWithLevelByAutoCompleteAsync(EMS.Plantilla.Transfer.Position.GetAutoCompleteInput param)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionWithLevelByAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetUserOrgGroup()
        {
            var Id = (_globalCurrentUser.OrgGroupID == 0 ? 1 : _globalCurrentUser.OrgGroupID);
            _resultView.Result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroup(Id);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetChildrenOrgDropDown(GetByIDInput param)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetChildrenOrgDropDown(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}