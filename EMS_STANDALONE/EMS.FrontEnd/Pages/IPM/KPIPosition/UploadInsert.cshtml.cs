using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilities.API;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using EMS.IPM.Transfer.KPIPosition;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.Pages.IPM.KPIPosition
{
    public class UploadInsertModel : SharedClasses.Utilities
    {

        public UploadInsertModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _env = env;
            _IsAdminAccess = IsAdminAccess;
        }

        public async Task<IActionResult> OnPostValidateUploadInsertKPIPosition()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI\\Uploads");
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

                    List<UploadFileEntity> uploadList = new List<UploadFileEntity>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                   !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadFileEntity obj = new UploadFileEntity
                                {
                                    RowNum = (i + 1).ToString(),
                                    EffectiveDate = row.GetCell(UploadFileColumn.EffectiveDate, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPICode = row.GetCell(UploadFileColumn.KPICode, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PositionCode = row.GetCell(UploadFileColumn.PositionCode, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Position = row.GetCell(UploadFileColumn.Position, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Weight = row.GetCell(UploadFileColumn.Weight, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
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
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("ValidateUploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadList, URL);
                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnPostUploadInsertKPIPosition()
        {
            IFormFile file = Request.Form.Files[0];
            string filePath = Path.Combine(_env.WebRootPath, "\\KPI\\Uploads");
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

                    List<UploadFileEntity> uploadList = new List<UploadFileEntity>();
                    int blankRows = 0;

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()) ||
                                      !string.IsNullOrEmpty(row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim()))
                            {
                                UploadFileEntity obj = new UploadFileEntity
                                {
                                    RowNum = (i + 1).ToString(),
                                    EffectiveDate = row.GetCell(UploadFileColumn.EffectiveDate, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    KPICode = row.GetCell(UploadFileColumn.KPICode, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    PositionCode = row.GetCell(UploadFileColumn.PositionCode, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Position = row.GetCell(UploadFileColumn.Position, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                    Weight = row.GetCell(UploadFileColumn.Weight, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("\uFFFD", String.Empty).Trim(),
                                };
                                uploadList.Add(obj);
                            }
                        }
                        else
                        {
                            blankRows++;
                            if(blankRows > 3)                            
                                break;
                        }

                    }

                    var URL = string.Concat(_ipmBaseURL,
                               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPIPosition").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(uploadList, URL);



                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Message;

                    if (IsSuccess)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.UPLOAD.ToString(),
                                TableName = "KPIPosition",
                                TableID = 0,
                                Remarks = string.Concat("KPI Position uploaded"),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            }); ;
                    }
                }
            }

            return new JsonResult(_resultView);
        }

    }
}