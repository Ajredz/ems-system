using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Plantilla.Transfer.Employee;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class EditModel : EMS.FrontEnd.SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }
        
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.EmployeeCompensationForm EmployeeCompensation { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.Workflow.AddWorkflowTransaction Workflow { get; set; }

        [BindProperty]
        public EMS.Workflow.Transfer.LogActivity.UpdateEmployeeLogActivityAssignedUserForm AssignedUser { get; set; }


        public EditModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasDataPrivacyFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=DATA_PRIVACY")).Count() > 0 ? "true" : "false";
                ViewData["HasMovementViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/MOVEMENT")).Count() > 0 ? "true" : "false";
                ViewData["HasConfidentialViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0 ? "true" : "false";
                ViewData["HasConfidentialEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasSkillsViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/SKILLS/VIEW")).Count() > 0 ? "true" : "false";
            }

            Employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(ID);

            /* Disable movement fields if employee is not new has a new employee ID.*/
            ViewData["DisableMovementFields"] = !string.IsNullOrEmpty(Employee.Code) ? "true" : "false";
            var orgGroupList = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
            var positionList = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);
            ViewData["OrgGroupSelectList"] = orgGroupList;
            ViewData["PositionSelectList"] = positionList;

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCRegionCode))
            {
                var regionList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetRegionDropdown(Employee.PersonalInformation.PSGCRegionCode);

                ViewData["PSGCRegionSelectList"] = regionList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value == Employee.PersonalInformation.PSGCRegionCode
                }).ToList();
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCProvinceCode))
            {
                var provinceList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetProvinceDropdownByRegion(Employee.PersonalInformation.PSGCRegionCode);

                ViewData["PSGCProvinceSelectList"] = provinceList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value == Employee.PersonalInformation.PSGCProvinceCode
                }).ToList();
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCCityMunicipalityCode))
            {
                var cityMunicipalityList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetCityMunicipalityDropdownByProvince(Employee.PersonalInformation.PSGCProvinceCode);

                ViewData["PSGCCityMunicipalitySelectList"] = cityMunicipalityList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value == Employee.PersonalInformation.PSGCCityMunicipalityCode
                }).ToList();
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCBarangayCode))
            {
                var barangayList = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetBarangayDropdownByCityMunicipality(Employee.PersonalInformation.PSGCCityMunicipalityCode);

                ViewData["PSGCBarangaySelectList"] = barangayList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value == Employee.PersonalInformation.PSGCBarangayCode
                }).ToList();
            }

            var statusList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString());

            ViewData["StatusSelectList"] = statusList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description,
                    Selected = x.Value == Employee.EmploymentStatus
                }).ToList();

            var companyTagList = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString());

            ViewData["CompanyTagSelectList"] = companyTagList.Select(
                x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Description
                }).ToList();

            if (Employee.OrgGroupID != 0)
            {
                var orgGroup = orgGroupList.Where(x => x.Value.Equals(Employee.OrgGroupID.ToString())).FirstOrDefault();
                ViewData["Movement_OrgGroup"] = orgGroup == null ? "" : orgGroup.Text;
            }

            if (Employee.PositionID != 0)
            {
                var position = positionList.Where(x => x.Value.Equals(Employee.PositionID.ToString())).FirstOrDefault();
                ViewData["Movement_Position"] = position == null ? "" : position.Text;
            }

            if (!string.IsNullOrEmpty(Employee.EmploymentStatus))
            {
                var status = statusList.Where(x => x.Value.Equals(Employee.EmploymentStatus)).FirstOrDefault();
                ViewData["Movement_Status"] = status.Description;
            }

            if (!string.IsNullOrEmpty(Employee.CompanyTag))
            {
                var companyTag = companyTagList.Where(x => x.Value.Equals(Employee.CompanyTag)).FirstOrDefault();
                ViewData["Movement_CompanyTag"] = companyTag == null ? "" : companyTag.Description;
            }

            var nationality = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_NATIONALITY.ToString());
            ViewData["NationalitySelectList"] =
                nationality.Select(x => new SelectListItem { Value = x.Value, Text = x.Description }).ToList();

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

        public async Task<JsonResult> OnPostAsync()
        {
            Employee.CreatedBy = _globalCurrentUser.UserID;
            Employee.IsDataPrivacy = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=DATA_PRIVACY")).Count() > 0;

            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("Edit").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            var oldValue = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(Employee.ID);

            if (Employee.OnboardingWorkflowID == 0)
            {
                var LastStep = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                    .GetLastStepByWorkflowCode("ONBOARDING");
                Employee.OnboardingWorkflowID = LastStep.WorkflowID;
            }

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(Employee, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsSuccess)
            {

                if (string.IsNullOrEmpty(oldValue.Code))
                {
                    Employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(Employee.ID);

                    var (SystemUserInfo, IsSystemUserSuccess, SystemUserMessage) = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                        .AddSystemUser(new EMS.Security.Transfer.SystemUser.GetByNameInput
                        {
                            EmployeeCode = Employee.Code,
                            FirstName = Employee.PersonalInformation.FirstName,
                            MiddleName = Employee.PersonalInformation.MiddleName,
                            LastName = Employee.PersonalInformation.LastName,
                            CreatedBy = _globalCurrentUser.UserID
                        });

                    if (IsSystemUserSuccess)
                    {
                        await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                            .UpdateSystemUser(new EMS.Plantilla.Transfer.Employee.UpdateSystemUserInput
                            {
                                EmployeeID = Employee.ID,
                                SystemUserID = SystemUserInfo.ID
                            });
                    }
                }

                StringBuilder Remarks = new StringBuilder();

                if (Employee.Code != oldValue.Code)
                    Remarks.Append(string.Concat("Code changed from ", oldValue.Code, " to ", Employee.Code, ". "));
                if (Employee.PersonalInformation.FirstName != oldValue.PersonalInformation.FirstName)
                    Remarks.Append(string.Concat("First Name changed from ", oldValue.PersonalInformation.FirstName, " to ", Employee.PersonalInformation.FirstName, ". "));
                if (Employee.PersonalInformation.MiddleName != oldValue.PersonalInformation.MiddleName)
                    Remarks.Append(string.Concat("Middle Name changed from ", oldValue.PersonalInformation.MiddleName, " to ", Employee.PersonalInformation.MiddleName, ". "));
                if (Employee.PersonalInformation.LastName != oldValue.PersonalInformation.LastName)
                    Remarks.Append(string.Concat("Last Name changed from ", oldValue.PersonalInformation.LastName, " to ", Employee.PersonalInformation.LastName, ". "));
                if (Employee.PersonalInformation.Suffix != oldValue.PersonalInformation.Suffix)
                    Remarks.Append(string.Concat("Suffix changed from ", oldValue.PersonalInformation.Suffix, " to ", Employee.PersonalInformation.Suffix, ". "));
                if (Employee.PersonalInformation.Nickname != oldValue.PersonalInformation.Nickname)
                    Remarks.Append(string.Concat("Nickname changed from ", oldValue.PersonalInformation.Nickname, " to ", Employee.PersonalInformation.Nickname, ". "));
                if (Employee.OrgGroupID != oldValue.OrgGroupID)
                    Remarks.Append(string.Concat("OrgGroupID changed from ", oldValue.OrgGroupID, " to ", Employee.OrgGroupID, ". "));
                if (Employee.PositionID != oldValue.PositionID)
                    Remarks.Append(string.Concat("PositionID changed from ", oldValue.OrgGroupID, " to ", Employee.OrgGroupID, ". "));
                if (Employee.SystemUserID != oldValue.SystemUserID)
                    Remarks.Append(string.Concat("SystemUserID changed from ", oldValue.SystemUserID, " to ", Employee.SystemUserID, ". "));
                if (Employee.OnboardingWorkflowID != oldValue.OnboardingWorkflowID)
                    Remarks.Append(string.Concat("OnboardingWorkflowID changed from ", oldValue.OnboardingWorkflowID, " to ", Employee.OnboardingWorkflowID, ". "));
                if (Employee.DateHired != oldValue.DateHired)
                    Remarks.Append(string.Concat("Date Hired changed from ", oldValue.DateHired, " to ", Employee.DateHired, ". "));
                if (Employee.EmploymentStatus != oldValue.EmploymentStatus)
                    Remarks.Append(string.Concat("Employment Status changed from ", oldValue.EmploymentStatus, " to ", Employee.EmploymentStatus, ". "));
                if (Employee.PersonalInformation.BirthDate != oldValue.PersonalInformation.BirthDate)
                    Remarks.Append(string.Concat("Birth Date changed from ", oldValue.PersonalInformation.BirthDate, " to ", Employee.PersonalInformation.BirthDate, ". "));
                if (Employee.PersonalInformation.AddressLine1 != oldValue.PersonalInformation.AddressLine1)
                    Remarks.Append(string.Concat("AddressLine1 changed from ", oldValue.PersonalInformation.AddressLine1, " to ", Employee.PersonalInformation.AddressLine1, ". "));
                if (Employee.PersonalInformation.AddressLine2 != oldValue.PersonalInformation.AddressLine2)
                    Remarks.Append(string.Concat("AddressLine2 changed from ", oldValue.PersonalInformation.AddressLine2, " to ", Employee.PersonalInformation.AddressLine2, ". "));
                //if (Employee.PersonalInformation.PSGCRegion != oldValue.PersonalInformation.PSGCRegion)
                //    Remarks.Append(string.Concat("PSGCRegion changed from ", oldValue.PersonalInformation.PSGCRegion, " to ", Employee.PersonalInformation.PSGCRegion, ". "));
                //if (Employee.PersonalInformation.PSGCCity != oldValue.PersonalInformation.PSGCCity)
                //    Remarks.Append(string.Concat("PSGCCity changed from ", oldValue.PersonalInformation.PSGCCity, " to ", Employee.PersonalInformation.PSGCCity, ". "));
                if (Employee.PersonalInformation.Email != oldValue.PersonalInformation.Email)
                    Remarks.Append(string.Concat("Email changed from ", oldValue.PersonalInformation.Email, " to ", Employee.PersonalInformation.Email, ". "));
                if (Employee.PersonalInformation.CellphoneNumber != oldValue.PersonalInformation.CellphoneNumber)
                    Remarks.Append(string.Concat("Cellphone Number changed from ", oldValue.PersonalInformation.CellphoneNumber, " to ", Employee.PersonalInformation.CellphoneNumber, ". "));
                if (Employee.PersonalInformation.SSSNumber != oldValue.PersonalInformation.SSSNumber)
                    Remarks.Append(string.Concat("SSS Number changed from ", oldValue.PersonalInformation.SSSNumber, " to ", Employee.PersonalInformation.SSSNumber, ". "));
                if (Employee.PersonalInformation.TIN != oldValue.PersonalInformation.TIN)
                    Remarks.Append(string.Concat("TIN changed from ", oldValue.PersonalInformation.TIN, " to ", Employee.PersonalInformation.TIN, ". "));
                if (Employee.PersonalInformation.PhilhealthNumber != oldValue.PersonalInformation.PhilhealthNumber)
                    Remarks.Append(string.Concat("Philhealth Number changed from ", oldValue.PersonalInformation.PhilhealthNumber, " to ", Employee.PersonalInformation.PhilhealthNumber, ". "));
                if (Employee.PersonalInformation.PagibigNumber != oldValue.PersonalInformation.PagibigNumber)
                    Remarks.Append(string.Concat("Pagibig Number changed from ", oldValue.PersonalInformation.PagibigNumber, " to ", Employee.PersonalInformation.PagibigNumber, ". "));
                if (Employee.PersonalInformation.Gender != oldValue.PersonalInformation.Gender)
                    Remarks.Append(string.Concat("Gender changed from ", oldValue.PersonalInformation.Gender, " to ", Employee.PersonalInformation.Gender, ". "));
                if (Employee.PersonalInformation.NationalityCode != oldValue.PersonalInformation.NationalityCode)
                    Remarks.Append(string.Concat("Nationality Code from ", oldValue.PersonalInformation.NationalityCode, " to ", Employee.PersonalInformation.NationalityCode, ". "));
                if (Employee.PersonalInformation.CitizenshipCode != oldValue.PersonalInformation.CitizenshipCode)
                    Remarks.Append(string.Concat("Citizenship Code from ", oldValue.PersonalInformation.CitizenshipCode, " to ", Employee.PersonalInformation.CitizenshipCode, ". "));
                if (Employee.PersonalInformation.BirthPlace != oldValue.PersonalInformation.BirthPlace)
                    Remarks.Append(string.Concat("Birth Place from ", oldValue.PersonalInformation.BirthPlace, " to ", Employee.PersonalInformation.BirthPlace, ". "));
                if (Employee.PersonalInformation.HeightCM != oldValue.PersonalInformation.HeightCM)
                    Remarks.Append(string.Concat("Height CM from ", oldValue.PersonalInformation.HeightCM, " to ", Employee.PersonalInformation.HeightCM, ". "));
                if (Employee.PersonalInformation.WeightLBS != oldValue.PersonalInformation.WeightLBS)
                    Remarks.Append(string.Concat("Weight LBS from ", oldValue.PersonalInformation.WeightLBS, " to ", Employee.PersonalInformation.WeightLBS, ". "));
                if (Employee.PersonalInformation.SSSStatusCode != oldValue.PersonalInformation.SSSStatusCode)
                    Remarks.Append(string.Concat("SSS Status Code from ", oldValue.PersonalInformation.SSSStatusCode, " to ", Employee.PersonalInformation.SSSStatusCode, ". "));
                if (Employee.PersonalInformation.ExemptionStatusCode != oldValue.PersonalInformation.ExemptionStatusCode)
                    Remarks.Append(string.Concat("Exemption Status Code from ", oldValue.PersonalInformation.ExemptionStatusCode, " to ", Employee.PersonalInformation.ExemptionStatusCode, ". "));
                if (Employee.PersonalInformation.CivilStatusCode != oldValue.PersonalInformation.CivilStatusCode)
                    Remarks.Append(string.Concat("Civil Status Code from ", oldValue.PersonalInformation.CivilStatusCode, " to ", Employee.PersonalInformation.CivilStatusCode, ". "));
                if (Employee.PersonalInformation.ReligionCode != oldValue.PersonalInformation.ReligionCode)
                    Remarks.Append(string.Concat("Religion Code from ", oldValue.PersonalInformation.ReligionCode, " to ", Employee.PersonalInformation.ReligionCode, ". "));
                
                if (Remarks.Length > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "Employee",
                            TableID = Employee.ID,
                            Remarks = Remarks.ToString(),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }

                if (Employee.EmployeeFamilyList != null && Employee.EmployeeFamilyList.Count() > 0)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "EmployeeFamily",
                            TableID = Employee.ID,
                            Remarks = string.Concat(Employee.Code, " Employee Family updated"),
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
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "EmployeeWorkingHistory",
                            TableID = Employee.ID,
                            Remarks = string.Concat(Employee.Code, " Employee Working History updated"),
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
                            EventType = Common_AuditLog.EventType.EDIT.ToString(),
                            TableName = "EmployeeRoving",
                            TableID = Employee.ID,
                            Remarks = string.Concat(Employee.Code, " Employee Roving updated"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveOnboardingWorkflow()
        {
            var (CurrentWorkflowStep, IsSuccess, Message) = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .AddTransaction(Workflow);

            if (IsSuccess)
            {
                List<int> roles =
                (await new Common_SystemRole(_iconfiguration, _globalCurrentUser, _env).GetSystemRoleByUserID())
                .Select(x => x.RoleID).ToList();

                await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .UpdateOnboardingCurrentWorkflowStep(new UpdateOnboardingCurrentWorkflowStepInput
                    {
                        EmployeeID = Workflow.RecordID,
                        CurrentStepCode = CurrentWorkflowStep.StepCode,
                        CurrentStepDescription = CurrentWorkflowStep.StepDescription,
                        CurrentStepApproverRoleIDs = CurrentWorkflowStep.ApproverRoleIDs,
                        WorkflowStatus = CurrentWorkflowStep.WorkflowStatus,
                        DateScheduled = Workflow.DateScheduled,
                        DateCompleted = Workflow.DateCompleted,
                        Remarks = Workflow.Remarks
                    });

                /*Add AuditLog*/
                await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                    .AddAuditLog(new Security.Transfer.AuditLog.Form
                    {
                        EventType = Common_AuditLog.EventType.ADD.ToString(),
                        TableName = "EmployeeOnboarding",
                        TableID = Workflow.RecordID,
                        Remarks = string.Concat(Workflow.RecordID, " Employee Onboarding updated"),
                        IsSuccess = true,
                        CreatedBy = _globalCurrentUser.UserID
                    });
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = IsSuccess ? MessageUtilities.SCSSMSG_REC_UPDATE : Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkflowStep(string WorkflowCode, string StepCode)
        {
            var result = await new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                .GetWorkflowStepByWorkflowIDAndCode(new GetWorkflowStepByWorkflowIDAndCodeInput
                {
                    WorkflowCode = WorkflowCode,
                    Code = StepCode
                });
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            var result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _workflowBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            _resultView.Result = result.Select(x => new SelectListItem()
            {
                Value = x.Value,
                Text = x.Description
            });
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostUpdateAssignedUser()
        {
            AssignedUser.ModifiedBy = _globalCurrentUser.UserID;

            if (AssignedUser.IsAssignToSelf)
            {
                AssignedUser.AssignedUserID = _globalCurrentUser.UserID;
            }

            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("UpdateEmployeeLogActivityAssignedUser").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(AssignedUser, URL);

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostSaveEmployeeCompensation()
        {

            /*Check if has Access*/
            if (_systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_EDIT")).Count() > 0)
            {
                var URL = string.Concat(_plantillaBaseURL,
                                      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UpdateCompensation").Value, "?",
                                      "userid=", _globalCurrentUser.UserID);

                var oldValue = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(EmployeeCompensation.EmployeeID);

                var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(EmployeeCompensation, URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Result = Message;

                if (IsSuccess)
                {  
                    StringBuilder Remarks = new StringBuilder();

                    if (EmployeeCompensation.MonthlySalary != oldValue.EmployeeCompensation.MonthlySalary)
                        Remarks.Append(string.Concat("Monthly Salary from ", oldValue.EmployeeCompensation.MonthlySalary, " to ", EmployeeCompensation.MonthlySalary, ". "));
                    if (EmployeeCompensation.DailySalary != oldValue.EmployeeCompensation.DailySalary)
                        Remarks.Append(string.Concat("Daily Salary from ", oldValue.EmployeeCompensation.DailySalary, " to ", EmployeeCompensation.DailySalary, ". "));
                    if (EmployeeCompensation.HourlySalary != oldValue.EmployeeCompensation.HourlySalary)
                        Remarks.Append(string.Concat("Hourly Salary from ", oldValue.EmployeeCompensation.HourlySalary, " to ", EmployeeCompensation.HourlySalary, ". "));

                    if (Remarks.Length > 0)
                    {
                        /*Add AuditLog*/
                        await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                            .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                            {
                                EventType = Common_AuditLog.EventType.EDIT.ToString(),
                                TableName = "EmployeeCompensation",
                                TableID = oldValue.ID,
                                Remarks = Remarks.ToString(),
                                IsSuccess = true,
                                CreatedBy = _globalCurrentUser.UserID
                            });
                    }
                }
            }
            else
            {
                _resultView.IsSuccess = false;
                _resultView.Result = MessageUtilities.ERRMSG_NOACCESS;
            }

            return new JsonResult(_resultView);
        }

    }
}