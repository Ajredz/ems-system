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
using System.Security.Policy;
using EMS.IPM.Data.DataDuplication.Employee;
//for movement checker 17-19, 29-30, 37, 40-42, 126-290, 435, 446-587, 589-590, 593-597
namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class MovementAddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.EmployeeMovement.Form EmployeeMovement { get; set; }
        [BindProperty]
        public EMS.Plantilla.Transfer.EmployeeMovement.BulkRemoveForm BulkRemove { get; set; }
        [BindProperty]
        public Utilities.API.ChangeStatus ChangeStatus { get; set; }

        public MovementAddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int EmployeeID, bool IsEdit = false)
        {
            ViewData["HasChangeStatus"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/MOVEMENTCHANGESTATUS")).Count() > 0 ? "true" : "false";
            if (_globalCurrentUser != null)
            {
                EmployeeMovement = new EMS.Plantilla.Transfer.EmployeeMovement.Form {
                    EmployeeID = EmployeeID,
                    Status = "PENDING"
                };

                if (IsEdit)
                {
                    EmployeeMovement = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                        .GetEmployeeMovement(EmployeeID);
                    EmployeeMovement.IsEdit = true;

                    if (EmployeeMovement.EmployeeField
                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.SECONDARY_DESIG.ToString()))
                    {
                        if (EmployeeMovement.NewValue.Split(",").Length > 1)
                        {
                            var secondayDesig = EmployeeMovement.NewValue.Split(",");
                            var orgGroup = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                                .GetOrgGroup(Convert.ToInt32(secondayDesig[0]));
                            var position = await new Common_Position(_iconfiguration, _globalCurrentUser, _env)
                                .GetPosition(Convert.ToInt32(secondayDesig[1]));

                            ViewData["SecondaryOrgGroupDescription"] = string.Concat(orgGroup.Code, " - ", orgGroup.Description);
                            ViewData["SecondaryPositionDescription"] = string.Concat(position.Code, " - ", position.Title);

                        }
                    }

                }

                var movementTypes = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.MOVEMENT_TYPE.ToString())).ToList();

                ViewData["MovementTypeSelectList"] = movementTypes.Select(
                        x => new SelectListItem
                        {
                            Value = x.Value,
                            Text = x.Description,
                            Selected = IsEdit ? x.Value == EmployeeMovement.MovementType : false
                        }).ToList();

                var employeeFields = (await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.MOVEMENT_EMP_FIELD.ToString())).ToList();

                ViewData["EmployeeFieldSelectList"] = employeeFields.Select(
                        x => new SelectListItem
                        {
                            Value = x.Value,
                            Text = x.Description,
                            Selected = IsEdit ? x.Value == EmployeeMovement.EmployeeField : false
                        }).ToList();

                ViewData["SpecialCases"] = ((await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode("SPECIAL_CASES")).ToList()).Where(x => x.Value.Equals("MOVEMENT_DETAILS")).Select(y => y.Description).FirstOrDefault();
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            
            EmployeeMovement.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeMovement, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "ADD",
                        TableName = "EmployeeMovement",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(EmployeeMovement.MovementType, "-"
                        , EmployeeMovement.EmployeeID, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                //IF TRUE DEACTIVATED USER ACCOUNT
                /*if (EmployeeMovement.EmployeeFieldList != null)
                {
                    foreach (var obj in EmployeeMovement.EmployeeFieldList)
                    {
                        if (obj.EmployeeField.Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                            {
                                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(EmployeeMovement.EmployeeID);

                                await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                    .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                    {
                                        Username = employee.Code,
                                        IsActive = false
                                    });
                            }
                            if ((obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                                &&
                                (!obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString())))
                            {
                                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(EmployeeMovement.EmployeeID);

                                await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                    .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                    {
                                        Username = employee.Code,
                                        IsActive = true
                                    });
                            }
                        }
                    }
                }*/

                /*if (EmployeeMovement.UseCurrent)
                {
                    if (EmployeeMovement.EmployeeFieldList != null && EmployeeMovement.MovementType.Equals(EMS.FrontEnd.SharedClasses.Utilities.ReferenceCodes_PlantillaReference.BRANCH_TRANSFER.ToString()))
                    {
                        foreach (var obj in EmployeeMovement.EmployeeFieldList)
                        {
                            if (obj.EmployeeField.Equals(EMS.FrontEnd.SharedClasses.Utilities.ReferenceCodes_PlantillaReference.ORG_GROUP.ToString()))
                            {
                                var corporateEmailOutput = (await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployeeEmailByOrgID(Convert.ToInt32(obj.NewValueID)));
                                if (corporateEmailOutput.Email != null)
                                {
                                    var oldValue = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(EmployeeMovement.EmployeeID);
                                    var URLEmployee = string.Concat(_plantillaBaseURL,
                                       _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("Edit").Value, "?",
                                       "userid=", _globalCurrentUser.UserID);
                                    oldValue.CorporateEmail = corporateEmailOutput.Email;
                                    var (IsSuccessEmployee, MessageEmployee) = await SharedUtilities.PutFromAPI(oldValue, URLEmployee);
                                }
                            }
                        }
                    }
                }*/
                /* Movement Special Cases */
                /* Company */
                /*if (EmployeeMovement.EmployeeFieldList != null) { 
                    foreach (var obj in EmployeeMovement.EmployeeFieldList.Select(x => x.EmployeeField))
                    {
                        EmployeeMovement.EmployeeField = obj;
                        EmployeeMovement.NewValueID = EmployeeMovement.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().NewValueID;

                        if (EmployeeMovement.EmployeeField
                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.COMPANY.ToString()))
                        {
                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeMovement.EmployeeID);

                            var _URL = string.Concat(_securityBaseURL,
                                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                      "userid=", _globalCurrentUser.UserID);

                            var (_IsSuccess, _Message) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                            {
                                ID = employee.SystemUserID,
                                Username = employee.Code
                            }, _URL);

                        }
                        *//* Employment Status *//*
                        else if (EmployeeMovement.EmployeeField
                           .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (EmployeeMovement.NewValueID
                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()))

                            {
                                var _URL = string.Concat(_workflowBaseURL,
                                          _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BatchAccountabilityAdd").Value, "?",
                                          "userid=", _globalCurrentUser.UserID);

                                var (_IsSuccess, _Message) =
                                    await SharedUtilities.PostFromAPI(new EMS.Workflow.Transfer.Accountability.BatchAccountabilityAddInput
                                    {
                                        EmployeeID = EmployeeMovement.EmployeeID,
                                        Status = EMS.Workflow.Transfer.Enums.AccountabilityStatus.FOR_CLEARANCE
                                    }, _URL);
                            }

                        }
                    }
                }

                else 
                {
                    if (EmployeeMovement.EmployeeField != null)
                    {
                        if (EmployeeMovement.EmployeeField
                                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.COMPANY.ToString()))
                        {
                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeMovement.EmployeeID);

                            var _URL = string.Concat(_securityBaseURL,
                                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                      "userid=", _globalCurrentUser.UserID);

                            var (_IsSuccess, _Message) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                            {
                                ID = employee.SystemUserID,
                                Username = employee.Code
                            }, _URL);

                        }
                        *//* Employment Status *//*
                        else if (EmployeeMovement.EmployeeField
                           .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (EmployeeMovement.NewValueID
                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()))

                            {
                                var _URL = string.Concat(_workflowBaseURL,
                                          _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BatchAccountabilityAdd").Value, "?",
                                          "userid=", _globalCurrentUser.UserID);

                                var (_IsSuccess, _Message) =
                                    await SharedUtilities.PostFromAPI(new EMS.Workflow.Transfer.Accountability.BatchAccountabilityAddInput
                                    {
                                        EmployeeID = EmployeeMovement.EmployeeID,
                                        Status = EMS.Workflow.Transfer.Enums.AccountabilityStatus.FOR_CLEARANCE
                                    }, _URL);
                            }

                        } 
                    }
                    
                }*/
                
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAutoCompleteByMovementType(GetAutoCompleteByMovementTypeInput param)
        {
            var result = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                .GetAutoCompleteByMovementType(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetAutoPopulateByMovementType(GetAutoPopulateByMovementTypeInput param)
        {
            var result = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                .GetAutoPopulateByMovementType(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<IActionResult> OnGetEmployeeFieldListAsync(string type,string EmployeeFieldType)
        {
            var (Result, IsSuccess, ErrorMessage) = await GetEmployeeFieldList(type);
            
            if (IsSuccess)
            {
                var jsonData = new
                {
                    total = Result.Count > 0 ? Result.FirstOrDefault().NoOfPages : 0,
                    sort = "",
                    records = Result.Count > 0 ? Result.Last().TotalNoOfRecord : 0,
                    rows = (EmployeeFieldType  == null ? Result : Result.Where(x=>x.EmployeeField.Equals(EmployeeFieldType)).ToList())
                };
                return new JsonResult(jsonData);
            }
            else
            {
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        private async Task<(List<EMS.Plantilla.Transfer.EmployeeMovement.GetEmployeeFieldListOutput>, bool, string)> GetEmployeeFieldList(string type)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetEmployeeFieldList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "type=", type);

            return await SharedUtilities.GetFromAPI(new List<EMS.Plantilla.Transfer.EmployeeMovement.GetEmployeeFieldListOutput>(), URL);
        }

        public async Task<JsonResult> OnGetEmployeeMovementMapping(MovementTypeForm param)
        {
            bool allowMultiple = false;
            var employeeField = "";
            List<GetMovementTypeOutput> movementType =
                   await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env)
                   .GetMovementType(param);

            var obj = movementType.Select(x => x.AllowMultiple).Distinct();
            if (movementType.Count > 0)
            {

                if (movementType.First().AllowMultiple == true)
                {
                    allowMultiple = true;
                }
                else
                {
                    employeeField = movementType.First().EmployeeField;
                }
            }
            else
            {
                allowMultiple = true;
            }

            var jsonData = new
            {
                condition = allowMultiple,
                rows = movementType,
                empField = employeeField
            };
            return new JsonResult(jsonData);
        }

        public async Task<IActionResult> OnPostBulkDeleteAsync()
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("BulkRemove").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(BulkRemove, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                StringBuilder stringIDs = new StringBuilder();

                foreach (var obj in BulkRemove.IDs)
                {
                    stringIDs.Append(string.Concat(obj + ", "));
                }

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.DELETE.ToString(),
                        TableName = "EmployeeMovementMapping",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat("Employee Field " + stringIDs + "deleted."),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostUpdateAsync()
        {

            EmployeeMovement.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("update").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(EmployeeMovement, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = "EDIT",
                        TableName = "EmployeeMovement",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(EmployeeMovement.MovementType, "-"
                        , EmployeeMovement.EmployeeID, " Update"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });


                //IF TRUE DEACTIVATED USER ACCOUNT
                /*if (EmployeeMovement.EmployeeFieldList != null)
                {
                    foreach (var obj in EmployeeMovement.EmployeeFieldList)
                    {
                        if (obj.EmployeeField.Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                            {
                                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(EmployeeMovement.EmployeeID);

                                await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                    .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                    {
                                        Username = employee.Code,
                                        IsActive = false
                                    });
                            }
                            if ((obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                obj.OldValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString()))
                                &&
                                (!obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.AWOL.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.DECEASED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.TERMINATED.ToString()) ||
                                !obj.NewValueID.Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.BACKOUT.ToString())))
                            {
                                var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                            .GetEmployee(EmployeeMovement.EmployeeID);

                                await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                                    .DisableEmployeeAccount(new Security.Transfer.SystemUser.UpdateSystemUser
                                    {
                                        Username = employee.Code,
                                        IsActive = true
                                    });
                            }
                        }
                    }
                }

                *//* Movement Special Cases */
                /* Company *//*
                if (EmployeeMovement.EmployeeFieldList != null)
                {
                    foreach (var obj in EmployeeMovement.EmployeeFieldList.Select(x => x.EmployeeField))
                    {
                        EmployeeMovement.EmployeeField = obj;
                        EmployeeMovement.NewValueID = EmployeeMovement.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().NewValueID;

                        if (EmployeeMovement.EmployeeField
                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.COMPANY.ToString()))
                        {
                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeMovement.EmployeeID);

                            var _URL = string.Concat(_securityBaseURL,
                                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                      "userid=", _globalCurrentUser.UserID);

                            var (_IsSuccess, _Message) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                            {
                                ID = employee.SystemUserID,
                                Username = employee.Code
                            }, _URL);

                        }
                        *//* Employment Status *//*
                        else if (EmployeeMovement.EmployeeField
                           .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (EmployeeMovement.NewValueID
                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()))

                            {
                                var _URL = string.Concat(_workflowBaseURL,
                                          _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BatchAccountabilityAdd").Value, "?",
                                          "userid=", _globalCurrentUser.UserID);

                                var (_IsSuccess, _Message) =
                                    await SharedUtilities.PostFromAPI(new EMS.Workflow.Transfer.Accountability.BatchAccountabilityAddInput
                                    {
                                        EmployeeID = EmployeeMovement.EmployeeID,
                                        Status = EMS.Workflow.Transfer.Enums.AccountabilityStatus.FOR_CLEARANCE
                                    }, _URL);
                            }

                        }
                    }
                }

                else
                {
                    if (EmployeeMovement.EmployeeField != null)
                    {
                        if (EmployeeMovement.EmployeeField
                                        .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.COMPANY.ToString()))
                        {
                            var employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                .GetEmployee(EmployeeMovement.EmployeeID);

                            var _URL = string.Concat(_securityBaseURL,
                                      _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("UpdateUsername").Value, "?",
                                      "userid=", _globalCurrentUser.UserID);

                            var (_IsSuccess, _Message) = await SharedUtilities.PutFromAPI(new EMS.Security.Transfer.SystemUser.UpdateUsername
                            {
                                ID = employee.SystemUserID,
                                Username = employee.Code
                            }, _URL);

                        }
                        *//* Employment Status *//*
                        else if (EmployeeMovement.EmployeeField
                           .Equals(EMS.Plantilla.Transfer.Enums.MOVEMENT_EMP_FIELD.EMPLOYMENT_STATUS.ToString()))
                        {
                            if (EmployeeMovement.NewValueID
                                .Equals(EMS.Plantilla.Transfer.Enums.EMPLOYMENT_STATUS.RESIGNED.ToString()))

                            {
                                var _URL = string.Concat(_workflowBaseURL,
                                          _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BatchAccountabilityAdd").Value, "?",
                                          "userid=", _globalCurrentUser.UserID);

                                var (_IsSuccess, _Message) =
                                    await SharedUtilities.PostFromAPI(new EMS.Workflow.Transfer.Accountability.BatchAccountabilityAddInput
                                    {
                                        EmployeeID = EmployeeMovement.EmployeeID,
                                        Status = EMS.Workflow.Transfer.Enums.AccountabilityStatus.FOR_CLEARANCE
                                    }, _URL);
                            }

                        }
                    }
                }*/
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostUpdateDateTo()
        {
            var (IsSuccess, Message) = await new Common_EmployeeMovement(_iconfiguration, _globalCurrentUser, _env).UpdateDateTo(EmployeeMovement);
            _resultView.Result = Message;
            _resultView.IsSuccess = IsSuccess;
            return new JsonResult(_resultView);
        }
    }
}