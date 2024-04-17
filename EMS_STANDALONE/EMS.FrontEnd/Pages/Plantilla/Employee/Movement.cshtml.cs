using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.EmployeeMovement;
using System.Text;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
//for movement checker , 16-17, 27-28, 39, 46-47, 49-71, 107, 253-360
namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class MovementModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.EmployeeMovement.BulkRemoveForm EmployeeMovement { get; set; }
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }
        [BindProperty]
        public Utilities.API.ChangeStatus ChangeStatus { get; set; }

        public MovementModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync()
        {

            if (_systemURL != null)
            {
                ViewData["HasAddFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/MOVEMENTADD")).Count() > 0 ? "true" : "false";
                ViewData["HasChangeStatus"] = "true";
            }

        }

        public async Task<IActionResult> OnGetListAsync([FromQuery] EMS.Plantilla.Transfer.EmployeeMovement.GetListInput param)
        {
            var StatusColor = (await new EMS.FrontEnd.SharedClasses.Common_Workflow.Common_Maintenance(_iconfiguration, _globalCurrentUser, _env).GetWorkflow(6)).WorkflowStepList;

            var (Result, IsSuccess, ErrorMessage) = await GetDataList(param, false);
            Result = (from left in Result
                      join right in StatusColor on left.Status equals right.StepCode
                      select new EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput
                      {
                          TotalNoOfRecord = left.TotalNoOfRecord,
                          NoOfPages = left.NoOfPages,
                          Row = left.Row,

                          ID = left.ID,
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
                          CreatedByName = left.CreatedByName
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

        private async Task<(List<EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput>, bool, string)> GetDataList([FromQuery] EMS.Plantilla.Transfer.EmployeeMovement.GetListInput param, 
            bool IsExport)
        {
            bool HasConfidentialView = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0;
         
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "EmployeeID=", param.EmployeeID, "&",
                  "EmployeeFieldDelimited=", param.EmployeeFieldDelimited, "&",
                  "MovementTypeDelimited=", param.MovementTypeDelimited, "&",
                  "StatusDelimited=", param.StatusDelimited, "&",
                  "From=", param.From, "&",
                  "To=", param.To, "&",
                  "DateEffectiveFromFrom=", param.DateEffectiveFromFrom, "&",
                  "DateEffectiveFromTo=", param.DateEffectiveFromTo, "&",
                  "DateEffectiveToFrom=", param.DateEffectiveToFrom, "&",
                  "DateEffectiveToTo=", param.DateEffectiveToTo, "&",
                  "CreatedDateFrom=", param.CreatedDateFrom, "&",
                  "CreatedDateTo=", param.CreatedDateTo, "&",
                  "CreatedByDelimited=", param.CreatedByDelimited, "&",
                  "Reason=", param.Reason, "&",
                  "HRDComments=", param.HRDComments, "&",
                  "IsShowActiveOnly=", param.IsShowActiveOnly, "&",
                  "HasConfidentialView=", HasConfidentialView, "&",
                  "IsExport=", IsExport);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput>(), URL);

            //// Get Employee Names by Employee IDs
            //List<EMS.Plantilla.Transfer.Employee.Form> employees =
            //   (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
            //   .GetEmployeeByUserIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
            //   .Distinct().ToList())).Item1;

            //if (employees.Count > 0)
            //{

            //    Result = Result
            //           .GroupJoin(employees,
            //           x => new { x.CreatedBy },
            //           y => new { CreatedBy = y.SystemUserID },
            //           (x, y) => new { movement = x, employees = y })
            //           .SelectMany(x => x.employees.DefaultIfEmpty(),
            //           (x, y) => new { movement = x, employees = y })
            //           .Select(x => new EMS.Plantilla.Transfer.EmployeeMovement.GetListOutput
            //           {
            //               ID = x.movement.movement.ID,
            //               NoOfPages = x.movement.movement.NoOfPages,
            //               Row = x.movement.movement.Row,
            //               TotalNoOfRecord = x.movement.movement.TotalNoOfRecord,
            //               EmployeeName = x.movement.movement.EmployeeName,
            //               OldEmployeeID = x.movement.movement.OldEmployeeID,
            //               OrgGroup = x.movement.movement.OrgGroup,
            //               EmployeeField = x.movement.movement.EmployeeField,
            //               MovementType = x.movement.movement.MovementType,
            //               From = x.movement.movement.From,
            //               To = x.movement.movement.To,
            //               DateEffectiveFrom = x.movement.movement.DateEffectiveFrom,
            //               DateEffectiveTo = x.movement.movement.DateEffectiveTo,
            //               CreatedDate = x.movement.movement.CreatedDate,
            //               CreatedBy = x.movement.movement.CreatedBy,
            //               HRDComments = x.movement.movement.HRDComments,
            //               Reason = x.movement.movement.Reason,
            //               CreatedByName = x.employees == null ? "" : x.employees.PersonalInformation == null ? "" :
            //                   string.Concat((x.employees.PersonalInformation.LastName ?? "").Trim(),
            //                        string.IsNullOrEmpty((x.employees.PersonalInformation.FirstName ?? "").Trim()) ? ""
            //                            : string.Concat(", ", x.employees.PersonalInformation.FirstName),
            //                        string.IsNullOrEmpty((x.employees.PersonalInformation.MiddleName ?? "").Trim()) ? ""
            //                            : string.Concat(" ", x.employees.PersonalInformation.MiddleName), " (", x.employees.Code, ") ")
            //           }).ToList();
            //}


            return (Result, IsSuccess, ErrorMessage);
        }


        public async Task<JsonResult> OnGetPrint(GetPrintInput param)
        {
            bool HasConfidentialView = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0;

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetPrint").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "IDDelimited=", param.IDDelimited, "&",
                  "HasConfidentialView=", HasConfidentialView);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetPrintOutput(), URL);

            if (IsSuccess)
                _resultView.Result = Result;
            else
                _resultView.Result = ErrorMessage;

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
            }

            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCreatedByAutoComplete(string Term, int TopResults)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployeeWithSystemUserAutoComplete(Term, TopResults);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        // Removed Delete function for Movement
        //public async Task<IActionResult> OnPostBulkDeleteAsync()
        //{
        //    var URL = string.Concat(_plantillaBaseURL,
        //          _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("Delete").Value, "?",
        //          "userid=", _globalCurrentUser.UserID);

        //    var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(EmployeeMovement, URL);
        //    _resultView.IsSuccess = IsSuccess;
        //    _resultView.Result = Message;

        //    if (IsSuccess)
        //    {
        //        StringBuilder stringIDs = new StringBuilder();

        //        foreach (var obj in EmployeeMovement.IDs)
        //        {
        //            stringIDs.Append(string.Concat(obj + ", "));
        //        }

        //        /*Add AuditLog*/
        //        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
        //            .AddAuditLog(new Security.Transfer.AuditLog.Form
        //            {
        //                EventType = Common_AuditLog.EventType.DELETE.ToString(),
        //                TableName = "EmployeeMovement",
        //                TableID = 0, // New Record, no ID yet
        //                Remarks = string.Concat("Employee Movement " + stringIDs + "deleted."),
        //                IsSuccess = true,
        //                CreatedBy = _globalCurrentUser.UserID
        //            });
        //    }

        //    return new JsonResult(_resultView);
        //}

        public async Task<JsonResult> OnGetMovementChangeStatus([FromQuery] string CurrentStatus)
        {
            var roles = (await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                               .GetSystemUserRoleDropDownByUserID(_globalCurrentUser.UserID)).ToList();

            var status = (await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetNextWorkflowStep(new GetNextWorkflowStepInput
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
            });
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
    }
}