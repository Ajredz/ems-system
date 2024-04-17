using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.LogActivity;
using System.IO;
using Utilities.API.ReferenceMaintenance;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{

    public class Common_LogActivity : Utilities
    {

        public Common_LogActivity(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : 
            base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetApplicantLogActivityByApplicantIDOutput>> GetApplicantLogActivityByApplicantID(int ApplicantID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantLogActivityByApplicantID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ApplicantID=", ApplicantID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApplicantLogActivityByApplicantIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetEmployeeLogActivityByEmployeeIDOutput>> GetEmployeeLogActivityByEmployeeID(int EmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeLogActivityByEmployeeID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeID=", EmployeeID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeLogActivityByEmployeeIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetApplicantLogActivityStatusHistoryOutput>> GetApplicantLogActivityStatusHistory(int ApplicantLogActivityID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantLogActivityStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ApplicantLogActivityID=", ApplicantLogActivityID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApplicantLogActivityStatusHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetEmployeeLogActivityStatusHistoryOutput>> GetEmployeeLogActivityStatusHistory(int EmployeeLogActivityID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeLogActivityStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeLogActivityID=", EmployeeLogActivityID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeLogActivityStatusHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<(bool, string)> AddApplicantActivity(TagToApplicantForm param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddApplicantActivity").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        
        public async Task<(bool, string)> AddEmployeeActivity(TagToEmployeeForm param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeeActivity").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }

        public async Task<Form> GetLogActivity(int ID)
        {

            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetLogActivityDropdownByModuleAndType(GetLogActivityByModuleTypeInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityDropdownByModuleAndType").Value, "?",
                     "userid=", _globalCurrentUser.UserID,
                     "&Modules=", string.Join("&Modules=", param.Modules), "&",
                     "Type=", param.Type, "&",
                     "SelectedID=", param.SelectedID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<LogActivityForm> GetLogActivityByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new LogActivityForm(), URL);
            _resultView.IsSuccess = IsSuccess;

            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<ApplicantLogActivityForm> GetApplicantLogActivityByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantLogActivityByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new ApplicantLogActivityForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ApplicantLogActivityGetCommentsOutput>> GetApplicantComments(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantComments").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ApplicantLogActivityGetCommentsOutput>(), URL);

            List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                await new Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(IDs);


            List<ApplicantLogActivityGetCommentsOutput> resultWithUsername =
                Result.Join(
                    systemUsers,
                    x => new { x.CreatedBy },
                    y => new { CreatedBy = y.ID },
                    (x, y) => new { x, y })
                .Select(x => new ApplicantLogActivityGetCommentsOutput
                {
                    Timestamp = x.x.Timestamp,
                    Comments = x.x.Comments,
                    Sender = x.y.Username
                }).ToList();

            if (IsSuccess)
                return resultWithUsername;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> SaveApplicantComments(ApplicantLogActivityCommentsForm CommentsForm)
        {
            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddApplicantComments").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(CommentsForm, URL);
        }

        public async Task<List<AttachmentForm>> GetApplicantAttachment(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetApplicantAttachment").Value, "?",
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

        public async Task<(bool, string)> SaveApplicantAttachment(ApplicantLogActivityAttachmentForm AttachmentForm)
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
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddApplicantAttachment").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(AttachmentForm, URL);

            if (IsSuccess)
            {
                foreach (var item in AttachmentForm.AddAttachmentForm)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_LogActivity_Attachment_Path").Value), item.File, item.ServerFile);
                    }
                }

                if (AttachmentForm.DeleteAttachmentForm?.Count > 0)
                {
                    foreach (var item in AttachmentForm.DeleteAttachmentForm)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_LogActivity_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }
            return (IsSuccess, Message);
        }

        public async Task<EmployeeLogActivityForm> GetEmployeeLogActivityByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeLogActivityByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EmployeeLogActivityForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<EmployeeLogActivityGetCommentsOutput>> GetEmployeeComments(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeComments").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EmployeeLogActivityGetCommentsOutput>(), URL);

            List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                await new Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(IDs);


            List<EmployeeLogActivityGetCommentsOutput> resultWithUsername =
                Result.Join(
                    systemUsers,
                    x => new { x.CreatedBy },
                    y => new { CreatedBy = y.ID },
                    (x, y) => new { x, y })
                .Select(x => new EmployeeLogActivityGetCommentsOutput
                {
                    Timestamp = x.x.Timestamp,
                    Comments = x.x.Comments,
                    Sender = x.y.Username
                }).ToList();

            if (IsSuccess)
                return resultWithUsername;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> SaveEmployeeComments(EmployeeLogActivityCommentsForm CommentsForm)
        {
            var URL = string.Concat(_workflowBaseURL,
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeeComments").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(CommentsForm, URL);
        }

        public async Task<List<AttachmentForm>> GetEmployeeAttachment(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetEmployeeAttachment").Value, "?",
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

        public async Task<(bool, string)> SaveEmployeeAttachment(EmployeeLogActivityAttachmentForm AttachmentForm)
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
                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("AddEmployeeAttachment").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(AttachmentForm, URL);

            if (IsSuccess)
            {
                foreach (var item in AttachmentForm.AddAttachmentForm)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_LogActivity_Attachment_Path").Value), item.File, item.ServerFile);
                    }
                }

                if (AttachmentForm.DeleteAttachmentForm?.Count > 0)
                {
                    foreach (var item in AttachmentForm.DeleteAttachmentForm)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("WorkflowService_LogActivity_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }
            return (IsSuccess, Message);
        }

        public async Task<List<SelectListItem>> GetLogActivityPreloadedDropdown()
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityPreloadedDropdown").Value, "?",
                     "userid=", _globalCurrentUser.UserID);
             
            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetLogActivityByPreloadedIDOutput>> GetLogActivityByPreloadedID(int LogActivityPreloadedID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityByPreloadedID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "LogActivityPreloadedID=", LogActivityPreloadedID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetLogActivityByPreloadedIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ReferenceValue>> GetLogActivitySubType()
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivitySubType").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ReferenceValue>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<string>> GetLogActivityModule()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityModule").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<string>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<LogActivityForm>> GetLogActivityData()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetAllLogActivity").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<LogActivityForm>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<LogActivityPreloadedForm> GetLogActivityPreloadedByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetLogActivityPreloadedByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new LogActivityPreloadedForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetLogActivityByPreloadedIDOutput>> GetPreloadedItemsByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("LogActivity").GetSection("GetPreloadedItemsByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetLogActivityByPreloadedIDOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}