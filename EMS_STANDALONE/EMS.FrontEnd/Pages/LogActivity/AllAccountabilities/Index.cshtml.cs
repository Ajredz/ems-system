using EMS.Plantilla.Transfer.Position;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.LogActivity;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.Workflow.Transfer.Accountability;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using NPOI.SS.Util;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using System;

namespace EMS.FrontEnd.Pages.LogActivity.AllAccountabilities
{
    public class IndexModel : LogActivity.MyAccountabilities.IndexModel
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = true) 
            : base(iconfiguration, env, IsAdminAccess)
        { }

        public async Task<IActionResult> OnGetDownloadAccountabilityTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Accountability.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                alignCenter.Alignment = HorizontalAlignment.Center;

                // Dropdown values
                var typeList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Workflow.Transfer.Enums.ReferenceCodes.ACCOUNTABILITY_TYPE.ToString()))
                    .OrderBy(y => y.Value).Select(x => x.Value).ToArray();


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


                void AddETFSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Accountability");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 50, 1, 1), typeList, excelSheet));

                    row.CreateCell(0).SetCellValue("* Old Employee ID");
                    row.CreateCell(1).SetCellValue("* Type");
                    row.CreateCell(2).SetCellValue("* Title");
                    row.CreateCell(3).SetCellValue("Description");
                    row.CreateCell(4).SetCellValue("Remarks");
                    row.CreateCell(5).SetCellValue("* Org Group Code");

                    excelSheet.SetColumnWidth(0, 6500);
                    excelSheet.SetColumnWidth(1, 4500);
                    excelSheet.SetColumnWidth(2, 4500);
                    excelSheet.SetColumnWidth(3, 6500);
                    excelSheet.SetColumnWidth(4, 6500);
                    excelSheet.SetColumnWidth(5, 6500);

                    for (int i = 0; i <= 5; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 5; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 5; i++)
                            rowDate.Cells[i].CellStyle = textCS;
                    }

                }

                AddETFSheet();

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostUploadInsert()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\Accountability\\Uploads");
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(filePath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    List<AccountabilityUploadFile> uploadList = new List<AccountabilityUploadFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                AccountabilityUploadFile obj = new AccountabilityUploadFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    OldEmployeeID = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Type = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Title = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Description = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Remarks = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OrgGroupCode = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if (blankRows > 3)
                                break;
                        }

                    }

                    var OldEmployeeCodesDelimited = string.Join(",", uploadList.Select(x => x.OldEmployeeID).Distinct());
                    var EmployeeList = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                        .GetByOldEmployeeIDs(OldEmployeeCodesDelimited);

                    var OrgGroupDelimited = string.Join(",", uploadList.Select(x => x.OrgGroupCode).Distinct());
                    var OrgGroupList = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                        .GetByCodes(OrgGroupDelimited);

                    if (EmployeeList.Count > 0)
                    {
                        uploadList = uploadList.GroupJoin(EmployeeList,
                        x => new { x.OldEmployeeID },
                        y => new { y.OldEmployeeID },
                        (x, y) => (x, y))
                        .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x, y })
                        .Select(x => new AccountabilityUploadFile
                        {
                            RowNum = x.x.x.RowNum,
                            OldEmployeeID = x.x.x.OldEmployeeID,
                            Type = x.x.x.Type,
                            Title = x.x.x.Title,
                            Description = x.x.x.Description,
                            Remarks = x.x.x.Remarks,
                            OrgGroupCode = x.x.x.OrgGroupCode,
                            EmployeeID = x.y == null ? 0 : x.y.ID
                        }).ToList();
                    }

                    if (OrgGroupList.Count > 0)
                    {
                        uploadList = uploadList.GroupJoin(OrgGroupList,
                        x => new { x.OrgGroupCode },
                        y => new { OrgGroupCode = y.Code },
                        (x, y) => (x, y))
                        .SelectMany(x => x.y.DefaultIfEmpty(),
                        (x, y) => new { x, y })
                        .Select(x => new AccountabilityUploadFile
                        {
                            RowNum = x.x.x.RowNum,
                            OldEmployeeID = x.x.x.OldEmployeeID,
                            Type = x.x.x.Type,
                            Title = x.x.x.Title,
                            Description = x.x.x.Description,
                            Remarks = x.x.x.Remarks,
                            OrgGroupCode = x.x.x.OrgGroupCode,
                            EmployeeID = x.x.x.EmployeeID,
                            OrgGroupID = x.y == null ? 0 : x.y.ID
                        }).ToList();
                    }


                    var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadList, URL);


                    if (IsSuccess)
                    {

                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "EmployeeAccountability",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "Employee Accountability uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });
                    }


                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

            return new JsonResult(_resultView);
        }

    }
}