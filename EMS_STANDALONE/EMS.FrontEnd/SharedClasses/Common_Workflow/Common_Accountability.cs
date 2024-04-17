using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.Accountability;
using System.IO;
using Utilities.API.ReferenceMaintenance;
using Microsoft.AspNetCore.Mvc;
using EMS.FrontEnd.SharedClasses.Common_Security;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{

    public class Common_Accountability : Utilities
    {

        public Common_Accountability(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) :
            base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<(List<GetMyAccountabilitiesListOutput>, bool, string)> GetList(GetMyAccountabilitiesListInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetMyAccountabilitiesList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&"
                  );

            return await SharedUtilities.PostFromAPI(new List<GetMyAccountabilitiesListOutput>(),param, URL);
        }

        public async Task<List<GetDetailsByIDOutput>> GetAccountabilityDetails(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetDetailsByAccountabilityID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetDetailsByIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<AccountabilityForm> GetByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new AccountabilityForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetAccountabilityDropdown()
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAccountabilityDropdown").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetEmployeeAccountabilityByEmployeeIDOutput>> GetEmployeeAccountabilityByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeAccountabilityByEmployeeIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetDetailsByIDOutput>> GetAccountabilityDetailsByID(int AccountabilityID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAccountabilityDetails").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "AccountabilityID=", AccountabilityID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetDetailsByIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<EmployeeAccountabilityForm> GetEmployeeAccountabilityByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EmployeeAccountabilityForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<EmployeeAccountabilityGetCommentsOutput>> GetEmployeeComments(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeComments").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EmployeeAccountabilityGetCommentsOutput>(), URL);

            List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                await new Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(IDs);


            List<EmployeeAccountabilityGetCommentsOutput> resultWithUsername =
                Result.Join(
                    systemUsers,
                    x => new { x.CreatedBy },
                    y => new { CreatedBy = y.ID },
                    (x, y) => new { x, y })
                .Select(x => new EmployeeAccountabilityGetCommentsOutput
                {
                    Timestamp = x.x.Timestamp,
                    Comments = x.x.Comments,
                    Sender = string.Concat(x.y.Username," ", x.y.FirstName)
                }).ToList();

            if (IsSuccess)
                return resultWithUsername;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> SaveEmployeeComments(EmployeeAccountabilityCommentsForm CommentsForm)
        {
            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeComments").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(CommentsForm, URL);
        }

        public async Task<List<AttachmentForm>> GetEmployeeAttachment(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAttachment").Value, "?",
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
                            AttachmentType = x.attachment.attachment.AttachmentType,
                            Remarks = x.attachment.attachment.Remarks,
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

        public async Task<(bool, string)> SaveEmployeeAttachment(EmployeeAccountabilityAttachmentForm AttachmentForm)
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

            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddEmployeeAttachment").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(AttachmentForm, URL);

            if (IsSuccess)
            {
                foreach (var item in AttachmentForm.AddAttachmentForm)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_Accountability_Attachment_Path").Value), item.File, item.ServerFile);
                    }
                }

                if (AttachmentForm.DeleteAttachmentForm?.Count > 0)
                {
                    foreach (var item in AttachmentForm.DeleteAttachmentForm)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_Accountability_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }
            return (IsSuccess, Message);
        }

        public async Task<List<GetEmployeeAccountabilityStatusHistoryOutput>> GetEmployeeAccountabilityStatusHistory(int EmployeeAccountabilityID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeAccountabilityID=", EmployeeAccountabilityID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeAccountabilityStatusHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool,string)> BulkEmployeeAccountabilityDelete(string ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("BulkEmployeeAccountabilityDelete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                   "id=", ID);

            var (IsSuccess, Message) = await SharedUtilities.DeleteFromAPI(URL);
            _resultView.IsSuccess=IsSuccess;
            _resultView.Result = Message;
            if (IsSuccess)
            {
                List<int> AccountablityID = (ID.Split(",")).Select(int.Parse).ToList();
                foreach (var item in AccountablityID)
                {
                    /*Add AuditLog*/
                    await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                        .AddAuditLog(new EMS.Security.Transfer.AuditLog.Form
                        {
                            EventType = Common_AuditLog.EventType.DELETE.ToString(),
                            TableName = "EmployeeAccountability",
                            TableID = item,
                            Remarks = string.Concat("Employee Accountability ID: ",item, " deleted"),
                            IsSuccess = true,
                            CreatedBy = _globalCurrentUser.UserID
                        });
                }
            }

            return (IsSuccess,Message);
        }
        public async Task<List<GetMyAccountabilitiesListOutput>> GetAllEmployeeAccountability()
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAllEmployeeAccountability").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetMyAccountabilitiesListOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> PostChangeStatus(ChangeStatus param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("ChangeStatus").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(List<EmployeeAccountabilityForm>, bool, string)> GetEmployeeByEmployeeAccountabilityIDs(List<long> IDs)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeByEmployeeAccountabilityIDs").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<EmployeeAccountabilityForm>(), IDs, URL);
        }
        public async Task<(List<GetEmployeeAccountabilityStatusPercentageOutput>, bool, string)> GetEmployeeAccountabilityStatusPercentage(GetEmployeeAccountabilityStatusPercentageInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityStatusPercentage").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeAccountabilityStatusPercentageOutput>(), param, URL);
        }
        public async Task<(List<GetAccountabilityDashboardOutput>, bool, string)> GetAccountabilityDashboard(GetAccountabilityDashboardInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAccountabilityDashboard").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetAccountabilityDashboardOutput>(), param, URL);
        }
        public async Task<(List<ClearedEmployeeListOutput>, bool, string)> GetEmployeeClearedList(ClearedEmployeeListInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeClearedList").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<ClearedEmployeeListOutput>(), param, URL);
        }
        public async Task<(List<ClearedEmployeeByIDOutput>, bool, string)> GetClearedEmployeeByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedEmployeeByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<ClearedEmployeeByIDOutput>(), ID, URL);
        }
        public async Task<(List<GetClearedEmployeeCommentsOutput>, bool, string)> GetClearedEmployeeComments(int ClearedEmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedEmployeeComments").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetClearedEmployeeCommentsOutput>(), ClearedEmployeeID, URL);
        }
        public async Task<(bool, string)> AddClearedEmployeeComments(PostClearedEmployeeCommentsInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddClearedEmployeeComments").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(List<GetClearedEmployeeStatusHistoryOutput>, bool, string)> GetClearedEmployeeStatusHistory(int ClearedEmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedEmployeeStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetClearedEmployeeStatusHistoryOutput>(), ClearedEmployeeID, URL);
        }
        public async Task<(List<GetEmployeeAccountabilityOutput>, bool, string)> GetEmployeeAccountability(int EmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountability").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<GetEmployeeAccountabilityOutput>(), EmployeeID, URL);
        }
        public async Task<(bool, string)> AddClearedEmployeeComputation(PostClearedEmployeeComputationInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddClearedEmployeeComputation").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> AddClearedEmployeeChangeStatus(PostClearedEmployeeStatusInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddClearedEmployeeChangeStatus").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
    }
}