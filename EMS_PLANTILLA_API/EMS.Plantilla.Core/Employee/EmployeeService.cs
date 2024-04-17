using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.Employee;
using EMS.Plantilla.Data.OrgGroup;
using EMS.Plantilla.Data.Position;
using EMS.Plantilla.Data.PSGC;
using EMS.Plantilla.Data.Reference;
using EMS.Plantilla.Transfer;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.Employee
{
    public interface IEmployeeService
    {
        Task<IActionResult> GetAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetByUserID(APICredentials credentials);

        Task<IActionResult> GetByPositionIDOrgGroupID(APICredentials credentials, GetByPositionIDOrgGroupIDInput param);

        Task<IActionResult> GetRovingByPositionIDOrgGroupID(APICredentials credentials, GetRovingByPositionIDOrgGroupIDInput param);

        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> GetRovingByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetByUserIDs(APICredentials credentials, List<int> UserIDs);

        Task<IActionResult> GetFamilyByEmployeeID(APICredentials credentials, int EmployeeID);
        
        Task<IActionResult> GetEducationByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> GetWorkingHistoryByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> GetEmploymentStatusByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> UpdateOnboardingCurrentWorkflowStep(APICredentials credentials, UpdateOnboardingCurrentWorkflowStepInput param);

        Task<IActionResult> GetEmployeeWithSystemUserAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetByCodes(APICredentials credentials, string CodesDelimited);

        Task<IActionResult> AutoAdd(APICredentials credentials, Form param);

        Task<IActionResult> UpdateSystemUser(APICredentials credentials, UpdateSystemUserInput param);

        Task<IActionResult> GetETFList(APICredentials credentials, GetListInput input);

        Task<IActionResult> UpdateCompensation(APICredentials credentials, EmployeeCompensationForm param);

        Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFile> param);

        Task<IActionResult> UploadInsertUpdateSystemUser(APICredentials credentials, List<UploadInsertUpdateSystemUserInput> param);

        Task<IActionResult> GetEmployeeByUsername(APICredentials credentials, string Username);

        Task<IActionResult> GetByIDs(APICredentials credentials, List<int> UserIDs);

        Task<IActionResult> GetPrintCOE(APICredentials credentials, int EmployeeID, int HREmployeeID);

        Task<IActionResult> GetOldEmployeeIDAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetByOldEmployeeIDs(APICredentials credentials, string CodesDelimited);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);
        
        Task<IActionResult> GetLastModifiedRoving(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeAttachmentForm param);

        Task<IActionResult> GetEmployeeAttachment(APICredentials credentials, int ID);
        Task<IActionResult> PostEmployeeSkills(APICredentials credentials, EmployeeSkillsForm param);
        Task<IActionResult> PutEmployeeSkills(APICredentials credentials, EmployeeSkillsForm param);
        Task<IActionResult> GetEmployeeSkillsById(APICredentials credentials, int Id);
        Task<IActionResult> GetEmployeeSkillsByEmployeeId(APICredentials credentials, EmployeeSkillsFormInput input);
        Task<IActionResult> GetExternalEmployeeDetails(APICredentials credentials, ExternalLogin param);
        Task<IActionResult> GetCorporateEmailList(APICredentials credentials, GetListCorporateEmailInput input);
        Task<IActionResult> UpdateEmployeeEmail(UpdateEmployeeEmailInput param);
        Task<IActionResult> GetEmployeeByOrgGroup(List<int> ID);
        Task<IActionResult> GetEmployeeByPosition(List<int> ID);
        Task<IActionResult> GetEmployeeByBirthday();
        Task<IActionResult> GetEmployeeEvaluation();

        Task<IActionResult> PostUpdateProfile(APICredentials credentials, Form param);
        Task<IActionResult> GetEmail(GetEmailInput param);
        Task<IActionResult> GetEmployeeIDDescendant(int EmployeeID);
        Task<IActionResult> PutConvertNewEmployee(APICredentials credentials, NewEmployeeForm param);
        Task<IActionResult> PutConvertNewEmployees(APICredentials credentials, List<NewEmployeeForm> param);
        Task<IActionResult> PutEmployeeDetails(APICredentials credentials, Form param);
        Task<IActionResult> PutDraftToProbationary(APICredentials credentials, List<int> IDs);
        Task<IActionResult> GetEmployeeLastEmploymentStatus(List<int> ID);
        Task<IActionResult> GetEmployeeLastEmploymentStatusByDate(GetEmployeeLastEmploymentStatusByDateInput param);
        Task<IActionResult> GetEmployeeByDateHired(GetEmployeeLastEmploymentStatusByDateInput param);
        Task<IActionResult> PostEmployeeReport(APICredentials credentials);
        Task<IActionResult> GetEmployeeReportByTDate(string TDate);
        Task<IActionResult> GetEmployeeReportOrgByTDate(string TDate);
        Task<IActionResult> GetEmployeeReportRegionByTDate(string TDate);
        Task<IActionResult> GetEmployeeIfExist(GetEmployeeIfExistInput param);
    }

    public class EmployeeService : Core.Shared.Utilities, IEmployeeService
    {
        private readonly IEmployeeDBAccess _dbAccess;
        private readonly IReferenceDBAccess _referenceDBAccess;
        private readonly IPSGCDBAccess _PSGCDBAccess;
        private readonly IPositionDBAccess _positionDBAccess;
        private readonly IOrgGroupDBAccess _orgGroupDBAccess;


        public EmployeeService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IEmployeeDBAccess dbAccess, IReferenceDBAccess referenceDBAccess, IPSGCDBAccess PSGCDBAccess,
            IPositionDBAccess positionDBAccess, IOrgGroupDBAccess orgGroupDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _referenceDBAccess = referenceDBAccess;
            _PSGCDBAccess = PSGCDBAccess;
            _positionDBAccess = positionDBAccess;
            _orgGroupDBAccess = orgGroupDBAccess;
        }

        public async Task<IActionResult> GetAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat("(", x.Code, ") ",x.LastName, ", ", x.FirstName, " ", x.MiddleName)
                })
            );
        }

        public async Task<IActionResult> GetByUserID(APICredentials credentials)
        {
            List<TableVarEmployeeGetByID> result = (await _dbAccess.GetByUserID(credentials.UserID)).ToList();

            return new OkObjectResult(result.Count > 0 ? new GetByIDOutput
            {
                ID = result.FirstOrDefault().ID,
                Code = result.FirstOrDefault().Code,
                FirstName = result.FirstOrDefault().FirstName,
                MiddleName = result.FirstOrDefault().MiddleName,
                LastName = result.FirstOrDefault().LastName,
                OrgGroupID = result.FirstOrDefault().OrgGroupID,
                OrgGroupCode = result.FirstOrDefault().OrgGroupCode,
                OrgGroupDescription = result.FirstOrDefault().OrgGroupDescription,
                OrgGroupConcatenated = result.FirstOrDefault().OrgGroupConcatenated,
                PositionID = result.FirstOrDefault().PositionID,
                PositionCode = result.FirstOrDefault().PositionCode,
                PositionTitle = result.FirstOrDefault().PositionTitle,
                PositionConcatenated = result.FirstOrDefault().PositionConcatenated,
                Company = result.FirstOrDefault().Company,
                RefValueCompanyTag = result.FirstOrDefault().RefValueCompanyTag,
                Email = result.FirstOrDefault().Email,
                CorporateEmail = result.FirstOrDefault().CorporateEmail,
                OfficeMobile = result.FirstOrDefault().OfficeMobile,
                OfficeLandline = result.FirstOrDefault().OfficeLandline
            } : new GetByIDOutput()
            );
        }

        public async Task<IActionResult> GetByPositionIDOrgGroupID(APICredentials credentials, GetByPositionIDOrgGroupIDInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByPositionIDOrgGroupID(param))
                .Select(x => new GetByPositionIDOrgGroupIDOutput
                {
                    EmployeeName = string.Concat(
                        x.LastName,
                    string.IsNullOrEmpty(x.FirstName) ? "" : string.Concat(", ", x.FirstName),
                    string.IsNullOrEmpty(x.MiddleName) ? "" : string.Concat(" ", x.MiddleName)
                    )
                }).ToList()
            );
        }

        public async Task<IActionResult> GetRovingByPositionIDOrgGroupID(APICredentials credentials, GetRovingByPositionIDOrgGroupIDInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetRovingByPositionIDOrgGroupID(param))
                .Select(x => new GetRovingByPositionIDOrgGroupIDOutput
                {
                    EmployeeName = string.Concat(
                        x.LastName,
                    string.IsNullOrEmpty(x.FirstName) ? "" : string.Concat(", ", x.FirstName),
                    string.IsNullOrEmpty(x.MiddleName) ? "" : string.Concat(" ", x.MiddleName)
                    )
                }).ToList()
            );
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployee> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Name = x.Name,
                OrgGroup = x.OrgGroup,
                Position = x.Position,
                EmploymentStatus = x.EmploymentStatus,
                DateStatus = x.DateStatus,
                MovementDate = x.MovementDate,
                CurrentStep = x.CurrentStep,
                DateScheduled = x.DateScheduled,
                DateCompleted = x.DateCompleted,
                Remarks = x.Remarks,
                DateHired = x.DateHired,
                BirthDate = x.BirthDate,
                OldEmployeeID = x.OldEmployeeID,
                HomeBranch = x.HomeBranch,
                Company = x.Company,
                Cluster = x.Cluster,
                Area = x.Area,
                Region = x.Region,
                Zone = x.Zone,
                Percent = x.Percent
            }).ToList());
        }

        // Orig
        //public async Task<IActionResult> GetRovingByEmployeeID(APICredentials credentials, int EmployeeID)
        //{

        //    return new OkObjectResult(
        //        (await _dbAccess.GetRovingByEmployeeID(EmployeeID))
                //.Select(x => new GetRovingByEmployeeIDOutput
        //        {

        //            EmployeeID = x.EmployeeID,
        //            OrgGroupID = x.OrgGroupID,
        //            PositionID = x.PositionID

        //        }).ToList());
        //}

        public async Task<IActionResult> GetRovingByEmployeeID(APICredentials credentials, int EmployeeID)
        {

         
            return new OkObjectResult((await _dbAccess.GetRovingByEmployeeID(EmployeeID))
               .Select(x => new GetRovingByEmployeeIDOutput
               {
                   EmployeeID = x.EmployeeID,
                   OrgGroupID = x.OrgGroupID,
                   PositionID = x.PositionID
               }).ToList());
        }
        
        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            Data.Employee.Employee result = await _dbAccess.GetByID(input.ID);
            Data.Employee.EmployeeCompensation compensation = await _dbAccess.GetCompensationByEmployeeID(input.ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                var region = 
                    await _orgGroupDBAccess.GetRegionByOrgGroupID(result.OrgGroupID);
                
                var jobClassCode =
                    await _positionDBAccess.GetByID(result.PositionID);

                var jobClassDescription = (await _referenceDBAccess
                    .GetByRefCodeValue(Enums.ReferenceCodes.JOB_CLASS.ToString(), jobClassCode.JobClassCode));

                var HomeBranch = await _orgGroupDBAccess.GetByID(result.HomeBranchID);

                return new OkObjectResult(
                  new Form
                  {
                      ID = result.ID,
                      Code = result.Code,
                      OldEmployeeID = result.OldEmployeeID,
                      OrgGroupID = result.OrgGroupID,
                      HomeBranchID = result.HomeBranchID,
                      HomeBranchDescription = HomeBranch != null? String.Concat(HomeBranch.Code," - ",HomeBranch.Description) : "",
                      Region = region.Region,
                      RegionCode = region.RegionCode,
                      JobClass = jobClassDescription != null ?
                        jobClassDescription.Count() > 0 ?
                        jobClassDescription.First().Description : "" : "",
                      PositionID = result.PositionID,
                      SystemUserID = result.SystemUserID,
                      DateHired = result.DateHired,
                      EmploymentStatus = result.EmploymentStatus,
                      CorporateEmail = result.CorporateEmail,
                      OfficeMobile = result.OfficeMobile,
                      OfficeLandline = result.OfficeLandline,
                      IsDisplayDirectory = result.IsDisplayDirectory,
                      CreatedBy = result.CreatedBy,
                      PersonalInformation = new PersonalInformation
                      {
                          FirstName = result.FirstName,
                          MiddleName = result.MiddleName,
                          LastName = result.LastName,
                          Suffix = result.Suffix,
                          Nickname = result.Nickname,
                          Gender = result.Gender,
                          NationalityCode = result.NationalityCode,
                          CitizenshipCode = result.CitizenshipCode,
                          BirthPlace = result.BirthPlace,
                          HeightCM = result.HeightCM,
                          WeightLBS = result.WeightLBS,
                          SSSStatusCode = result.SSSStatusCode,
                          ExemptionStatusCode = result.ExemptionStatusCode,
                          CivilStatusCode = result.CivilStatusCode,
                          ReligionCode = result.ReligionCode,
                          AddressLine1 = result.AddressLine1,
                          AddressLine2 = result.AddressLine2,
                          PSGCRegionCode = result.PSGCRegionCode,
                          PSGCProvinceCode = result.PSGCProvinceCode,
                          PSGCCityMunicipalityCode = result.PSGCCityMunicipalityCode,
                          PSGCBarangayCode = result.PSGCBarangayCode,
                          BirthDate = result.BirthDate,
                          Email = result.Email,
                          CellphoneNumber = result.CellphoneNumber,
                          SSSNumber = result.SSSNumber,
                          TIN = result.TIN,
                          PhilhealthNumber = result.PhilhealthNumber,
                          PagibigNumber = result.PagibigNumber,
                          ContactPersonName = result.ContactPersonName,
                          ContactPersonNumber = result.ContactPersonNumber,
                          ContactPersonAddress = result.ContactPersonAddress,
                          ContactPersonRelationship = result.ContactPersonRelationship
                      },
                      EmployeeCompensation = new Transfer.Employee.EmployeeCompensationForm
                      {
                          ID = compensation == null ? 0 : compensation.ID,
                          EmployeeID = result.ID,
                          MonthlySalary = compensation == null ? "0.00" : compensation.MonthlySalary + "",
                          DailySalary = compensation == null ? "0.00" : compensation.DailySalary + "",
                          HourlySalary = compensation == null ? "0.00" : compensation.HourlySalary + ""
                      },
                      OnboardingWorkflowID = result.OnboardingWorkflowID,
                      CompanyTag = result.CompanyTag
                  });
            }
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            var NewEmployeeCode = (await _dbAccess.GetNewEmployeeCode(param.CompanyTag)).ToList();
            if(NewEmployeeCode != null)
                param.Code = NewEmployeeCode.First().NewEmployeeCode;
            param.OldEmployeeID = (param.OldEmployeeID ?? "").Trim();
            param.EmploymentStatus = (param.EmploymentStatus ?? "").Trim();
            param.CompanyTag = (param.CompanyTag ?? "").Trim();
            param.PersonalInformation.FirstName = (param.PersonalInformation.FirstName ?? "").Trim();
            param.PersonalInformation.LastName = (param.PersonalInformation.LastName ?? "").Trim();
            param.PersonalInformation.MiddleName = (param.PersonalInformation.MiddleName ?? "").Trim();
            param.PersonalInformation.AddressLine1 = (param.PersonalInformation.AddressLine1 ?? "").Trim();
            param.PersonalInformation.AddressLine2 = (param.PersonalInformation.AddressLine2 ?? "").Trim();
            param.PersonalInformation.Email = (param.PersonalInformation.Email ?? "").Trim();
            param.PersonalInformation.CellphoneNumber = (param.PersonalInformation.CellphoneNumber ?? "").Trim();
            param.PersonalInformation.SSSNumber = (param.PersonalInformation.SSSNumber ?? "").Trim();
            param.PersonalInformation.TIN = (param.PersonalInformation.TIN ?? "").Trim();
            param.PersonalInformation.PagibigNumber = (param.PersonalInformation.PagibigNumber ?? "").Trim();
            param.PersonalInformation.PhilhealthNumber = (param.PersonalInformation.PhilhealthNumber ?? "").Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add(string.Concat("Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                if ((await _dbAccess.GetByCode(param.Code)).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
                else if (param.Code.Length > 50)
                {
                    ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                }
            }

            if (!string.IsNullOrEmpty(param.OldEmployeeID))
                if (param.OldEmployeeID.Length > 5)
                    ErrorMessages.Add(string.Concat("Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));

            if (!string.IsNullOrEmpty(param.CorporateEmail))
            {
                param.CorporateEmail = (param.CorporateEmail ?? "").Trim();
                if (param.CorporateEmail.Length > 255)
                    ErrorMessages.Add(string.Concat("Corporate Email", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.EmploymentStatus))
                ErrorMessages.Add(string.Concat("Employment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.EmploymentStatus.Length > 20)
                ErrorMessages.Add(string.Concat("Employment Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (string.IsNullOrEmpty(param.CompanyTag))
                ErrorMessages.Add(string.Concat("Company ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.CompanyTag.Length > 20)
                ErrorMessages.Add(string.Concat("Company", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.FirstName))
                ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.LastName))
                ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.LastName.Length > 50)
                ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.MiddleName))
                if (param.PersonalInformation.MiddleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.Suffix))
            {
                param.PersonalInformation.Suffix = param.PersonalInformation.Suffix.Trim();
                if (param.PersonalInformation.Suffix.Length > 10)
                    ErrorMessages.Add(string.Concat("Suffix", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));
            }

            if (!string.IsNullOrEmpty(param.PersonalInformation.Nickname))
            {
                param.PersonalInformation.Nickname = param.PersonalInformation.Nickname.Trim();
                if (param.PersonalInformation.Nickname.Length > 50)
                    ErrorMessages.Add(string.Concat("Nickname", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (string.IsNullOrEmpty(param.PersonalInformation.Gender))
                ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.Gender = param.PersonalInformation.Gender.Trim();
                if (param.PersonalInformation.Gender.Length > 2)
                    ErrorMessages.Add(string.Concat("Gender", MessageUtilities.COMPARE_NOT_EXCEED, "2 characters."));


                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_GENDER.ToString(), param.PersonalInformation.Gender);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
            }

            if (string.IsNullOrEmpty(param.PersonalInformation.BirthPlace))
                ErrorMessages.Add(string.Concat("Birth Place ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.BirthPlace = param.PersonalInformation.BirthPlace.Trim();
                if (param.PersonalInformation.BirthPlace.Length > 255)
                    ErrorMessages.Add(string.Concat("Birth Place", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            //if (string.IsNullOrEmpty(param.PersonalInformation.HeightCM))
            //    ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    param.PersonalInformation.HeightCM = param.PersonalInformation.HeightCM.Trim();
            //    if (param.PersonalInformation.HeightCM.Length > 50)
            //        ErrorMessages.Add(string.Concat("Height (in cm)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            //    if (!decimal.TryParse(param.PersonalInformation.HeightCM, out decimal height))
            //    {
            //        ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //    }
            //    else
            //    {
            //        if (height <= 0)
            //            ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //    }
            //}


            //if (string.IsNullOrEmpty(param.PersonalInformation.WeightLBS))
            //    ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    param.PersonalInformation.WeightLBS = param.PersonalInformation.WeightLBS.Trim();
            //    if (param.PersonalInformation.WeightLBS.Length > 50)
            //        ErrorMessages.Add(string.Concat("Weight (in lbs)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            //    if (!decimal.TryParse(param.PersonalInformation.WeightLBS, out decimal height))
            //    {
            //        ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //    }
            //    else
            //    {
            //        if (height <= 0)
            //            ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //    }
            //}

            if (string.IsNullOrEmpty(param.PersonalInformation.SSSStatusCode))
                ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.SSSStatusCode = param.PersonalInformation.SSSStatusCode.Trim();
                if (param.PersonalInformation.SSSStatusCode.Length > 50)
                    ErrorMessages.Add(string.Concat("SSS Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_SSS_STAT.ToString(), param.PersonalInformation.SSSStatusCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }

            if (string.IsNullOrEmpty(param.PersonalInformation.ExemptionStatusCode))
                ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode.Trim();
                if (param.PersonalInformation.ExemptionStatusCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Exemption Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString(), param.PersonalInformation.ExemptionStatusCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }

            if (string.IsNullOrEmpty(param.PersonalInformation.NationalityCode))
                ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.NationalityCode = param.PersonalInformation.NationalityCode.Trim();
                if (param.PersonalInformation.NationalityCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Nationality", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_NATIONALITY.ToString(), param.PersonalInformation.NationalityCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if(referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }

            if (string.IsNullOrEmpty(param.PersonalInformation.CitizenshipCode))
                ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.CitizenshipCode = param.PersonalInformation.CitizenshipCode.Trim();
                if (param.PersonalInformation.CitizenshipCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Citizenship", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString(), param.PersonalInformation.CitizenshipCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }

            if (string.IsNullOrEmpty(param.PersonalInformation.CivilStatusCode))
                ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.CivilStatusCode = param.PersonalInformation.CivilStatusCode.Trim();
                if (param.PersonalInformation.CivilStatusCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Civil Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString(), param.PersonalInformation.CivilStatusCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }

            if (string.IsNullOrEmpty(param.PersonalInformation.ReligionCode))
                ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.PersonalInformation.ReligionCode = param.PersonalInformation.ReligionCode.Trim();
                if (param.PersonalInformation.ReligionCode.Length > 50)
                    ErrorMessages.Add(string.Concat("Religion", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_RELIGION.ToString(), param.PersonalInformation.ReligionCode);
                if (referenceValue == null)
                {
                    ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (referenceValue.Count() == 0)
                        ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }

            }


            if (string.IsNullOrEmpty(param.PersonalInformation.AddressLine1))
                ErrorMessages.Add(string.Concat("Address Line 1 ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.AddressLine1.Length > 255)
                ErrorMessages.Add(string.Concat("Address Line 1", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.AddressLine2))
                if (param.PersonalInformation.AddressLine2.Length > 255)
                    ErrorMessages.Add(string.Concat("Address Line 2", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCRegionCode))
                ErrorMessages.Add(string.Concat("Region ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCProvinceCode))
                ErrorMessages.Add(string.Concat("Province ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCCityMunicipalityCode))
                ErrorMessages.Add(string.Concat("City/Municipality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCBarangayCode))
                ErrorMessages.Add(string.Concat("Barangay ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (string.IsNullOrEmpty(param.PersonalInformation.Email))
            //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            if (param.PersonalInformation.Email.Length > 255)
                ErrorMessages.Add(string.Concat("Email Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            //if (!RegexUtilities.IsValidEmail(param.PersonalInformation.Email))
            //{
            //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //}

            if (string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.CellphoneNumber.Length > 15)
                ErrorMessages.Add(string.Concat("Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));

            if (!Regex.IsMatch(param.PersonalInformation.CellphoneNumber, RegexUtilities.REGEX_PHONE_NUMBER))
            {
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
            }

            if (string.IsNullOrEmpty(param.PersonalInformation.SSSNumber))
                ErrorMessages.Add(string.Concat("SSS Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.SSSNumber.Length > 10)
                ErrorMessages.Add(string.Concat("SSS Number", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.TIN))
                ErrorMessages.Add(string.Concat("TIN ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.TIN.Length > 12)
                ErrorMessages.Add(string.Concat("TIN", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.PhilhealthNumber))
                ErrorMessages.Add(string.Concat("Philhealth Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.PhilhealthNumber.Length > 12)
                ErrorMessages.Add(string.Concat("Philhealth Number", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.PagibigNumber))
                ErrorMessages.Add(string.Concat("Pagibig Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
               if (param.PersonalInformation.PagibigNumber.Length > 12)
                ErrorMessages.Add(string.Concat("Pagibig Number", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

            //if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonName))
            //    ErrorMessages.Add(string.Concat("Contact Person Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    param.PersonalInformation.ContactPersonName = param.PersonalInformation.ContactPersonName.Trim();
            //    if (param.PersonalInformation.ContactPersonName.Length > 255)
            //        ErrorMessages.Add(string.Concat("Contact Person Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            //}
            ////if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonNumber))
            ////    ErrorMessages.Add(string.Concat("Contact Person Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            ////else
            ////{
            //    //param.PersonalInformation.ContactPersonNumber = param.PersonalInformation.ContactPersonNumber.Trim();
            //    //if (param.PersonalInformation.ContactPersonNumber.Length > 50)
            //    //    ErrorMessages.Add(string.Concat("Contact Person Number", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            ////}
            ////if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonAddress))
            ////    ErrorMessages.Add(string.Concat("Contact Person Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            ////else
            ////{
            //    param.PersonalInformation.ContactPersonAddress = param.PersonalInformation.ContactPersonAddress.Trim();
            //    if (param.PersonalInformation.ContactPersonAddress.Length > 255)
            //        ErrorMessages.Add(string.Concat("Contact Person Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            ////}
            //if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonRelationship))
            //    ErrorMessages.Add(string.Concat("Contact Person Relationship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    param.PersonalInformation.ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship.Trim();
            //    if (param.PersonalInformation.ContactPersonRelationship.Length > 50)
            //        ErrorMessages.Add(string.Concat("Contact Person Relationship", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            //}
            
            if (param.EmployeeFamilyList != null)
            {
                foreach (var item in param.EmployeeFamilyList)
                {
                    if (string.IsNullOrEmpty(item.Name))
                        ErrorMessages.Add(string.Concat("Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                        if (item.Name.Length > 255)
                        ErrorMessages.Add(string.Concat("Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));


                    if (string.IsNullOrEmpty(item.Relationship))
                        ErrorMessages.Add(string.Concat("Relationship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                        if (item.Relationship.Length > 20)
                        ErrorMessages.Add(string.Concat("Relationship", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

                    if (string.IsNullOrEmpty(item.Occupation))
                        ErrorMessages.Add(string.Concat("Occupation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.Occupation = item.Occupation.Trim();
                        if (item.Occupation.Length > 255)
                            ErrorMessages.Add(string.Concat("Occupation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (item.Relationship.Equals("SPOUSE"))
                    {
                        if (string.IsNullOrEmpty(item.SpouseEmployer))
                            ErrorMessages.Add(string.Concat("Spouse Employer ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.SpouseEmployer = item.SpouseEmployer.Trim();
                            if (item.SpouseEmployer.Length > 255)
                                ErrorMessages.Add(string.Concat("Spouse Employer", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }
                    }

                    //if (string.IsNullOrEmpty(item.ContactNumber))
                    //    ErrorMessages.Add(string.Concat("Contact Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    //else
                    //    if (item.ContactNumber.Length > 15)
                    //    ErrorMessages.Add(string.Concat("Contact Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));

                    //if (!Regex.IsMatch(item.ContactNumber, RegexUtilities.REGEX_PHONE_NUMBER))
                    //{
                    //    ErrorMessages.Add(string.Concat("Contact Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    //}
                }
            } 

            if (param.EmployeeWorkingHistoryList != null)
            {
                foreach (var item in param.EmployeeWorkingHistoryList)
                {
                    if (string.IsNullOrEmpty(item.CompanyName))
                        ErrorMessages.Add(string.Concat("Company Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                        if (item.CompanyName.Length > 255)
                        ErrorMessages.Add(string.Concat("Company Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                    if (string.IsNullOrEmpty(item.Position))
                        ErrorMessages.Add(string.Concat("Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                        if (item.Position.Length > 100)
                        ErrorMessages.Add(string.Concat("Position", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));


                    //if (string.IsNullOrEmpty(item.ReasonForLeaving))
                    //    ErrorMessages.Add(string.Concat("Reason for Leaving ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    //else
                    //    if (item.ReasonForLeaving.Length > 255)
                    //    ErrorMessages.Add(string.Concat("Reason for Leaving", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }
            } 

            if (string.IsNullOrEmpty(param.EmployeeCompensation.MonthlySalary))
                ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.EmployeeCompensation.MonthlySalary = param.EmployeeCompensation.MonthlySalary.Trim();

                if (!decimal.TryParse(param.EmployeeCompensation.MonthlySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }

            if (string.IsNullOrEmpty(param.EmployeeCompensation.DailySalary))
                ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.EmployeeCompensation.DailySalary = param.EmployeeCompensation.DailySalary.Trim();

                if (!decimal.TryParse(param.EmployeeCompensation.DailySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }

            if (string.IsNullOrEmpty(param.EmployeeCompensation.HourlySalary))
                ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.EmployeeCompensation.HourlySalary = param.EmployeeCompensation.HourlySalary.Trim();

                if (!decimal.TryParse(param.EmployeeCompensation.HourlySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            } 

            if (param.EmployeeEducationList != null)
            {
                foreach (var item in param.EmployeeEducationList)
                {

                    if (string.IsNullOrEmpty(item.School))
                        ErrorMessages.Add(string.Concat("School ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.School = item.School.Trim();
                        if (item.School.Length > 255)
                            ErrorMessages.Add(string.Concat("School", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (string.IsNullOrEmpty(item.SchoolAddress))
                        ErrorMessages.Add(string.Concat("School Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.SchoolAddress = item.SchoolAddress.Trim();
                        if (item.SchoolAddress.Length > 255)
                            ErrorMessages.Add(string.Concat("School Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (string.IsNullOrEmpty(item.SchoolLevelCode))
                        ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.SchoolLevelCode = item.SchoolLevelCode.Trim();
                        if (item.SchoolLevelCode.Length > 50)
                            ErrorMessages.Add(string.Concat("School Level", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                        var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_SCHOOL_LEVEL.ToString(), item.SchoolLevelCode);
                        if (referenceValue == null)
                        {
                            ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }
                        else
                        {
                            if (referenceValue.Count() == 0)
                                ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }

                    }


                    if (string.IsNullOrEmpty(item.Course))
                        ErrorMessages.Add(string.Concat("Course ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.Course = item.Course.Trim();
                        if (item.Course.Length > 255)
                            ErrorMessages.Add(string.Concat("Course", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }


                    if (item.YearFrom > item.YearTo)
                        ErrorMessages.Add(string.Concat("Year From ", MessageUtilities.COMPARE_EQUAL_LESS, "Year To"));

                    if (string.IsNullOrEmpty(item.EducationalAttainmentDegreeCode))
                        ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.EducationalAttainmentDegreeCode = item.EducationalAttainmentDegreeCode.Trim();
                        if (item.EducationalAttainmentDegreeCode.Length > 50)
                            ErrorMessages.Add(string.Concat("Educational Attainment Degree", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                        var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_ED_ATT_DEG.ToString(), item.EducationalAttainmentDegreeCode);
                        if (referenceValue == null)
                        {
                            ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }
                        else
                        {
                            if (referenceValue.Count() == 0)
                                ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }

                    }

                    if (string.IsNullOrEmpty(item.EducationalAttainmentStatusCode))
                        ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.EducationalAttainmentStatusCode = item.EducationalAttainmentStatusCode.Trim();
                        if (item.EducationalAttainmentStatusCode.Length > 50)
                            ErrorMessages.Add(string.Concat("Educational Attainment Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                        var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_ED_ATT_STAT.ToString(), item.EducationalAttainmentStatusCode);
                        if (referenceValue == null)
                        {
                            ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }
                        else
                        {
                            if (referenceValue.Count() == 0)
                                ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                        }

                    }


                }
            }
            

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.Employee.Employee
                {
                    Code = param.Code,
                    OldEmployeeID = param.OldEmployeeID,
                    FirstName = param.PersonalInformation.FirstName,
                    LastName = param.PersonalInformation.LastName,
                    MiddleName = param.PersonalInformation.MiddleName,
                    Suffix = param.PersonalInformation.Suffix,
                    Nickname = param.PersonalInformation.Nickname,
                    Gender = param.PersonalInformation.Gender,
                    NationalityCode = param.PersonalInformation.NationalityCode,
                    CitizenshipCode = param.PersonalInformation.CitizenshipCode,
                    HeightCM = param.PersonalInformation.HeightCM,
                    WeightLBS = param.PersonalInformation.WeightLBS,
                    BirthPlace = param.PersonalInformation.BirthPlace,
                    SSSStatusCode = param.PersonalInformation.SSSStatusCode,
                    ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode,
                    CivilStatusCode = param.PersonalInformation.CivilStatusCode,
                    ReligionCode = param.PersonalInformation.ReligionCode,
                    OnboardingWorkflowID = param.OnboardingWorkflowID,
                    OrgGroupID = param.OrgGroupID,
                    PositionID = param.PositionID,
                    //SystemUserID = param.SystemUserID,
                    DateHired = param.DateHired,
                    EmploymentStatus = param.EmploymentStatus,
                    CompanyTag = param.CompanyTag,
                    BirthDate = param.PersonalInformation.BirthDate,
                    AddressLine1 = param.PersonalInformation.AddressLine1,
                    AddressLine2 = param.PersonalInformation.AddressLine2,
                    PSGCRegionCode = param.PersonalInformation.PSGCRegionCode,
                    PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode,
                    PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode,
                    PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode,
                    Email = param.PersonalInformation.Email,
                    CorporateEmail = param.CorporateEmail,
                    OfficeMobile = param.OfficeMobile,
                    OfficeLandline = param.OfficeLandline,
                    CellphoneNumber = param.PersonalInformation.CellphoneNumber,
                    SSSNumber = param.PersonalInformation.SSSNumber,
                    TIN = param.PersonalInformation.TIN,
                    PhilhealthNumber = param.PersonalInformation.PhilhealthNumber,
                    PagibigNumber = param.PersonalInformation.PagibigNumber,
                    ContactPersonName = param.PersonalInformation.ContactPersonName,
                    ContactPersonNumber = param.PersonalInformation.ContactPersonNumber,
                    ContactPersonAddress = param.PersonalInformation.ContactPersonAddress,
                    ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship,
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                }
                , param.EmployeeRovingList?.Select(x => new Data.Employee.EmployeeRoving
                {
                    OrgGroupID = x.OrgGroupID,
                    PositionID = x.PositionID,
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                }).ToList()
                , param.EmployeeFamilyList?.Select(x => new Data.Employee.EmployeeFamily
                {
                    Name = x.Name,
                    Relationship = x.Relationship,
                    BirthDate = x.BirthDate,
                    Occupation = x.Occupation,
                    SpouseEmployer = x.SpouseEmployer,
                    ContactNumber = x.ContactNumber,
                    CreatedBy = param.CreatedBy
                }).ToList()
                , param.EmployeeWorkingHistoryList?.Select(x => new Data.Employee.EmployeeWorkingHistory
                { 
                    CompanyName = x.CompanyName,
                    From = x.From,
                    To = x.From,
                    Position = x.Position,
                    ReasonForLeaving = x.ReasonForLeaving,
                    CreatedBy = param.CreatedBy
                }).ToList()
                , new Data.Employee.EmployeeCompensation 
                { 
                    MonthlySalary = Convert.ToDecimal(param.EmployeeCompensation.MonthlySalary),
                    DailySalary = Convert.ToDecimal(param.EmployeeCompensation.DailySalary),
                    HourlySalary = Convert.ToDecimal(param.EmployeeCompensation.HourlySalary),
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                }
                , param.EmployeeEducationList?.Select(x => new Data.Employee.EmployeeEducation 
                { 
                    School = x.School,
                    SchoolAddress = x.SchoolAddress,
                    SchoolLevelCode = x.SchoolLevelCode,
                    Course = x.Course,
                    YearFrom = x.YearFrom,
                    YearTo = x.YearTo,
                    EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                    EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                    IsActive = true,
                    CreatedBy = param.CreatedBy

                }).ToList()
                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(
                    string.Concat(MessageUtilities.SCSSMSG_REC_SAVE,
                    Environment.NewLine,
                    "New Employee Code: ", NewEmployeeCode != null ? NewEmployeeCode.First().NewEmployeeCode : "N/A")
                    );
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            if (!string.IsNullOrEmpty(param.CompanyTag) && string.IsNullOrEmpty(param.Code))
            {
                var NewEmployeeCode = (await _dbAccess.GetNewEmployeeCode(param.CompanyTag)).ToList();
                if (NewEmployeeCode != null)
                    param.Code = NewEmployeeCode.First().NewEmployeeCode;
            }
            //param.Code = (param.Code ?? "").Trim();
            param.OldEmployeeID = (param.OldEmployeeID ?? "").Trim();
            param.EmploymentStatus = (param.EmploymentStatus ?? "").Trim();
            param.CompanyTag = (param.CompanyTag ?? "").Trim();
            param.PersonalInformation.FirstName = (param.PersonalInformation.FirstName ?? "").Trim();
            param.PersonalInformation.LastName = (param.PersonalInformation.LastName ?? "").Trim();
            param.PersonalInformation.MiddleName = (param.PersonalInformation.MiddleName ?? "").Trim();
            param.PersonalInformation.AddressLine1 = (param.PersonalInformation.AddressLine1 ?? "").Trim();
            param.PersonalInformation.AddressLine2 = (param.PersonalInformation.AddressLine2 ?? "").Trim();
            param.PersonalInformation.Email = (param.PersonalInformation.Email ?? "").Trim();
            param.PersonalInformation.CellphoneNumber = (param.PersonalInformation.CellphoneNumber ?? "").Trim();
            param.PersonalInformation.SSSNumber = (param.PersonalInformation.SSSNumber ?? "").Trim();
            param.PersonalInformation.TIN = (param.PersonalInformation.TIN ?? "").Trim();
            param.PersonalInformation.PagibigNumber = (param.PersonalInformation.PagibigNumber ?? "").Trim();
            param.PersonalInformation.PhilhealthNumber = (param.PersonalInformation.PhilhealthNumber ?? "").Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add(string.Concat("Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Code.Length > 50)
                ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            else
            {
                List<Data.Employee.Employee> form = (await _dbAccess.GetByCode(param.Code)).ToList();

                if (form.Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                }
            }

            if (!string.IsNullOrEmpty(param.OldEmployeeID))
                if (param.OldEmployeeID.Length > 5)
                    ErrorMessages.Add(string.Concat("Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));

            if (!string.IsNullOrEmpty(param.CorporateEmail))
            {
                param.CorporateEmail = (param.CorporateEmail ?? "").Trim();
                if (param.CorporateEmail.Length > 255)
                    ErrorMessages.Add(string.Concat("Corporate Email", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                if (!RegexUtilities.IsValidEmail(param.CorporateEmail))
                {
                    ErrorMessages.Add(string.Concat("Corporate Email ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
            }

            if (!string.IsNullOrEmpty(param.OfficeMobile))
            {
                if (param.OfficeMobile.Substring(0,2) !="09" || param.OfficeMobile.Length != 11)
                {
                    ErrorMessages.Add(string.Concat("Office Mobile Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
            }

            if (string.IsNullOrEmpty(param.EmploymentStatus))
                ErrorMessages.Add(string.Concat("Employment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.EmploymentStatus.Length > 20)
                ErrorMessages.Add(string.Concat("Employment Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (string.IsNullOrEmpty(param.CompanyTag))
                ErrorMessages.Add(string.Concat("Company ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.CompanyTag.Length > 20)
                ErrorMessages.Add(string.Concat("Company", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (param.IsDataPrivacy == true)
            {
                if (string.IsNullOrEmpty(param.PersonalInformation.FirstName))
                    ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                if (param.PersonalInformation.FirstName.Length > 50)
                    ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (string.IsNullOrEmpty(param.PersonalInformation.LastName))
                    ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (param.PersonalInformation.LastName.Length > 50)
                    ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (!string.IsNullOrEmpty(param.PersonalInformation.MiddleName))
                    if (param.PersonalInformation.MiddleName.Length > 50)
                        ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                if (!string.IsNullOrEmpty(param.PersonalInformation.Suffix))
                {
                    param.PersonalInformation.Suffix = param.PersonalInformation.Suffix.Trim();
                    if (param.PersonalInformation.Suffix.Length > 10)
                        ErrorMessages.Add(string.Concat("Suffix", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));
                }

                if (!string.IsNullOrEmpty(param.PersonalInformation.Nickname))
                {
                    param.PersonalInformation.Nickname = param.PersonalInformation.Nickname.Trim();
                    if (param.PersonalInformation.Nickname.Length > 50)
                        ErrorMessages.Add(string.Concat("Nickname", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                }

                if (string.IsNullOrEmpty(param.PersonalInformation.Gender))
                    ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.Gender = param.PersonalInformation.Gender.Trim();
                    if (param.PersonalInformation.Gender.Length > 2)
                        ErrorMessages.Add(string.Concat("Gender", MessageUtilities.COMPARE_NOT_EXCEED, "2 characters."));


                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_GENDER.ToString(), param.PersonalInformation.Gender);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Gender ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                if (string.IsNullOrEmpty(param.PersonalInformation.BirthPlace))
                    ErrorMessages.Add(string.Concat("Birth Place ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.BirthPlace = param.PersonalInformation.BirthPlace.Trim();
                    if (param.PersonalInformation.BirthPlace.Length > 255)
                        ErrorMessages.Add(string.Concat("Birth Place", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                }

                //if (string.IsNullOrEmpty(param.PersonalInformation.HeightCM))
                //    ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    param.PersonalInformation.HeightCM = param.PersonalInformation.HeightCM.Trim();
                //    if (param.PersonalInformation.HeightCM.Length > 50)
                //        ErrorMessages.Add(string.Concat("Height (in cm)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                //    if (!decimal.TryParse(param.PersonalInformation.HeightCM, out decimal height))
                //    {
                //        ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //    else
                //    {
                //        if (height <= 0)
                //            ErrorMessages.Add(string.Concat("Height (in cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}


                //if (string.IsNullOrEmpty(param.PersonalInformation.WeightLBS))
                //    ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    param.PersonalInformation.WeightLBS = param.PersonalInformation.WeightLBS.Trim();
                //    if (param.PersonalInformation.WeightLBS.Length > 50)
                //        ErrorMessages.Add(string.Concat("Weight (in lbs)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                //    if (!decimal.TryParse(param.PersonalInformation.WeightLBS, out decimal height))
                //    {
                //        ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //    else
                //    {
                //        if (height <= 0)
                //            ErrorMessages.Add(string.Concat("Weight (in lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                if (string.IsNullOrEmpty(param.PersonalInformation.SSSStatusCode))
                    ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.SSSStatusCode = param.PersonalInformation.SSSStatusCode.Trim();
                    if (param.PersonalInformation.SSSStatusCode.Length > 50)
                        ErrorMessages.Add(string.Concat("SSS Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_SSS_STAT.ToString(), param.PersonalInformation.SSSStatusCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("SSS Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.ExemptionStatusCode))
                    ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode.Trim();
                    if (param.PersonalInformation.ExemptionStatusCode.Length > 50)
                        ErrorMessages.Add(string.Concat("Exemption Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString(), param.PersonalInformation.ExemptionStatusCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Exemption Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.NationalityCode))
                    ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.NationalityCode = param.PersonalInformation.NationalityCode.Trim();
                    if (param.PersonalInformation.NationalityCode.Length > 50)
                        ErrorMessages.Add(string.Concat("Nationality", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_NATIONALITY.ToString(), param.PersonalInformation.NationalityCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Nationality ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.CitizenshipCode))
                    ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED)); 
                else
                {
                    param.PersonalInformation.CitizenshipCode = param.PersonalInformation.CitizenshipCode.Trim();
                    if (param.PersonalInformation.CitizenshipCode.Length > 50)
                        ErrorMessages.Add(string.Concat("Citizenship", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString(), param.PersonalInformation.CitizenshipCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Citizenship ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.CivilStatusCode))
                    ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.CivilStatusCode = param.PersonalInformation.CivilStatusCode.Trim();
                    if (param.PersonalInformation.CivilStatusCode.Length > 50)
                        ErrorMessages.Add(string.Concat("Civil Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString(), param.PersonalInformation.CivilStatusCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Civil Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.ReligionCode))
                    ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.PersonalInformation.ReligionCode = param.PersonalInformation.ReligionCode.Trim();
                    if (param.PersonalInformation.ReligionCode.Length > 50)
                        ErrorMessages.Add(string.Concat("Religion", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_RELIGION.ToString(), param.PersonalInformation.ReligionCode);
                    if (referenceValue == null)
                    {
                        ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (referenceValue.Count() == 0)
                            ErrorMessages.Add(string.Concat("Religion ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                }

                if (string.IsNullOrEmpty(param.PersonalInformation.AddressLine1))
                    ErrorMessages.Add(string.Concat("Address Line 1 ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (param.PersonalInformation.AddressLine1.Length > 255)
                    ErrorMessages.Add(string.Concat("Address Line 1", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                if (!string.IsNullOrEmpty(param.PersonalInformation.AddressLine2))
                    if (param.PersonalInformation.AddressLine2.Length > 255)
                        ErrorMessages.Add(string.Concat("Address Line 2", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                if (string.IsNullOrEmpty(param.PersonalInformation.PSGCRegionCode))
                    ErrorMessages.Add(string.Concat("Region ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                if (string.IsNullOrEmpty(param.PersonalInformation.PSGCProvinceCode))
                    ErrorMessages.Add(string.Concat("Province ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                if (string.IsNullOrEmpty(param.PersonalInformation.PSGCCityMunicipalityCode))
                    ErrorMessages.Add(string.Concat("City/Municipality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                if (string.IsNullOrEmpty(param.PersonalInformation.PSGCBarangayCode))
                    ErrorMessages.Add(string.Concat("Barangay ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

                //if (string.IsNullOrEmpty(param.PersonalInformation.Email))
                //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                if (param.PersonalInformation.Email.Length > 255)
                    ErrorMessages.Add(string.Concat("Email Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                //if (!RegexUtilities.IsValidEmail(param.PersonalInformation.Email))
                //{
                //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //}

                //if (string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                //    ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //    if (param.PersonalInformation.CellphoneNumber.Length > 15)
                //    ErrorMessages.Add(string.Concat("Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));

                //if (!Regex.IsMatch(param.PersonalInformation.CellphoneNumber, RegexUtilities.REGEX_PHONE_NUMBER))
                //{
                //    ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //}

                if (!string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                {
                    if (param.PersonalInformation.CellphoneNumber.Substring(0, 2) != "09" || param.PersonalInformation.CellphoneNumber.Length != 11)
                    {
                        ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                if (string.IsNullOrEmpty(param.PersonalInformation.SSSNumber))
                    ErrorMessages.Add(string.Concat("SSS Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (param.PersonalInformation.SSSNumber.Length > 10)
                    ErrorMessages.Add(string.Concat("SSS Number", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));

                if (string.IsNullOrEmpty(param.PersonalInformation.TIN))
                    ErrorMessages.Add(string.Concat("TIN ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (param.PersonalInformation.TIN.Length > 12)
                    ErrorMessages.Add(string.Concat("TIN", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

                if (string.IsNullOrEmpty(param.PersonalInformation.PhilhealthNumber))
                    ErrorMessages.Add(string.Concat("Philhealth Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                    if (param.PersonalInformation.PhilhealthNumber.Length > 12)
                    ErrorMessages.Add(string.Concat("Philhealth Number", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

                if (string.IsNullOrEmpty(param.PersonalInformation.PagibigNumber))
                    ErrorMessages.Add(string.Concat("Pagibig Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                   if (param.PersonalInformation.PagibigNumber.Length > 12)
                    ErrorMessages.Add(string.Concat("Pagibig Number", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));

                //if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonName))
                //    ErrorMessages.Add(string.Concat("Contact Person Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    param.PersonalInformation.ContactPersonName = param.PersonalInformation.ContactPersonName.Trim();
                //    if (param.PersonalInformation.ContactPersonName.Length > 255)
                //        ErrorMessages.Add(string.Concat("Contact Person Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                //}
                ////if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonNumber))
                ////    ErrorMessages.Add(string.Concat("Contact Person Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                ////else
                ////{
                //    //param.PersonalInformation.ContactPersonNumber = param.PersonalInformation.ContactPersonNumber.Trim();
                //    //if (param.PersonalInformation.ContactPersonNumber.Length > 50)
                //    //    ErrorMessages.Add(string.Concat("Contact Person Number", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                ////}
                ////if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonAddress))
                ////    ErrorMessages.Add(string.Concat("Contact Person Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                ////else
                //////{
                ////    param.PersonalInformation.ContactPersonAddress = param.PersonalInformation.ContactPersonAddress.Trim();
                ////    if (param.PersonalInformation.ContactPersonAddress.Length > 255)
                ////        ErrorMessages.Add(string.Concat("Contact Person Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                ////}
                //if (string.IsNullOrEmpty(param.PersonalInformation.ContactPersonRelationship))
                //    ErrorMessages.Add(string.Concat("Contact Person Relationship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    param.PersonalInformation.ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship.Trim();
                //    if (param.PersonalInformation.ContactPersonRelationship.Length > 50)
                //        ErrorMessages.Add(string.Concat("Contact Person Relationship", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                //}

                if (param.IsViewedFamilyBackground)
                {
                    if (param.EmployeeFamilyList != null)
                    {
                        foreach (var item in param.EmployeeFamilyList)
                        {
                            if (string.IsNullOrEmpty(item.Name))
                                ErrorMessages.Add(string.Concat("Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                            else
                                if (item.Name.Length > 255)
                                ErrorMessages.Add(string.Concat("Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                            if (string.IsNullOrEmpty(item.Relationship))
                                ErrorMessages.Add(string.Concat("Relationship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                            else
                                if (item.Relationship.Length > 20)
                                ErrorMessages.Add(string.Concat("Relationship", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

                            if (string.IsNullOrEmpty(item.Occupation))
                                ErrorMessages.Add(string.Concat("Occupation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                            else
                            {
                                item.Occupation = item.Occupation.Trim();
                                if (item.Occupation.Length > 255)
                                    ErrorMessages.Add(string.Concat("Occupation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                            }

                            if (item.Relationship.Equals("SPOUSE"))
                            {
                                if (string.IsNullOrEmpty(item.SpouseEmployer))
                                    ErrorMessages.Add(string.Concat("Spouse Employer ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                                else
                                {
                                    item.SpouseEmployer = item.SpouseEmployer.Trim();
                                    if (item.SpouseEmployer.Length > 255)
                                        ErrorMessages.Add(string.Concat("Spouse Employer", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                                }
                            }

                            //if (string.IsNullOrEmpty(item.ContactNumber))
                            //    ErrorMessages.Add(string.Concat("Contact Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                            //else
                            //    if (item.ContactNumber.Length > 15)
                            //    ErrorMessages.Add(string.Concat("Contact Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));

                            //if (!Regex.IsMatch(item.ContactNumber, RegexUtilities.REGEX_PHONE_NUMBER))
                            //{
                            //    ErrorMessages.Add(string.Concat("Contact Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            //}
                        }
                    } 
                }


            }

            //if (param.IsCompensationEdit)
            //{
            //    if (string.IsNullOrEmpty(param.EmployeeCompensation.MonthlySalary))
            //        ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //    else
            //    {
            //        param.EmployeeCompensation.MonthlySalary = param.EmployeeCompensation.MonthlySalary.Trim();

            //        if (!decimal.TryParse(param.EmployeeCompensation.MonthlySalary, out decimal parsed))
            //        {
            //            ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //        }
            //        else
            //        {
            //            if (parsed <= 0)
            //                ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //        }
            //    }

            //    if (string.IsNullOrEmpty(param.EmployeeCompensation.DailySalary))
            //        ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //    else
            //    {
            //        param.EmployeeCompensation.DailySalary = param.EmployeeCompensation.DailySalary.Trim();

            //        if (!decimal.TryParse(param.EmployeeCompensation.DailySalary, out decimal parsed))
            //        {
            //            ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //        }
            //        else
            //        {
            //            if (parsed <= 0)
            //                ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //        }
            //    }

            //    if (string.IsNullOrEmpty(param.EmployeeCompensation.HourlySalary))
            //        ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //    else
            //    {
            //        param.EmployeeCompensation.HourlySalary = param.EmployeeCompensation.HourlySalary.Trim();

            //        if (!decimal.TryParse(param.EmployeeCompensation.HourlySalary, out decimal parsed))
            //        {
            //            ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //        }
            //        else
            //        {
            //            if (parsed <= 0)
            //                ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //        }
            //    } 
            //}

            if (param.IsViewedWorkingHistory)
            {

                if (param.EmployeeWorkingHistoryList != null)
                {
                    foreach (var item in param.EmployeeWorkingHistoryList)
                    {
                        if (string.IsNullOrEmpty(item.CompanyName))
                            ErrorMessages.Add(string.Concat("Company Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                            if (item.CompanyName.Length > 255)
                            ErrorMessages.Add(string.Concat("Company Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

                        if (string.IsNullOrEmpty(item.Position))
                            ErrorMessages.Add(string.Concat("Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                            if (item.Position.Length > 100)
                            ErrorMessages.Add(string.Concat("Position", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));


                        //if (string.IsNullOrEmpty(item.ReasonForLeaving))
                        //    ErrorMessages.Add(string.Concat("Reason for Leaving ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        //else
                        //    if (item.ReasonForLeaving.Length > 255)
                        //    ErrorMessages.Add(string.Concat("Reason for Leaving", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

            }

            if (param.IsViewedEducation)
            {
                if (param.EmployeeEducationList != null)
                {
                    foreach (var item in param.EmployeeEducationList)
                    {

                        if (string.IsNullOrEmpty(item.School))
                            ErrorMessages.Add(string.Concat("School ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.School = item.School.Trim();
                            if (item.School.Length > 255)
                                ErrorMessages.Add(string.Concat("School", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }

                        if (string.IsNullOrEmpty(item.SchoolAddress))
                            ErrorMessages.Add(string.Concat("School Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.SchoolAddress = item.SchoolAddress.Trim();
                            if (item.SchoolAddress.Length > 255)
                                ErrorMessages.Add(string.Concat("School Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }

                        if (string.IsNullOrEmpty(item.SchoolLevelCode))
                            ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.SchoolLevelCode = item.SchoolLevelCode.Trim();
                            if (item.SchoolLevelCode.Length > 50)
                                ErrorMessages.Add(string.Concat("School Level", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                            var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_SCHOOL_LEVEL.ToString(), item.SchoolLevelCode);
                            if (referenceValue == null)
                            {
                                ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }
                            else
                            {
                                if (referenceValue.Count() == 0)
                                    ErrorMessages.Add(string.Concat("School Level ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }

                        }


                        if (string.IsNullOrEmpty(item.Course))
                            ErrorMessages.Add(string.Concat("Course ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.Course = item.Course.Trim();
                            if (item.Course.Length > 255)
                                ErrorMessages.Add(string.Concat("Course", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }


                        if (item.YearFrom > item.YearTo)
                            ErrorMessages.Add(string.Concat("Year From ", MessageUtilities.COMPARE_EQUAL_LESS, "Year To"));

                        if (string.IsNullOrEmpty(item.EducationalAttainmentDegreeCode))
                            ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.EducationalAttainmentDegreeCode = item.EducationalAttainmentDegreeCode.Trim();
                            if (item.EducationalAttainmentDegreeCode.Length > 50)
                                ErrorMessages.Add(string.Concat("Educational Attainment Degree", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                            var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_ED_ATT_DEG.ToString(), item.EducationalAttainmentDegreeCode);
                            if (referenceValue == null)
                            {
                                ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }
                            else
                            {
                                if (referenceValue.Count() == 0)
                                    ErrorMessages.Add(string.Concat("Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }

                        }

                        if (string.IsNullOrEmpty(item.EducationalAttainmentStatusCode))
                            ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                        else
                        {
                            item.EducationalAttainmentStatusCode = item.EducationalAttainmentStatusCode.Trim();
                            if (item.EducationalAttainmentStatusCode.Length > 50)
                                ErrorMessages.Add(string.Concat("Educational Attainment Status", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                            var referenceValue = await _referenceDBAccess.GetByRefCodeValue(Enums.ReferenceCodes.EMP_ED_ATT_STAT.ToString(), item.EducationalAttainmentStatusCode);
                            if (referenceValue == null)
                            {
                                ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }
                            else
                            {
                                if (referenceValue.Count() == 0)
                                    ErrorMessages.Add(string.Concat("Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INVALID));
                            }

                        }


                    }
                } 
            }

            if (ErrorMessages.Count == 0)
            {
                List<Data.Employee.EmployeeRoving> GetEmployeeRovingToAdd(List<Data.Employee.EmployeeRoving> left, List<Data.Employee.EmployeeRoving> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.EmployeeID, x.OrgGroupID, x.PositionID },
                             y => new { y.EmployeeID, y.OrgGroupID, y.PositionID },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeRoving
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                OrgGroupID = x.newSet.newSet.OrgGroupID,
                                PositionID = x.newSet.newSet.PositionID,
                                IsActive = true,
                                CreatedBy = x.newSet.newSet.CreatedBy
                            }).ToList();
                }

                List<Data.Employee.EmployeeRoving> GetEmployeeRovingToDelete(List<Data.Employee.EmployeeRoving> left, List<Data.Employee.EmployeeRoving> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.OrgGroupID, x.PositionID },
                             y => new { y.EmployeeID, y.OrgGroupID, y.PositionID },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeRoving
                            {
                                ID = x.oldSet.oldSet.ID,
                                EmployeeID = x.oldSet.oldSet.EmployeeID,
                                OrgGroupID = x.oldSet.oldSet.OrgGroupID,
                                PositionID = x.oldSet.oldSet.PositionID,
                                IsActive = false,
                                CreatedBy = x.oldSet.oldSet.CreatedBy,
                                CreatedDate = x.oldSet.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToDelete(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeFamily
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToAdd(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeFamily
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                Name = x.newSet.newSet.Name,
                                Relationship = x.newSet.newSet.Relationship,
                                BirthDate = x.newSet.newSet.BirthDate,
                                Occupation = x.newSet.newSet.Occupation,
                                SpouseEmployer = x.newSet.newSet.SpouseEmployer,
                                ContactNumber = x.newSet.newSet.ContactNumber,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToUpdate(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return left.Join(
                        right,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Name.Equals(x.newSet.Name) ||
                                    !x.oldSet.Relationship.Equals(x.newSet.Relationship) ||
                                    (x.oldSet.ContactNumber != x.newSet.ContactNumber) ||
                                    (x.oldSet.BirthDate != x.newSet.BirthDate) ||
                                    (x.oldSet.Occupation != x.newSet.Occupation)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeFamily
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                Name = y.newSet.Name,
                                Relationship = y.newSet.Relationship,
                                BirthDate = y.newSet.BirthDate,
                                Occupation = y.newSet.Occupation,
                                SpouseEmployer = y.newSet.SpouseEmployer,
                                ContactNumber = y.newSet.ContactNumber,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToDelete(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToAdd(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                CompanyName = x.newSet.newSet.CompanyName,
                                From = x.newSet.newSet.From,
                                To = x.newSet.newSet.To,
                                Position = x.newSet.newSet.Position,
                                ReasonForLeaving = x.newSet.newSet.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToUpdate(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return left.Join(
                        right,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.CompanyName.Equals(x.newSet.CompanyName) ||
                                    (x.oldSet.From != x.newSet.From) ||
                                    (x.oldSet.To != x.newSet.To) ||
                                    (x.oldSet.Position != x.newSet.Position) ||
                                    (x.oldSet.ReasonForLeaving != x.newSet.ReasonForLeaving)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                CompanyName = y.newSet.CompanyName,
                                From = y.newSet.From,
                                To = y.newSet.To,
                                Position = y.newSet.Position,
                                ReasonForLeaving = y.newSet.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToDelete(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.SchoolLevelCode },
                             y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeEducation
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToAdd(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return right.GroupJoin(
                        left,
                        x => new { x.EmployeeID, x.SchoolLevelCode },
                        y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                School = x.newSet.newSet.School,
                                SchoolAddress = x.newSet.newSet.SchoolAddress,
                                SchoolLevelCode = x.newSet.newSet.SchoolLevelCode,
                                Course = x.newSet.newSet.Course,
                                YearFrom = x.newSet.newSet.YearFrom,
                                YearTo = x.newSet.newSet.YearTo,
                                EducationalAttainmentDegreeCode = x.newSet.newSet.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.newSet.newSet.EducationalAttainmentStatusCode,
                                
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToUpdate(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return left.Join(
                        right,
                            x => new { x.EmployeeID, x.SchoolLevelCode },
                            y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.School.Equals(x.newSet.School) ||
                                    (x.oldSet.SchoolAddress != x.newSet.SchoolAddress) ||
                                    (x.oldSet.Course != x.newSet.Course) ||
                                    (x.oldSet.YearFrom != x.newSet.YearFrom) ||
                                    (x.oldSet.YearTo != x.newSet.YearTo) ||
                                    (x.oldSet.EducationalAttainmentDegreeCode != x.newSet.EducationalAttainmentDegreeCode) ||
                                    (x.oldSet.EducationalAttainmentStatusCode != x.newSet.EducationalAttainmentStatusCode)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeEducation
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                School = y.newSet.School,
                                SchoolAddress = y.newSet.SchoolAddress,
                                SchoolLevelCode = y.newSet.SchoolLevelCode,
                                Course = y.newSet.Course,
                                YearFrom = y.newSet.YearFrom,
                                YearTo = y.newSet.YearTo,
                                EducationalAttainmentDegreeCode = y.newSet.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = y.newSet.EducationalAttainmentStatusCode,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }


                //Employee Roving
                List<Data.Employee.EmployeeRoving> EmployeeRovingToAdd = new List<EmployeeRoving>();
                List<Data.Employee.EmployeeRoving> EmployeeRovingToDelete = new List<EmployeeRoving>();

                //Orig
                //if (param.IsViewedSecondaryDesignation)
                //{
                //    List<Data.Employee.EmployeeRoving> OldEmployeeRoving = (await _dbAccess.GetRovingByEmployeeID(param.ID)).ToList();

                //    EmployeeRovingToAdd = GetEmployeeRovingToAdd(OldEmployeeRoving,
                //        param.EmployeeRovingList == null ? new List<Data.Employee.EmployeeRoving>() :
                //        param.EmployeeRovingList.Select(x => new Data.Employee.EmployeeRoving
                //        {
                //            EmployeeID = param.ID,
                //            OrgGroupID = x.OrgGroupID,
                //            PositionID = x.PositionID,
                //            IsActive = true,
                //            CreatedBy = credentials.UserID
                //        }).ToList()).ToList();

                //    EmployeeRovingToDelete = GetEmployeeRovingToDelete(OldEmployeeRoving,
                //        param.EmployeeRovingList == null ? new List<Data.Employee.EmployeeRoving>() :
                //        param.EmployeeRovingList.Select(x => new Data.Employee.EmployeeRoving
                //        {
                //            EmployeeID = param.ID,
                //            OrgGroupID = x.OrgGroupID,
                //            PositionID = x.PositionID
                //        }).ToList()).ToList();
                //}

                if (param.IsViewedSecondaryDesignation)
                {
                    List<Data.Employee.EmployeeRoving> OldEmployeeRoving = (await _dbAccess.GetRovingByEmployeeID(param.ID)).ToList();

                    //    EmployeeRovingToAdd = GetEmployeeRovingToAdd(OldEmployeeRoving,
                    //        param.EmployeeRovingList == null ? new List<Data.Employee.EmployeeRoving>() :
                    //        param.EmployeeRovingList.Select(x => new Data.Employee.EmployeeRoving
                    //        {
                    //            EmployeeID = param.ID,
                    //            OrgGroupID = x.OrgGroupID,
                    //            PositionID = x.PositionID,
                    //            IsActive = true,
                    //            CreatedBy = credentials.UserID
                    //        }).ToList()).ToList();

                    //    EmployeeRovingToDelete = GetEmployeeRovingToDelete(OldEmployeeRoving,
                    //        param.EmployeeRovingList == null ? new List<Data.Employee.EmployeeRoving>() :
                    //        param.EmployeeRovingList.Select(x => new Data.Employee.EmployeeRoving
                    //        {
                    //            EmployeeID = param.ID,
                    //            OrgGroupID = x.OrgGroupID,
                    //            PositionID = x.PositionID
                    //        }).ToList()).ToList();
                }

                //Employee Family
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToAdd = new List<Data.Employee.EmployeeFamily>();
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToUpdate = new List<Data.Employee.EmployeeFamily>();
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToDelete = new List<Data.Employee.EmployeeFamily>();
                if (param.IsViewedFamilyBackground)
                {
                    List<Data.Employee.EmployeeFamily> OldEmployeeFamily = (await _dbAccess.GetFamilyByEmployeeID(param.ID)).ToList();

                    EmployeeFamilyToAdd = GetFamilyToAdd(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                            BirthDate = x.BirthDate,
                            Occupation = x.Occupation,
                            SpouseEmployer = x.SpouseEmployer,
                            ContactNumber = x.ContactNumber,
                            CreatedBy = credentials.UserID
                        }).ToList()).ToList();

                    EmployeeFamilyToUpdate = GetFamilyToUpdate(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                            BirthDate = x.BirthDate,
                            Occupation = x.Occupation,
                            SpouseEmployer = x.SpouseEmployer,
                            ContactNumber = x.ContactNumber,
                            CreatedBy = x.CreatedBy
                        }).ToList()).ToList();

                    EmployeeFamilyToDelete = GetFamilyToDelete(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                        }).ToList()).ToList();

                }

                //Employee Working History
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToAdd = new List<Data.Employee.EmployeeWorkingHistory>();
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToUpdate = new List<Data.Employee.EmployeeWorkingHistory>();
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToDelete = new List<Data.Employee.EmployeeWorkingHistory>();

                if (param.IsViewedWorkingHistory)
                {
                    if (param.EmployeeWorkingHistoryList != null)
                    {
                        List<Data.Employee.EmployeeWorkingHistory> OldWorkingHistory = (await _dbAccess.GetWorkingHistoryByEmployeeID(param.ID)).ToList();

                        WorkingHistoryToAdd = GetWorkingHistoryToAdd(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position,
                                ReasonForLeaving = x.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList()).ToList();

                        WorkingHistoryToUpdate = GetWorkingHistoryToUpdate(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position,
                                ReasonForLeaving = x.ReasonForLeaving,
                                CreatedBy = x.CreatedBy
                            }).ToList()).ToList();

                        WorkingHistoryToDelete = GetWorkingHistoryToDelete(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position
                            }).ToList()).ToList();
                    } 
                }

                //Employee Working History
                List<Data.Employee.EmployeeEducation> EducationToAdd = new List<Data.Employee.EmployeeEducation>();
                List<Data.Employee.EmployeeEducation> EducationToUpdate = new List<Data.Employee.EmployeeEducation>();
                List<Data.Employee.EmployeeEducation> EducationToDelete = new List<Data.Employee.EmployeeEducation>();

                if (param.IsViewedEducation)
                {
                    if (param.EmployeeEducationList != null)
                    {
                        List<Data.Employee.EmployeeEducation> OldEducation = (await _dbAccess.GetEducationByEmployeeID(param.ID)).ToList();

                        EducationToAdd = GetEducationToAdd(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                                CreatedBy = credentials.UserID
                            }).ToList()).ToList();

                        EducationToUpdate = GetEducationToUpdate(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                            }).ToList()).ToList();

                        EducationToDelete = GetEducationToDelete(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                            }).ToList()).ToList();
                    }
                }

                Data.Employee.Employee employeeData = await _dbAccess.GetByID(param.ID);
                //Data.Employee.EmployeeCompensation EmployeeCompensationToUpdate = (await _dbAccess.GetCompensationByEmployeeID(param.ID));
                //Data.Employee.EmployeeCompensation EmployeeCompensationToAdd = null;
                Data.Employee.EmploymentStatusHistory employmentStatus = new Data.Employee.EmploymentStatusHistory();
                
                //if (employeeData.EmploymentStatus != param.EmploymentStatus)
                //{
                //    employmentStatus =
                //    new Data.Employee.EmploymentStatusHistory
                //    {
                //        EmployeeID = param.ID,
                //        EmploymentStatus = param.EmploymentStatus,
                //        DateEffective = DateTime.Now,
                //        CreatedBy = param.CreatedBy
                //    };
                //}

                employeeData.Code = param.Code;
                employeeData.OldEmployeeID = param.OldEmployeeID;
                employeeData.CorporateEmail = param.CorporateEmail;
                employeeData.OfficeMobile = param.OfficeMobile;
                employeeData.OfficeLandline = param.OfficeLandline;
                employeeData.IsDisplayDirectory = param.IsDisplayDirectory;
                employeeData.FirstName = param.PersonalInformation.FirstName;
                employeeData.OnboardingWorkflowID = param.OnboardingWorkflowID;
                employeeData.MiddleName = param.PersonalInformation.MiddleName;
                employeeData.LastName = param.PersonalInformation.LastName;
                employeeData.Suffix = param.PersonalInformation.Suffix;
                employeeData.Nickname = param.PersonalInformation.Nickname;

                employeeData.Gender = param.PersonalInformation.Gender;
                employeeData.NationalityCode = param.PersonalInformation.NationalityCode;
                employeeData.CitizenshipCode = param.PersonalInformation.CitizenshipCode;
                employeeData.BirthPlace = param.PersonalInformation.BirthPlace;
                employeeData.HeightCM = param.PersonalInformation.HeightCM;
                employeeData.WeightLBS = param.PersonalInformation.WeightLBS;
                employeeData.SSSStatusCode = param.PersonalInformation.SSSStatusCode;
                employeeData.ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode;
                employeeData.CivilStatusCode = param.PersonalInformation.CivilStatusCode;
                employeeData.ReligionCode = param.PersonalInformation.ReligionCode;
                
                employeeData.OrgGroupID = param.OrgGroupID;
                employeeData.HomeBranchID = param.HomeBranchID;
                employeeData.PositionID = param.PositionID;
                //employeeData.SystemUserID = param.SystemUserID;
                employeeData.CompanyTag = param.CompanyTag;
                employeeData.DateHired = param.DateHired;
                employeeData.EmploymentStatus = param.EmploymentStatus;
                employeeData.BirthDate = param.PersonalInformation.BirthDate;
                employeeData.AddressLine1 = param.PersonalInformation.AddressLine1;
                employeeData.AddressLine2 = param.PersonalInformation.AddressLine2;
                employeeData.PSGCRegionCode = param.PersonalInformation.PSGCRegionCode;
                employeeData.PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode;
                employeeData.PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode;
                employeeData.PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode;
                employeeData.Email = param.PersonalInformation.Email;
                employeeData.CellphoneNumber = param.PersonalInformation.CellphoneNumber;
                employeeData.SSSNumber = param.PersonalInformation.SSSNumber;
                employeeData.TIN = param.PersonalInformation.TIN;
                employeeData.PhilhealthNumber = param.PersonalInformation.PhilhealthNumber;
                employeeData.PagibigNumber = param.PersonalInformation.PagibigNumber;
                employeeData.ContactPersonName = param.PersonalInformation.ContactPersonName;
                employeeData.ContactPersonNumber = param.PersonalInformation.ContactPersonNumber;
                employeeData.ContactPersonAddress = param.PersonalInformation.ContactPersonAddress;
                employeeData.ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship;
                employeeData.IsActive = true;
                employeeData.ModifiedBy = credentials.UserID;
                employeeData.ModifiedDate = DateTime.Now;


                //if (EmployeeCompensationToUpdate != null)
                //{
                //    EmployeeCompensationToUpdate.MonthlySalary = Convert.ToDecimal(param.EmployeeCompensation.MonthlySalary);
                //    EmployeeCompensationToUpdate.DailySalary = Convert.ToDecimal(param.EmployeeCompensation.DailySalary);
                //    EmployeeCompensationToUpdate.HourlySalary = Convert.ToDecimal(param.EmployeeCompensation.HourlySalary);
                //    EmployeeCompensationToUpdate.IsActive = true;
                //    EmployeeCompensationToUpdate.ModifiedBy = credentials.UserID;
                //    EmployeeCompensationToUpdate.ModifiedDate = DateTime.Now;
                //}
                //else
                //{
                //    EmployeeCompensationToAdd = new Data.Employee.EmployeeCompensation
                //    {
                //        EmployeeID = param.ID,
                //        MonthlySalary = Convert.ToDecimal(param.EmployeeCompensation.MonthlySalary),
                //        DailySalary = Convert.ToDecimal(param.EmployeeCompensation.DailySalary),
                //        HourlySalary = Convert.ToDecimal(param.EmployeeCompensation.HourlySalary),
                //        IsActive = true,
                //        CreatedBy = credentials.UserID
                //    };
                //}


                await _dbAccess.Put(
                    employeeData,
                    param.IsViewedSecondaryDesignation,
                    EmployeeRovingToAdd,
                    EmployeeRovingToDelete,
                    param.IsViewedFamilyBackground,
                    EmployeeFamilyToDelete,
                    EmployeeFamilyToAdd,
                    EmployeeFamilyToUpdate,
                    param.IsViewedWorkingHistory,
                    WorkingHistoryToDelete,
                    WorkingHistoryToAdd,
                    WorkingHistoryToUpdate,
                    //EmployeeCompensationToUpdate,
                    //EmployeeCompensationToAdd,
                    param.IsViewedEducation,
                    EducationToDelete,
                    EducationToAdd,
                    EducationToUpdate

                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Plantilla.Data.Employee.Employee employee = await _dbAccess.GetByID(ID);
            employee.IsActive = false;
            employee.ModifiedBy = credentials.UserID;
            employee.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(employee))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> GetByUserIDs(APICredentials credentials, List<int> UserIDs)
        {
            List<Data.Employee.Employee> result = (await _dbAccess.GetByUserIDs(UserIDs)).ToList();


            //if (result == null)
            //    return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            //else
            //    return new OkObjectResult(
            //        result.Select(x =>
            //    new Form
            //    {
            //        ID = x.ID,
            //        Code = x.Code,
            //        LastName = x.LastName,
            //        FirstName = x.FirstName,
            //        MiddleName = x.MiddleName,
            //        OrgGroupID = x.OrgGroupID,
            //        SystemUserID = x.SystemUserID,
            //        DateHired = x.DateHired,
            //        PositionID = x.PositionID,
            //        CreatedBy = x.CreatedBy
            //    }).ToList());

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x =>
                new Form
                {
                    ID = x.ID,
                    Code = x.Code,
                    OrgGroupID = x.OrgGroupID,
                    SystemUserID = x.SystemUserID,
                    DateHired = x.DateHired,
                    PositionID = x.PositionID,
                    CreatedBy = x.CreatedBy,
                    CorporateEmail = x.CorporateEmail,
                    PersonalInformation = new PersonalInformation
                    {
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        Email = x.Email
                    }
                }).ToList());
        }

        public async Task<IActionResult> GetFamilyByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetFamilyByEmployeeID(EmployeeID))
                .Select(x => new GetFamilyOutput
                {
                    Name = x.Name,
                    Relationship = x.Relationship,
                    ContactNumber = x.ContactNumber,
                    BirthDate = x.BirthDate?.ToString("MM/dd/yyyy"),
                    Occupation = x.Occupation,
                    SpouseEmployer = x.SpouseEmployer

                }).ToList());
        } 
        
        public async Task<IActionResult> GetEducationByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEducationByEmployeeID(EmployeeID))
                .Select(x => new GetEducationOutput
                {
                    School = x.School,
                    SchoolAddress = x.SchoolAddress,
                    SchoolLevelCode = x.SchoolLevelCode,
                    Course = x.Course,
                    YearFrom = x.YearFrom,
                    YearTo = x.YearTo,
                    EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                    EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode
                }).ToList());
        }

        public async Task<IActionResult> GetWorkingHistoryByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetWorkingHistoryByEmployeeID(EmployeeID))
                .Select(x => new GetWorkingHistoryOutput
                {
                    CompanyName = x.CompanyName,
                    From = x.From.ToString("MM/yyyy"),
                    To = x.To.ToString("MM/yyyy"),
                    Position = x.Position,
                    ReasonForLeaving = x.ReasonForLeaving
                }).ToList());
        }

        public async Task<IActionResult> GetEmploymentStatusByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmploymentStatusByEmployeeID(EmployeeID))
                .Select(x => new GetEmploymentStatusOutput
                {
                    EmploymentStatus = x.EmploymentStatus,
                    DateEffective = x.DateEffective.ToString("MM/dd/yyyy")
                }).ToList());
        }

        public async Task<IActionResult> UpdateOnboardingCurrentWorkflowStep(APICredentials credentials, UpdateOnboardingCurrentWorkflowStepInput param)
        {
            EmployeeOnboarding employeeOnboarding= (await _dbAccess.GetEmployeeOnboarding(param.EmployeeID));

            if (employeeOnboarding == null)
            {
                if (await _dbAccess.Post(new Data.Employee.EmployeeOnboarding { 
                    EmployeeID = param.EmployeeID,
                    WorkflowID = param.WorkflowID,
                    CurrentStepCode = param.CurrentStepCode,
                    CurrentStepDescription = param.CurrentStepDescription,
                    CurrentStepApproverRoleIDs = param.CurrentStepApproverRoleIDs,
                    DateScheduled = string.IsNullOrEmpty(param.DateScheduled) ? default(DateTime?) : DateTime.ParseExact(param.DateScheduled, "MM/dd/yyyy", null),
                    DateCompleted = string.IsNullOrEmpty(param.DateCompleted) ? default(DateTime?) : DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null),
                    Remarks = param.Remarks,
                    IsActive = true,
                    CreatedBy = credentials.UserID
                }))
                    return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
                else
                    return new OkObjectResult(MessageUtilities.PRE_ERRMSG_REC_UPDATE);
            }
            else
            { 
            
                employeeOnboarding.CurrentStepCode = param.CurrentStepCode;
                employeeOnboarding.CurrentStepDescription = param.CurrentStepDescription;
                employeeOnboarding.WorkflowStatus = param.WorkflowStatus;
                employeeOnboarding.CurrentStepApproverRoleIDs = param.CurrentStepApproverRoleIDs;
                employeeOnboarding.DateScheduled = string.IsNullOrEmpty(param.DateScheduled) ? default(DateTime?) : DateTime.ParseExact(param.DateScheduled, "MM/dd/yyyy", null);
                employeeOnboarding.DateCompleted = string.IsNullOrEmpty(param.DateCompleted) ? default(DateTime?) : DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null);
                employeeOnboarding.Remarks = param.Remarks;

                if (await _dbAccess.Put(employeeOnboarding))
                    return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
                else
                    return new OkObjectResult(MessageUtilities.PRE_ERRMSG_REC_UPDATE);
            }

        }

        public async Task<IActionResult> GetEmployeeWithSystemUserAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeWithSystemUserAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SystemUserID,
                    Description = x.FirstName
                })
            );
        }

        public async Task<IActionResult> GetByCodes(APICredentials credentials, string CodesDelimited)
        {
            List<string> CodesList = CodesDelimited == null ? new List<string>() : CodesDelimited.Split(",").ToList();

            List<Data.Employee.Employee> result = (await _dbAccess.GetByCodes(CodesList)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x => 
                new Form
                {
                    ID = x.ID,
                    Code = x.Code,
                    OrgGroupID = x.OrgGroupID,
                    PositionID = x.PositionID,
                    SystemUserID = x.SystemUserID,
                    DateHired = x.DateHired,
                    EmploymentStatus = x.EmploymentStatus,
                    CreatedBy = x.CreatedBy,
                    PersonalInformation = new PersonalInformation
                    {
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        PSGCRegionCode = x.PSGCRegionCode,
                        PSGCProvinceCode = x.PSGCProvinceCode,
                        PSGCCityMunicipalityCode = x.PSGCCityMunicipalityCode,
                        PSGCBarangayCode = x.PSGCBarangayCode,
                        BirthDate = x.BirthDate,
                        Email = x.Email,
                        CellphoneNumber = x.CellphoneNumber,
                        SSSNumber = x.SSSNumber,
                        TIN = x.TIN,
                        PhilhealthNumber = x.PhilhealthNumber,
                        PagibigNumber = x.PagibigNumber,
                        ContactPersonName = x.ContactPersonName,
                        ContactPersonNumber = x.ContactPersonNumber,
                        ContactPersonAddress = x.ContactPersonAddress,
                        ContactPersonRelationship = x.ContactPersonRelationship
                    },
                    OnboardingWorkflowID = x.OnboardingWorkflowID
                }).ToList());
        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFile> param)
        {
            
            var nationalityList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_NATIONALITY.ToString() })).Select(x => x.Value).ToList();
            var citizenshipList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_CITIZENSHIP.ToString() })).Select(x => x.Value).ToList();
            var genderList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_GENDER.ToString() })).Select(x => x.Value).ToList();
            var civilStatusList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_CIVIL_STATUS.ToString() })).Select(x => x.Value).ToList();
            var religionList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_RELIGION.ToString() })).Select(x => x.Value).ToList();
            var sssStatusList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_SSS_STAT.ToString() })).Select(x => x.Value).ToList();
            var exemptionStatusList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_EXEMPT_STAT.ToString() })).Select(x => x.Value).ToList();
            var contactPersonRelationshipList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.FAMILY_RELATIONSHIP.ToString() })).Select(x => x.Value).ToList();
            var schoolLevelList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_SCHOOL_LEVEL.ToString() })).Select(x => x.Value).ToList();
            var educationalAttainmentDegreeList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_ED_ATT_DEG.ToString() })).Select(x => x.Value).ToList();
            var educationalAttainmentStatusList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.EMP_ED_ATT_STAT.ToString() })).Select(x => x.Value).ToList();
            var companyCodeList = (await _referenceDBAccess.GetByRefCodes(new List<string> { 
                Transfer.Enums.ReferenceCodes.COMPANY_TAG.ToString() })).Select(x => x.Value).ToList();
            var orgGroupList = (await _orgGroupDBAccess.GetAll()).ToList();
            var positionList = (await _positionDBAccess.GetAll()).ToList();
            var regionPSGC = (await _PSGCDBAccess.GetAllRegion()).ToList();
            var provincePSGC = (await _PSGCDBAccess.GetAllProvince()).ToList();
            var cityMunicipalityPSGC = (await _PSGCDBAccess.GetAllCityMunicipality()).ToList();
            var barangayPSGC = (await _PSGCDBAccess.GetAllBarangay()).ToList();
            var employmentStatusList = (await _referenceDBAccess.GetByRefCodes(new List<string> {
                Transfer.Enums.ReferenceCodes.EMPLOYMENT_STATUS.ToString() })).Select(x => x.Value).ToList();

            /*Checking of required and invalid fields*/
            foreach (UploadFile obj in param)
            {
                /*OldEmployeeCode*/
                if (string.IsNullOrEmpty(obj.OldEmployeeCode))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Old Employee Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.OldEmployeeCode = obj.OldEmployeeCode.Trim();
                    if (obj.OldEmployeeCode.Length > 5)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Old Employee Code", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));
                    }
                }

                /*LastName*/
                if (string.IsNullOrEmpty(obj.LastName))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Last Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.LastName = obj.LastName.Trim();
                    if (obj.LastName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Last Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*FirstName*/
                if (string.IsNullOrEmpty(obj.FirstName))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "First Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.FirstName = obj.FirstName.Trim();
                    if (obj.FirstName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "First Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*MiddleName*/
                if (string.IsNullOrEmpty(obj.MiddleName))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Middle Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.MiddleName = obj.MiddleName.Trim();
                    if (obj.MiddleName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*Suffix*/
                if (!string.IsNullOrEmpty(obj.Suffix))
                {
                    obj.Suffix = obj.Suffix.Trim();
                    if (obj.Suffix.Length > 10)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Suffix", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));
                    }
                }

                /*Nickname*/
                if (string.IsNullOrEmpty(obj.Nickname))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Nickname ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Nickname = obj.Nickname.Trim();
                    if (obj.Nickname.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Nickname", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*Nationality*/
                if (string.IsNullOrEmpty(obj.Nationality))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Nationality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Nationality = obj.Nationality.Trim();
                    if (obj.Nationality.Length > 20)
                    { 
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Nationality", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.Nationality, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Nationality ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (nationalityList.Where(x => obj.Nationality.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Nationality ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Citizenship*/
                if (string.IsNullOrEmpty(obj.Citizenship))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Citizenship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Citizenship = obj.Citizenship.Trim();
                    if (obj.Citizenship.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Citizenship", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.Citizenship, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Citizenship ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (citizenshipList.Where(x => obj.Citizenship.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Citizenship ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*BirthDate*/
                if (string.IsNullOrEmpty(obj.BirthDate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Birth Date ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.BirthDate = obj.BirthDate.Trim();
                    if (!DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Birth Date", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.BirthDateConverted = birthDate;
                    }
                }

                /*BirthPlace*/
                if (string.IsNullOrEmpty(obj.BirthPlace))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Birth Place ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.BirthPlace = obj.BirthPlace.Trim();
                    if (obj.BirthPlace.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Birth Place", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*Gender*/
                if (string.IsNullOrEmpty(obj.Gender))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Gender ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Gender = obj.Gender.Trim();
                    if (obj.Gender.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Gender", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.Gender, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Gender ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (genderList.Where(x => obj.Gender.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Gender ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                //if (string.IsNullOrEmpty(obj.HeightCM))
                //    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Height (cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    obj.HeightCM = obj.HeightCM.Trim();
                //    if (obj.HeightCM.Length > 50)
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Height (cm)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                //    if (!decimal.TryParse(obj.HeightCM, out decimal height))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Height (cm) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //    else
                //    {
                //        if (height <= 0)
                //            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Height (cm) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                //if (string.IsNullOrEmpty(obj.WeightLBS))
                //    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Weight (lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //else
                //{
                //    obj.WeightLBS = obj.WeightLBS.Trim();
                //    if (obj.WeightLBS.Length > 50)
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Weight (lbs)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                //    if (!decimal.TryParse(obj.WeightLBS, out decimal height))
                //    {
                //        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Weight (lbs) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                //    }
                //    else
                //    {
                //        if (height <= 0)
                //            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Weight (lbs) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                //    }
                //}

                /*CivilStatus*/
                if (string.IsNullOrEmpty(obj.CivilStatus))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Civil Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.CivilStatus = obj.CivilStatus.Trim();
                    if (obj.CivilStatus.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Civil Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.CivilStatus, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Civil Status ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (civilStatusList.Where(x => obj.CivilStatus.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Civil Status ", MessageUtilities.COMPARE_INVALID));
                    }
                }
                
                /*Religion*/
                if (string.IsNullOrEmpty(obj.Religion))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Religion ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Religion = obj.Religion.Trim();
                    if (obj.Religion.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Religion", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.Religion, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Religion ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (religionList.Where(x => obj.Religion.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Religion ", MessageUtilities.COMPARE_INVALID));
                    }
                }
                
                /*SSSStatus*/
                if (string.IsNullOrEmpty(obj.SSSStatus))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.SSSStatus = obj.SSSStatus.Trim();
                    if (obj.SSSStatus.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.SSSStatus, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS Status ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (sssStatusList.Where(x => obj.SSSStatus.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS Status ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*ExemptionStatus*/
                if (string.IsNullOrEmpty(obj.ExemptionStatus))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Exemption Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.ExemptionStatus = obj.ExemptionStatus.Trim();
                    if (obj.ExemptionStatus.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Exemption Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.ExemptionStatus, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Exemption Status ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (exemptionStatusList.Where(x => obj.ExemptionStatus.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Exemption Status ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*HomeAddress*/
                if (string.IsNullOrEmpty(obj.HomeAddress))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Home Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.HomeAddress = obj.HomeAddress.Trim();
                    if (obj.HomeAddress.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Home Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*MobileNo*/
                if (string.IsNullOrEmpty(obj.MobileNo))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mobile No ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else

                {
                    obj.MobileNo = obj.MobileNo.Trim();
                    if (obj.MobileNo.Length > 13)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mobile No", MessageUtilities.COMPARE_NOT_EXCEED, "11 digits."));

                    if (!Regex.IsMatch(obj.MobileNo, RegexUtilities.REGEX_PHONE_NUMBER))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mobile No ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }

                    if (obj.MobileNo.Contains("+63"))
                        obj.MobileNo = obj.MobileNo.Replace("+63","0");
                }

                /*PSGCRegionCode*/
                if (string.IsNullOrEmpty(obj.PSGCRegionCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCRegionCode = obj.PSGCRegionCode.Trim();
                    if (obj.PSGCRegionCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCRegionCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var region = regionPSGC.Where(x => obj.PSGCRegionCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (region.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCProvinceCode*/
                if (string.IsNullOrEmpty(obj.PSGCProvinceCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCProvinceCode = obj.PSGCProvinceCode.Trim();
                    if (obj.PSGCProvinceCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCProvinceCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var province = provincePSGC.Where(x => obj.PSGCProvinceCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (province.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCCityMunicipalityCode*/
                if (string.IsNullOrEmpty(obj.PSGCCityMunicipalityCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "City/Municipality (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCCityMunicipalityCode = obj.PSGCCityMunicipalityCode.Trim();
                    if (obj.PSGCCityMunicipalityCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "City/Municipality (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCCityMunicipalityCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CityMunicipality (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var cityMunicipality = cityMunicipalityPSGC.Where(x => obj.PSGCCityMunicipalityCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (cityMunicipality.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CityMunicipality (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCBarangayCode*/
                if (string.IsNullOrEmpty(obj.PSGCBarangayCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCBarangayCode = obj.PSGCBarangayCode.Trim();
                    if (obj.PSGCBarangayCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCBarangayCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var barangay = barangayPSGC.Where(x => obj.PSGCBarangayCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (barangay.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }

                

                /*SSSNo*/
                if (string.IsNullOrEmpty(obj.SSSNo))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS No ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.SSSNo = obj.SSSNo.Trim();
                    if (obj.SSSNo.Length > 10)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "SSS No", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));
                }

                /*TIN*/
                if (string.IsNullOrEmpty(obj.TIN))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "TIN ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.TIN = obj.TIN.Trim();
                    if (obj.TIN.Length > 12)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "TIN", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));
                }

                /*PhilHealthNo*/
                if (string.IsNullOrEmpty(obj.PhilHealthNo))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "PhilHealth No ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PhilHealthNo = obj.PhilHealthNo.Trim();
                    if (obj.PhilHealthNo.Length > 12)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "PhilHealth No", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));
                }

                /*HDMFNo*/
                if (string.IsNullOrEmpty(obj.HDMFNo))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "HDMF No ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.HDMFNo = obj.HDMFNo.Trim();
                    if (obj.HDMFNo.Length > 12)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "HDMF No", MessageUtilities.COMPARE_NOT_EXCEED, "12 characters."));
                }

                /*ContactPerson*/
                if (string.IsNullOrEmpty(obj.ContactPerson))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Contact Person ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.ContactPerson = obj.ContactPerson.Trim();
                    if (obj.ContactPerson.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Contact Person", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*ContactPersonRelationship*/
                if (string.IsNullOrEmpty(obj.ContactPersonRelationship))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Contact Person Relationship ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.ContactPersonRelationship = obj.ContactPersonRelationship.Trim();
                    if (obj.ContactPersonRelationship.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Contact Person Relationship", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.ContactPersonRelationship, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Contact Person Relationship ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (contactPersonRelationshipList.Where(x => obj.ContactPersonRelationship.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Contact Person Relationship ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Spouse*/
                if (!string.IsNullOrEmpty(obj.Spouse))
                {
                    obj.Spouse = obj.Spouse.Trim();
                    if (obj.Suffix.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Spouse", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    /*SpouseBirthDate*/
                    if (string.IsNullOrEmpty(obj.SpouseBirthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Spouse Birth Date ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.SpouseBirthDate = obj.SpouseBirthDate.Trim();
                        if (!DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime spouseBirthDate))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Spouse Birth Date", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        else
                        {
                            obj.SpouseBirthDateConverted = spouseBirthDate;
                        }
                    }

                    /*SpouseEmployer*/
                    if (string.IsNullOrEmpty(obj.SpouseEmployer))
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Spouse Employer ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.SpouseEmployer = obj.SpouseEmployer.Trim();
                        if (obj.SpouseEmployer.Length > 255)
                        {
                            ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Spouse Employer", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }
                    }

                    /*SpouseOccupation*/
                    if (string.IsNullOrEmpty(obj.SpouseOccupation))
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Spouse Occupation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.SpouseOccupation = obj.SpouseOccupation.Trim();
                        if (obj.SpouseOccupation.Length > 255)
                        {
                            ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Spouse Occupation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }
                    }
                }

                /*Father*/
                if (string.IsNullOrEmpty(obj.Father))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Father ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                { 
                    obj.Father = obj.Father.Trim();
                    if (obj.Suffix.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Father", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                
                }

                /*FatherBirthDate*/
                if (string.IsNullOrEmpty(obj.FatherBirthDate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Father Birth Date ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.FatherBirthDate = obj.FatherBirthDate.Trim();
                    if (!DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime fatherBirthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Father Birth Date", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.FatherBirthDateConverted = fatherBirthDate;
                    }
                }

                /*FatherOccupation*/
                if (string.IsNullOrEmpty(obj.FatherOccupation))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Father Occupation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.FatherOccupation = obj.FatherOccupation.Trim();
                    if (obj.FatherOccupation.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Father Occupation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*Mother*/
                if (string.IsNullOrEmpty(obj.Mother))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mother ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Mother = obj.Mother.Trim();
                    if (obj.Suffix.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Mother", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }


                /*MotherBirthDate*/
                if (string.IsNullOrEmpty(obj.MotherBirthDate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mother Birth Date ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.MotherBirthDate = obj.MotherBirthDate.Trim();
                    if (!DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime motherBirthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Mother Birth Date", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.MotherBirthDateConverted = motherBirthDate;
                    }
                }

                /*MotherOccupation*/
                if (string.IsNullOrEmpty(obj.MotherOccupation))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Mother Occupation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.MotherOccupation = obj.MotherOccupation.Trim();
                    if (obj.MotherOccupation.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Mother Occupation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*School*/
                if (string.IsNullOrEmpty(obj.School))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "School ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.School = obj.School.Trim();
                    if (obj.School.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "School", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*SchoolAddress*/
                if (string.IsNullOrEmpty(obj.SchoolAddress))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "School Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.SchoolAddress = obj.SchoolAddress.Trim();
                    if (obj.SchoolAddress.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "School Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*SchoolLevel*/
                if (string.IsNullOrEmpty(obj.SchoolLevel))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.SchoolLevel = obj.SchoolLevel.Trim();
                    if (obj.SchoolLevel.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Level", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.SchoolLevel, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Level ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (schoolLevelList.Where(x => obj.SchoolLevel.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Level ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Course*/
                if (string.IsNullOrEmpty(obj.Course))
                {
                    ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Course ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Course = obj.Course.Trim();
                    if (obj.Course.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Course", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*SchoolYearFrom*/
                if (string.IsNullOrEmpty(obj.SchoolYearFrom))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Year From ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.SchoolYearFrom = obj.SchoolYearFrom.Trim();
                    if (!DateTime.TryParseExact(obj.SchoolYearFrom, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Year From", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                }

                /*SchoolYearTo*/
                if (string.IsNullOrEmpty(obj.SchoolYearTo))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Year From ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.SchoolYearTo = obj.SchoolYearTo.Trim();
                    if (!DateTime.TryParseExact(obj.SchoolYearTo, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "School Year From", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                }


                /*EducationalAttainmentDegree*/
                if (string.IsNullOrEmpty(obj.EducationalAttainmentDegree))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Degree ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.EducationalAttainmentDegree = obj.EducationalAttainmentDegree.Trim();
                    if (obj.EducationalAttainmentDegree.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Degree", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.EducationalAttainmentDegree, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Degree ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (educationalAttainmentDegreeList.Where(x => obj.EducationalAttainmentDegree.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Degree ", MessageUtilities.COMPARE_INVALID));
                    }
                }


                /*EducationalAttainmentStatus*/
                if (string.IsNullOrEmpty(obj.EducationalAttainmentStatus))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.EducationalAttainmentStatus = obj.EducationalAttainmentStatus.Trim();
                    if (obj.EducationalAttainmentStatus.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.EducationalAttainmentStatus, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Status ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (educationalAttainmentStatusList.Where(x => obj.EducationalAttainmentStatus.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Educational Attainment Status ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*CompanyCode*/
                if (string.IsNullOrEmpty(obj.CompanyCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CompanyCode ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.CompanyCode = obj.CompanyCode.Trim();
                    if (obj.CompanyCode.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CompanyCode", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.CompanyCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CompanyCode ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (companyCodeList.Where(x => obj.CompanyCode.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CompanyCode ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*BranchCode*/
                if (string.IsNullOrEmpty(obj.BranchCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Branch Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.BranchCode = obj.BranchCode.Trim();
                    if (obj.BranchCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Branch Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.BranchCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Branch Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                    var orgGroup = orgGroupList.Where(x => obj.BranchCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (orgGroup.Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Branch Code ", MessageUtilities.COMPARE_INVALID));
                    }
                    else
                    {
                        obj.BranchID = orgGroup.First().ID;
                    }
                }

                if (string.IsNullOrEmpty(obj.MonthlySalary))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.MonthlySalary = obj.MonthlySalary.Trim();

                    if (!decimal.TryParse(obj.MonthlySalary, out decimal parsed))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (parsed <= 0)
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                }

                if (string.IsNullOrEmpty(obj.DailySalary))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.DailySalary = obj.DailySalary.Trim();

                    if (!decimal.TryParse(obj.DailySalary, out decimal parsed))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Daily Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (parsed <= 0)
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                }

                if (string.IsNullOrEmpty(obj.HourlySalary))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.HourlySalary = obj.HourlySalary.Trim();

                    if (!decimal.TryParse(obj.HourlySalary, out decimal parsed))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    else
                    {
                        if (parsed <= 0)
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                }

                /*DepartmentCode*/
                if (string.IsNullOrEmpty(obj.DepartmentCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Department Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DepartmentCode = obj.DepartmentCode.Trim();
                    if (obj.DepartmentCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Department Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.DepartmentCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Department Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                    var orgGroup = orgGroupList.Where(x => obj.DepartmentCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (orgGroup.Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Department Code ", MessageUtilities.COMPARE_INVALID));
                    }
                    else
                    {
                        obj.DepartmentID = orgGroup.First().ID;
                    }
                }

                /*DesignationCode*/
                if (string.IsNullOrEmpty(obj.DesignationCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Designation Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DesignationCode = obj.DesignationCode.Trim();
                    if (obj.DesignationCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Designation Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.DesignationCode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Designation Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                    var position = positionList.Where(x => obj.DesignationCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (position.Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Designation Code ", MessageUtilities.COMPARE_INVALID));
                    }
                    else
                    {
                        obj.DesignationID = position.First().ID;
                    }
                }

                /*DateHired*/
                if (string.IsNullOrEmpty(obj.DateHired))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Hired ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DateHired = obj.DateHired.Trim();
                    if (!DateTime.TryParseExact(obj.DateHired, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateHired))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Hired", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.DateHiredConverted = dateHired;
                    }
                }


                /*PreviousEmployer*/
                if (!string.IsNullOrEmpty(obj.PreviousEmployer))
                {
                    obj.PreviousEmployer = obj.PreviousEmployer.Trim();
                    if (obj.Suffix.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Previous Employer", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    /*PreviousDesignation*/
                    if (string.IsNullOrEmpty(obj.PreviousDesignation))
                    {
                        ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Previous Designation ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.PreviousDesignation = obj.PreviousDesignation.Trim();
                        if (obj.PreviousDesignation.Length > 255)
                        {
                            ErrorMessages.Add(string.Concat("Row [" + obj.RowNum + "] : ", "Previous Designation", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                        }
                    }

                    /*YearStarted*/
                    if (string.IsNullOrEmpty(obj.YearStarted))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Year Started ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.YearStarted = obj.YearStarted.Trim();
                        if (!DateTime.TryParseExact(obj.YearStarted, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearStartedDate))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Year Started", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        else
                        {
                            obj.YearStartedConverted = yearStartedDate;
                        }
                    }

                    /*YearEnded*/
                    if (string.IsNullOrEmpty(obj.YearEnded))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Year Ended ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.YearEnded = obj.YearEnded.Trim();
                        if (!DateTime.TryParseExact(obj.YearEnded, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearEndedDate))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Year Ended", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        else
                        {
                            obj.YearEndedConverted = yearEndedDate;
                        }
                    }
                }

                /*EmploymentStatus*/
                if (string.IsNullOrEmpty(obj.EmploymentStatus))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.EmploymentStatus = obj.EmploymentStatus.Trim();
                    if (obj.EmploymentStatus.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employment Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.EmploymentStatus, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employment Status ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (employmentStatusList.Where(x => obj.EmploymentStatus.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employment Status ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                if ((await _dbAccess.GetEmployeeIfExist(obj.FirstName, obj.LastName, obj.BirthDateConverted)).Count() > 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), MessageUtilities.ERRMSG_DUPLICATE_EMPLOYEE, obj.LastName, ", ", obj.FirstName));
                }

            }

            List<string> Duplicates = new List<string>();

            /*Remove Duplicates*/
            if (ErrorMessages.Count == 0)
            {
                var tempParam = param.ToList();
                foreach (var obj in tempParam.ToList())
                {
                    /* Remove duplicates within file */
                    var duplicateWithinFile = param.Where(x =>
                    obj.OldEmployeeCode.Equals(x.OldEmployeeCode, StringComparison.OrdinalIgnoreCase) &
                    //obj.FirstName.Equals(x.FirstName, StringComparison.OrdinalIgnoreCase) &
                    //obj.MiddleName.Equals(x.MiddleName, StringComparison.OrdinalIgnoreCase) &
                    //obj.LastName.Equals(x.LastName, StringComparison.OrdinalIgnoreCase) &
                    //obj.BirthDate.Equals(x.BirthDate, StringComparison.OrdinalIgnoreCase) &
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                        Duplicates.Add("Row [" + obj.RowNum + "]");
                    }


                    /* Remove duplicates from database */
                    var duplicateFromDatabase = (await _dbAccess.GetByOldCode(obj.OldEmployeeCode)).ToList();
                    if (duplicateFromDatabase != null)
                    {
                        if (duplicateFromDatabase.Count() > 0)
                        {
                            param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        }
                    }


                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                List<Data.Employee.Employee> employees = new List<Data.Employee.Employee>();
                List<Transfer.Employee.EmployeeCompensationForm> employeeCompensation = 
                    new List<Transfer.Employee.EmployeeCompensationForm>();
                List<Transfer.Employee.EmployeeEducation> employeeEducation = 
                    new List<Transfer.Employee.EmployeeEducation>();
                List<Transfer.Employee.EmployeeFamily> employeeFamily = 
                    new List<Transfer.Employee.EmployeeFamily>();
                List<Transfer.Employee.EmployeeWorkingHistory> employeeWorkingHistory = 
                    new List<Transfer.Employee.EmployeeWorkingHistory>();


                if (param != null)
                {
                    foreach (var obj in param.OrderBy(x => x.DateHiredConverted).ToList())
                    {
                        // FOR BRANCH EMPLOYEE ASSIGN EMAIL
                        EMS.Plantilla.Transfer.OrgGroup.CorporateEmailOutput corporateEmailOutput = new EMS.Plantilla.Transfer.OrgGroup.CorporateEmailOutput();
                        var OrgCode = (await _orgGroupDBAccess.GetByID(obj.BranchID));
                        var Company = (await _orgGroupDBAccess.GetTagsByOrgGroupID(obj.BranchID)).Where(y => y.TagRefCode.Equals("COMPANY_TAG"));

                        // Generate New Employee Code
                        var NewEmployeeCodeDB = (await _dbAccess.GetNewEmployeeCode(obj.CompanyCode)).ToList();
                        string newEmployeeCode = "";
                        if (NewEmployeeCodeDB != null)
                            newEmployeeCode = NewEmployeeCodeDB.First().NewEmployeeCode;

                        obj.NewEmployeeCode = newEmployeeCode;

                        // Personal Information
                        employees.Add(new Data.Employee.Employee
                        {
                            Code = newEmployeeCode,
                            OldEmployeeID = obj.OldEmployeeCode,
                            FirstName = obj.FirstName,
                            MiddleName = obj.MiddleName,
                            LastName = obj.LastName,
                            Suffix = obj.Suffix,
                            Nickname = obj.Nickname,
                            Gender = obj.Gender,
                            NationalityCode = obj.Nationality,
                            CitizenshipCode = obj.Citizenship,
                            BirthPlace = obj.BirthPlace,
                            HeightCM = obj.HeightCM,
                            WeightLBS = obj.WeightLBS,
                            CivilStatusCode = obj.CivilStatus,
                            SSSStatusCode = obj.SSSStatus,
                            ExemptionStatusCode = obj.ExemptionStatus,
                            ReligionCode = obj.Religion,
                            OrgGroupID = obj.DepartmentID != 0 ? obj.DepartmentID : obj.BranchID,
                            PositionID = obj.DesignationID,
                            SystemUserID = 0,
                            OnboardingWorkflowID = obj.OnboardingWorkflowID,
                            CompanyTag = obj.CompanyCode,
                            Status = "", 
                            DateHired = obj.DateHiredConverted,
                            //EmploymentStatus = Transfer.Enums.EMPLOYMENT_STATUS.PROBATIONARY.ToString(),
                            EmploymentStatus = obj.EmploymentStatus,
                            BirthDate = obj.BirthDateConverted,
                            AddressLine1 = obj.HomeAddress,
                            AddressLine2 = "",
                            PSGCRegionCode = obj.PSGCRegionCode,
                            PSGCProvinceCode = obj.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = obj.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = obj.PSGCBarangayCode,
                            Email = "",
                            CellphoneNumber = obj.MobileNo,
                            SSSNumber = obj.SSSNo,
                            TIN = obj.TIN,
                            PhilhealthNumber = obj.PhilHealthNo,
                            PagibigNumber = obj.HDMFNo,
                            ContactPersonName = obj.ContactPerson,
                            ContactPersonNumber = "",
                            ContactPersonAddress = "",
                            ContactPersonRelationship = obj.ContactPersonRelationship,
                            IsActive = true,
                            CreatedBy = credentials.UserID,
                            CorporateEmail = (Company.Count() > 0 && OrgCode.OrgType == "BRN" ? String.Concat((Company.Select(x => x.TagValue).FirstOrDefault()).ToLower(), OrgCode.Code, "@motortrade.com.ph") : null)
                        });

                        // Compensation
                        employeeCompensation.Add(new EmployeeCompensationForm { 
                            UploadInsertEmployeeCode = newEmployeeCode,
                            EmployeeID = 0, // to be populated later
                            MonthlySalary = obj.MonthlySalary,
                            DailySalary = obj.DailySalary,
                            HourlySalary = obj.HourlySalary,
                        });

                        // Latest Education
                        if (!string.IsNullOrEmpty(obj.School))
                        {
                            employeeEducation.Add(new Transfer.Employee.EmployeeEducation
                            {
                                UploadInsertEmployeeCode = newEmployeeCode,
                                School = obj.School,
                                SchoolAddress = obj.SchoolAddress,
                                SchoolLevelCode = obj.SchoolLevel,
                                Course = obj.Course,
                                YearFrom = Convert.ToInt32(obj.SchoolYearFrom),
                                YearTo = Convert.ToInt32(obj.SchoolYearTo),
                                EducationalAttainmentDegreeCode = obj.EducationalAttainmentDegree,
                                EducationalAttainmentStatusCode = obj.EducationalAttainmentStatus
                            }); 
                        }

                        // Spouse
                        if (!string.IsNullOrEmpty(obj.Spouse))
                        {
                            employeeFamily.Add(new Transfer.Employee.EmployeeFamily { 
                                UploadInsertEmployeeCode = newEmployeeCode,
                                Name = obj.Spouse,
                                BirthDate = obj.SpouseBirthDateConverted ?? default,
                                ContactNumber = "",
                                CreatedBy = credentials.UserID,
                                Occupation = obj.SpouseOccupation,
                                Relationship = Enums.FAMILY_RELATIONSHIP.SPOUSE.ToString(),
                                SpouseEmployer = obj.SpouseEmployer
                            }); 
                        }

                        // Father
                        if (!string.IsNullOrEmpty(obj.Father))
                        {
                            employeeFamily.Add(new Transfer.Employee.EmployeeFamily
                            {
                                UploadInsertEmployeeCode = newEmployeeCode,
                                Name = obj.Father,
                                BirthDate = obj.FatherBirthDateConverted,
                                ContactNumber = "",
                                CreatedBy = credentials.UserID,
                                Occupation = obj.FatherOccupation,
                                Relationship = Enums.FAMILY_RELATIONSHIP.FATHER.ToString()
                            });
                        }

                        // Mother
                        if (!string.IsNullOrEmpty(obj.Mother))
                        {
                            employeeFamily.Add(new Transfer.Employee.EmployeeFamily
                            {
                                UploadInsertEmployeeCode = newEmployeeCode,
                                Name = obj.Mother,
                                BirthDate = obj.MotherBirthDateConverted,
                                ContactNumber = "",
                                CreatedBy = credentials.UserID,
                                Occupation = obj.MotherOccupation,
                                Relationship = Enums.FAMILY_RELATIONSHIP.MOTHER.ToString()
                            });
                        }

                        // Working History
                        if (!string.IsNullOrEmpty(obj.PreviousEmployer))
                        {
                            employeeWorkingHistory.Add(new Transfer.Employee.EmployeeWorkingHistory
                            {
                                UploadInsertEmployeeCode = newEmployeeCode,
                                CompanyName = obj.PreviousEmployer,
                                Position = obj.PreviousDesignation,
                                From = obj.YearStartedConverted ?? default,
                                To = obj.YearEndedConverted ?? default,
                                ReasonForLeaving = "",
                                CreatedBy = credentials.UserID,
                                
                            });
                        }

                    }

                    await _dbAccess.UploadInsert(employees, employeeCompensation, 
                        employeeEducation, employeeFamily, employeeWorkingHistory);
                }

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
            {
                if (Duplicates.Count > 0)
                {
                    return new OkObjectResult(
                        new UploadInsertOutput
                        {
                            NewSystemUsers = param?.Select(x =>
                                new UploadInsertSystemUser
                                {
                                    Username = x.NewEmployeeCode,
                                    FirstName = x.FirstName,
                                    MiddleName = x.MiddleName,
                                    LastName = x.LastName,
                                }
                            ).ToList(),
                            Message = string.Concat(param?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                            MessageUtilities.ERRMSG_DUPLICATE_EMPLOYEE, "<br>",
                            string.Join("<br>", Duplicates.Distinct().ToArray()))
                        }
                        );
                }
                else
                {
                    return new OkObjectResult(
                        new UploadInsertOutput
                        {
                            NewSystemUsers = param?.Select(x =>
                                new UploadInsertSystemUser
                                {
                                    Username = x.NewEmployeeCode,
                                    FirstName = x.FirstName,
                                    MiddleName = x.MiddleName,
                                    LastName = x.LastName,
                                }
                            ).ToList(),
                            Message = string.Concat(param?.Count, " Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD)
                        });
                }
            }
            else
            {

                if (ErrorMessages.Count > 52)
                {
                    string ErrorMessage = string.Join("<br>", ErrorMessages.Take(52).ToArray());
                    ErrorMessage += string.Concat("<br><br> ", ErrorMessages.Count - 52, " other errors found.");
                    return new BadRequestObjectResult(ErrorMessage);
                }
                else
                {
                    return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
                }
            }
        }

        public async Task<IActionResult> AutoAdd(APICredentials credentials, Form param)
        {
            //var NewEmployeeCode = (await _dbAccess.GetNewEmployeeCode(param.CompanyTag)).ToList();
            //if (NewEmployeeCode != null)
            //    param.Code = NewEmployeeCode.First().NewEmployeeCode;
            param.Code = (param.Code ?? "").Trim();
            param.OldEmployeeID = (param.OldEmployeeID ?? "").Trim();
            param.EmploymentStatus = (param.EmploymentStatus ?? "").Trim();
            param.CompanyTag = (param.CompanyTag ?? "").Trim();
            param.PersonalInformation.FirstName = (param.PersonalInformation.FirstName ?? "").Trim();
            param.PersonalInformation.LastName = (param.PersonalInformation.LastName ?? "").Trim();
            param.PersonalInformation.MiddleName = (param.PersonalInformation.MiddleName ?? "").Trim();
            param.PersonalInformation.AddressLine1 = (param.PersonalInformation.AddressLine1 ?? "").Trim();
            param.PersonalInformation.AddressLine2 = (param.PersonalInformation.AddressLine2 ?? "").Trim();
            param.PersonalInformation.Email = (param.PersonalInformation.Email ?? "").Trim();
            param.PersonalInformation.CellphoneNumber = (param.PersonalInformation.CellphoneNumber ?? "").Trim();


            //if (string.IsNullOrEmpty(param.Code))
            //    ErrorMessages.Add(string.Concat("Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    if ((await _dbAccess.GetByCode(param.Code)).Count() > 0)
            //    {
            //        ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            //    }
                if (param.Code.Length > 50)
                {
                    ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                }
            //}

            if (!string.IsNullOrEmpty(param.OldEmployeeID))
                if (param.OldEmployeeID.Length > 5)
                    ErrorMessages.Add(string.Concat("Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));

            if (string.IsNullOrEmpty(param.EmploymentStatus))
                ErrorMessages.Add(string.Concat("Employment Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.EmploymentStatus.Length > 20)
                ErrorMessages.Add(string.Concat("Employment Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            //if (string.IsNullOrEmpty(param.CompanyTag))
            //    ErrorMessages.Add(string.Concat("Company ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
                if (param.CompanyTag.Length > 20)
                ErrorMessages.Add(string.Concat("Company", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.FirstName))
                ErrorMessages.Add(string.Concat("FirstName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("FirstName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.LastName))
                ErrorMessages.Add(string.Concat("LastName ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.LastName.Length > 50)
                ErrorMessages.Add(string.Concat("LastName", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.MiddleName))
                if (param.PersonalInformation.MiddleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.AddressLine1))
                ErrorMessages.Add(string.Concat("Address Line 1 ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.AddressLine1.Length > 255)
                ErrorMessages.Add(string.Concat("Address Line 1", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.AddressLine2))
                if (param.PersonalInformation.AddressLine2.Length > 255)
                    ErrorMessages.Add(string.Concat("Address Line 2", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCRegionCode))
                ErrorMessages.Add(string.Concat("Region ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCProvinceCode))
                ErrorMessages.Add(string.Concat("Province ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCCityMunicipalityCode))
                ErrorMessages.Add(string.Concat("City/Municipality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCBarangayCode))
                ErrorMessages.Add(string.Concat("Barangay ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (string.IsNullOrEmpty(param.PersonalInformation.Email))
            //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            if (param.PersonalInformation.Email.Length > 255)
                ErrorMessages.Add(string.Concat("Email Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            //if (!RegexUtilities.IsValidEmail(param.PersonalInformation.Email))
            //{
            //    ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INVALID));
            //}

            if (string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.CellphoneNumber.Length > 11)
                ErrorMessages.Add(string.Concat("Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "11 characters."));

            if (!Regex.IsMatch(param.PersonalInformation.CellphoneNumber, RegexUtilities.REGEX_PHONE_NUMBER))
            {
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
            }

            Data.Employee.Employee result = new Data.Employee.Employee();

            if (ErrorMessages.Count == 0)
            {
                result =  await _dbAccess.AutoAdd(new Data.Employee.Employee
                {
                    Code = string.Empty,
                    OldEmployeeID = param.OldEmployeeID,
                    FirstName = param.PersonalInformation.FirstName,
                    LastName = param.PersonalInformation.LastName,
                    MiddleName = param.PersonalInformation.MiddleName,
                    OrgGroupID = param.OrgGroupID,
                    PositionID = param.PositionID,
                    SystemUserID = param.SystemUserID,
                    DateHired = param.DateHired,
                    EmploymentStatus = param.EmploymentStatus,
                    CompanyTag = string.Empty,
                    BirthDate = param.PersonalInformation.BirthDate,
                    AddressLine1 = param.PersonalInformation.AddressLine1,
                    AddressLine2 = param.PersonalInformation.AddressLine2,
                    PSGCRegionCode = param.PersonalInformation.PSGCRegionCode,
                    PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode,
                    PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode,
                    PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode,
                    Email = param.PersonalInformation.Email,
                    CellphoneNumber = param.PersonalInformation.CellphoneNumber,
                    IsActive = true,
                    CreatedBy = credentials.UserID
                }
                , new Data.Employee.EmploymentStatusHistory
                {
                    EmploymentStatus = param.EmploymentStatus,
                    DateEffective = DateTime.Now,
                    CreatedBy = credentials.UserID
                }
                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
            {
                if (result == null)
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
                else
                    //return new OkObjectResult(result);
                    return new OkObjectResult(
                    new Form
                    {
                        ID = result.ID,
                        Code = result.Code,
                        OldEmployeeID = result.OldEmployeeID,
                        OrgGroupID = result.OrgGroupID,
                        PositionID = result.PositionID,
                        SystemUserID = result.SystemUserID,
                        DateHired = result.DateHired,
                        EmploymentStatus = result.EmploymentStatus,
                        CreatedBy = result.CreatedBy,
                        PersonalInformation = new PersonalInformation
                        {
                            FirstName = result.FirstName,
                            MiddleName = result.MiddleName,
                            LastName = result.LastName,
                            AddressLine1 = result.AddressLine1,
                            AddressLine2 = result.AddressLine2,
                            PSGCRegionCode = result.PSGCRegionCode,
                            PSGCProvinceCode = result.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = result.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = result.PSGCBarangayCode,
                            BirthDate = result.BirthDate,
                            Email = result.Email,
                            CellphoneNumber = result.CellphoneNumber,
                            SSSNumber = result.SSSNumber,
                            TIN = result.TIN,
                            PhilhealthNumber = result.PhilhealthNumber,
                            PagibigNumber = result.PagibigNumber,
                            ContactPersonName = result.ContactPersonName,
                            ContactPersonNumber = result.ContactPersonNumber,
                            ContactPersonAddress = result.ContactPersonAddress,
                            ContactPersonRelationship = result.ContactPersonRelationship
                        },
                        OnboardingWorkflowID = result.OnboardingWorkflowID,
                        CompanyTag = result.CompanyTag
                    });
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> UpdateSystemUser(APICredentials credentials, UpdateSystemUserInput param)
        {
            Data.Employee.Employee form = await _dbAccess.GetByID(param.EmployeeID);

            form.SystemUserID = param.SystemUserID;
            form.ModifiedBy = credentials.UserID;
            form.ModifiedDate = DateTime.Now;

            if (await _dbAccess.Put(form))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetETFList(APICredentials credentials, GetListInput input)
        {

            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeETF> result = await _dbAccess.GetETFList(input, rowStart);

            
                return new OkObjectResult(result.Select
                (x => new GetETFListOutput
                {
                    Code = x.Code ?? "",
                    OldEmployeeID = x.OldEmployeeID ?? "",
                    LastName = x.LastName ?? "",
                    FirstName = x.FirstName ?? "",
                    MiddleName = x.MiddleName ?? "",
                    Suffix = x.Suffix ?? "",
                    Nickname = x.Nickname ?? "",
                    Nationality = x.Nationality ?? "",
                    Citizenship = x.Citizenship ?? "",
                    BirthDate = x.BirthDate ?? "",
                    BirthPlace = x.BirthPlace ?? "",
                    Gender = x.Gender ?? "",
                    Height = x.Height ?? "",
                    Weight = x.Weight ?? "",
                    CivilStatus = x.CivilStatus ?? "",
                    Religion = x.Religion ?? "",
                    SSSStatus = x.SSSStatus ?? "",
                    ExemptionStatus = x.ExemptionStatus ?? "",
                    HomeAddress = x.HomeAddress ?? "",
                    CellphoneNumber = x.CellphoneNumber ?? "",
                    HomeCity = x.HomeCity ?? "",
                    HomeRegion = x.HomeRegion ?? "",
                    PagibigNumber = String.IsNullOrEmpty(x.PagibigNumber) ? "" : String.Format("{0:####-####-####}", x.PagibigNumber),
                    SSSNumber = String.IsNullOrEmpty(x.SSSNumber) ? "" : String.Format("{0:##-#######-#}", x.SSSNumber),
                    TIN = String.IsNullOrEmpty(x.TIN) ? "" : String.Format("{0:###-###-###-###}", x.TIN),
                    PhilhealthNumber = String.IsNullOrEmpty(x.PhilhealthNumber) ? "" : String.Format("{0:####-####-####}", x.PhilhealthNumber),
                    ContactPersonName = x.ContactPersonName ?? "",
                    ContactPersonRelationship = x.ContactPersonRelationship ?? "",
                    SpouseName = x.SpouseName ?? "",
                    SpouseBirthdate = x.SpouseBirthdate ?? "",
                    SpouseEmployer = x.SpouseEmployer ?? "",
                    SpouseOccupation = x.SpouseOccupation ?? "",
                    FatherName = x.FatherName ?? "",
                    FatherBirthdate = x.FatherBirthdate ?? "",
                    FatherOccupation = x.FatherOccupation ?? "",
                    MotherName = x.MotherName ?? "",
                    MotherBirthdate = x.MotherBirthdate ?? "",
                    MotherOccupation = x.MotherOccupation ?? "",
                    School = x.School ?? "",
                    SchoolAddress = x.SchoolAddress ?? "",
                    SchoolLevel = x.SchoolLevel ?? "",
                    Course = x.Course ?? "",
                    YearFrom = x.YearFrom ?? "",
                    YearTo = x.YearTo ?? "",
                    AttainmentDegree = x.AttainmentDegree ?? "",
                    AttainmentStatus = x.AttainmentStatus ?? "",
                    CompanyCode = x.CompanyCode ?? "",
                    HomeBranch = x.HomeBranch ?? "",
                    BranchCode = x.BranchCode ?? "",
                    BranchName = x.BranchName ?? "",
                    Cluster = x.Cluster ?? "",
                    Area = x.Area ?? "",
                    Region = x.Region ?? "",
                    Zone = x.Zone ?? "",
                    MonthlySalary = x.MonthlySalary,
                    DailySalary = x.DailySalary,
                    HourlySalary = x.HourlySalary,
                    DeptCode = x.DeptCode ?? "",
                    DeptName = x.DeptName ?? "",
                    PositionCode = x.PositionCode ?? "",
                    PositionTitle = x.PositionTitle ?? "",
                    PositionLevel = x.PositionLevel ?? "",
                    JobClass = x.JobClass ?? "",
                    DateHired = x.DateHired ?? "",
                    PreviousEmployer = x.PreviousEmployer ?? "",
                    PreviousDesignation = x.PreviousDesignation ?? "",
                    YearStarted = x.YearStarted ?? "",
                    YearEnded = x.YearEnded ?? "",
                    EmploymentStatus = x.EmploymentStatus ?? ""
                }).ToList());
        }

        public async Task<IActionResult> UpdateCompensation(APICredentials credentials, EmployeeCompensationForm param)
        {

            if (param.EmployeeID == 0)
                ErrorMessages.Add(string.Concat("Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.MonthlySalary))
                ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.MonthlySalary = param.MonthlySalary.Trim();

                if (!decimal.TryParse(param.MonthlySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Monthly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }

            if (string.IsNullOrEmpty(param.DailySalary))
                ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.DailySalary = param.DailySalary.Trim();

                if (!decimal.TryParse(param.DailySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Daily Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }

            if (string.IsNullOrEmpty(param.HourlySalary))
                ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.HourlySalary = param.HourlySalary.Trim();

                if (!decimal.TryParse(param.HourlySalary, out decimal parsed))
                {
                    ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INVALID));
                }
                else
                {
                    if (parsed <= 0)
                        ErrorMessages.Add(string.Concat("Hourly Salary ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }

            if (ErrorMessages.Count == 0)
            {
                Data.Employee.EmployeeCompensation EmployeeCompensationToUpdate = (await _dbAccess.GetCompensationByEmployeeID(param.EmployeeID));
                Data.Employee.EmployeeCompensation EmployeeCompensationToAdd = null;


                if (EmployeeCompensationToUpdate != null)
                {
                    EmployeeCompensationToUpdate.MonthlySalary = Convert.ToDecimal(param.MonthlySalary);
                    EmployeeCompensationToUpdate.DailySalary = Convert.ToDecimal(param.DailySalary);
                    EmployeeCompensationToUpdate.HourlySalary = Convert.ToDecimal(param.HourlySalary);
                    EmployeeCompensationToUpdate.IsActive = true;
                    EmployeeCompensationToUpdate.ModifiedBy = credentials.UserID;
                    EmployeeCompensationToUpdate.ModifiedDate = DateTime.Now;
                }
                else
                {
                    EmployeeCompensationToAdd = new Data.Employee.EmployeeCompensation
                    {
                        EmployeeID = param.EmployeeID,
                        MonthlySalary = Convert.ToDecimal(param.MonthlySalary),
                        DailySalary = Convert.ToDecimal(param.DailySalary),
                        HourlySalary = Convert.ToDecimal(param.HourlySalary),
                        IsActive = true,
                        CreatedBy = credentials.UserID
                    };
                }

                await _dbAccess.Put(EmployeeCompensationToUpdate,EmployeeCompensationToAdd);
                _resultView.IsSuccess = true;

            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> UploadInsertUpdateSystemUser(APICredentials credentials, List<UploadInsertUpdateSystemUserInput> param)
        {
            List<Data.Employee.Employee> employees = (await _dbAccess.GetByNewCodes(param.Select(x => x.NewEmployeeCode).ToList())).ToList();

            employees = employees.Join(
                param,
                x => new { x.Code },
                y => new { Code = y.NewEmployeeCode },
                (x, y) => new { x, y }
                ).Select(x =>
                {
                    x.x.SystemUserID = x.y.SystemUserID;
                    return x.x;
                }
                ).ToList();

            if (await _dbAccess.Put(employees))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetEmployeeByUsername(APICredentials credentials, string Username)
        {
            List<TableVarEmployeeGetByID> result = (await _dbAccess.GetEmployeeByNewCode(Username)).ToList();

            return new OkObjectResult(result.Count > 0 ? new GetEmployeeByUsernameOutput
            {
                ID = result.FirstOrDefault().ID,
                Code = result.FirstOrDefault().Code,
                FirstName = result.FirstOrDefault().FirstName,
                MiddleName = result.FirstOrDefault().MiddleName,
                LastName = result.FirstOrDefault().LastName,
                OrgGroupID = result.FirstOrDefault().OrgGroupID,
                OrgGroupCode = result.FirstOrDefault().OrgGroupCode,
                OrgGroupDescription = result.FirstOrDefault().OrgGroupDescription,
                OrgGroupConcatenated = result.FirstOrDefault().OrgGroupConcatenated,
                PositionID = result.FirstOrDefault().PositionID,
                PositionCode = result.FirstOrDefault().PositionCode,
                PositionTitle = result.FirstOrDefault().PositionTitle,
                PositionConcatenated = result.FirstOrDefault().PositionConcatenated,
                Company = result.FirstOrDefault().Company,
                RefValueCompanyTag = result.FirstOrDefault().RefValueCompanyTag,
                Email = result.FirstOrDefault().Email
            } : new GetEmployeeByUsernameOutput()
            );
        }

        public async Task<IActionResult> GetByIDs(APICredentials credentials, List<int> UserIDs)
        {
            List<Data.Employee.Employee> result = (await _dbAccess.GetByIDs(UserIDs)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x =>
                new Form
                {
                    ID = x.ID,
                    Code = x.Code,
                    PositionID = x.PositionID,
                    OrgGroupID = x.OrgGroupID,
                    SystemUserID = x.SystemUserID,
                    DateHired = x.DateHired,
                    CreatedBy = x.CreatedBy,
                    CorporateEmail = x.CorporateEmail,
                    EmploymentStatus = x.EmploymentStatus,
                    PersonalInformation = new PersonalInformation
                    {
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        Email = x.Email
                    },
                    OldEmployeeID = x.OldEmployeeID
                }).ToList());
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }
        
        public async Task<IActionResult> GetLastModifiedRoving(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModifiedRoving(From, To));
        }

        public async Task<IActionResult> GetPrintCOE(APICredentials credentials, int EmployeeID, int HREmployeeID)
        {
            List<TableVarPrintCOE> result = (await _dbAccess.GetPrintCOE(EmployeeID, HREmployeeID)).ToList();

            return new OkObjectResult(result.Count > 0 ? new GetPrintCOEOutput
            {
                Content = result.First().Content,
                HRPosition = result.First().HRPosition,
                HREmployeeName = result.First().HREmployeeName

            } : new GetPrintCOEOutput()
            );
        }

        public async Task<IActionResult> GetOldEmployeeIDAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetOldEmployeeIDAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.OldEmployeeID
                })
            );
        }

        public async Task<IActionResult> GetByOldEmployeeIDs(APICredentials credentials, string CodesDelimited)
        {
            List<string> CodesList = CodesDelimited == null ? new List<string>() : CodesDelimited.Split(",").ToList();

            List<Data.Employee.Employee> result = (await _dbAccess.GetByOldEmployeeIDs(CodesList)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                    result.Select(x =>
                new Form
                {
                    ID = x.ID,
                    Code = x.Code,
                    OldEmployeeID = x.OldEmployeeID,
                    OrgGroupID = x.OrgGroupID,
                    PositionID = x.PositionID,
                    SystemUserID = x.SystemUserID,
                    DateHired = x.DateHired,
                    EmploymentStatus = x.EmploymentStatus,
                    CreatedBy = x.CreatedBy,
                    PersonalInformation = new PersonalInformation
                    {
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        PSGCRegionCode = x.PSGCRegionCode,
                        PSGCProvinceCode = x.PSGCProvinceCode,
                        PSGCCityMunicipalityCode = x.PSGCCityMunicipalityCode,
                        PSGCBarangayCode = x.PSGCBarangayCode,
                        BirthDate = x.BirthDate,
                        Email = x.Email,
                        CellphoneNumber = x.CellphoneNumber,
                        SSSNumber = x.SSSNumber,
                        TIN = x.TIN,
                        PhilhealthNumber = x.PhilhealthNumber,
                        PagibigNumber = x.PagibigNumber,
                        ContactPersonName = x.ContactPersonName,
                        ContactPersonNumber = x.ContactPersonNumber,
                        ContactPersonAddress = x.ContactPersonAddress,
                        ContactPersonRelationship = x.ContactPersonRelationship
                    },
                    OnboardingWorkflowID = x.OnboardingWorkflowID
                }).ToList());
        }

        public async Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeAttachmentForm param)
        {

            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.AddAttachmentForm?.Count > 0)
            {
                foreach (var item in param.AddAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.Description))
                        ErrorMessages.Add(string.Concat("Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.Description = item.Description.Trim();
                        if (item.Description.Length > 255)
                            ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }


                    if (string.IsNullOrEmpty(item.SourceFile))
                        ErrorMessages.Add(string.Concat("SourceFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.SourceFile = item.SourceFile.Trim();
                        if (item.SourceFile.Length > 255)
                            ErrorMessages.Add(string.Concat("SourceFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (string.IsNullOrEmpty(item.ServerFile))
                        ErrorMessages.Add(string.Concat("ServerFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.ServerFile = item.ServerFile.Trim();
                        if (item.ServerFile.Length > 255)
                            ErrorMessages.Add(string.Concat("ServerFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }
            }
            else
            {
                ErrorMessages.Add(string.Concat("AddAttachmentForm ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            if (param.DeleteAttachmentForm?.Count > 0)
            {
                foreach (var item in param.DeleteAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.ServerFile))
                        ErrorMessages.Add(string.Concat("ServerFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.ServerFile = item.ServerFile.Trim();
                        if (item.ServerFile.Length > 255)
                            ErrorMessages.Add(string.Concat("ServerFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }
            }

            if (ErrorMessages.Count == 0)
            {
                List<EmployeeAttachment> GetToAdd(List<EmployeeAttachment> left, List<EmployeeAttachment> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.EmployeeID, x.ServerFile },
                        y => new { y.EmployeeID, y.ServerFile },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new EmployeeAttachment
                        {
                            EmployeeID = x.newSet.newSet.EmployeeID,
                            Description = x.newSet.newSet.Description,
                            SourceFile = x.newSet.newSet.SourceFile,
                            ServerFile = x.newSet.newSet.ServerFile,
                            CreatedBy = credentials.UserID
                        })
                    .ToList();
                }

                List<EmployeeAttachment> GetToUpdate(List<EmployeeAttachment> left, List<EmployeeAttachment> right)
                {
                    return left.Join(
                    right,
                    x => new { x.EmployeeID, x.ServerFile },
                    y => new { y.EmployeeID, y.ServerFile },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.Description.Equals(x.newSet.Description)
                        || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                    )
                    .Select(y => new EmployeeAttachment
                    {
                        ID = y.oldSet.ID,
                        EmployeeID = y.newSet.EmployeeID,
                        Description = y.newSet.Description,
                        SourceFile = y.newSet.SourceFile,
                        ServerFile = y.newSet.ServerFile,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    })
                    .ToList();
                }

                List<EmployeeAttachment> GetToDelete(List<EmployeeAttachment> left, List<EmployeeAttachment> right)
                {
                    return left.GroupJoin(
                            right,
                            x => new { x.EmployeeID, x.ServerFile },
                            y => new { y.EmployeeID, y.ServerFile },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new EmployeeAttachment
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
                }

                List<EmployeeAttachment> oldSet = (await _dbAccess.GetEmployeeAttachment(param.EmployeeID)).ToList();
                List<EmployeeAttachment> paramAttachment =
                    param.AddAttachmentForm.Select(x => new EmployeeAttachment
                    {
                        EmployeeID = param.EmployeeID,
                        Description = x.Description,
                        ServerFile = x.ServerFile,
                        SourceFile = x.SourceFile
                    }).ToList();

                List<EmployeeAttachment> ValueToAdd = GetToAdd(paramAttachment, oldSet).ToList();
                List<EmployeeAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachment).ToList();
                List<EmployeeAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachment).ToList();

                List<EmployeeAttachment> addAttachment = new List<EmployeeAttachment>();
                foreach (var item in param.AddAttachmentForm)
                {
                    addAttachment.Add(new EmployeeAttachment
                    {
                        EmployeeID = param.EmployeeID,
                        Description = item.Description,
                        ServerFile = item.ServerFile,
                        SourceFile = item.SourceFile,
                        CreatedBy = credentials.UserID,
                    });
                }

                await _dbAccess.PostEmployeeAttachment(ValueToAdd, ValueToUpdate, ValueToDelete);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetEmployeeAttachment(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeAttachment(ID))
                .Select(x => new AttachmentForm
                {
                    Description = x.Description,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile,
                    Timestamp = x.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    CreatedBy = x.CreatedBy
                }));
        }
        public async Task<IActionResult> PostEmployeeSkills(APICredentials credentials, EmployeeSkillsForm param)
        {
            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.SkillsCode))
                ErrorMessages.Add(string.Concat("Skills Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.SkillsDescription))
                ErrorMessages.Add(string.Concat("Skills Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.Rate))
                ErrorMessages.Add(string.Concat("Rate ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostEmployeeSkills(new Data.Employee.EmployeeSkills
                {
                    EmployeeID = param.EmployeeID,
                    SkillsCode = param.SkillsCode,
                    SkillsDescription = param.SkillsDescription,
                    Rate = param.Rate,
                    Remarks = param.Remarks,
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> PutEmployeeSkills(APICredentials credentials, EmployeeSkillsForm param)
        {
            if (string.IsNullOrEmpty(param.SkillsCode))
                ErrorMessages.Add(string.Concat("Skills Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.SkillsDescription))
                ErrorMessages.Add(string.Concat("Skills Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (string.IsNullOrEmpty(param.Rate))
                ErrorMessages.Add(string.Concat("Rate ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                EmployeeSkills employeeSkills = (await _dbAccess.GetEmployeeSkillsById(param.ID));

                employeeSkills.SkillsCode=param.SkillsCode;
                employeeSkills.SkillsDescription = param.SkillsDescription;
                employeeSkills.Rate = param.Rate;
                employeeSkills.Remarks = param.Remarks;
                employeeSkills.IsActive = param.IsActive;
                employeeSkills.ModifiedBy = param.ModifiedBy;
                employeeSkills.ModifiedDate = DateTime.Now;
                

                if (await _dbAccess.PutEmployeeSkills(employeeSkills))
                    _resultView.IsSuccess = true;
                else
                    _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> GetEmployeeSkillsById(APICredentials credentials, int Id)
        {
            var result = (await _dbAccess.GetEmployeeSkillsById(Id));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(result);
        }
        public async Task<IActionResult> GetEmployeeSkillsByEmployeeId(APICredentials credentials, EmployeeSkillsFormInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeSkills> result = await _dbAccess.GetEmployeeSkillsByEmployeeId(input, rowStart);
            return new OkObjectResult(result.Select(x => new EmployeeSkillsFormOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                EmployeeID = x.EmployeeID,
                SkillsCode = x.SkillsCode,
                SkillsDescription = x.SkillsDescription,
                Rate = x.Rate,
                Remarks = x.Remarks,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate=x.ModifiedDate
            }).ToList());
        }

        // LOGIN VIA EXTERNAL CLEARANCE FORM
        public async Task<IActionResult> GetExternalEmployeeDetails(APICredentials credentials, ExternalLogin param)
        {
            var result = (await _dbAccess.GetExternalEmployeeDetails(param)).ToList();
            /*var EmployeeDetails = result
                .Select(x=> new { 
                    Id = x.ID,
                    SystemAccessId = x.SystemUserID,
                    EmployeeId = x.Code,
                    Firstname=x.FirstName,
                    Middlename=x.MiddleName,
                    Lastname=x.LastName,
                    Email = x.Email,
                    Position=x.PositionID,
                    Company=x.CompanyTag
                }).FirstOrDefault();*/

            if (result.Count() > 0)
                return new OkObjectResult(result.FirstOrDefault());
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
        }

        public async Task<IActionResult> GetCorporateEmailList(APICredentials credentials, GetListCorporateEmailInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeCorporateEmail> result = await _dbAccess.GetCorporateEmailList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListCorporateEmailOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Name = x.Name,
                OrgGroup = x.OrgGroup,
                Position = x.Position,
                EmploymentStatus = x.EmploymentStatus,
                CorporateEmail = x.CorporateEmail,
                OfficeMobile = x.OfficeMobile,
                IsDisplayDirectory = x.IsDisplayDirectory,
                OldEmployeeID = x.OldEmployeeID
            }).ToList());
        }
        public async Task<IActionResult> UpdateEmployeeEmail(UpdateEmployeeEmailInput param)
        {
            Data.Employee.Employee employeeData = await _dbAccess.GetByID(param.ID);
            employeeData.Email = param.Email;

            if (await _dbAccess.Put(employeeData))
                _resultView.IsSuccess = true;
            else
                _resultView.IsSuccess = false;

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> GetEmployeeByBirthday()
        {
            var Month = DateTime.Now.ToString("MM");
            return new OkObjectResult((await _dbAccess.GetAll()).Where(x=>x.BirthDate.ToString("MM").Equals(Month) && x.IsActive).ToList());
        }
        public async Task<IActionResult> GetEmployeeEvaluation()
        {
            /*var DateNow = (DateTime.Now).ToString("yyyyMM");
            DateTime date = DateTime.ParseExact(DateNow, "yyyyMM", CultureInfo.InvariantCulture).AddMonths(-4);
            String newDate = date.ToString("yyyyMM");*/

            var DateNow = DateTime.Now;
            var date = DateNow.AddDays(-110);
            String newDate = date.ToString("yyyyMMdd");

            return new OkObjectResult(await _dbAccess.GetEmployeeEvaluation(newDate));
        }
        public async Task<IActionResult> PostUpdateProfile(APICredentials credentials, Form param)
        {
            var Emp = _dbAccess.GetByID(param.ID).Result;
            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostUpdateProfile(new Data.Employee.UpdateEmployee
                {
                    Code = Emp.Code,
                    OldEmployeeID = param.OldEmployeeID,
                    FirstName = param.PersonalInformation.FirstName,
                    LastName = param.PersonalInformation.LastName,
                    MiddleName = param.PersonalInformation.MiddleName,
                    Suffix = param.PersonalInformation.Suffix,
                    Nickname = param.PersonalInformation.Nickname,
                    Gender = param.PersonalInformation.Gender,
                    NationalityCode = param.PersonalInformation.NationalityCode,
                    CitizenshipCode = param.PersonalInformation.CitizenshipCode,
                    HeightCM = param.PersonalInformation.HeightCM,
                    WeightLBS = param.PersonalInformation.WeightLBS,
                    BirthPlace = param.PersonalInformation.BirthPlace,
                    SSSStatusCode = param.PersonalInformation.SSSStatusCode,
                    ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode,
                    CivilStatusCode = param.PersonalInformation.CivilStatusCode,
                    ReligionCode = param.PersonalInformation.ReligionCode,
                    OnboardingWorkflowID = Emp.OnboardingWorkflowID,
                    OrgGroupID = Emp.OrgGroupID,
                    PositionID = Emp.PositionID,
                    HomeBranchID = Emp.HomeBranchID,
                    //SystemUserID = param.SystemUserID,
                    DateHired = Emp.DateHired,
                    EmploymentStatus = Emp.EmploymentStatus,
                    CompanyTag = Emp.CompanyTag,
                    BirthDate = param.PersonalInformation.BirthDate,
                    AddressLine1 = param.PersonalInformation.AddressLine1,
                    AddressLine2 = param.PersonalInformation.AddressLine2,
                    PSGCRegionCode = param.PersonalInformation.PSGCRegionCode,
                    PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode,
                    PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode,
                    PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode,
                    Email = param.PersonalInformation.Email,
                    CorporateEmail = Emp.CorporateEmail,
                    OfficeMobile = Emp.OfficeMobile,
                    OfficeLandline = Emp.OfficeLandline,
                    CellphoneNumber = param.PersonalInformation.CellphoneNumber,
                    SSSNumber = param.PersonalInformation.SSSNumber,
                    TIN = param.PersonalInformation.TIN,
                    PhilhealthNumber = param.PersonalInformation.PhilhealthNumber,
                    PagibigNumber = param.PersonalInformation.PagibigNumber,
                    ContactPersonName = param.PersonalInformation.ContactPersonName,
                    ContactPersonNumber = param.PersonalInformation.ContactPersonNumber,
                    ContactPersonAddress = param.PersonalInformation.ContactPersonAddress,
                    ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship,
                    IsActive = true,
                    CreatedBy = param.CreatedBy
                }
                , param.EmployeeFamilyList?.Select(x => new Data.Employee.UpdateEmployeeFamily
                {
                    Name = x.Name,
                    Relationship = x.Relationship,
                    BirthDate = x.BirthDate,
                    Occupation = x.Occupation,
                    SpouseEmployer = x.SpouseEmployer,
                    ContactNumber = x.ContactNumber,
                    CreatedBy = param.CreatedBy
                }).ToList()
                , param.EmployeeWorkingHistoryList?.Select(x => new Data.Employee.UpdateEmployeeWorkingHistory
                {
                    CompanyName = x.CompanyName,
                    From = x.From,
                    To = x.From,
                    Position = x.Position,
                    ReasonForLeaving = x.ReasonForLeaving,
                    CreatedBy = param.CreatedBy
                }).ToList()
                , param.EmployeeEducationList?.Select(x => new Data.Employee.UpdateEmployeeEducation
                {
                    School = x.School,
                    SchoolAddress = x.SchoolAddress,
                    SchoolLevelCode = x.SchoolLevelCode,
                    Course = x.Course,
                    YearFrom = x.YearFrom,
                    YearTo = x.YearTo,
                    EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                    EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                    IsActive = true,
                    CreatedBy = param.CreatedBy

                }).ToList()
                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(
                    string.Concat(MessageUtilities.SCSSMSG_REC_SAVE));
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }
        public async Task<IActionResult> GetEmail(GetEmailInput param)
        {
            return new OkObjectResult(await _dbAccess.GetEmail(param));
        }
        public async Task<IActionResult> GetEmployeeIDDescendant(int EmployeeID)
        {
            return new OkObjectResult((await _dbAccess.GetEmployeeIDDescendant(EmployeeID)));
        }
        public async Task<IActionResult> PutConvertNewEmployee(APICredentials credentials, NewEmployeeForm param)
        {
            var employee = (await _dbAccess.GetByID(param.ID));

            var NewEmployeeCode = (await _dbAccess.GetNewEmployeeCode(param.CompanyTag)).ToList();
            if (NewEmployeeCode != null)
                employee.Code = NewEmployeeCode.First().NewEmployeeCode;

            employee.CompanyTag = param.CompanyTag;
            employee.OldEmployeeID = param.OldEmployeeID;
            employee.Email = param.Email;
            employee.ModifiedBy = credentials.UserID;
            employee.ModifiedDate = DateTime.Now;

            var Result = (await _dbAccess.Put(employee));
            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_SAVE);
        }
        public async Task<IActionResult> PutConvertNewEmployees(APICredentials credentials, List<NewEmployeeForm> param)
        {
            var employee = (await _dbAccess.GetByIDs(param.Select(x=>x.ID).ToList())).ToList();

            foreach (var item in param)
            {
                var NewEmployeeCode = (await _dbAccess.GetNewEmployeeCode(item.CompanyTag)).ToList();
                item.Code = NewEmployeeCode.First().NewEmployeeCode;
            };

            var updateEmployee = (from left in employee
                                  join right in param on left.ID equals right.ID
                                  select new EMS.Plantilla.Data.Employee.Employee()
                                  {
                                      ID = left.ID,
                                      Code = right.Code,
                                      OldEmployeeID = right.OldEmployeeID,
                                      FirstName = left.FirstName,
                                      LastName = left.LastName,
                                      MiddleName = left.MiddleName,
                                      Suffix = left.Suffix,
                                      Nickname = left.Nickname,
                                      NationalityCode = left.NationalityCode,
                                      CitizenshipCode = left.CitizenshipCode,
                                      OrgGroupID = left.OrgGroupID,
                                      PositionID = left.PositionID,
                                      HomeBranchID = left.HomeBranchID,
                                      Gender = left.Gender,
                                      CivilStatusCode = left.CivilStatusCode,
                                      SSSStatusCode = left.SSSStatusCode,
                                      ExemptionStatusCode = left.ExemptionStatusCode,
                                      ReligionCode = left.ReligionCode,
                                      HeightCM = left.HeightCM,
                                      WeightLBS = left.WeightLBS,
                                      SystemUserID = left.SystemUserID,
                                      OnboardingWorkflowID = left.OnboardingWorkflowID,
                                      CompanyTag = right.CompanyTag,
                                      DateHired = left.DateHired,
                                      EmploymentStatus = left.EmploymentStatus,
                                      BirthDate = left.BirthDate,
                                      BirthPlace = left.BirthPlace,
                                      AddressLine1 = left.AddressLine1,
                                      AddressLine2 = left.AddressLine2,
                                      PSGCRegionCode = left.PSGCRegionCode,
                                      PSGCProvinceCode = left.PSGCProvinceCode,
                                      PSGCCityMunicipalityCode = left.PSGCCityMunicipalityCode,
                                      PSGCBarangayCode = left.PSGCBarangayCode,
                                      Email = right.Email,
                                      CorporateEmail = left.CorporateEmail,
                                      OfficeMobile = left.OfficeMobile,
                                      OfficeLandline = left.OfficeLandline,
                                      CellphoneNumber = left.CellphoneNumber,
                                      SSSNumber = left.SSSNumber,
                                      TIN = left.TIN,
                                      PhilhealthNumber = left.PhilhealthNumber,
                                      PagibigNumber = left.PagibigNumber,
                                      ContactPersonName = left.ContactPersonName,
                                      ContactPersonNumber = left.ContactPersonNumber,
                                      ContactPersonAddress = left.ContactPersonAddress,
                                      ContactPersonRelationship = left.ContactPersonRelationship,
                                      IsDisplayDirectory = left.IsDisplayDirectory,
                                      IsActive = left.IsActive,
                                      CreatedBy = left.CreatedBy,
                                      CreatedDate = left.CreatedDate,
                                      ModifiedBy = credentials.UserID,
                                      ModifiedDate = DateTime.Now
                                  }).ToList();

            await (_dbAccess.PutEmployees(updateEmployee));

            return new OkObjectResult(updateEmployee);
        }

        public async Task<IActionResult> PutEmployeeDetails(APICredentials credentials, Form param)
        {
            param.PersonalInformation.FirstName = (param.PersonalInformation.FirstName ?? "").Trim();
            param.PersonalInformation.LastName = (param.PersonalInformation.LastName ?? "").Trim();
            param.PersonalInformation.MiddleName = (param.PersonalInformation.MiddleName ?? "").Trim();
            param.PersonalInformation.AddressLine1 = (param.PersonalInformation.AddressLine1 ?? "").Trim();
            param.PersonalInformation.AddressLine2 = (param.PersonalInformation.AddressLine2 ?? "").Trim();
            param.PersonalInformation.Email = (param.PersonalInformation.Email ?? "").Trim();
            param.PersonalInformation.CellphoneNumber = (param.PersonalInformation.CellphoneNumber ?? "").Trim();
            param.PersonalInformation.SSSNumber = (param.PersonalInformation.SSSNumber ?? "").Trim();
            param.PersonalInformation.TIN = (param.PersonalInformation.TIN ?? "").Trim();
            param.PersonalInformation.PagibigNumber = (param.PersonalInformation.PagibigNumber ?? "").Trim();
            param.PersonalInformation.PhilhealthNumber = (param.PersonalInformation.PhilhealthNumber ?? "").Trim();

            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToDelete(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeFamily
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToAdd(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeFamily
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                Name = x.newSet.newSet.Name,
                                Relationship = x.newSet.newSet.Relationship,
                                BirthDate = x.newSet.newSet.BirthDate,
                                Occupation = x.newSet.newSet.Occupation,
                                SpouseEmployer = x.newSet.newSet.SpouseEmployer,
                                ContactNumber = x.newSet.newSet.ContactNumber,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeFamily> GetFamilyToUpdate(List<Data.Employee.EmployeeFamily> left, List<Data.Employee.EmployeeFamily> right)
                {
                    return left.Join(
                        right,
                             x => new { x.EmployeeID, x.Name, x.Relationship },
                             y => new { y.EmployeeID, y.Name, y.Relationship },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Name.Equals(x.newSet.Name) ||
                                    !x.oldSet.Relationship.Equals(x.newSet.Relationship) ||
                                    (x.oldSet.ContactNumber != x.newSet.ContactNumber) ||
                                    (x.oldSet.BirthDate != x.newSet.BirthDate) ||
                                    (x.oldSet.Occupation != x.newSet.Occupation)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeFamily
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                Name = y.newSet.Name,
                                Relationship = y.newSet.Relationship,
                                BirthDate = y.newSet.BirthDate,
                                Occupation = y.newSet.Occupation,
                                SpouseEmployer = y.newSet.SpouseEmployer,
                                ContactNumber = y.newSet.ContactNumber,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToDelete(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToAdd(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                CompanyName = x.newSet.newSet.CompanyName,
                                From = x.newSet.newSet.From,
                                To = x.newSet.newSet.To,
                                Position = x.newSet.newSet.Position,
                                ReasonForLeaving = x.newSet.newSet.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeWorkingHistory> GetWorkingHistoryToUpdate(List<Data.Employee.EmployeeWorkingHistory> left, List<Data.Employee.EmployeeWorkingHistory> right)
                {
                    return left.Join(
                        right,
                             x => new { x.EmployeeID, x.CompanyName, x.From, x.To, x.Position },
                             y => new { y.EmployeeID, y.CompanyName, y.From, y.To, y.Position },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.CompanyName.Equals(x.newSet.CompanyName) ||
                                    (x.oldSet.From != x.newSet.From) ||
                                    (x.oldSet.To != x.newSet.To) ||
                                    (x.oldSet.Position != x.newSet.Position) ||
                                    (x.oldSet.ReasonForLeaving != x.newSet.ReasonForLeaving)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeWorkingHistory
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                CompanyName = y.newSet.CompanyName,
                                From = y.newSet.From,
                                To = y.newSet.To,
                                Position = y.newSet.Position,
                                ReasonForLeaving = y.newSet.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToDelete(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.EmployeeID, x.SchoolLevelCode },
                             y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeEducation
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToAdd(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return right.GroupJoin(
                        left,
                        x => new { x.EmployeeID, x.SchoolLevelCode },
                        y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                School = x.newSet.newSet.School,
                                SchoolAddress = x.newSet.newSet.SchoolAddress,
                                SchoolLevelCode = x.newSet.newSet.SchoolLevelCode,
                                Course = x.newSet.newSet.Course,
                                YearFrom = x.newSet.newSet.YearFrom,
                                YearTo = x.newSet.newSet.YearTo,
                                EducationalAttainmentDegreeCode = x.newSet.newSet.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.newSet.newSet.EducationalAttainmentStatusCode,

                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                IEnumerable<Data.Employee.EmployeeEducation> GetEducationToUpdate(List<Data.Employee.EmployeeEducation> left, List<Data.Employee.EmployeeEducation> right)
                {
                    return left.Join(
                        right,
                            x => new { x.EmployeeID, x.SchoolLevelCode },
                            y => new { y.EmployeeID, y.SchoolLevelCode },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.School.Equals(x.newSet.School) ||
                                    (x.oldSet.SchoolAddress != x.newSet.SchoolAddress) ||
                                    (x.oldSet.Course != x.newSet.Course) ||
                                    (x.oldSet.YearFrom != x.newSet.YearFrom) ||
                                    (x.oldSet.YearTo != x.newSet.YearTo) ||
                                    (x.oldSet.EducationalAttainmentDegreeCode != x.newSet.EducationalAttainmentDegreeCode) ||
                                    (x.oldSet.EducationalAttainmentStatusCode != x.newSet.EducationalAttainmentStatusCode)
                              )
                        .Select(y =>
                            new Data.Employee.EmployeeEducation
                            {
                                ID = y.oldSet.ID,
                                EmployeeID = y.newSet.EmployeeID,
                                School = y.newSet.School,
                                SchoolAddress = y.newSet.SchoolAddress,
                                SchoolLevelCode = y.newSet.SchoolLevelCode,
                                Course = y.newSet.Course,
                                YearFrom = y.newSet.YearFrom,
                                YearTo = y.newSet.YearTo,
                                EducationalAttainmentDegreeCode = y.newSet.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = y.newSet.EducationalAttainmentStatusCode,
                                CreatedBy = credentials.UserID
                            }).ToList();
                }

                //Employee Family
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToAdd = new List<Data.Employee.EmployeeFamily>();
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToUpdate = new List<Data.Employee.EmployeeFamily>();
                List<Data.Employee.EmployeeFamily> EmployeeFamilyToDelete = new List<Data.Employee.EmployeeFamily>();
                if (param.IsViewedFamilyBackground)
                {
                    List<Data.Employee.EmployeeFamily> OldEmployeeFamily = (await _dbAccess.GetFamilyByEmployeeID(param.ID)).ToList();

                    EmployeeFamilyToAdd = GetFamilyToAdd(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                            BirthDate = x.BirthDate,
                            Occupation = x.Occupation,
                            SpouseEmployer = x.SpouseEmployer,
                            ContactNumber = x.ContactNumber,
                            CreatedBy = credentials.UserID
                        }).ToList()).ToList();

                    EmployeeFamilyToUpdate = GetFamilyToUpdate(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                            BirthDate = x.BirthDate,
                            Occupation = x.Occupation,
                            SpouseEmployer = x.SpouseEmployer,
                            ContactNumber = x.ContactNumber,
                            CreatedBy = x.CreatedBy
                        }).ToList()).ToList();

                    EmployeeFamilyToDelete = GetFamilyToDelete(OldEmployeeFamily,
                        param.EmployeeFamilyList.Select(x => new Data.Employee.EmployeeFamily
                        {
                            EmployeeID = param.ID,
                            Name = x.Name,
                            Relationship = x.Relationship,
                        }).ToList()).ToList();

                }

                //Employee Working History
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToAdd = new List<Data.Employee.EmployeeWorkingHistory>();
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToUpdate = new List<Data.Employee.EmployeeWorkingHistory>();
                List<Data.Employee.EmployeeWorkingHistory> WorkingHistoryToDelete = new List<Data.Employee.EmployeeWorkingHistory>();

                if (param.IsViewedWorkingHistory)
                {
                    if (param.EmployeeWorkingHistoryList != null)
                    {
                        List<Data.Employee.EmployeeWorkingHistory> OldWorkingHistory = (await _dbAccess.GetWorkingHistoryByEmployeeID(param.ID)).ToList();

                        WorkingHistoryToAdd = GetWorkingHistoryToAdd(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position,
                                ReasonForLeaving = x.ReasonForLeaving,
                                CreatedBy = credentials.UserID
                            }).ToList()).ToList();

                        WorkingHistoryToUpdate = GetWorkingHistoryToUpdate(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position,
                                ReasonForLeaving = x.ReasonForLeaving,
                                CreatedBy = x.CreatedBy
                            }).ToList()).ToList();

                        WorkingHistoryToDelete = GetWorkingHistoryToDelete(OldWorkingHistory,
                            param.EmployeeWorkingHistoryList.Select(x => new Data.Employee.EmployeeWorkingHistory
                            {
                                EmployeeID = param.ID,
                                CompanyName = x.CompanyName,
                                From = x.From,
                                To = x.To,
                                Position = x.Position
                            }).ToList()).ToList();
                    }
                }

                //Employee Working History
                List<Data.Employee.EmployeeEducation> EducationToAdd = new List<Data.Employee.EmployeeEducation>();
                List<Data.Employee.EmployeeEducation> EducationToUpdate = new List<Data.Employee.EmployeeEducation>();
                List<Data.Employee.EmployeeEducation> EducationToDelete = new List<Data.Employee.EmployeeEducation>();

                if (param.IsViewedEducation)
                {
                    if (param.EmployeeEducationList != null)
                    {
                        List<Data.Employee.EmployeeEducation> OldEducation = (await _dbAccess.GetEducationByEmployeeID(param.ID)).ToList();

                        EducationToAdd = GetEducationToAdd(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                                CreatedBy = credentials.UserID
                            }).ToList()).ToList();

                        EducationToUpdate = GetEducationToUpdate(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                            }).ToList()).ToList();

                        EducationToDelete = GetEducationToDelete(OldEducation,
                            param.EmployeeEducationList.Select(x => new Data.Employee.EmployeeEducation
                            {
                                EmployeeID = param.ID,
                                School = x.School,
                                SchoolAddress = x.SchoolAddress,
                                Course = x.Course,
                                SchoolLevelCode = x.SchoolLevelCode,
                                YearFrom = x.YearFrom,
                                YearTo = x.YearTo,
                                EducationalAttainmentDegreeCode = x.EducationalAttainmentDegreeCode,
                                EducationalAttainmentStatusCode = x.EducationalAttainmentStatusCode,
                            }).ToList()).ToList();
                    }
                }

                Data.Employee.Employee employeeData = await _dbAccess.GetByID(param.ID);
                Data.Employee.EmploymentStatusHistory employmentStatus = new Data.Employee.EmploymentStatusHistory();

                //employeeData.Code = param.Code;
                //employeeData.OldEmployeeID = param.OldEmployeeID;
                employeeData.FirstName = param.PersonalInformation.FirstName;
                employeeData.MiddleName = param.PersonalInformation.MiddleName;
                employeeData.LastName = param.PersonalInformation.LastName;
                employeeData.Suffix = param.PersonalInformation.Suffix;
                employeeData.Nickname = param.PersonalInformation.Nickname;
                employeeData.NationalityCode = param.PersonalInformation.NationalityCode;
                employeeData.CitizenshipCode = param.PersonalInformation.CitizenshipCode;
                //employeeData.OrgGroupID = param.OrgGroupID;
                //employeeData.PositionID = param.PositionID;
                //employeeData.HomeBranchID = param.HomeBranchID;
                employeeData.Gender = param.PersonalInformation.Gender;
                employeeData.CivilStatusCode = param.PersonalInformation.CivilStatusCode;
                employeeData.SSSStatusCode = param.PersonalInformation.SSSStatusCode;
                employeeData.ExemptionStatusCode = param.PersonalInformation.ExemptionStatusCode;
                employeeData.ReligionCode = param.PersonalInformation.ReligionCode;
                employeeData.HeightCM = param.PersonalInformation.HeightCM;
                employeeData.WeightLBS = param.PersonalInformation.WeightLBS;
                //employeeData.SystemUserID = param.SystemUserID;
                //employeeData.OnboardingWorkflowID = param.OnboardingWorkflowID;
                //employeeData.CompanyTag = param.CompanyTag;
                //employeeData.DateHired = param.DateHired;
                //employeeData.EmploymentStatus = param.EmploymentStatus;
                employeeData.BirthDate = param.PersonalInformation.BirthDate;
                employeeData.BirthPlace = param.PersonalInformation.BirthPlace;
                employeeData.AddressLine1 = param.PersonalInformation.AddressLine1;
                employeeData.AddressLine2 = param.PersonalInformation.AddressLine2;
                employeeData.PSGCRegionCode = param.PersonalInformation.PSGCRegionCode;
                employeeData.PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode;
                employeeData.PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode;
                employeeData.PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode;
                employeeData.Email = param.PersonalInformation.Email;
                //employeeData.CorporateEmail = param.CorporateEmail;
                //employeeData.OfficeMobile = param.OfficeMobile;
                //employeeData.OfficeLandline = param.OfficeLandline;
                employeeData.CellphoneNumber = param.PersonalInformation.CellphoneNumber;
                employeeData.SSSNumber = param.PersonalInformation.SSSNumber;
                employeeData.TIN = param.PersonalInformation.TIN;
                employeeData.PhilhealthNumber = param.PersonalInformation.PhilhealthNumber;
                employeeData.PagibigNumber = param.PersonalInformation.PagibigNumber;
                employeeData.ContactPersonName = param.PersonalInformation.ContactPersonName;
                employeeData.ContactPersonNumber = param.PersonalInformation.ContactPersonNumber;
                employeeData.ContactPersonAddress = param.PersonalInformation.ContactPersonAddress;
                employeeData.ContactPersonRelationship = param.PersonalInformation.ContactPersonRelationship;
                //employeeData.IsDisplayDirectory = param.IsDisplayDirectory;
                employeeData.IsActive = true;
                employeeData.ModifiedBy = credentials.UserID;
                employeeData.ModifiedDate = DateTime.Now;

                List<EmployeeRoving> employeeRovings = new List<EmployeeRoving>();

                await _dbAccess.Put(
                    employeeData,
                    false,
                    employeeRovings,
                    employeeRovings,
                    param.IsViewedFamilyBackground,
                    EmployeeFamilyToDelete,
                    EmployeeFamilyToAdd,
                    EmployeeFamilyToUpdate,
                    param.IsViewedWorkingHistory,
                    WorkingHistoryToDelete,
                    WorkingHistoryToAdd,
                    WorkingHistoryToUpdate,
                    param.IsViewedEducation,
                    EducationToDelete,
                    EducationToAdd,
                    EducationToUpdate

                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> PutDraftToProbationary(APICredentials credentials, List<int> IDs)
        {
            var GetEmployees = (await _dbAccess.GetByIDs(IDs));

            var employees = GetEmployees.Select(x => new EMS.Plantilla.Data.Employee.Employee() {
                ID = x.ID,
                Code = x.Code,
                OldEmployeeID = x.OldEmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Suffix = x.Suffix,
                Nickname = x.Nickname,
                NationalityCode = x.NationalityCode,
                CitizenshipCode = x.CitizenshipCode,
                OrgGroupID = x.OrgGroupID,
                PositionID = x.PositionID,
                HomeBranchID = x.HomeBranchID,
                Gender = x.Gender,
                CivilStatusCode = x.CivilStatusCode,
                SSSStatusCode = x.SSSStatusCode,
                ExemptionStatusCode = x.ExemptionStatusCode,
                ReligionCode = x.ReligionCode,
                HeightCM = x.HeightCM,
                WeightLBS = x.WeightLBS,
                SystemUserID = x.SystemUserID,
                OnboardingWorkflowID = x.OnboardingWorkflowID,
                CompanyTag = x.CompanyTag,
                DateHired = x.DateHired,
                EmploymentStatus = "PROBATIONARY",
                BirthDate = x.BirthDate,
                BirthPlace = x.BirthPlace,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                PSGCRegionCode = x.PSGCRegionCode,
                PSGCProvinceCode = x.PSGCProvinceCode,
                PSGCCityMunicipalityCode = x.PSGCCityMunicipalityCode,
                PSGCBarangayCode = x.PSGCBarangayCode,
                Email = x.Email,
                CorporateEmail = x.CorporateEmail,
                OfficeMobile = x.OfficeMobile,
                OfficeLandline = x.OfficeLandline,
                CellphoneNumber = x.CellphoneNumber,
                SSSNumber = x.SSSNumber,
                TIN = x.TIN,
                PhilhealthNumber = x.PhilhealthNumber,
                PagibigNumber = x.PagibigNumber,
                ContactPersonName = x.ContactPersonName,
                ContactPersonNumber = x.ContactPersonNumber,
                ContactPersonAddress = x.ContactPersonAddress,
                ContactPersonRelationship = x.ContactPersonRelationship,
                IsDisplayDirectory = x.IsDisplayDirectory,
                IsActive = true,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = credentials.UserID,
                ModifiedDate = DateTime.Now
            }).ToList();

            await _dbAccess.Put(employees);

            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        }
        public async Task<IActionResult> GetEmployeeByOrgGroup(List<int> ID)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeByOrgGroup(ID));
        }
        public async Task<IActionResult> GetEmployeeByPosition(List<int> ID)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeByPosition(ID));
        }
        public async Task<IActionResult> GetEmployeeLastEmploymentStatus(List<int> ID)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeLastEmploymentStatus(ID));
        }
        public async Task<IActionResult> GetEmployeeLastEmploymentStatusByDate(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeLastEmploymentStatusByDate(param));
        }
        public async Task<IActionResult> GetEmployeeByDateHired(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeByDateHired(param));
        }

        public async Task<IActionResult> PostEmployeeReport(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.PostEmployeeReport(credentials.UserID));
        }
        public async Task<IActionResult> GetEmployeeReportByTDate(string TDate)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeReportByTDate(TDate));
        }
        public async Task<IActionResult> GetEmployeeReportOrgByTDate(string TDate)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeReportOrgByTDate(TDate));
        }
        public async Task<IActionResult> GetEmployeeReportRegionByTDate(string TDate)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeReportRegionByTDate(TDate));
        }
        public async Task<IActionResult> GetEmployeeIfExist(GetEmployeeIfExistInput param)
        {
            DateTime.TryParseExact(param.BDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime BDate);
            return new OkObjectResult(await _dbAccess.GetEmployeeIfExist(param.FName,param.LName, BDate));
        }

    }
}
