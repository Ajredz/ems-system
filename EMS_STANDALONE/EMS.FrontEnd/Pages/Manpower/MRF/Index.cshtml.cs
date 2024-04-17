using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Manpower;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_PSGC;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Manpower.Transfer.DataDuplication.OrgGroup;
using EMS.Manpower.Transfer.MRF;
using EMS.Recruitment.Transfer.PSGC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF
{
    public class IndexModel : SharedClasses.Utilities
    {
        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env, bool IsAdminAccess = false) : base(iconfiguration, env)
        {
            _IsAdminAccess = IsAdminAccess;
        }

        public virtual async Task OnGet()
        {
            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/ADD")).Count() > 0 ? "true" : "false";
                ViewData["HasExportFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/MANPOWER/MRF/EXPORT")).Count() > 0 ? "true" : "false";
            }

            ViewData["OrgListFilter"] = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Manpower.ORGLIST_FILTER.ToString())).FirstOrDefault().Value;

        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Manpower.Transfer.MRF.GetListInput param)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);

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

     
        public async Task<IActionResult> OnGetApplicantPickerListAsync([FromQuery] EMS.Recruitment.Transfer.Applicant.GetApplicantPickerListInput param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("ApplicantPickerList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "IsHired=", false, "&",
                  "IsTaggedToMRF=", param.IsTaggedToMRF, "&",
                  "ID=", param.ID, "&",
                  "SelectedIDDelimited=", param.SelectedIDDelimited, "&",
                  //"ApplicantName=", param.ApplicantName, "&",
                  "LastName=", param.LastName, "&",
                  "FirstName=", param.FirstName, "&",
                  "MiddleName=", param.MiddleName, "&",
                  "Suffix=", param.Suffix, "&",
                  "ApplicationSourceDelimited=", param.ApplicationSourceDelimited, "&",
                  "MRFTransactionID=", param.MRFTransactionID, "&",
                  "CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  "DateScheduledFrom=", param.DateScheduledFrom, "&",
                  "DateScheduledTo=", param.DateScheduledTo, "&",
                  "DateCompletedFrom=", param.DateCompletedFrom, "&",
                  "DateCompletedTo=", param.DateCompletedTo, "&",
                  "ApproverRemarks=", param.ApproverRemarks, "&",
                  //"CurrentStepDelimited=", param.CurrentStepDelimited, "&",
                  //"WorkflowDelimited=", param.WorkflowDelimited, "&",
                  //"OrgGroupRemarks=", param.OrgGroupRemarks, "&",
                  //"OrgGroupDelimited=", param.OrgGroupDelimited, "&",
                  "PositionRemarks=", param.PositionRemarks, "&",
                  //"PositionDelimited=", param.PositionDelimited, "&",
                  "Course=", param.Course, "&",
                  "CurrentPositionTitle=", param.CurrentPositionTitle, "&",
                  "ExpectedSalaryFrom=", param.ExpectedSalaryFrom, "&",
                  "ExpectedSalaryTo=", param.ExpectedSalaryTo, "&",
                  "DateAppliedFrom=", param.DateAppliedFrom, "&",
                  "DateAppliedTo=", param.DateAppliedTo, "&",
                  "ScopeOrgGroupDelimited=", param.ScopeOrgGroupDelimited, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Recruitment.Transfer.Applicant.GetListOutput>(), URL);

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

        public async Task<JsonResult> OnGetPositionLevelAutoCompleteAsync(string Term, int TopResults)
        {
            var result = await new Common_Synced_PositionLevel(_iconfiguration, _globalCurrentUser, _env).GetPositionLevelAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOrgTypeAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetPositionAutoCompleteAsync(string term, int TopResults)
        {
            var result = await new Common_Synced_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionAutoComplete(term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValueByCodes(List<string> RefCodes)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCodes(RefCodes);

            //result.Add(new Utilities.API.ReferenceMaintenance.ReferenceValue
            //{
            //    Value = "IN_PROGRESS",
            //    Description = "In-Progress"
            //});

            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetValidateExistingActual(int OrgGroupID, int PositionID)
        {

            EMS.Plantilla.Transfer.OrgGroup.GetByOrgGroupIDAndPositionIDOutput manpowerCount =
                await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetByOrgGroupAndPosition(
                    new EMS.Plantilla.Transfer.OrgGroup.GetByOrgGroupIDAndPositionIDInput
                    {
                        OrgGroupID = OrgGroupID,
                        PositionID = PositionID
                    });

            //List<EMS.Plantilla.Transfer.OrgGroup
            //    .GetOrgGroupRollupPositionDropdownOutput> positionList =
            //    (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
            //    .GetOrgGroupRollupPositionDropdown(OrgGroupID)).ToList();

            //EMS.Plantilla.Transfer.OrgGroup
            //    .GetOrgGroupRollupPositionDropdownOutput manpowerCount = 
            //    new EMS.Plantilla.Transfer.OrgGroup.GetOrgGroupRollupPositionDropdownOutput();

            //if (positionList != null)
            //{
            //    if (positionList.Count > 0)
            //    {
            //        manpowerCount = positionList.Where(x => x.ID == PositionID).FirstOrDefault();
            //    }
            //}

            ResultView validate = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .PostValidateExistingActual(new EMS.Manpower.Transfer.MRF.ValidateMRFExistingActualInput {
                    OrgGroupID = OrgGroupID,
                    PositionID = PositionID,
                    PlannedCount = manpowerCount.PlannedCount,
                    ActiveCount = manpowerCount.ActiveCount,
                    InactiveCount = manpowerCount.InactiveCount,
                });

            _resultView.IsSuccess = validate.IsSuccess;
            _resultView.Result = new {
                MRFMessage = validate.Result,
                manpowerCount.PlannedCount,
                manpowerCount.ActiveCount,
                manpowerCount.InactiveCount,
                Variance = manpowerCount.ActiveCount - manpowerCount.PlannedCount
            };

            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetCheckMRFExportListAsync([FromQuery] EMS.Manpower.Transfer.MRF.GetListInput param)
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

        public async Task<IActionResult> OnGetDownloadMRFExportListAsync([FromQuery] EMS.Manpower.Transfer.MRF.GetListInput param)
        {
         var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, true);

            if (IsSuccess)
            {

                var createdBy = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                    .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy).Distinct().ToList());
                
                Result = (from main in Result
                          join created in createdBy on main.CreatedBy equals created.ID
                          select new GetListOutput()
                          {

                              TotalNoOfRecord = main.TotalNoOfRecord,
                              NoOfPages = main.NoOfPages,
                              ID = main.ID,
                              MRFID = main.MRFID,
                              OrgGroupDescription = main.OrgGroupDescription,
                              ScopeOrgGroup = main.ScopeOrgGroup,
                              PositionLevelDescription = main.PositionLevelDescription,
                              PositionDescription = main.PositionDescription,
                              NatureOfEmployment = main.NatureOfEmployment,
                              Purpose = main.Purpose,
                              NoOfApplicant = main.NoOfApplicant,
                              Status = main.Status,
                              ApprovedDate = main.ApprovedDate,
                              HiredDate = main.HiredDate,
                              Age = main.Age,
                              IsApproved = main.IsApproved,
                              CreatedBy = main.CreatedBy,
                              CreatedByName = string.Concat("(", created.Username, ") ", created.LastName, ", ", created.FirstName, " ", ((created.MiddleName == null || created.MiddleName == "") ? "" : created.MiddleName.Substring(0, 1))),
                              CreatedDate = main.CreatedDate,
                              ModifiedBy = main.ModifiedBy,
                              ModifiedDate = main.ModifiedDate
                          }).ToList();

                string sWebRootFolder = _env.WebRootPath;
                string sFileName = "MRF Request List Export.xlsx";
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("MRF Request");
                    int irow = 0;

                    #region Column Headers
                    IRow row = excelSheet.CreateRow(irow); irow++;

                    row.CreateCell(0).SetCellValue("MRF ID");
                    row.CreateCell(1).SetCellValue("Organizational Group");
                    row.CreateCell(2).SetCellValue("Position Level");
                    row.CreateCell(3).SetCellValue("Position");
                    row.CreateCell(4).SetCellValue("Nature of Employment");
                    row.CreateCell(5).SetCellValue("Purpose");
                    row.CreateCell(6).SetCellValue("No. of Applicant");
                    row.CreateCell(7).SetCellValue("Status");
                    row.CreateCell(8).SetCellValue("Created By");
                    row.CreateCell(9).SetCellValue("Created Date");
                    row.CreateCell(10).SetCellValue("Date Approved");
                    row.CreateCell(11).SetCellValue("Date Hired");
                    row.CreateCell(12).SetCellValue("Age");
                    if(_IsAdminAccess)
                        row.CreateCell(13).SetCellValue("Region");


                    excelSheet.SetColumnWidth(0, 5000);
                    excelSheet.SetColumnWidth(1, 9000);
                    excelSheet.SetColumnWidth(2, 7000);
                    excelSheet.SetColumnWidth(3, 8000);
                    excelSheet.SetColumnWidth(4, 5500);
                    excelSheet.SetColumnWidth(5, 5500);
                    excelSheet.SetColumnWidth(6, 5500);
                    excelSheet.SetColumnWidth(7, 4500);
                    excelSheet.SetColumnWidth(8, 6500);
                    excelSheet.SetColumnWidth(9, 6500);
                    excelSheet.SetColumnWidth(10, 6500);
                    excelSheet.SetColumnWidth(11, 6500);
                    excelSheet.SetColumnWidth(12, 3500);
                    if (_IsAdminAccess)
                        excelSheet.SetColumnWidth(13, 5500);

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
                    if (_IsAdminAccess)
                        row.Cells[13].CellStyle = colHeaderStyle;
                    #endregion

                    #region Column Details
                    foreach (var item in Result)
                    {
                        row = excelSheet.CreateRow(Convert.ToInt32(irow));
                        row.CreateCell(0).SetCellValue(item.MRFID);
                        row.CreateCell(1).SetCellValue(item.OrgGroupDescription);
                        row.CreateCell(2).SetCellValue(item.PositionLevelDescription);
                        row.CreateCell(3).SetCellValue(item.PositionDescription);
                        row.CreateCell(4).SetCellValue(item.NatureOfEmployment);
                        row.CreateCell(5).SetCellValue(item.Purpose);
                        row.CreateCell(6).SetCellValue(item.NoOfApplicant);
                        row.CreateCell(7).SetCellValue(item.Status);
                        row.CreateCell(8).SetCellValue(item.CreatedByName);
                        row.CreateCell(9).SetCellValue(item.CreatedDate);
                        row.CreateCell(10).SetCellValue(item.ApprovedDate);
                        row.CreateCell(11).SetCellValue(item.HiredDate);
                        row.CreateCell(12).SetCellValue(item.Age);
                        if (_IsAdminAccess)
                            row.CreateCell(13).SetCellValue(item.ScopeOrgGroup);

                        row.Cells[6].CellStyle = alignCenter;
                        row.Cells[7].CellStyle = alignCenter;
                        row.Cells[12].CellStyle = alignCenter;

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
                        TableName = "MRF",
                        TableID = 0, // New Record, no ID yet
                        Remarks = "MRF List exported",
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

        private async Task<(List<GetListOutput>, bool, string)> GetDataList(GetListInput param, bool IsExport)
        {
            List<int> GetOrg = new List<int>();
            if (_globalCurrentUser.OrgGroupDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupDescendants);
            if (_globalCurrentUser.OrgGroupRovingDescendants != null)
                GetOrg.AddRange(_globalCurrentUser.OrgGroupRovingDescendants);
            if (GetOrg.Count() > 0)
                //param.ScopeOrgGroupDelimited = string.Join(",", GetOrg);

            param.IsAdmin = _IsAdminAccess;
            param.IsExport = IsExport;
            param.OrgDescendant = string.Join(",", GetOrg);

            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("List").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetListOutput>(), param, URL);
        }

        public async Task<JsonResult> OnGetOrgGroupByOrgTypeAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            var result = await new SharedClasses.Common_Manpower.Common_Synced_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                .GetOrgGroupByOrgTypeAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowDropdown()
        {
            _resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowDropDown();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowStepDropdown(string WorkflowCode,string CurrentStatus)
        {
            /*_resultView.Result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowStepDropDown(WorkflowCode);*/

            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                               .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new EMS.Workflow.Transfer.Workflow.GetNextWorkflowStepInput
                {
                    WorkflowCode = WorkflowCode,
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

        public async Task<JsonResult> OnGetApplicantInfo(int ID)
        {
            _resultView.Result = await new SharedClasses.Common_Recruitment.Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ID);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSystemUserAsync([FromQuery] Security.Transfer.SystemUser.GetByNameInput param)
        {
            param.CreatedBy = _globalCurrentUser.UserID;

            var result = await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).AddSystemUser(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRegionAutoComplete(GetRegionAutoCompleteInput param)
        {
            var result = await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env)
                .GetRegionAutoComplete(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCompanyDropDown()
        {
            var res = await new Common_Reference(_iconfiguration, _globalCurrentUser, _manpowerBaseURL, _env)
                .GetReferenceValueByRefCode(ReferenceCodes_Manpower.COMPANY_TAG.ToString());

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetOnlineJobPosition(int ID)
        {
            var result = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPosition(ID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetMRFKickoutQuestionByMRFID(int MRFID)
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .GetMRFKickoutQuestionByMRFID(MRFID);

            var jsonData = new
            {
                rows = Result
            };
            return new JsonResult(jsonData);
        }
        public async Task<JsonResult> OnGetKickoutQuestionAutoComplete(GetByKickoutQuestionAutoCompleteInput param)
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .GetKickoutQuestionAutoComplete(param);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetMRFKickoutQuestionByID(int ID)
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .GetMRFKickoutQuestionByID(ID);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnGetKickoutQuestionByID(int ID)
        {
            var (Result, IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .GetKickoutQuestionByID(ID);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Result;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostAddKickoutQuestionToMRF([FromForm] AddKickoutQuestionToMRFInput param)
        {
            var (IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .AddKickoutQuestionToMRF(param);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostEditKickoutQuestionToMRF([FromForm] AddKickoutQuestionToMRFInput param)
        {
            var (IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .EditKickoutQuestionToMRF(param);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostRemoveKickoutQuestionToMRF([FromQuery] string ID)
        {
            List<int> IDs = ID.Split(",").Select(x=>int.Parse(x)).ToList();
            var (IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .RemoveKickoutQuestionToMRF(IDs);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostCloseInternalMRFAsync([FromForm] MRFChangeStatusInput param)
        {
            var (IsSuccess, Message) = await new Common_MRF(_iconfiguration, _globalCurrentUser, _env)
                .MRFChangeStatus(param);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;
            return new JsonResult(_resultView);
        }
    }
}