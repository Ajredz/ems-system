using EMS.Plantilla.Transfer.EmployeeMovement;
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
using EMS.FrontEnd.SharedClasses;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using EMS.IPM.Data.DataDuplication.EmployeeMovement;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.Workflow.Transfer.Accountability;

namespace EMS.FrontEnd.Pages.Plantilla.MovementApproval
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public Utilities.API.ChangeStatus ChangeStatus { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {
        }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/MOVEMENTAPPROVAL/CHANGESTATUS")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Plantilla.Transfer.EmployeeMovement.GetListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(6)).WorkflowStepList;

            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);

            Result = (from left in Result
                     join right in StatusColor on left.Status equals right.StepCode
                     select new GetListOutput
                     {
                         TotalNoOfRecord = left.TotalNoOfRecord,
                         NoOfPages = left.NoOfPages,
                         Row = left.Row,

                         ID = left.ID,
                         EmployeeName = left.EmployeeName,
                         OrgGroup = left.OrgGroup,
                         EmployeeField = left.EmployeeField,
                         MovementType = left.MovementType,
                         Status = left.Status,
                         StatusDescription = right.StepDescription,
                         StatusColor = right.StatusColor,
                         From = left.From,
                         To = left.To,
                         DateEffectiveFrom = left.DateEffectiveFrom,
                         DateEffectiveTo = left.DateEffectiveTo,
                         OldEmployeeID = left.OldEmployeeID,
                         CreatedDate = left.CreatedDate,
                         CreatedByName = left.CreatedByName,
                         Reason = left.Reason,
                         HRDComments = left.HRDComments
                     }).ToList();

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

        private async Task<(List<EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput>, bool, string)> GetDataList([FromQuery] EMS.Plantilla.Transfer.EmployeeMovement.GetListInput param, bool IsExport)
        {
            bool HasConfidentialView = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0;
            
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "EmployeeName=", param.EmployeeName, "&",
                  "OldEmployeeID=", param.OldEmployeeID, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "EmployeeFieldDelimited=", param.EmployeeFieldDelimited, "&",
                  "MovementTypeDelimited=", param.MovementTypeDelimited, "&",
                  "StatusDelimited=", (param.StatusDelimited == null ? "PENDING": param.StatusDelimited), "&",
                  "From=", param.From, "&",
                  "To=", param.To, "&",
                  "DateEffectiveFromFrom=", param.DateEffectiveFromFrom, "&",
                  "DateEffectiveFromTo=", param.DateEffectiveFromTo, "&",
                  "DateEffectiveToTo=", param.DateEffectiveToTo, "&",
                  "DateEffectiveToFrom=", param.DateEffectiveToFrom, "&",
                  "CreatedDateFrom=", param.CreatedDateFrom, "&",
                  "CreatedDateTo=", param.CreatedDateTo, "&",
                  "CreatedByDelimited=", param.CreatedByDelimited, "&",
                  "Reason=", param.Reason, "&",
                  "HRDComments=", param.HRDComments, "&",
                  "IsShowActiveOnly=", param.IsShowActiveOnly, "&",
                  "HasConfidentialView=", HasConfidentialView, "&",
                  "IsExport=", IsExport);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput>(), URL);

            return (Result, IsSuccess, ErrorMessage);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadMovementTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "Movement.xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

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

                XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                XSSFCellStyle legendStyle = (XSSFCellStyle)workbook.CreateCellStyle();

                string legendsMessage = string.Concat("Legends:", Environment.NewLine
                            , "*       - Required, Case Sensitive.", Environment.NewLine
                            , "**     - see Reference Lookup Worksheet, Required, Case Sensitive.");

                var format = workbook.CreateDataFormat().GetFormat("text");
                var colHeaderFont = workbook.CreateFont();
                var legendFont = workbook.CreateFont();
                colHeaderFont.IsBold = true;
                legendFont.IsBold = true;
                legendFont.IsItalic = true;
                colHeaderStyle.SetFont(colHeaderFont);
                colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                colHeaderStyle.Alignment = HorizontalAlignment.Center;
                colHeaderStyle.DataFormat = format;
                legendStyle.SetFont(legendFont);
                legendStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                legendStyle.FillPattern = FillPattern.SolidForeground;
                legendStyle.Alignment = HorizontalAlignment.Left;
                legendStyle.DataFormat = format;
                legendStyle.WrapText = true;
                alignCenter.Alignment = HorizontalAlignment.Center;

                var movementTypeList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.MOVEMENT_TYPE.ToString())).ToList();

                var employeeFieldList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.MOVEMENT_EMP_FIELD.ToString())).ToList();

                var companyList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString())).ToList();
                
                var employmentStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString())).ToList();

                var civilStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString())).ToList();
                
                var exemptionStatusList = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString())).ToList();


                void Movement()
                {
                    ISheet excelSheet = workbook.CreateSheet("Movement");
                    int irow = 0;
                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;
                    int maxColIndex = 8;

                    var removeSecDes = employeeFieldList
                        .Where(x => x.Value
                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.SECONDARY_DESIG.ToString()))
                        .First();

                    var removeOthers = employeeFieldList
                        .Where(x => x.Value
                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString()))
                        .First();

                    var _employeeFieldList = employeeFieldList;
                    _employeeFieldList.Remove(removeSecDes);
                    _employeeFieldList.Remove(removeOthers);

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 100, 1, 1),
                       _employeeFieldList.Select(x => x.Value).ToArray(), excelSheet));

                    row.CreateCell(0).SetCellValue("* Old Employee ID");
                    row.CreateCell(1).SetCellValue("* Employee Field");
                    row.CreateCell(2).SetCellValue("** Movement Type (Code)");
                    row.CreateCell(3).SetCellValue("** Old Value (Code)");
                    row.CreateCell(4).SetCellValue("** New Value (Code)");
                    row.CreateCell(5).SetCellValue("* Effective Date From (MM/DD/YYYY)");
                    row.CreateCell(6).SetCellValue("Effective Date To (MM/DD/YYYY)");
                    row.CreateCell(7).SetCellValue("Remarks");
                    row.CreateCell(8).SetCellValue("Additional Remarks");
                    
                    excelSheet.SetColumnWidth(0, 8000);
                    excelSheet.SetColumnWidth(1, 8000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 8000);
                    excelSheet.SetColumnWidth(4, 8000);
                    excelSheet.SetColumnWidth(5, 10000);
                    excelSheet.SetColumnWidth(6, 10000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 10000);
                    excelSheet.SetColumnWidth(9, 10000); // Legend

                    for (int i = 0; i <= maxColIndex; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;
                   

                    #endregion

                    for (int x = irow; x < 100; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle dateCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        dateCS.DataFormat = textFormat;
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.Cells[i].CellStyle = dateCS;

                        if (x == 1)
                        {
                            rowDate.CreateCell(maxColIndex + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[maxColIndex + 1].CellStyle = legendStyle;
                        }
                    }
                }

                void SecondaryDesignation()
                {
                    ISheet excelSheet = workbook.CreateSheet("Secondary Designation");
                    int irow = 0;
                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;
                    int maxColIndex = 8;

                    excelSheet.AddValidationData(getDropdownValidation(new CellRangeAddressList(1, 100, 4, 4),
                      new string[] { "ADD", "REMOVE" }, excelSheet));

                    row.CreateCell(0).SetCellValue("* Old Employee ID");
                    row.CreateCell(1).SetCellValue("** Movement Type (Code)");
                    row.CreateCell(2).SetCellValue("* Org Group Code");
                    row.CreateCell(3).SetCellValue("* Position Code");
                    row.CreateCell(4).SetCellValue("* Add/Remove");
                    row.CreateCell(5).SetCellValue("* Effective Date From (MM/DD/YYYY)");
                    row.CreateCell(6).SetCellValue("Effective Date To (MM/DD/YYYY)");
                    row.CreateCell(7).SetCellValue("Remarks");
                    row.CreateCell(8).SetCellValue("Additional Remarks");

                    excelSheet.SetColumnWidth(0, 8000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 8000);
                    excelSheet.SetColumnWidth(3, 8000);
                    excelSheet.SetColumnWidth(4, 5000);
                    excelSheet.SetColumnWidth(5, 10000);
                    excelSheet.SetColumnWidth(6, 10000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 10000);
                    excelSheet.SetColumnWidth(9, 10000); // Legends

                    for (int i = 0; i <= maxColIndex; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = irow; x < 100; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle dateCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var dateFormat = workbook.CreateDataFormat().GetFormat("text");
                        dateCS.DataFormat = dateFormat;
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.Cells[i].CellStyle = dateCS;

                        if (x == 1)
                        {
                            rowDate.CreateCell(maxColIndex + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[maxColIndex + 1].CellStyle = legendStyle;
                        }
                    }
                }

                void Others()
                {
                    ISheet excelSheet = workbook.CreateSheet("Others");
                    int irow = 0;
                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;
                    int maxColIndex = 5;

                    row.CreateCell(0).SetCellValue("* Old Employee ID");
                    row.CreateCell(1).SetCellValue("** Movement Type (Code)");
                    row.CreateCell(2).SetCellValue("* Effective Date From (MM/DD/YYYY)");
                    row.CreateCell(3).SetCellValue("Effective Date To (MM/DD/YYYY)");
                    row.CreateCell(4).SetCellValue("* Remarks");
                    row.CreateCell(5).SetCellValue("* Additional Remarks");

                    excelSheet.SetColumnWidth(0, 8000);
                    excelSheet.SetColumnWidth(1, 10000);
                    excelSheet.SetColumnWidth(2, 10000);
                    excelSheet.SetColumnWidth(3, 10000);
                    excelSheet.SetColumnWidth(4, 10000);
                    excelSheet.SetColumnWidth(5, 10000);
                    excelSheet.SetColumnWidth(6, 10000); // Legend

                    for (int i = 0; i <= maxColIndex; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = irow; x < 100; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle dateCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var dateFormat = workbook.CreateDataFormat().GetFormat("text");
                        dateCS.DataFormat = dateFormat;
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");
                        for (int i = 0; i <= maxColIndex; i++)
                            rowDate.Cells[i].CellStyle = dateCS;

                        if (x == 1)
                        {
                            rowDate.CreateCell(maxColIndex + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[maxColIndex + 1].CellStyle = legendStyle;
                        }
                    }
                }

                void AddReferenceLookupSheet()
                {
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("Movement Type (Code)");
                    pRow.CreateCell(1).SetCellValue("Description");
                    pRow.CreateCell(2).SetCellValue("");
                    pRow.CreateCell(3).SetCellValue(string.Concat("Employment Status (Code)"));
                    pRow.CreateCell(4).SetCellValue("Description");
                    pRow.CreateCell(5).SetCellValue("");
                    pRow.CreateCell(6).SetCellValue(string.Concat("Company (Code)"));
                    pRow.CreateCell(7).SetCellValue("Description");
                    pRow.CreateCell(8).SetCellValue("");
                    pRow.CreateCell(9).SetCellValue(string.Concat("Civil Status (Code)"));
                    pRow.CreateCell(10).SetCellValue("Description");
                    pRow.CreateCell(11).SetCellValue("");
                    pRow.CreateCell(12).SetCellValue(string.Concat("Exemption Status (Code)"));
                    pRow.CreateCell(13).SetCellValue("Description");

                    ReferenceLookupSheet.SetColumnWidth(0, 8000);
                    ReferenceLookupSheet.SetColumnWidth(1, 10000);
                    ReferenceLookupSheet.SetColumnWidth(2, 1000);
                    ReferenceLookupSheet.SetColumnWidth(3, 8000);
                    ReferenceLookupSheet.SetColumnWidth(4, 10000);
                    ReferenceLookupSheet.SetColumnWidth(5, 1000);
                    ReferenceLookupSheet.SetColumnWidth(6, 8000);
                    ReferenceLookupSheet.SetColumnWidth(7, 10000);
                    ReferenceLookupSheet.SetColumnWidth(8, 1000);
                    ReferenceLookupSheet.SetColumnWidth(9, 8000);
                    ReferenceLookupSheet.SetColumnWidth(10, 10000);
                    ReferenceLookupSheet.SetColumnWidth(11, 1000);
                    ReferenceLookupSheet.SetColumnWidth(12, 8000);
                    ReferenceLookupSheet.SetColumnWidth(13, 10000);

                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[3].CellStyle = colHeaderStyle;
                    pRow.Cells[4].CellStyle = colHeaderStyle;
                    pRow.Cells[6].CellStyle = colHeaderStyle;
                    pRow.Cells[7].CellStyle = colHeaderStyle;
                    pRow.Cells[9].CellStyle = colHeaderStyle;
                    pRow.Cells[10].CellStyle = colHeaderStyle;
                    pRow.Cells[12].CellStyle = colHeaderStyle;
                    pRow.Cells[13].CellStyle = colHeaderStyle;
                    
                    foreach (var item in movementTypeList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.CreateRow(rowCtr);
                        rowLookup.CreateCell(0, CellType.String).SetCellValue(item.Value);
                        rowLookup.CreateCell(1, CellType.String).SetCellValue(item.Description);
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in employmentStatusList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(3, CellType.String).SetCellValue(item.Value);
                            rowLookup.CreateCell(4, CellType.String).SetCellValue(item.Description);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(3, CellType.String).SetCellValue(item.Value);
                            newRow.CreateCell(4, CellType.String).SetCellValue(item.Description);
                        }
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in companyList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {                        
                            rowLookup.CreateCell(6, CellType.String).SetCellValue(item.Value);
                            rowLookup.CreateCell(7, CellType.String).SetCellValue(item.Description);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(6, CellType.String).SetCellValue(item.Value);
                            newRow.CreateCell(7, CellType.String).SetCellValue(item.Description);
                        }
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in civilStatusList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(9, CellType.String).SetCellValue(item.Value);
                            rowLookup.CreateCell(10, CellType.String).SetCellValue(item.Description);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(9, CellType.String).SetCellValue(item.Value);
                            newRow.CreateCell(10, CellType.String).SetCellValue(item.Description);
                        }
                        rowCtr++;
                    }
                    rowCtr = 1;
                    foreach (var item in exemptionStatusList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(12, CellType.String).SetCellValue(item.Value);
                            rowLookup.CreateCell(13, CellType.String).SetCellValue(item.Description);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(12, CellType.String).SetCellValue(item.Value);
                            newRow.CreateCell(13, CellType.String).SetCellValue(item.Description);
                        }
                        rowCtr++;
                    }
                   
                }

                Movement();
                SecondaryDesignation();
                Others();
                AddReferenceLookupSheet();

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
                string filePath = Path.Combine(_env.WebRootPath, "\\Movement\\Uploads");
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet movementSheet;
                    ISheet secondarySheet;
                    ISheet othersSheet;
                    string fullPath = Path.Combine(filePath, file.FileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        List<string> ErrorMessages = new List<string>();
                        bool isValid = true;

                        if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            movementSheet = hssfwb.GetSheet("Movement");
                            secondarySheet = hssfwb.GetSheet("Secondary Designation");
                            othersSheet = hssfwb.GetSheet("Others");

                            if (movementSheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Movement' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                            if (secondarySheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Secondary Designation' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                            if (othersSheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Others' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                        }
                        else //This will read 2007 Excel format    
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            movementSheet = hssfwb.GetSheet("Movement");
                            secondarySheet = hssfwb.GetSheet("Secondary Designation");
                            othersSheet = hssfwb.GetSheet("Others");

                            if (movementSheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Movement' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                            if (secondarySheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Secondary Designation' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                            if (othersSheet == null)
                            {
                                ErrorMessages.Add(string.Concat("'Others' Sheet ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                isValid = false;
                            }
                        }

                        if (!isValid)
                        {
                            _resultView.IsSuccess = false;
                            _resultView.Result = string.Join("<br>", ErrorMessages.ToArray());
                            return new JsonResult(_resultView);
                        }


                        List<UploadFileMovement> uploadMovementList = new List<UploadFileMovement>();
                        List<UploadFileSecondary> uploadSecondaryList = new List<UploadFileSecondary>();
                        List<UploadFileOthers> uploadOthersList = new List<UploadFileOthers>();

                        void fetchMovement(ISheet sheet)
                        {

                            IRow headerRow = movementSheet.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            int blankRows = 0;

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);

                                if (row != null)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                                    {
                                        UploadFileMovement obj = new UploadFileMovement
                                        {
                                            RowNum = (i + 1).ToString(),
                                            OldEmployeeID = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EmployeeField = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            MovementType = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            FromValue = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            NewValue = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateFrom = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateTo = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            Reason = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            HRDComments = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                        };
                                        uploadMovementList.Add(obj);
                                    }
                                }
                                else
                                {
                                    blankRows++;
                                    if (blankRows > 3)
                                        break;
                                }

                            }
                        }

                        void fetchSecondary(ISheet sheet)
                        {
                            IRow headerRow = secondarySheet.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            int blankRows = 0;

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);

                                if (row != null)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                                    {
                                        UploadFileSecondary obj = new UploadFileSecondary
                                        {
                                            RowNum = (i + 1).ToString(),
                                            OldEmployeeID = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            MovementType = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            OrgGroupCode = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            PositionCode = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            AddRemove = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateFrom = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateTo = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            Reason = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            HRDComments = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                        };
                                        uploadSecondaryList.Add(obj);
                                    }
                                }
                                else
                                {
                                    blankRows++;
                                    if (blankRows > 3)
                                        break;
                                }

                            }
                        }

                        void fetchOthers(ISheet sheet)
                        {
                            IRow headerRow = othersSheet.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            int blankRows = 0;

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);

                                if (row != null)
                                {
                                    if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                                    {
                                        UploadFileOthers obj = new UploadFileOthers
                                        {
                                            RowNum = (i + 1).ToString(),
                                            OldEmployeeID = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            MovementType = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateFrom = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            EffectiveDateTo = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            Reason = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                            HRDComments = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                        };
                                        uploadOthersList.Add(obj);
                                    }
                                }
                                else
                                {
                                    blankRows++;
                                    if (blankRows > 3)
                                        break;
                                }

                            }
                        }

                        fetchMovement(movementSheet);
                        fetchSecondary(secondarySheet);
                        fetchOthers(othersSheet);
                    
                    var URL = string.Concat(_plantillaBaseURL,
                                   _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("UploadInsert").Value, "?",
                                   "userid=", _globalCurrentUser.UserID);

                        var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadFile
                        {
                            UploadFileMovement = uploadMovementList,
                            UploadFileSecondary = uploadSecondaryList,
                            UploadFileOthers = uploadOthersList
                        }, URL);
                        
                        if (IsSuccess)
                        {
                            /*Add AuditLog*/
                            await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                                .AddAuditLog(new Security.Transfer.AuditLog.Form
                                {
                                    EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                    TableName = "Movement",
                                    TableID = 0, // New Record, no ID yet
                                    Remarks = "Movement List uploaded",
                                    IsSuccess = true,
                                    CreatedBy = _globalCurrentUser.UserID
                                });

                            if (uploadMovementList != null)
                            {
                                foreach (var obj in uploadMovementList.Select(x => new { x.EmployeeField, x.OldEmployeeID, x.NewValueID, x.NewValue, x.FromValue }))
                                {
                                    if (obj.EmployeeField.Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.COMPANY.ToString()))
                                    {
                                        var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetByOldEmployeeIDs(obj.OldEmployeeID);

                                        var _URL = string.Concat(_securityBaseURL,
                                              _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                              "userid=", _globalCurrentUser.UserID);

                                        var (_IsSuccess, _Message1) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                                        {
                                            ID = employee.FirstOrDefault().SystemUserID,
                                            Username = employee.FirstOrDefault().Code
                                        }, _URL);

                                    }
                                    else if (obj.EmployeeField.Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                                    {
                                        //if employment status change
                                        if (obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                        obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                        obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                        obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                        obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                                        {
                                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                                        .GetByOldEmployeeIDs(obj.OldEmployeeID);

                                            await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                                .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                                {
                                                    Username = employee.FirstOrDefault().Code,
                                                    IsActive = false
                                                });
                                        }
                                        if ((obj.FromValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                            obj.FromValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                            obj.FromValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                            obj.FromValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                            obj.FromValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                                            &&
                                            (!obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                            !obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                            !obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                            !obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                            !obj.NewValue.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString())))
                                        {
                                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                                        .GetByOldEmployeeIDs(obj.OldEmployeeID);

                                            await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                                .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                                {
                                                    Username = employee.FirstOrDefault().Code,
                                                    IsActive = true
                                                });
                                        }

                                        if (obj.NewValue
                                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString())
                                                ||
                                                obj.NewValue
                                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.OUTGOING.ToString())
                                                )

                                        {
                                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetByOldEmployeeIDs(obj.OldEmployeeID);

                                            var _URL = string.Concat(_workflowBaseURL,
                                                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BatchAccountabilityAdd").Value, "?",
                                                      "userid=", _globalCurrentUser.UserID);

                                            var (_IsSuccess, _Message) =
                                                await SharedUtilities.PostFromAPI(new EMS.Workflow.Transfer.Accountability.BatchAccountabilityAddInput
                                                {
                                                    EmployeeID = employee.FirstOrDefault().ID,
                                                    Status = EMS.Workflow.Transfer.Enums.AccountabilityStatus.FOR_CLEARANCE
                                                }, _URL);
                                        }
                                    }
                                }
                            }

                            foreach (var row in uploadMovementList)
                            {
                                if (string.IsNullOrEmpty(row.EffectiveDateTo))
                                {
                                    if (row.EmployeeField.Equals(EMS.FrontEnd.SharedClasses.Utilities.ReferenceCodes_PlantillaReference.ORG_GROUP.ToString()))
                                    {
                                        var OrgCode = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                                                        .GetByCodes(row.NewValue)).FirstOrDefault();
                                        var corporateEmailOutput = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                                                            .GetEmployeeEmailByOrgID(Convert.ToInt32(OrgCode.ID)));
                                        if (corporateEmailOutput.Email != null)
                                        {
                                            var EmployeeCode = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetByOldEmployeeIDs(row.OldEmployeeID)).FirstOrDefault();
                                            var oldValue = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(EmployeeCode.ID);
                                            var URLEmployee = string.Concat(_plantillaBaseURL,
                                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("Edit").Value, "?",
                                               "userid=", _globalCurrentUser.UserID);
                                            oldValue.CorporateEmail = corporateEmailOutput.Email;
                                            var (IsSuccessEmployee, MessageEmployee) = await SharedUtilities.PutFromAPI(oldValue, URLEmployee);
                                        }
                                    }
                                }
                            }
                        }
                        else {

                            /*Add AuditLog*/
                            await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                                .AddAuditLog(new Security.Transfer.AuditLog.Form
                                {
                                    EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                    TableName = "Movement",
                                    TableID = 0, // New Record, no ID yet
                                        Remarks = "Movement List upload failed",
                                    IsSuccess = false,
                                    CreatedBy = _globalCurrentUser.UserID
                                });
                        }

                        _resultView.IsSuccess = IsSuccess;
                        _resultView.Result = Message;
                    }
                }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckMovementExportListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

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

        public async Task<IActionResult> OnGetDownloadMovementExportListAsync([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "Movement List Export.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Movement List");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("New Employee ID");
                    row.CreateCell(1).SetCellValue("Employee Name");
                    row.CreateCell(2).SetCellValue("Old Employee ID");
                    row.CreateCell(3).SetCellValue("Date Hired");
                    row.CreateCell(4).SetCellValue("Employment Status");
                    row.CreateCell(5).SetCellValue("Region");
                    row.CreateCell(6).SetCellValue("Department / Section");
                    row.CreateCell(7).SetCellValue("Org Group");
                    row.CreateCell(8).SetCellValue("Employee Field");
                    row.CreateCell(9).SetCellValue("Movement Type");
                    row.CreateCell(10).SetCellValue("From");
                    row.CreateCell(11).SetCellValue("To");
                    row.CreateCell(12).SetCellValue("Date Effective From");
                    row.CreateCell(13).SetCellValue("Date Effective To");
                    row.CreateCell(14).SetCellValue("Remarks");
                    row.CreateCell(15).SetCellValue("Additional Remarks");
                    row.CreateCell(16).SetCellValue("Age");
                    row.CreateCell(17).SetCellValue("Gender");
                    row.CreateCell(18).SetCellValue("Modified Date");
                    row.CreateCell(19).SetCellValue("Modified By");


                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 12000);
                    excelSheet.SetColumnWidth(2, 5000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 7000);
                    excelSheet.SetColumnWidth(5, 10000);
                    excelSheet.SetColumnWidth(6, 10000);
                    excelSheet.SetColumnWidth(7, 10000);
                    excelSheet.SetColumnWidth(8, 8000);
                    excelSheet.SetColumnWidth(9, 10000);
                    excelSheet.SetColumnWidth(10, 14000);
                    excelSheet.SetColumnWidth(11, 14000);
                    excelSheet.SetColumnWidth(12, 5000);
                    excelSheet.SetColumnWidth(13, 5000);
                    excelSheet.SetColumnWidth(14, 12000);
                    excelSheet.SetColumnWidth(15, 16000);
                    excelSheet.SetColumnWidth(16, 4000);
                    excelSheet.SetColumnWidth(17, 4000);
                    excelSheet.SetColumnWidth(18, 8000);
                    excelSheet.SetColumnWidth(19, 10000);

                    XSSFCellStyle colHeaderStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle alignCenter = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFCellStyle wrapText = (XSSFCellStyle)workbook.CreateCellStyle();

                    var colHeaderFont = workbook.CreateFont();
                    colHeaderFont.IsBold = true;
                    colHeaderStyle.SetFont(colHeaderFont);
                    colHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                    colHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    colHeaderStyle.Alignment = HorizontalAlignment.Center;
                    alignCenter.Alignment = HorizontalAlignment.Center;
                    wrapText.WrapText = true;

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
                    row.Cells[14].CellStyle = colHeaderStyle;
                    row.Cells[15].CellStyle = colHeaderStyle;
                    row.Cells[16].CellStyle = colHeaderStyle;
                    row.Cells[17].CellStyle = colHeaderStyle;
                    row.Cells[18].CellStyle = colHeaderStyle;
                    row.Cells[19].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.NewEmployeeID);
                        row.CreateCell(1).SetCellValue(item.Name);
                        row.CreateCell(2).SetCellValue(item.OldEmployeeID);
                        row.CreateCell(3).SetCellValue(item.DateHired);
                        row.CreateCell(4).SetCellValue(item.EmploymentStatus);
                        row.CreateCell(5).SetCellValue(item.Region);
                        row.CreateCell(6).SetCellValue(item.DeptSection);
                        row.CreateCell(7).SetCellValue(item.OrgGroup);
                        row.CreateCell(8).SetCellValue(item.EmployeeField);
                        row.CreateCell(9).SetCellValue(item.MovementType);
                        row.CreateCell(10).SetCellValue(item.From);
                        row.CreateCell(11).SetCellValue(item.To);
                        row.CreateCell(12).SetCellValue(item.DateEffectiveFrom);
                        row.CreateCell(13).SetCellValue(item.DateEffectiveTo);
                        row.CreateCell(14).SetCellValue(item.Reason);
                        row.CreateCell(15).SetCellValue(item.HRDComments);
                        row.CreateCell(16).SetCellValue(item.Age);
                        row.CreateCell(17).SetCellValue(item.Gender);
                        row.CreateCell(18).SetCellValue(item.CreatedDate);
                        row.CreateCell(19).SetCellValue(item.CreatedByName);

                        row.Cells[3].CellStyle = alignCenter;
                        row.Cells[12].CellStyle = alignCenter;
                        row.Cells[13].CellStyle = alignCenter;
                        row.Cells[16].CellStyle = alignCenter;
                        row.Cells[18].CellStyle = alignCenter;

                        //row.Cells[14].CellStyle = wrapText;
                        //row.Cells[15].CellStyle = wrapText;

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
                        TableName = "Movement",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "Movement List exported",
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

        public async Task<JsonResult> OnGetPrint(GetPrintInput param)
        {
            bool HasConfidentialView = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0;
         
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetPrint").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "IDDelimited=", param.IDDelimited, "&",
                  "HasConfidentialView=", HasConfidentialView
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPrintOutput(), URL);


            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.PRINT.ToString(),
                        TableName = "EmployeeMovement",
                        TableID = 0,
                        Remarks = string.Concat("Record: ", param.IDDelimited, " successfully printed"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
                _resultView.Result = Result; 
            }
            else
                _resultView.Result = ErrorMessage;

            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetOrgGroupAutoComplete(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCreatedByAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetMovementChangeStatus([FromQuery] string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                               .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new EMS.Workflow.Transfer.Workflow.GetNextWorkflowStepInput
                {
                    WorkflowCode = "EMPLOYEE_MOVEMENT",
                    CurrentStepCode = CurrentStatus,
                    RoleIDDelimited = string.Join(",", roles.Select(x => x.Value).ToArray())
                })).Where(x => !x.Code.Equals(CurrentStatus)).ToList();

            _resultView.Result = status.OrderByDescending(x => x.Order).Select(x => new SelectListItem
            {
                Value = x.Code,
                Text = x.Description
            }).ToList();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostChangeStatusAsync(string ID)
        {
            ChangeStatus.ID = (ID.Split(",")).Select(x => long.Parse(x)).ToList();
            var (IsSuccess, Message) = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env).PostChangeStatus(ChangeStatus);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "CHANGE STATUS",
                        TableName = "EmployeeMovementStatusHistory",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Change Status ID: ", ID),
                        IsSuccess = IsSuccess,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                if (ChangeStatus.Status.Equals("APPROVED"))
                {
                    var UpdateEmployeeStatus = ((await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeMovementByIDs(ChangeStatus.ID)).Item1).Where(x => x.DateEffectiveFrom <= DateTime.Now
                    && (x.DateEffectiveTo.Equals(null) || x.DateEffectiveTo.Equals("") || x.DateEffectiveTo > DateTime.Now)).ToList(); ;

                    foreach (var item in UpdateEmployeeStatus.Where(x => x.EmployeeField.Equals("EMPLOYMENT_STATUS")))
                    {
                        var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(item.EmployeeID);
                        if (item.To.Equals("AWOL") ||
                            item.To.Equals("DECEASED") ||
                            item.To.Equals("RESIGNED") ||
                            item.To.Equals("TERMINATED") ||
                            item.To.Equals("BACKOUT"))
                        {
                            await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                {
                                    Username = employee.Code,
                                    IsActive = false
                                });
                        }
                        else
                        {
                            await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                {
                                    Username = employee.Code,
                                    IsActive = true
                                });
                        }
                    }

                    foreach (var item in UpdateEmployeeStatus.Where(x => x.EmployeeField.Equals("COMPANY")))
                    {
                        var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(item.EmployeeID);
                        var _URL = string.Concat(_securityBaseURL,
                                  _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                  "userid=", _globalCurrentUser.UserID);
                        var (_IsSuccess, _Message) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                        {
                            ID = employee.SystemUserID,
                            Username = employee.Code
                        }, _URL);
                    }

                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetStatusFilter()
        {
            var StatusFilter = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(6)).WorkflowStepList;
            _resultView.Result = StatusFilter.Select(x => new Utilities.API.ReferenceMaintenance.MultiSelectedFilter()
            {
                ID = x.StepCode,
                Description = x.StepDescription
            }).Where(y => y.ID.Equals("PENDING"));
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}