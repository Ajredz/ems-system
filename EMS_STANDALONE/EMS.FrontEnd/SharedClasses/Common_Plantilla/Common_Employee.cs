using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Transfer.Employee;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using EMS.IPM.Data.DataDuplication.Employee;
using Microsoft.AspNetCore.Mvc;
using EMS.IPM.Data.EmployeeScore;
//using static NPOI.XSSF.UserModel.Charts.XSSFLineChartData<Tx, Ty>;

namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_Employee : Utilities
    {
        public Common_Employee(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetEmployeeAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetByIDOutput> GetEmployeeByUserID(int UserID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByUserID").Value, "?",
                     "userid=", UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetByIDOutput(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetByPositionIDOrgGroupIDOutput>> GetByPositionIDOrgGroupID(GetByPositionIDOrgGroupIDInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByPositionIDOrgGroupID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "PositionID=", param.PositionID, "&",
                     "OrgGroupID=", param.OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetByPositionIDOrgGroupIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetRovingByPositionIDOrgGroupIDOutput>> GetRovingByPositionIDOrgGroupID(GetRovingByPositionIDOrgGroupIDInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetRovingByPositionIDOrgGroupID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "PositionID=", param.PositionID, "&",
                     "OrgGroupID=", param.OrgGroupID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetRovingByPositionIDOrgGroupIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        //ORIG
        public async Task<List<GetRovingByEmployeeIDOutput>> GetRovingByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetRovingByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetRovingByEmployeeIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetWorkingHistoryOutput>> GetWorkingHistoryByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetWorkingHistoryByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetWorkingHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetEmployee(int ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<(List<Form>, bool, string)> GetEmployeeByUserIDs(List<int> UserIDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByUserIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<Form>(), UserIDs, URL);
        }

        public async Task<List<GetFamilyOutput>> GetFamilyByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetFamilyByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetFamilyOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetEducationOutput>> GetEducationByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEducationByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEducationOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

       

        public async Task<List<GetEmploymentStatusOutput>> GetEmploymentStatusByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmploymentStatusByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmploymentStatusOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<bool> UpdateOnboardingCurrentWorkflowStep(UpdateOnboardingCurrentWorkflowStepInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UpdateOnboardingCurrentWorkflowStep").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetEmployeeWithSystemUserAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeWithSystemUserByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<Form>> GetByCodes(string CodesDelimited)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByCodes").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.PostFromAPI(new List<Form>(), CodesDelimited, URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<(Form, bool, string)> AutoAddEmployee(Form param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                        _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("AutoAdd").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

            //var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(param, URL);
            //_resultView.IsSuccess = IsSuccess;
            //_resultView.Result = Message;

            //return _resultView;
            return await SharedUtilities.PostFromAPI(new Form(), param, URL);
        }

        public async Task<bool> UpdateSystemUser(UpdateSystemUserInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UpdateSystemUser").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }

        public async Task<GetEmployeeByUsernameOutput> GetEmployeeByUsername(string Username)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByUsername").Value, "?",
                     "Username=", Username);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetEmployeeByUsernameOutput(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<Form>, bool, string)> GetEmployeeByIDs(List<int> IDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<Form>(), IDs, URL);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetOldEmployeeIDAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetOldEmployeeIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<Form>> GetByOldEmployeeIDs(string CodesDelimited)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetByOldEmployeeIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "CodesDelimited=", CodesDelimited);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<Form>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);

        }

        public async Task<List<AttachmentForm>> GetEmployeeAttachment(int ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                      _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetAttachment").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<AttachmentForm>(), URL);

            if (IsSuccess)
            {
                // Get Employee Names by User IDs
                List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                   await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                   .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
                   .Union(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy))
                   .Distinct().ToList());

                Result = Result
                        .GroupJoin(systemUsers,
                        x => new { x.CreatedBy },
                        y => new { CreatedBy = y.ID },
                        (x, y) => new { attachment = x, employees = y })
                        .SelectMany(x => x.employees.DefaultIfEmpty(),
                        (x, y) => new { attachment = x, employees = y })
                        .Select(x => new AttachmentForm
                        {
                            Description = x.attachment.attachment.Description,
                            ServerFile = x.attachment.attachment.ServerFile,
                            SourceFile = x.attachment.attachment.SourceFile,
                            Timestamp = x.attachment.attachment.Timestamp,
                            UploadedBy = x.employees == null ? "" : string.Concat(x.employees.LastName,
                                string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
                        }).ToList();
            }


            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> SaveEmployeeAttachment(EmployeeAttachmentForm AttachmentForm)
        {
            AttachmentForm.AddAttachmentForm = AttachmentForm.AddAttachmentForm.Select(x =>
            {
                if (x.File != null)
                {
                    string dateTimePreFix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                    x.ServerFile = string.Concat(dateTimePreFix, Guid.NewGuid().ToString("N").Substring(0, 4), "_", x.File.FileName);
                    x.SourceFile = x.File.FileName;
                }
                return x;
            }).ToList();

            var URL = string.Concat(_plantillaBaseURL,
                    _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("AddAttachment").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(AttachmentForm, URL);

            if (IsSuccess)
            {
                foreach (var item in AttachmentForm.AddAttachmentForm)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("PlantillaService_Employee_Attachment_Path").Value), item.File, item.ServerFile);
                    }
                }

                if (AttachmentForm.DeleteAttachmentForm?.Count > 0)
                {
                    foreach (var item in AttachmentForm.DeleteAttachmentForm)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("PlantillaService_Employee_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }
            return (IsSuccess, Message);
        }

        public async Task<EmployeeSkillsForm> GetEmployeeSkillsById(int id)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeSkillsById").Value, "?",
                     "Id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EmployeeSkillsForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<EmployeeDetails>, bool, string)> GetEmployeeByBirthday()
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByBirthday").Value);

            return await SharedUtilities.GetFromAPI(new List<EmployeeDetails>(), URL);
        }

        public async Task<(List<GetEmployeeEvaluationOutput>, bool, string)> GetEmployeeEvaluation()
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeEvaluation").Value);

            return await SharedUtilities.GetFromAPI(new List<GetEmployeeEvaluationOutput>(), URL);
        }


        public async Task<List<GetEmailOutput>> GetEmail(int ID, string Condition)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmail").Value, "?",
                     "ID=", ID,
                     "&Condition=", Condition);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmailOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<EmployeeOutput>> GetEmployeeIDDescendant(int EmployeeID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeIDDescendant").Value, "?",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EmployeeOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<(bool, string)> ConvertNewEmployee(NewEmployeeForm param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("ConvertNewEmployee").Value);

            return await SharedUtilities.PostFromAPI(param, URL);
        }

        public async Task<(bool, string)> EditEmployeeDetails(Form param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("EditEmployeeDetails").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> EditDraftToProbationary(List<int> IDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("EditDraftToProbationary").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(IDs, URL);
        }

        public async Task<(List<EmployeeOutput>, bool, string)> GetEmployeeByOrgGroup(List<int> ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByOrgGroup").Value);

            return await SharedUtilities.PostFromAPI(new List<EmployeeOutput>(), ID, URL);
        }

        public async Task<(List<EmployeeOutput>, bool, string)> GetEmployeeByPosition(List<int> ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByPosition").Value);

            return await SharedUtilities.PostFromAPI(new List<EmployeeOutput>(), ID, URL);
        }
        public async Task<(List<GetEmployeeLastEmploymentStatusOutput>, bool, string)> GetEmployeeLastEmploymentStatus(List<int> ID)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeLastEmploymentStatus").Value);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeLastEmploymentStatusOutput>(), ID, URL);
        }
        public async Task<(List<GetEmployeeLastEmploymentStatusOutput>, bool, string)> GetEmployeeLastEmploymentStatusByDate(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeLastEmploymentStatusByDate").Value);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeLastEmploymentStatusOutput>(), param, URL);
        }
        public async Task<(List<EmployeeOutput>, bool, string)> ConvertNewEmployees(List<NewEmployeeForm> param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("ConvertNewEmployees").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<EmployeeOutput>(), param, URL);
        }
        public async Task<(List<EmployeeOutput>, bool, string)> GetEmployeeByDateHired(GetEmployeeLastEmploymentStatusByDateInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByDateHired").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<EmployeeOutput>(), param, URL);
        }

        public async Task<(List<AddEmployeeReportOutput>, bool, string)> AddEmployeeReport()
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("AddEmployeeReport").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<AddEmployeeReportOutput>(), "", URL);
        }
        public async Task<(List<GetEmployeeReportByTDateOutput>, bool, string)> GetEmployeeReportByTDate(string TDate)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeReportByTDate").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeReportByTDateOutput>(), TDate, URL);
        }
        public async Task<(List<GetEmployeeReportOrgByTDateOutput>, bool, string)> GetEmployeeReportOrgByTDate(string TDate)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeReportOrgByTDate").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeReportOrgByTDateOutput>(), TDate, URL);
        }
        public async Task<(List<GetEmployeeReportOrgByTDateOutput>, bool, string)> GetEmployeeReportRegionByTDate(string TDate)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeReportRegionByTDate").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeReportOrgByTDateOutput>(), TDate, URL);
        }
        public async Task<(List<Employee>, bool, string)> GetEmployeeIfExist(GetEmployeeIfExistInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeIfExist").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<Employee>(), param, URL);
        }
    }

}
