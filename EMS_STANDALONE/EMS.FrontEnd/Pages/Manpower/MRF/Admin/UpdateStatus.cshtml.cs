using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using EMS.FrontEnd.SharedClasses.Common_Recruitment;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.Plantilla.Transfer.Employee;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.Pages.Manpower.MRF.Admin
{
    public class UpdateStatusModel : SharedClasses.Utilities
    {

        [BindProperty]
        public EMS.Manpower.Transfer.MRF.UpdateStatusInput MRFStatus { get; set; }
        
        [BindProperty]
        public UpdateMRFTransactionIDForm Applicants { get; set; }

        public UpdateStatusModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task<JsonResult> OnPostAsync(bool IsExist, int EmployeeID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                   _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("UpdateStatus").Value, "?",
                   "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(MRFStatus, URL);

            if (IsSuccess)
            {
                await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).UpdateMRFTransactionID(Applicants);
            }

            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            if (IsExist)
            {
                await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                        .UpdateEmployeeID(new EMS.Recruitment.Transfer.Applicant.UpdateEmployeeIDInput
                        {
                            ApplicantID = Applicants.HiredApplicantID,
                            EmployeeID = EmployeeID
                        });
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetConvertApplicant(int ApplicantID, int OrgGroupID, int PositionID, DateTime DateHired)
        {
            EMS.Recruitment.Transfer.Applicant.Form ApplicantInfo = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ApplicantID);

            if (ApplicantInfo != null)
            {
                var (EmployeeInfo, IsEmployeeSuccess, EmployeeMessage) = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                    .AutoAddEmployee(new EMS.Plantilla.Transfer.Employee.Form
                    {
                        OrgGroupID = OrgGroupID,
                        PositionID = PositionID,
                        DateHired = DateHired,
                        EmploymentStatus = "DRAFT",
                        PersonalInformation = new EMS.Plantilla.Transfer.Employee.PersonalInformation
                        {
                            FirstName = ApplicantInfo.PersonalInformation.FirstName,
                            LastName = ApplicantInfo.PersonalInformation.LastName,
                            MiddleName = ApplicantInfo.PersonalInformation.MiddleName,
                            BirthDate = ApplicantInfo.PersonalInformation.BirthDate,
                            AddressLine1 = ApplicantInfo.PersonalInformation.AddressLine1,
                            AddressLine2 = ApplicantInfo.PersonalInformation.AddressLine2,
                            PSGCRegionCode = ApplicantInfo.PersonalInformation.PSGCRegionCode,
                            PSGCProvinceCode = ApplicantInfo.PersonalInformation.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = ApplicantInfo.PersonalInformation.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = ApplicantInfo.PersonalInformation.PSGCBarangayCode,
                            Email = ApplicantInfo.PersonalInformation.Email,
                            CellphoneNumber = ApplicantInfo.PersonalInformation.CellphoneNumber
                        }
                    });

                if (IsEmployeeSuccess)
                {
                    await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env)
                        .UpdateEmployeeID(new EMS.Recruitment.Transfer.Applicant.UpdateEmployeeIDInput
                        {
                            ApplicantID = ApplicantInfo.ID,
                            EmployeeID = EmployeeInfo.ID
                        });
                }
                NewEmployee newEmployee = new NewEmployee() { EmployeeID = EmployeeInfo.ID, ApplicantID = ApplicantInfo.ID };
                _resultView.IsSuccess = true;
                _resultView.Result = newEmployee;
            }

            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetEmployeeIfExistAsync(int ApplicantID)
        {
            EMS.Recruitment.Transfer.Applicant.Form ApplicantInfo = await new Common_Applicant(_iconfiguration, _globalCurrentUser, _env).GetApplicant(ApplicantID);
            GetEmployeeIfExistInput param = new GetEmployeeIfExistInput()
            {
                FName = ApplicantInfo.PersonalInformation.FirstName,
                LName = ApplicantInfo.PersonalInformation.LastName,
                BDate = ApplicantInfo.PersonalInformation.BirthDate.ToString("MM/dd/yyyy")
            };
            var (Result, IsSuccess, Message) = await new Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                .GetEmployeeIfExist(param);

            var jsonResult = new
            {
                IsSuccess = true,
                IsExist = (Result.Count() > 0 ? true : false),
                EmployeeID = Result.Select(x => x.ID).FirstOrDefault()
            };

            return new JsonResult(jsonResult);
        }

    }
}