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

namespace EMS.FrontEnd.Pages.Plantilla.Employee
{
    public class ViewModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }

        public ViewModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public virtual async Task OnGetAsync(int ID)
        {

            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/DELETE")).Count() > 0 ? "true" : "false";
                ViewData["HasEditFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/EDIT")).Count() > 0 ? "true" : "false";
                ViewData["HasDataPrivacyFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=DATA_PRIVACY")).Count() > 0 ? "true" : "false";
                ViewData["HasMovementViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/MOVEMENT")).Count() > 0 ? "true" : "false";
                ViewData["HasConfidentialViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/VIEW?HANDLER=CONFIDENTIAL_VIEW")).Count() > 0 ? "true" : "false";
                ViewData["HasSkillsViewFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/SKILLS/VIEW")).Count() > 0 ? "true" : "false";
                ViewData["HasTrainingView"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/PLANTILLA/EMPLOYEE/TRAINING")).Count() > 0 ? "true" : "false";
            }

            Employee = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEmployee(ID);
            
            /* Disable movement fields if employee is not new has a new employee ID.*/
            ViewData["DisableMovementFields"] = !string.IsNullOrEmpty(Employee.Code) ? "true" : "false";
            

            //ViewData["OrgGroupSelectList"] = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
            if (Employee.OrgGroupID != 0)
            {
                var orgGroupList = await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();
                var orgGroup = orgGroupList.Where(x => x.Value.Equals(Employee.OrgGroupID.ToString())).First();

                ViewData["OrgGroupSelectList"] = orgGroupList.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value.Equals(Employee.OrgGroupID.ToString())
                }).ToList();

                ViewData["Movement_OrgGroup"] = orgGroup.Text;

            }
            //ViewData["PositionSelectList"] = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);
            if (Employee.PositionID != 0)
            {
                var positionList = await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown(0);
                var position = positionList.Where(x => x.Value.Equals(Employee.PositionID.ToString())).First();

                ViewData["PositionSelectList"] = positionList.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = x.Value.Equals(Employee.PositionID.ToString())
                }).ToList();

                ViewData["Movement_Position"] = position.Text;

            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCRegionCode))
            {
                var region = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetRegionValueByCode(Employee.PersonalInformation.PSGCRegionCode);
                ViewData["PSGCRegionSelectList"] =
                    new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = region.Code,
                            Text = region.Description,
                            Selected = true
                        }
                    };
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCProvinceCode))
            {
                var province = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetProvinceValueByCode(Employee.PersonalInformation.PSGCProvinceCode);
                ViewData["PSGCProvinceSelectList"] =
                    new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = province.Code,
                            Text = province.Description,
                            Selected = true
                        }
                    };
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCCityMunicipalityCode))
            {
                var cityMun = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetCityMunicipalityValueByCode(Employee.PersonalInformation.PSGCCityMunicipalityCode);
                ViewData["PSGCCityMunicipalitySelectList"] =
                    new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = cityMun.Code,
                            Text = cityMun.Description,
                            Selected = true
                        }
                    };
            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.PSGCBarangayCode))
            {
                var barangay = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env)
                .GetBarangayValueByCode(Employee.PersonalInformation.PSGCBarangayCode);
                ViewData["PSGCBarangaySelectList"] =
                    new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = barangay.Code,
                            Text = barangay.Description,
                            Selected = true
                        }
                    };
            }

            //var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
            //        EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString());

            //ViewData["StatusSelectList"] = status.Select(
            //    x => new SelectListItem
            //    {
            //        Value = x.Value,
            //        Text = x.Description,
            //        Selected = x.Value == Employee.EmploymentStatus
            //    }).ToList();

            var status = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                    EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString(), Employee.EmploymentStatus);
            ViewData["StatusSelectList"] =
                new List<SelectListItem> {
                        new SelectListItem
                        {
                            Value = status.Value,
                            Text = status.Description,
                            Selected = true
                        }
                  };

            ViewData["Movement_Status"] = status.Description;


            //var companyTag = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCode(
            //        EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString());

            //ViewData["CompanyTagSelectList"] = companyTag.Select(
            //    x => new SelectListItem
            //    {
            //        Value = x.Value,
            //        Text = x.Description
            //    }).ToList();

            if (!string.IsNullOrEmpty(Employee.CompanyTag))
            {
                var companyTag = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                          EMS.Plantilla.Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString(), Employee.CompanyTag);
                ViewData["CompanyTagSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = companyTag.Value, Text = companyTag.Description, Selected = true } };
                
                ViewData["Movement_CompanyTag"] = companyTag.Description;

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.NationalityCode))
            {
                var nationality = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_NATIONALITY.ToString(), Employee.PersonalInformation.NationalityCode);
                ViewData["NationalitySelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = nationality.Value, Text = nationality.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.CitizenshipCode))
            {
                var citizenship = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString(), Employee.PersonalInformation.CitizenshipCode);
                ViewData["CitizenshipSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = citizenship.Value, Text = citizenship.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.CivilStatusCode))
            {
                var civilStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString(), Employee.PersonalInformation.CivilStatusCode);
                ViewData["CivilStatusSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = civilStatus.Value, Text = civilStatus.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.ReligionCode))
            {
                var religion = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_RELIGION.ToString(), Employee.PersonalInformation.ReligionCode);
                ViewData["ReligionSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = religion.Value, Text = religion.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.SSSStatusCode))
            {
                var sssStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_SSS_STAT.ToString(), Employee.PersonalInformation.SSSStatusCode);
                ViewData["SSSStatusSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = sssStatus.Value, Text = sssStatus.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.ExemptionStatusCode))
            {
                var exemptionStatus = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString(), Employee.PersonalInformation.ExemptionStatusCode);
                ViewData["ExemptionStatusSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = exemptionStatus.Value, Text = exemptionStatus.Description, Selected = true } };

            }
            if (!string.IsNullOrEmpty(Employee.PersonalInformation.Gender))
            {
                var gender = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env).GetReferenceValueByRefCodeAndValue(
                           EMS.Plantilla.Transfer.Enums.ReferenceCodes.EMP_GENDER.ToString(), Employee.PersonalInformation.Gender);
                ViewData["GenderSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = gender.Value, Text = gender.Description, Selected = true } };

            }

            if (!string.IsNullOrEmpty(Employee.PersonalInformation.ContactPersonRelationship))
            {
                var contactPersonRelationship = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                            .GetReferenceValueByRefCodeAndValue(EMS.Plantilla.Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString(), Employee.PersonalInformation.ContactPersonRelationship);
                ViewData["ContactPersonRelationshipSelectList"] =
                    new List<SelectListItem> { new SelectListItem { Value = contactPersonRelationship.Value, Text = contactPersonRelationship.Description, Selected = true } }; 
            }
        }

        //ORIG
        public async Task<JsonResult> OnGetEmployeeRoving(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetRovingByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetSystemUserName(int ID)
        {

            _resultView.Result = await new Common_SystemUser(_iconfiguration, _globalCurrentUser, _env).GetSystemUserByID(ID);
            _resultView.IsSuccess = true;

            return new JsonResult(_resultView);
        }
    }
}