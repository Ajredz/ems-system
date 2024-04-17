using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.FrontEnd.Pages.EmployeeProfile.EmployeeDetails
{
    public class IndexModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.Plantilla.Transfer.Employee.Form Employee { get; set; }

        public IndexModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }


        public virtual async Task OnGetAsync()
        {
            var ID = _globalCurrentUser.EmployeeID;

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

            ViewData["HasEditFeature"] = (Employee.EmploymentStatus == "DRAFT" ? "true" : "false");
        }

        public async Task<JsonResult> OnGetProvinceDropDownByRegion(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetProvinceDropdownByRegion(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetCityMunicipalityDropDownByProvince(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetCityMunicipalityDropdownByProvince(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetBarangayDropDownByCityMunicipality(string Code)
        {
            var res = await new Common_Region_City(_iconfiguration, _globalCurrentUser, _env).GetBarangayDropdownByCityMunicipality(Code);

            _resultView.Result =
                res.Select(
                    x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetRelationshipDropDown()
        {
            var res = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(EMS.Plantilla.Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString());

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

        public async Task<JsonResult> OnGetFamilyByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetFamilyByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEducationByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetEducationByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetWorkingHistoryByEmployeeID(int EmployeeID)
        {
            var result = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env).GetWorkingHistoryByEmployeeID(EmployeeID);
            _resultView.IsSuccess = true;
            _resultView.Result = result;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmploymentStatusByEmployeeID(int EmployeeID)
        {
            List<GetEmploymentStatusOutput> statusHistory =
                   await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                   .GetEmploymentStatusByEmployeeID(EmployeeID);

            var jsonData = new
            {
                rows = statusHistory
            };
            return new JsonResult(jsonData);
        }
        public async Task<JsonResult> OnGetReferenceValue(string RefCode)
        {
            _resultView.Result = await new Common_Reference(_iconfiguration, _globalCurrentUser, _plantillaBaseURL, _env)
                .GetReferenceValueByRefCode(RefCode);
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }
        public async Task<JsonResult> OnPostAsync()
        {
            var (IsSucess, Message) = (await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .EditEmployeeDetails(Employee));
            _resultView.Result = Message;
            _resultView.IsSuccess = IsSucess;
            return new JsonResult(_resultView);
        }
    }
}
