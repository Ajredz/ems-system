using EMS.FrontEnd.SharedClasses.Common_IPM;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.IPM.Transfer.KPIScore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.IPM.KPIScore
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public void OnGet()
        {
            if (_systemURL != null)
            {
                //ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPISCORE/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasUploadFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/KPISCORE?HANDLER=UPLOAD")).Count() > 0 ? "true" : "false";
            }
        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] GetListInput param)
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

        public async Task<IActionResult> OnGetDownloadGroupTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "KPI Score(quantitative).xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
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
                alignCenter.Alignment = HorizontalAlignment.Center;
                legendStyle.SetFont(legendFont);
                legendStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                legendStyle.FillPattern = FillPattern.SolidForeground;
                legendStyle.Alignment = HorizontalAlignment.Left;
                legendStyle.DataFormat = format;
                legendStyle.WrapText = true;
                alignCenter.Alignment = HorizontalAlignment.Center;

                void AddScoresSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Scores");
                    int rowCtr = 0;
                    
                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    row.CreateCell(0).SetCellValue("** KPI Code");
                    row.CreateCell(1).SetCellValue("* Branch/Org. Group");
                    row.CreateCell(2).SetCellValue("* Period (MM/YYYY)");
                    row.CreateCell(3).SetCellValue("* Target");
                    row.CreateCell(4).SetCellValue("* Actual");
                    row.CreateCell(5).SetCellValue("* Rate");

                    excelSheet.SetColumnWidth(0, 4500);
                    excelSheet.SetColumnWidth(1, 6500);
                    excelSheet.SetColumnWidth(2, 5000);
                    excelSheet.SetColumnWidth(3, 4500);
                    excelSheet.SetColumnWidth(4, 4500);
                    excelSheet.SetColumnWidth(5, 4500);
                    excelSheet.SetColumnWidth(6, 10000); // Legend

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

                        if (x == 1)
                        {
                            rowDate.CreateCell(5 + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[5 + 1].CellStyle = legendStyle;
                        }
                    }

                }

                var kpiList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails()).ToList();

                void AddReferenceLookupSheet()
                {
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("KPI Codes");
                    pRow.CreateCell(1).SetCellValue("Old KPI Codes");
                    pRow.CreateCell(2).SetCellValue("KPI Description");
                    ReferenceLookupSheet.SetColumnWidth(0, 5000);
                    ReferenceLookupSheet.SetColumnWidth(1, 5000);
                    ReferenceLookupSheet.SetColumnWidth(2, 25000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[2].CellStyle = colHeaderStyle;
                    
                    foreach (var item in kpiList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            rowLookup.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            rowLookup.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            newRow.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            newRow.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        rowCtr++;
                    }
                }

                AddScoresSheet();
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

        public async Task<IActionResult> OnPostValidateUploadScores()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI Score\\Uploads");
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

                    List<UploadScoresFile> uploadList = new List<UploadScoresFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadScoresFile obj = new UploadScoresFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    KPICode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OrgGroupCode = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Period = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Target = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Actual = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Rate = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPIType = "QUANTITATIVE"
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

                    var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("ValidateUploadScores").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadScoresOutput(), uploadList, URL);

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = IsSuccess ? Result.Message : Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostUploadScores()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI Score\\Uploads");
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

                    List<UploadScoresFile> uploadList = new List<UploadScoresFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadScoresFile obj = new UploadScoresFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    KPICode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    OrgGroupCode = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Period = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Target = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Actual = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Rate = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPIType = "QUANTITATIVE"
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

                    var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("UploadScores").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadScoresOutput(), uploadList, URL);

                    if (IsSuccess)
                    {

                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "KPI Score",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "KPI Score (quantitative) uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });

                    }

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = IsSuccess ? Result.Message : Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostValidateUploadScoresPerEmployee()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI Score\\Uploads");
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

                    List<UploadScoresFile> uploadList = new List<UploadScoresFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadScoresFile obj = new UploadScoresFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    KPICode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EmployeeCode = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Period = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Target = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Actual = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Rate = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPIType = "QUALITATIVE"
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

                    var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("ValidateUploadScores").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadScoresOutput(), uploadList, URL);

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = IsSuccess ? Result.Message : Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostUploadScoresPerEmployee()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI Score\\Uploads");
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

                    List<UploadScoresFile> uploadList = new List<UploadScoresFile>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadScoresFile obj = new UploadScoresFile
                                {
                                    RowNum = (i + 1).ToString(),
                                    KPICode = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    EmployeeCode = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Period = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Target = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Actual = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Rate = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPIType = "QUALITATIVE"
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

                    var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("UploadScores").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (Result, IsSuccess, Message) = await SharedUtilities.PostFromAPI(new UploadScoresOutput(), uploadList, URL);

                    if (IsSuccess)
                    {

                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "KPI Score Per Employee",
                                TableID = 0, // New Record, no ID yet
                                Remarks = "KPI Score (qualitative) uploaded",
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });

                    }

                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = IsSuccess ? Result.Message : Message;
                }
            }

            return new JsonResult(_resultView);
        }


        public async Task<JsonResult> OnGetPositionByOrgGroupDropdown(int OrgGroupID)
        {
            //// Get all KPI Scores
            //var existingKPIScoresList =
            //    await new Common_KPIScore(_iconfiguration, _globalCurrentUser, _env).GetAllKPIScore();
            //// Get all unique KPI Position with scores
            //var existingKPIScoresPosID = existingKPIScoresList.Select(x => x.KPIPosition).Distinct().ToList();

            //// Get all Positions with KPI
            //var existingKPIPosList =
            //    await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetAllKPIPosition();
            //// Get all Positions with scores from KPI Score via KPI Position
            //var existingKPIPosID = existingKPIPosList.Where(x => existingKPIScoresPosID.Contains(x.ID))
            //                                         .Select(x => x.Position.ToString()).Distinct().ToList();

            //// Exclude Positions with KPI from Select List
            //var positionList = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionByOrgGroupDropdown(OrgGroupID);
            //var filteredPositionList = positionList.Where(x => !existingKPIPosID.Contains(x.Value)).ToList();

            //_resultView.Result = filteredPositionList;

            _resultView.Result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionByOrgGroupDropdown(OrgGroupID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKPIByPosition(int PositionID)
        {
            _resultView.Result = await new Common_KPIPosition(_iconfiguration, _globalCurrentUser, _env).GetKPIPosition(PositionID, DateTime.Now.ToString("MM/dd/yyyy"));
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetKPIAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetKPIAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        private async Task<(List<GetListOutput>, bool, string)> GetListData(GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIScore").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",
                  "IsExport=", IsExport, "&",

                  "ID=", param.ID, "&",
                  "EmployeeDelimited=", param.EmployeeDelimited, "&",
                  "ParentOrgGroup=", param.ParentOrgGroup, "&",
                  "OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "KPIDelimited=", param.KPIDelimited, "&",
                  "Target=", param.Target, "&",
                  "Actual=", param.Actual, "&",
                  "Rate=", param.Rate, "&",
                  "PeriodFrom=", param.PeriodFrom, "&",
                  "PeriodTo=", param.PeriodTo,"&",
                  "KPIType=", param.KPIType);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }

        public async Task<IActionResult> OnGetCheckExportList([FromQuery] GetListInput param)
        {

            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, true);

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

        public async Task<IActionResult> OnGetDownloadExportList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "KPI Score List(quantitative).xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("KPI Score List(quantitative)");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Org. Group");
                    row.CreateCell(1).SetCellValue("KPI");
                    row.CreateCell(2).SetCellValue("Period");
                    row.CreateCell(3).SetCellValue("Target");
                    row.CreateCell(4).SetCellValue("Actual");
                    row.CreateCell(5).SetCellValue("Rate");

                    excelSheet.SetColumnWidth(0, 10000);
                    excelSheet.SetColumnWidth(1, 9000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 7000);
                    excelSheet.SetColumnWidth(4, 7000);
                    excelSheet.SetColumnWidth(5, 7000);

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
                        row.CreateCell(0).SetCellValue(item.OrgGroup);
                        row.CreateCell(1).SetCellValue(item.KPI);
                        row.CreateCell(2).SetCellValue(item.Period);
                        row.CreateCell(3).SetCellValue(item.Target);
                        row.CreateCell(4).SetCellValue(item.Actual);
                        row.CreateCell(5).SetCellValue(item.Rate);
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
                        TableName = "KPI Score",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "KPI Score (quantitative) exported",
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
        
        public async Task<IActionResult> OnGetDownloadExportPerEmployeeList([FromQuery] GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetListData(param, true);

            if (IsSuccess)
            {
                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "KPI Score List(qualitative).xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("KPI Score List(qualitative)");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("Employee");
                    row.CreateCell(1).SetCellValue("KPI");
                    row.CreateCell(2).SetCellValue("Period");
                    row.CreateCell(3).SetCellValue("Target");
                    row.CreateCell(4).SetCellValue("Actual");
                    row.CreateCell(5).SetCellValue("Rate");

                    excelSheet.SetColumnWidth(0, 10000);
                    excelSheet.SetColumnWidth(1, 9000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 7000);
                    excelSheet.SetColumnWidth(4, 7000);
                    excelSheet.SetColumnWidth(5, 7000);

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
                        row.CreateCell(0).SetCellValue(item.Employee);
                        row.CreateCell(1).SetCellValue(item.KPI);
                        row.CreateCell(2).SetCellValue(item.Period);
                        row.CreateCell(3).SetCellValue(item.Target);
                        row.CreateCell(4).SetCellValue(item.Actual);
                        row.CreateCell(5).SetCellValue(item.Rate);
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
                        TableName = "KPI Score Per Employee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "KPI Score (qualitative) exported",
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

        public async Task<JsonResult> OnGetEmployeeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetDownloadGroupPerEmployeeTemplate()
        {
            string sWebRootFolder = _env.WebRootPath;
            string sFileName = "KPI Score(qualitative).xlsx";
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // Header style
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
                alignCenter.Alignment = HorizontalAlignment.Center;
                alignCenter.Alignment = HorizontalAlignment.Center;
                legendStyle.SetFont(legendFont);
                legendStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
                legendStyle.FillPattern = FillPattern.SolidForeground;
                legendStyle.Alignment = HorizontalAlignment.Left;
                legendStyle.DataFormat = format;
                legendStyle.WrapText = true;
                alignCenter.Alignment = HorizontalAlignment.Center;

                void AddScoresSheet()
                {
                    ISheet excelSheet = workbook.CreateSheet("Scores");
                    int rowCtr = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(rowCtr); rowCtr++;

                    row.CreateCell(0).SetCellValue("** KPI Code");
                    row.CreateCell(1).SetCellValue("* Employee ID");
                    row.CreateCell(2).SetCellValue("Name");
                    row.CreateCell(3).SetCellValue("* Period (MM/YYYY)");
                    row.CreateCell(4).SetCellValue("* Target");
                    row.CreateCell(5).SetCellValue("* Actual");
                    row.CreateCell(6).SetCellValue("* Rate");


                    excelSheet.SetColumnWidth(0, 4500);
                    excelSheet.SetColumnWidth(1, 5000);
                    excelSheet.SetColumnWidth(2, 8000);
                    excelSheet.SetColumnWidth(3, 5000);
                    excelSheet.SetColumnWidth(4, 4500);
                    excelSheet.SetColumnWidth(5, 4500);
                    excelSheet.SetColumnWidth(6, 4500);
                    excelSheet.SetColumnWidth(7, 10000); // Legend

                    for (int i = 0; i <= 6; i++)
                        row.Cells[i].CellStyle = colHeaderStyle;

                    #endregion

                    for (int x = rowCtr; x <= 10000; x++)
                    {
                        IRow rowDate = excelSheet.CreateRow(x);
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;

                        for (int i = 0; i <= 6; i++)
                            rowDate.CreateCell(i, CellType.String).SetCellValue("");

                        for (int i = 0; i <= 6; i++)
                            rowDate.Cells[i].CellStyle = textCS;

                        if (x == 1)
                        {
                            rowDate.CreateCell(6 + 1).SetCellValue(legendsMessage);
                            rowDate.Cells[6 + 1].CellStyle = legendStyle;
                        }
                    }

                }

                var kpiList = (await new Common_KPI(_iconfiguration, _globalCurrentUser, _env).GetAllKPIDetails()).ToList();

                void AddReferenceLookupSheet()
                {
                    ISheet ReferenceLookupSheet = workbook.CreateSheet("Reference Lookup");
                    int rowCtr = 0;
                    IRow pRow = ReferenceLookupSheet.CreateRow(rowCtr); rowCtr++;
                    pRow.CreateCell(0).SetCellValue("KPI Codes");
                    pRow.CreateCell(1).SetCellValue("Old KPI Codes");
                    pRow.CreateCell(2).SetCellValue("KPI Description");
                    ReferenceLookupSheet.SetColumnWidth(0, 5000);
                    ReferenceLookupSheet.SetColumnWidth(1, 5000);
                    ReferenceLookupSheet.SetColumnWidth(2, 25000);
                    pRow.Cells[0].CellStyle = colHeaderStyle;
                    pRow.Cells[1].CellStyle = colHeaderStyle;
                    pRow.Cells[2].CellStyle = colHeaderStyle;

                    foreach (var item in kpiList.ToList())
                    {
                        XSSFCellStyle textCS = (XSSFCellStyle)workbook.CreateCellStyle();
                        var textFormat = workbook.CreateDataFormat().GetFormat("text");
                        textCS.DataFormat = textFormat;
                        IRow rowLookup = ReferenceLookupSheet.GetRow(rowCtr);
                        if (rowLookup != null)
                        {
                            rowLookup.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            rowLookup.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            rowLookup.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        else
                        {
                            IRow newRow = ReferenceLookupSheet.CreateRow(rowCtr);
                            newRow.CreateCell(0, CellType.String).SetCellValue(item.KPICode);
                            newRow.CreateCell(1, CellType.String).SetCellValue(item.OldKPICode);
                            newRow.CreateCell(2, CellType.String).SetCellValue(item.KPIDescription);
                        }
                        rowCtr++;
                    }
                }

                AddScoresSheet();
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

    }
}