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
using EMS.Security.Transfer.SystemUser;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Plantilla.Transfer.Employee;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGet()
        {
            if (_globalCurrentUser != null)
            {
                Employee = new EMS.Plantilla.Transfer.Employee.Form();

                ViewData["HasDataPrivacyFeature"] = "true";
                ViewData["HasConfidentialViewFeature"] = "true";
                ViewData["HasSkillsViewFeature"] = "false";

                ViewData["OrgGroupSelectList"] = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();

                //ViewData["PositionSelectList"] = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);

                var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString());

                ViewData["StatusSelectList"] = status.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                ViewData["PSGCRegionSelectList"] = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetRegionDropdown();

                var companyTag = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString());

                ViewData["CompanyTagSelectList"] = companyTag.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Description
                    }).ToList();

                var nationality = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_NATIONALITY.ToString());
                ViewData["NationalitySelectList"] = 
                    nationality.Select( x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var citizenship = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString());
                ViewData["CitizenshipSelectList"] =
                    citizenship.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var civilStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString());
                ViewData["CivilStatusSelectList"] =
                    civilStatus.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var religion = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_RELIGION.ToString());
                ViewData["ReligionSelectList"] =
                    religion.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var schoolLevel = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_SCHOOL_LEVEL.ToString());
                ViewData["SchoolLevelSelectList"] =
                    schoolLevel.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var educationalAttainmentDegree = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_ED_ATT_DEG.ToString());
                ViewData["EducationalAttainmentDegreeSelectList"] =
                    educationalAttainmentDegree.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var educationalAttainmentStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_ED_ATT_STAT.ToString());
                ViewData["EducationalAttainmentStatusSelectList"] =
                    educationalAttainmentStatus.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var sssStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_SSS_STAT.ToString());
                ViewData["SSSStatusSelectList"] =
                    sssStatus.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var exemptionStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString());
                ViewData["ExemptionStatusSelectList"] =
                    exemptionStatus.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var gender = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_GENDER.ToString());
                ViewData["GenderSelectList"] =
                    gender.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();
                
                var contactPersonRelationship = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                    .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString());
                ViewData["ContactPersonRelationshipSelectList"] =
                    contactPersonRelationship.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

                var dailyWageDivisor = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                   EMS.Plantilla.Transfer.Enums.ReferenceCodes.WAGE_DAILY_DIVISOR.ToString(), "DAILY_DIVISOR");

                ViewData["DailyWageDivisor"] = dailyWageDivisor.Description;

                var hourlyWageDivisor = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                       EMS.Plantilla.Transfer.Enums.ReferenceCodes.WAGE_HOURLY_DIVISOR.ToString(), "HOURLY_DIVISOR");

                ViewData["HourlyWageDivisor"] = hourlyWageDivisor.Description;
            }
        }

        public async Task<JsonResult> OnPostAsync()
        {
            Employee.CreatedBy = _globalCurrentUser.UserID;

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("Add").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            if (Employee.OnboardingWorkflowID == 0)
            {
                var LastStep = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetLastStepByWorkflowCode("ONBOARDING");
                Employee.OnboardingWorkflowID = LastStep.WorkflowID;
            }

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(Employee, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {
                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "Employee",
                        TableID = 0, // New Record, no ID yet
                        Remarks = string.Concat(Employee.Code, " added"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });

                //Message.Substring(Message.Length - 6);

                List<EmployeeUploadInsertInput> employeeUploadInsertInputs = new List<EmployeeUploadInsertInput>() {
                    new EmployeeUploadInsertInput(){
                        NewEmployeeCode = Message.Substring(Message.Length - 6).ToString(), //GET USERNAME CODE
                        FirstName = Employee.PersonalInformation.FirstName,
                        MiddleName = Employee.PersonalInformation.MiddleName,
                        LastName = Employee.PersonalInformation.LastName,
                        CreatedBy = _globalCurrentUser.UserID
                    }
                };

                #region Create System Users for the newly uploaded employees
                var securityURL = string.Concat(_securityBaseURL,
                                 _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("EmployeeUploadInsert").Value, "?",
                                 "userid=", _globalCurrentUser.UserID);

                var (ResultSystemUsers, IsSuccess1, Message1) =
                    await SharedUtilities.PostFromAPI(new List<EMS.Security.Transfer.SystemUser.EmployeeUploadInsertOutput>(),
                    employeeUploadInsertInputs, securityURL);
                #endregion

                #region Update system user ID on Employee
                if (IsSuccess1)
                {
                    var UploadInsertUpdateSystemUser = string.Concat(_plantillaBaseURL,
                                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UploadInsertUpdateSystemUser").Value, "?",
                                     "userid=", _globalCurrentUser.UserID);

                    var (IsSuccess2, Message2) = await SharedUtilities.PostFromAPI(
                        ResultSystemUsers.Select(x => new UploadInsertUpdateSystemUserInput
                        {
                            SystemUserID = x.SystemUserID,
                            NewEmployeeCode = x.NewEmployeeCode
                        }), UploadInsertUpdateSystemUser);
                }
                #endregion


                if (Employee.EmployeeFamilyList != null && Employee.EmployeeFamilyList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "EmployeeFamily",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Employee.Code, " Employee Family added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (Employee.EmployeeWorkingHistoryList != null && Employee.EmployeeWorkingHistoryList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "EmployeeWorkingHistory",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Employee.Code, " Employee Working History added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (Employee.EmployeeRovingList != null && Employee.EmployeeRovingList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.ADD.ToString(),
                            TableName = "EmployeeRoving",
                            TableID = 0, // New Record, no ID yet
                            Remarks = string.Concat(Employee.Code, " Employee Roving added"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSystemUserAsync([FromQuery] GetByNameInput param)
        {
            var result = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).AddSystemUser(param);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }
    }
}