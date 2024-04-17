using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.LogActivity;
using EMS.Workflow.Transfer.LogActivity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.LogActivity
{
    public interface ILogActivityService
    {
        Task<IActionResult> GetApplicantLogActivityByApplicantID(APICredentials credentials, int ApplicantID);
        
        Task<IActionResult> GetEmployeeLogActivityByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> GetApplicantLogActivityStatusHistory(APICredentials credentials, int ApplicantLogActivityID);
        
        Task<IActionResult> GetEmployeeLogActivityStatusHistory(APICredentials credentials, int EmployeeLogActivityID);

        Task<IActionResult> AddApplicantActivity(APICredentials credentials, TagToApplicantForm param);
        
        Task<IActionResult> AddEmployeeActivity(APICredentials credentials, TagToEmployeeForm param);

        Task<IActionResult> GetLogActivityDropdownByModuleAndType(APICredentials credentials, GetLogActivityByModuleTypeInput param);

        Task<IActionResult> GetLogActivityByID(APICredentials credentials, int ID);

        Task<IActionResult> GetApplicantLogActivityByID(APICredentials credentials, int ID);

        Task<IActionResult> AddApplicantActivityStatusHistory(APICredentials credentials, ApplicantLogActivityForm param);
        
        Task<IActionResult> GetEmployeeLogActivityByID(APICredentials credentials, int ID);

        Task<IActionResult> AddEmployeeActivityStatusHistory(APICredentials credentials, EmployeeLogActivityForm param);

        Task<IActionResult> GetAssignedActivitiesList(APICredentials credentials, GetAssignedActivitiesListInput input);

        Task<IActionResult> PostApplicantComments(APICredentials credentials, ApplicantLogActivityCommentsForm param);

        Task<IActionResult> GetApplicantComments(APICredentials credentials, int ID);

        Task<IActionResult> PostApplicantAttachment(APICredentials credentials, ApplicantLogActivityAttachmentForm param);

        Task<IActionResult> GetApplicantAttachment(APICredentials credentials, int ID);
        
        Task<IActionResult> PostEmployeeComments(APICredentials credentials, EmployeeLogActivityCommentsForm param);

        Task<IActionResult> GetEmployeeComments(APICredentials credentials, int ID);

        Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeLogActivityAttachmentForm param);

        Task<IActionResult> GetEmployeeAttachment(APICredentials credentials, int ID);

        Task<IActionResult> GetLogActivityPreloadedDropdown(APICredentials credentials);

        Task<IActionResult> GetLogActivityByPreloadedID(APICredentials credentials, int LogActivityPreloadedID);

        Task<IActionResult> AddApplicantPreLoadedActivities(APICredentials credentials, AddApplicantPreLoadedActivitiesInput param);
        
        Task<IActionResult> AddEmployeePreLoadedActivities(APICredentials credentials, AddEmployeePreLoadedActivitiesInput param);

        Task<IActionResult> GetLogActivitySubType(APICredentials credentials);

        Task<IActionResult> GetLogActivityPreloadedList(APICredentials credentials, GetPreLoadedListInput input);

        Task<IActionResult> GetLogActivityPreloadedByID(APICredentials credentials, int ID);

        Task<IActionResult> AddLogActivityPreloaded(APICredentials credentials, LogActivityPreloadedForm param);

        Task<IActionResult> DeleteLogActivityPreloaded(APICredentials credentials, int ID);

        Task<IActionResult> EditLogActivityPreloaded(APICredentials credentials, LogActivityPreloadedForm param);

        Task<IActionResult> GetPreloadedItemsByID(APICredentials credentials, int ID);

        Task<IActionResult> GetApplicantLogActivityPendingEmail(APICredentials credentials);

        Task<IActionResult> GetEmployeeLogActivityPendingEmail(APICredentials credentials);

        Task<IActionResult> UpdateApplicantLogActivityPendingEmail(APICredentials credentials, int ID);

        Task<IActionResult> UpdateEmployeeLogActivityPendingEmail(APICredentials credentials, int ID);

        Task<IActionResult> GetChecklistList(APICredentials credentials, GetChecklistListInput input);

        Task<IActionResult> UpdateEmployeeLogActivityAssignedUser(APICredentials credentials, UpdateEmployeeLogActivityAssignedUserForm param);

        Task<IActionResult> UpdateApplicantLogActivityAssignedUser(APICredentials credentials, UpdateApplicantLogActivityAssignedUserForm param);

        Task<IActionResult> GetApplicantLogActivityList(APICredentials credentials, GetApplicantLogActivityListInput input);

        Task<IActionResult> GetEmployeeLogActivityList(APICredentials credentials, GetChecklistListInput input);

        Task<IActionResult> BatchUpdateLogActivity(APICredentials credentials, BatchTaskForm param);

        Task<IActionResult> UploadLogActivityInsert(APICredentials credentials, List<UploadLogActivityFile> param);
    }

    public class LogActivityService : Core.Shared.Utilities, ILogActivityService
    {
        private readonly EMS.Workflow.Data.LogActivity.ILogActivityDBAccess _dbAccess;
        private readonly EMS.Workflow.Data.Reference.IReferenceDBAccess _dbReferenceService;

        public LogActivityService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.LogActivity.ILogActivityDBAccess dBAccess, Data.Reference.IReferenceDBAccess dbReferenceService) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
            _dbReferenceService = dbReferenceService;
        }

        public async Task<IActionResult> GetApplicantLogActivityByApplicantID(APICredentials credentials, int ApplicantID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApplicantLogActivityByApplicantID(ApplicantID))
                .Select(x => new GetApplicantLogActivityByApplicantIDOutput
                {
                    ID = x.ID,
                    Type = x.Type,
                    SubType = x.SubType,
                    Title = x.Title,
                    Description = x.Description,
                    CurrentStatus = x.CurrentStatus,
                    CurrentTimestamp = x.CurrentTimestamp,
                    AssignedUserID = x.AssignedUserID,
                    AssignedOrgGroupID = x.AssignedOrgGroupID,
                    IsPass = x.IsPass,
                    CreatedBy = x.CreatedBy
                }).ToList()
            );
        } 
        
        public async Task<IActionResult> GetApplicantLogActivityStatusHistory(APICredentials credentials, int ApplicantLogActivityID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApplicantLogActivityStatusHistory(ApplicantLogActivityID))
                .Select(x => new GetApplicantLogActivityStatusHistoryOutput
                {
                    Status = x.Status,
                    Timestamp = x.Timestamp,
                    UserID = x.UserID,
                    Remarks = x.Remarks ?? "",
                    User = "",
                    IsPass = x.IsPass

                }).ToList()
            );
        }
        
        public async Task<IActionResult> GetEmployeeLogActivityByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeLogActivityByEmployeeID(EmployeeID))
                .Select(x => new GetEmployeeLogActivityByEmployeeIDOutput
                {
                    ID = x.ID,
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    CurrentStatus = x.CurrentStatus,
                    CurrentTimestamp = x.CurrentTimestamp,
                    AssignedUserID = x.AssignedUserID,
                    AssignedOrgGroupID = x.AssignedOrgGroupID,
                    IsPass = x.IsPass,
                    CreatedBy = x.CreatedBy
                }).ToList()
            );
        } 
        
        public async Task<IActionResult> GetEmployeeLogActivityStatusHistory(APICredentials credentials, int EmployeeLogActivityID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeLogActivityStatusHistory(EmployeeLogActivityID))
                .Select(x => new GetEmployeeLogActivityStatusHistoryOutput
                {
                    Status = x.Status,
                    Timestamp = x.Timestamp,
                    UserID = x.UserID,
                    Remarks = x.Remarks ?? "",
                    User = "",
                    IsPass = x.IsPass

                }).ToList()
            );
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarLogActivity> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Module = x.Module,
                Type = x.Type,
                SubType = x.SubType,
                Title = x.Title,
                Description = x.Description,
                IsPassFail = x.IsPassFail,
                IsAssignment = x.IsAssignment
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.LogActivity.LogActivity result = await _dbAccess.GetByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Module = result.Module,
                    Type = result.Type,
                    SubType = result.SubType,
                    Title = result.Title,
                    Description = result.Description,
                    IsPassFail = result.IsWithPassFail,
                    IsAssignment = result.IsWithAssignment,
                    //IsPreLoaded = result.IsPreloaded,
                    IsActive = result.IsActive,
                    CreatedBy = result.CreatedBy,
                    IsVisible = result.IsVisible,
                    AssignedUserID = result.AssignedUserID
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Module = param.Module.Trim();
            if (string.IsNullOrEmpty(param.Module))
                ErrorMessages.Add("Module " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Module.Length > 50)
                ErrorMessages.Add(string.Concat("Module", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            param.Type = param.Type.Trim();
            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Type.Length > 50)
                ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            //param.SubType = param.SubType.Trim();
            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //    if (param.SubType.Length > 50)
            //    ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            param.SubType = "";

            param.Title = param.Title.Trim();
            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Title.Length > 100)
                ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));

            if (!string.IsNullOrEmpty(param.Description))
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }
            

            if ((await _dbAccess.GetActivityByModuleTypeTitle(param.Module, param.Type, param.SubType, param.Title)).Count() > 0)
            {
                ErrorMessages.Add("Log Activity " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.LogActivity.LogActivity
                {
                    Module = param.Module,
                    Type = param.Type,
                    SubType = param.SubType,
                    Title = param.Title,
                    Description = param.Description,
                    IsWithPassFail = param.IsPassFail,
                    IsWithAssignment = param.IsAssignment,
                    //IsPreloaded = param.IsPreLoaded,
                    IsActive = true,
                    CreatedBy = param.CreatedBy,
                    IsVisible = param.IsVisible,
                    AssignedUserID = param.AssignedUserID
                });

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            param.Module = param.Module.Trim();
            if (string.IsNullOrEmpty(param.Module))
                ErrorMessages.Add("Module " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Module.Length > 50)
                ErrorMessages.Add(string.Concat("Module", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            param.Type = param.Type.Trim();
            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Type.Length > 50)
                ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            //param.SubType = param.SubType.Trim();
            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //    if (param.SubType.Length > 50)
            //    ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            param.SubType = "";

            param.Title = param.Title.Trim();
            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Title.Length > 100)
                ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));

            if (!string.IsNullOrEmpty(param.Description))
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if ((await _dbAccess.GetActivityByModuleTypeTitle(param.Module, param.Type, param.SubType, param.Title)).Where(x => x.ID != param.ID).Count() > 0)
            {
                ErrorMessages.Add("Log Activity " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {
                Data.LogActivity.LogActivity logActivity = await _dbAccess.GetByID(param.ID);
                logActivity.Module = param.Module;
                logActivity.Type = param.Type;
                logActivity.SubType = param.SubType;
                logActivity.Title = param.Title;
                logActivity.Description = param.Description;
                logActivity.IsWithPassFail = param.IsPassFail;
                logActivity.IsWithAssignment = param.IsAssignment;
                //logActivity.IsPreloaded = param.IsPreLoaded;
                logActivity.IsActive = true;
                logActivity.ModifiedBy = credentials.UserID;
                logActivity.ModifiedDate = DateTime.Now;
                logActivity.IsVisible = param.IsVisible;
                logActivity.AssignedUserID = param.AssignedUserID;

                await _dbAccess.Put(logActivity);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            Data.LogActivity.LogActivity logActivity = await _dbAccess.GetByID(ID);
            logActivity.IsActive = false;
            logActivity.ModifiedBy = credentials.UserID;
            logActivity.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(logActivity))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> AddApplicantActivity(APICredentials credentials, TagToApplicantForm param)
        {
            //List<ApplicantLogActivity> unique = 
            //    (await _dbAccess.GetApplicantLogActivityByUnique(param.ApplicantID, param.LogActivityID)).ToList();

            //if (unique.Count > 0)
            //    ErrorMessages.Add(string.Concat("Record ", MessageUtilities.SUFF_ERRMSG_REC_EXISTS));

            if (param.ApplicantID == 0)
                ErrorMessages.Add("ApplicantID " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            //if (param.AssignedUserID > 0)
            //{
            //    if (string.IsNullOrEmpty(param.Email))
            //        ErrorMessages.Add("Email " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //    else
            //    {
            //        param.Email = param.Email.Trim();
            //        if (param.Email.Length > 255)
            //            ErrorMessages.Add(string.Concat("Email", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            //    }

            //}
            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //{
            //    param.SubType = param.SubType.Trim();
            //    if (param.SubType.Length > 50)
            //        ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            //}

            param.SubType = "";

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Title = param.Title.Trim();
                if (param.Title.Length > 100)
                    ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (string.IsNullOrEmpty(param.DueDate.ToString()))
                ErrorMessages.Add("Due Date " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (!string.IsNullOrEmpty(param.Description))
            //    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (!string.IsNullOrEmpty(param.Remarks))
            //    ErrorMessages.Add("Remarks " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0 )
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;
            
            await _dbAccess.AddApplicantActivity(new ApplicantLogActivity
            {
                ApplicantID = param.ApplicantID,
                Title = param.Title,
                Description = param.Description,
                Type = param.Type,
                SubType = param.SubType,
                IsWithPassFail = param.IsWithPassFail,
                IsWithAssignment = param.IsWithAssignment,
                CurrentStatus = param.Status.ToString(),
                CurrentTimestamp = currentDatetime,
                IsActive = true,
                CreatedBy = credentials.UserID,
                CreatedDate = currentDatetime,
                AssignedUserID = param.AssignedUserID,
                Email = param.Email,
                ApplicantName = param.ApplicantName,
                AssignedOrgGroupID = param.AssignedOrgGroupID,
                DueDate = param.DueDate
            },
            new ApplicantLogActivityStatusHistory
            {
                Status = param.Status.ToString(),
                Timestamp = currentDatetime,
                Remarks = param.Remarks,
                UserID = credentials.UserID
            }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> AddEmployeeActivity(APICredentials credentials, TagToEmployeeForm param)
        {
            //List<EmployeeLogActivity> unique = 
            //    (await _dbAccess.GetEmployeeLogActivityByUnique(param.EmployeeID, param.LogActivityID)).ToList();

            //if (unique.Count > 0)
            //    ErrorMessages.Add(string.Concat("Record ", MessageUtilities.SUFF_ERRMSG_REC_EXISTS));

            if (param.EmployeeID == 0)
                ErrorMessages.Add("EmployeeID " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            //if (param.AssignedUserID > 0)
            //{
            //    if (string.IsNullOrEmpty(param.Email))
            //        ErrorMessages.Add("Email " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //    else
            //    {
            //        param.Email = param.Email.Trim();
            //        if (param.Email.Length > 255)
            //            ErrorMessages.Add(string.Concat("Email", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            //    }

            //}

            if (string.IsNullOrEmpty(param.DueDate.ToString()))
                ErrorMessages.Add("Due Date " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //{
            //    param.SubType = param.SubType.Trim();
            //    if (param.SubType.Length > 50)
            //        ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            //}

            param.SubType = "";

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Title = param.Title.Trim();
                if (param.Title.Length > 100)
                    ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (!string.IsNullOrEmpty(param.Description))
            //    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (!string.IsNullOrEmpty(param.Remarks))
            //    ErrorMessages.Add("Remarks " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0 )
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;
            
            await _dbAccess.AddEmployeeActivity(new EmployeeLogActivity
            {
                EmployeeID = param.EmployeeID,
                Title = param.Title,
                Description = param.Description,
                Type = param.Type,
                SubType = param.SubType,
                IsWithPassFail = param.IsWithPassFail,
                IsWithAssignment = param.IsWithAssignment,
                CurrentStatus = param.Status.ToString(),
                CurrentTimestamp = currentDatetime,
                IsActive = true,
                CreatedBy = credentials.UserID,
                CreatedDate = currentDatetime,
                AssignedUserID = param.AssignedUserID,
                Email = param.Email,
                EmployeeName = param.EmployeeName,
                AssignedOrgGroupID = param.AssignedOrgGroupID,
                IsVisible = param.IsVisible,
                DueDate = param.DueDate
            },
            new EmployeeLogActivityStatusHistory
            {
                Status = param.Status.ToString(),
                Timestamp = currentDatetime,
                Remarks = param.Remarks,
                UserID = credentials.UserID
            }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> GetLogActivityDropdownByModuleAndType(APICredentials credentials, GetLogActivityByModuleTypeInput param)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetLogActivityByModuleAndType(param)).ToList(), "ID", "Title", null, param.SelectedID));
        }

        public async Task<IActionResult> GetLogActivityByID(APICredentials credentials, int ID)
        {
            Data.LogActivity.LogActivity result = await _dbAccess.GetLogActivityByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(new LogActivityForm { 
                    ID = result.ID,
                    Module = result.Module,
                    Type = result.Type,
                    Title = result.Title,
                    Description = result.Description,
                    IsWithPassFail = result.IsWithPassFail,
                    IsWithAssignment = result.IsWithAssignment,
                    //IsPreloaded = result.IsPreloaded
                });

        }

        public async Task<IActionResult> GetApplicantLogActivityByID(APICredentials credentials, int ID)
        {
            Data.LogActivity.ApplicantLogActivity result = await _dbAccess.GetApplicantLogActivityByID(ID);
          
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                List<Data.Reference.ReferenceValue> referenceValues =
                              (await _dbReferenceService.GetByRefCodes(new List<string> { "ACTIVITY_TYPE" })).Where(x => x.Value.Equals(result.Type)).ToList();

                if (referenceValues.Count == 0)
                    return new BadRequestObjectResult(string.Concat("LogActivity: ", MessageUtilities.ERRMSG_REC_NOT_EXIST));


                return new OkObjectResult(
                 new ApplicantLogActivityForm
                 {
                     ID = result.ID,
                     Type = result.Type,
                     SubType = result.SubType,
                     Title = result.Title,
                     Description = result.Description,
                     CurrentStatus = result.CurrentStatus,
                     AssignedOrgGroupID = result.AssignedOrgGroupID,
                     AssignedUserID = result.AssignedUserID,
                     IsWithAssignment = result.IsWithAssignment,
                     IsPass = result.IsPass,
                     IsWithPassFail = result.IsWithPassFail,
                     DueDate = result.DueDate,
                     ApplicantID = result.ApplicantID
                 });
            }
        }

        public async Task<IActionResult> AddApplicantActivityStatusHistory(APICredentials credentials, ApplicantLogActivityForm param)
        {
            if (!string.IsNullOrEmpty(param.Remarks))
            //    ErrorMessages.Add("Remarks " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
              if (param.Title.Length > 100)
            {
                param.Title = param.Title.Trim();
                ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            }

            if (string.IsNullOrEmpty(param.DueDate.ToString()))
                ErrorMessages.Add("Due Date " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //{
            //    param.SubType = param.SubType.Trim();
            //    if (param.SubType.Length > 50)
            //        ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            //}

            param.SubType = "";

            if (!string.IsNullOrEmpty(param.Description))
            //    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }


            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0)
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;

            Data.LogActivity.ApplicantLogActivity result = await _dbAccess.GetApplicantLogActivityByID(param.ID);
            // Update current status and timestamp
            result.Type = param.Type;
            result.SubType = param.SubType;
            result.Title = param.Title;
            result.Description = param.Description;
            result.IsPass = param.IsPass;
            if (!string.IsNullOrEmpty(param.Status))
            {
                result.CurrentStatus = param.Status ?? ""; 
            }
            result.CurrentTimestamp = currentDatetime;
            result.AssignedUserID = param.AssignedUserID;
            result.IsWithAssignment = param.AssignedUserID > 0;
            result.AssignedOrgGroupID = param.AssignedOrgGroupID;
            result.DueDate = param.DueDate;

            await _dbAccess.AddApplicantActivityStatusHistory(
           result
                , new ApplicantLogActivityStatusHistory
            {
                ApplicantLogActivityID = param.ID,
                Status = param.Status ?? "",
                Timestamp = currentDatetime,
                Remarks = param.Remarks,
                UserID = credentials.UserID,
                IsPass = param.IsPass
            }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }
        
        public async Task<IActionResult> GetEmployeeLogActivityByID(APICredentials credentials, int ID)
        {
            Data.LogActivity.EmployeeLogActivity result = await _dbAccess.GetEmployeeLogActivityByID(ID);
          
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                List<Data.Reference.ReferenceValue> referenceValues =
                              (await _dbReferenceService.GetByRefCodes(new List<string> { "ACTIVITY_TYPE" })).Where(x => x.Value.Equals(result.Type)).ToList();

                if (referenceValues.Count == 0)
                    return new BadRequestObjectResult(string.Concat("LogActivity: ", MessageUtilities.ERRMSG_REC_NOT_EXIST));


                return new OkObjectResult(
                 new EmployeeLogActivityForm
                 {
                     ID = result.ID,
                     Type = result.Type,
                     SubType = result.SubType,
                     Title = result.Title,
                     Description = result.Description,
                     CurrentStatus = result.CurrentStatus,
                     AssignedOrgGroupID = result.AssignedOrgGroupID,
                     AssignedUserID = result.AssignedUserID,
                     IsWithAssignment = result.IsWithAssignment,
                     IsPass = result.IsPass,
                     IsWithPassFail = result.IsWithPassFail,
                     IsVisible = result.IsVisible,
                     DueDate = result.DueDate,
                     EmployeeID = result.EmployeeID
                 });
            }
        }

        public async Task<IActionResult> AddEmployeeActivityStatusHistory(APICredentials credentials, EmployeeLogActivityForm param)
        {
            if (!string.IsNullOrEmpty(param.Remarks))
            //    ErrorMessages.Add("Remarks " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
              if (param.Title.Length > 100)
            {
                param.Title = param.Title.Trim();
                ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            }


            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (string.IsNullOrEmpty(param.DueDate.ToString()))
                ErrorMessages.Add("Due Date " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            //if (string.IsNullOrEmpty(param.SubType))
            //    ErrorMessages.Add("SubType " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            //{
            //    param.SubType = param.SubType.Trim();
            //    if (param.SubType.Length > 50)
            //        ErrorMessages.Add(string.Concat("SubType", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            //}

            param.SubType = "";

            if (!string.IsNullOrEmpty(param.Description))
            //    ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            //else
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }


            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0)
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;

            Data.LogActivity.EmployeeLogActivity result = await _dbAccess.GetEmployeeLogActivityByID(param.ID);
            // Update current status and timestamp
            result.Type = param.Type;
            result.SubType = param.SubType;
            result.Title = param.Title;
            result.Description = param.Description;
            result.IsPass = param.IsPass;
            if (!string.IsNullOrEmpty(param.Status))
            {
                result.CurrentStatus = param.Status ?? "";
            }
            result.CurrentTimestamp = currentDatetime;
            result.AssignedUserID = param.AssignedUserID;
            result.IsWithAssignment = param.AssignedUserID > 0;
            result.AssignedOrgGroupID = param.AssignedOrgGroupID;
            result.IsVisible = param.IsVisible;
            result.DueDate = param.DueDate;

            await _dbAccess.AddEmployeeActivityStatusHistory(
           result
                , new EmployeeLogActivityStatusHistory
            {
                EmployeeLogActivityID = param.ID,
                Status = param.Status ?? "",
                Timestamp = currentDatetime,
                Remarks = param.Remarks,
                UserID = credentials.UserID,
                IsPass = param.IsPass
            }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> GetAssignedActivitiesList(APICredentials credentials, GetAssignedActivitiesListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarAssignedActivities> result = await _dbAccess.GetAssignedActivitiesList(input, rowStart, credentials.UserID);

            return new OkObjectResult(result.Select(x => new GetAssignedActivitiesListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Type = x.Type,
                SubType = x.SubType,
                Title = x.Title,
                Description = x.Description,                
                CurrentStatus = x.CurrentStatus,
                CurrentTimestamp = x.CurrentTimestamp,
                CreatedBy = x.CreatedBy,
                ApplicantID = x.ApplicantID,
                EmployeeID = x.EmployeeID,
                DueDate = x.DueDate,
                OrgGroupID = x.OrgGroupID,
                CurrentStatusCode = x.CurrentStatusCode,
                Remarks = x.Remarks
            }).ToList());
        }

        public async Task<IActionResult> PostApplicantComments(APICredentials credentials, ApplicantLogActivityCommentsForm param)
        {

            if (param.ApplicantLogActivityID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantLogActivityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Comments = param.Comments.Trim();
            if (string.IsNullOrEmpty(param.Comments))
                ErrorMessages.Add(string.Concat("Comments ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Comments.Length > 500)
                ErrorMessages.Add(string.Concat("Comments", MessageUtilities.COMPARE_NOT_EXCEED, "500 characters."));

            if (param.CreatedBy <= 0)
                ErrorMessages.Add(string.Concat("CreatedBy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostApplicantComments(new ApplicantLogActivityComments
                {
                    ApplicantLogActivityID = param.ApplicantLogActivityID,
                    Comments = param.Comments,
                    CreatedBy = param.CreatedBy
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetApplicantComments(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApplicantComments(ID))
                .Select(x => new ApplicantLogActivityGetCommentsOutput
                {
                    Timestamp = x.CreatedDate.ToString("yyyy-MM-dd (hh:mm:ss tt)"),
                    Comments = x.Comments,
                    CreatedBy = x.CreatedBy
                }));
        }
        
        public async Task<IActionResult> PostApplicantAttachment(APICredentials credentials, ApplicantLogActivityAttachmentForm param)
        {

            if (param.ApplicantLogActivityID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantLogActivityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.AddAttachmentForm?.Count > 0)
            {
                foreach (var item in param.AddAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.AttachmentType))
                        ErrorMessages.Add(string.Concat("AttachmentType ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.AttachmentType = item.AttachmentType.Trim();
                        if (item.AttachmentType.Length > 20)
                            ErrorMessages.Add(string.Concat("AttachmentType", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!string.IsNullOrEmpty(item.Remarks))
                        //ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    //else
                    {
                        item.Remarks = item.Remarks.Trim();
                        if (item.Remarks.Length > 255)
                            ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
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
                List<ApplicantLogActivityAttachment> GetToAdd(List<ApplicantLogActivityAttachment> left, List<ApplicantLogActivityAttachment> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.ApplicantLogActivityID, x.ServerFile },
                        y => new { y.ApplicantLogActivityID, y.ServerFile },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new ApplicantLogActivityAttachment
                        {
                            ApplicantLogActivityID = x.newSet.newSet.ApplicantLogActivityID,
                            AttachmentType = x.newSet.newSet.AttachmentType,
                            Remarks = x.newSet.newSet.Remarks,
                            SourceFile = x.newSet.newSet.SourceFile,
                            ServerFile = x.newSet.newSet.ServerFile,
                            CreatedBy = credentials.UserID
                        })
                    .ToList();
                }

                List<ApplicantLogActivityAttachment> GetToUpdate(List<ApplicantLogActivityAttachment> left, List<ApplicantLogActivityAttachment> right)
                {
                    return left.Join(
                    right,
                    x => new { x.ApplicantLogActivityID, x.ServerFile },
                    y => new { y.ApplicantLogActivityID, y.ServerFile },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.AttachmentType.Equals(x.newSet.AttachmentType)
                        || !(x.oldSet.Remarks ?? "").Equals(x.newSet.Remarks ?? "")
                        || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                    )
                    .Select(y => new ApplicantLogActivityAttachment
                    {
                        ID = y.oldSet.ID,
                        ApplicantLogActivityID = y.newSet.ApplicantLogActivityID,
                        AttachmentType = y.newSet.AttachmentType,
                        Remarks = y.newSet.Remarks,
                        SourceFile = y.newSet.SourceFile,
                        ServerFile = y.newSet.ServerFile,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    })
                    .ToList();
                }

                List<ApplicantLogActivityAttachment> GetToDelete(List<ApplicantLogActivityAttachment> left, List<ApplicantLogActivityAttachment> right)
                {
                    return left.GroupJoin(
                            right,
                            x => new { x.ApplicantLogActivityID, x.ServerFile },
                            y => new { y.ApplicantLogActivityID, y.ServerFile },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new ApplicantLogActivityAttachment
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
                }

                List<ApplicantLogActivityAttachment> oldSet = (await _dbAccess.GetApplicantAttachment(param.ApplicantLogActivityID)).ToList();
                List<ApplicantLogActivityAttachment> paramAttachment = 
                    param.AddAttachmentForm.Select(x => new ApplicantLogActivityAttachment { 
                    ApplicantLogActivityID = param.ApplicantLogActivityID,
                    AttachmentType = x.AttachmentType,
                    ServerFile = x.ServerFile,
                    SourceFile = x.SourceFile,
                    Remarks = x.Remarks
                }).ToList();

                List<ApplicantLogActivityAttachment> ValueToAdd = GetToAdd(paramAttachment, oldSet).ToList();
                List<ApplicantLogActivityAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachment).ToList();
                List<ApplicantLogActivityAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachment).ToList();

                List<ApplicantLogActivityAttachment> addAttachment = new List<ApplicantLogActivityAttachment>();
                foreach (var item in param.AddAttachmentForm)
                {
                    addAttachment.Add(new ApplicantLogActivityAttachment { 
                        ApplicantLogActivityID = param.ApplicantLogActivityID,
                        AttachmentType = item.AttachmentType,
                        ServerFile = item.ServerFile,
                        SourceFile = item.SourceFile,
                        Remarks = item.Remarks,
                        CreatedBy = credentials.UserID,
                    });
                }

                await _dbAccess.PostApplicantAttachment(ValueToAdd, ValueToUpdate, ValueToDelete);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetApplicantAttachment(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApplicantAttachment(ID))
                .Select(x => new AttachmentForm
                {
                    AttachmentType = x.AttachmentType,
                    Remarks = x.Remarks,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile,
                    Timestamp = x.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    CreatedBy = x.CreatedBy
                }));
        }
        public async Task<IActionResult> PostEmployeeComments(APICredentials credentials, EmployeeLogActivityCommentsForm param)
        {

            if (param.EmployeeLogActivityID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeLogActivityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Comments = param.Comments.Trim();
            if (string.IsNullOrEmpty(param.Comments))
                ErrorMessages.Add(string.Concat("Comments ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Comments.Length > 500)
                ErrorMessages.Add(string.Concat("Comments", MessageUtilities.COMPARE_NOT_EXCEED, "500 characters."));

            if (param.CreatedBy <= 0)
                ErrorMessages.Add(string.Concat("CreatedBy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostEmployeeComments(new EmployeeLogActivityComments
                {
                    EmployeeLogActivityID = param.EmployeeLogActivityID,
                    Comments = param.Comments,
                    CreatedBy = param.CreatedBy
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetEmployeeComments(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeComments(ID))
                .Select(x => new EmployeeLogActivityGetCommentsOutput
                {
                    Timestamp = x.CreatedDate.ToString("yyyy-MM-dd (hh:mm:ss tt)"),
                    Comments = x.Comments,
                    CreatedBy = x.CreatedBy
                }));
        }
        
        public async Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeLogActivityAttachmentForm param)
        {

            if (param.EmployeeLogActivityID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeLogActivityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.AddAttachmentForm?.Count > 0)
            {
                foreach (var item in param.AddAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.AttachmentType))
                        ErrorMessages.Add(string.Concat("AttachmentType ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.AttachmentType = item.AttachmentType.Trim();
                        if (item.AttachmentType.Length > 20)
                            ErrorMessages.Add(string.Concat("AttachmentType", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!string.IsNullOrEmpty(item.Remarks))
                        //ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    //else
                    {
                        item.Remarks = item.Remarks.Trim();
                        if (item.Remarks.Length > 255)
                            ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
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
                List<EmployeeLogActivityAttachment> GetToAdd(List<EmployeeLogActivityAttachment> left, List<EmployeeLogActivityAttachment> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.EmployeeLogActivityID, x.ServerFile },
                        y => new { y.EmployeeLogActivityID, y.ServerFile },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new EmployeeLogActivityAttachment
                        {
                            EmployeeLogActivityID = x.newSet.newSet.EmployeeLogActivityID,
                            AttachmentType = x.newSet.newSet.AttachmentType,
                            Remarks = x.newSet.newSet.Remarks,
                            SourceFile = x.newSet.newSet.SourceFile,
                            ServerFile = x.newSet.newSet.ServerFile,
                            CreatedBy = credentials.UserID
                        })
                    .ToList();
                }

                List<EmployeeLogActivityAttachment> GetToUpdate(List<EmployeeLogActivityAttachment> left, List<EmployeeLogActivityAttachment> right)
                {
                    return left.Join(
                    right,
                    x => new { x.EmployeeLogActivityID, x.ServerFile },
                    y => new { y.EmployeeLogActivityID, y.ServerFile },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.AttachmentType.Equals(x.newSet.AttachmentType)
                        || !(x.oldSet.Remarks ?? "").Equals(x.newSet.Remarks ?? "")
                        || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                    )
                    .Select(y => new EmployeeLogActivityAttachment
                    {
                        ID = y.oldSet.ID,
                        EmployeeLogActivityID = y.newSet.EmployeeLogActivityID,
                        AttachmentType = y.newSet.AttachmentType,
                        Remarks = y.newSet.Remarks,
                        SourceFile = y.newSet.SourceFile,
                        ServerFile = y.newSet.ServerFile,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    })
                    .ToList();
                }

                List<EmployeeLogActivityAttachment> GetToDelete(List<EmployeeLogActivityAttachment> left, List<EmployeeLogActivityAttachment> right)
                {
                    return left.GroupJoin(
                            right,
                            x => new { x.EmployeeLogActivityID, x.ServerFile },
                            y => new { y.EmployeeLogActivityID, y.ServerFile },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new EmployeeLogActivityAttachment
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
                }

                List<EmployeeLogActivityAttachment> oldSet = (await _dbAccess.GetEmployeeAttachment(param.EmployeeLogActivityID)).ToList();
                List<EmployeeLogActivityAttachment> paramAttachment = 
                    param.AddAttachmentForm.Select(x => new EmployeeLogActivityAttachment { 
                    EmployeeLogActivityID = param.EmployeeLogActivityID,
                    AttachmentType = x.AttachmentType,
                    ServerFile = x.ServerFile,
                    SourceFile = x.SourceFile,
                    Remarks = x.Remarks
                }).ToList();

                List<EmployeeLogActivityAttachment> ValueToAdd = GetToAdd(paramAttachment, oldSet).ToList();
                List<EmployeeLogActivityAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachment).ToList();
                List<EmployeeLogActivityAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachment).ToList();

                List<EmployeeLogActivityAttachment> addAttachment = new List<EmployeeLogActivityAttachment>();
                foreach (var item in param.AddAttachmentForm)
                {
                    addAttachment.Add(new EmployeeLogActivityAttachment { 
                        EmployeeLogActivityID = param.EmployeeLogActivityID,
                        AttachmentType = item.AttachmentType,
                        ServerFile = item.ServerFile,
                        SourceFile = item.SourceFile,
                        Remarks = item.Remarks,
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
                    AttachmentType = x.AttachmentType,
                    Remarks = x.Remarks,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile,
                    Timestamp = x.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    CreatedBy = x.CreatedBy
                }));
        }

        public async Task<IActionResult> GetLogActivityPreloadedDropdown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllLogActivityPreloaded()).ToList(), "ID", "PreloadName", null));
        }

        public async Task<IActionResult> GetLogActivityByPreloadedID(APICredentials credentials, int LogActivityPreloadedID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetLogActivityByPreloadedID(LogActivityPreloadedID))
                .Select(x => new GetLogActivityByPreloadedIDOutput
                {
                    Module = x.Module,
                    Type = x.Type,
                    SubType = x.SubType,
                    Title = x.Title,
                    Description = x.Description,
                    IsPassFail = x.IsWithPassFail,
                    IsAssignment = x.IsWithAssignment
                }).ToList()
            );

            //return new OkObjectResult(
            //    (await _dbAccess.GetPreloadedItemsByID(LogActivityPreloadedID))
            //    .Select(x => new GetLogActivityByPreloadedIDOutput
            //    {
            //        Module = x.Module,
            //        Type = x.Type,
            //        SubType = x.SubType,
            //        Title = x.Title,
            //        Description = x.Description,
            //        IsPassFail = x.IsWithPassFail,
            //        IsAssignment = x.IsWithAssignment
            //    }).ToList()
            //);
        }

        public async Task<IActionResult> AddApplicantPreLoadedActivities(APICredentials credentials, AddApplicantPreLoadedActivitiesInput param)
        {
            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.LogActivityPreloadedIDs.Count <= 0)
                ErrorMessages.Add(string.Concat("LogActivityPreloadedIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            
            if (ErrorMessages.Count == 0)
            {
                List<ApplicantLogActivityStatusHistory> result =
                        (await _dbAccess.AddApplicantPreLoadedActivities(param, credentials.UserID)).ToList();
                return new OkObjectResult(string.Concat(result.First().ID, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED));
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }
        
        public async Task<IActionResult> AddEmployeePreLoadedActivities(APICredentials credentials, AddEmployeePreLoadedActivitiesInput param)
        {
            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.LogActivityPreloadedIDs.Count <= 0)
                ErrorMessages.Add(string.Concat("LogActivityPreloadedIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            
            if (ErrorMessages.Count == 0)
            {
                List<EmployeeLogActivityStatusHistory> result =
                        (await _dbAccess.AddEmployeePreLoadedActivities(param, credentials.UserID)).ToList();
                return new OkObjectResult(string.Concat(result.First().ID, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED));
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetLogActivitySubType(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetLogActivitySubType()).ToList());
        }

        public async Task<IActionResult> GetLogActivityPreloadedList(APICredentials credentials, GetPreLoadedListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarLogActivityPreLoaded> result = await _dbAccess.GetLogActivityPreloadedList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetPreLoadedListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                PreloadName = x.PreloadName,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> GetLogActivityPreloadedByID(APICredentials credentials, int ID)
        {
            Data.LogActivity.LogActivityPreloaded result = await _dbAccess.GetLogActivityPreloadedByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new LogActivityPreloadedForm
                {
                    ID = result.ID,
                    PreloadedName = result.PreloadName
                });
        }

        public async Task<IActionResult> AddLogActivityPreloaded(APICredentials credentials, LogActivityPreloadedForm param)
        {
            param.PreloadedName = param.PreloadedName.Trim();
            if (string.IsNullOrEmpty(param.PreloadedName))
                ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.PreloadedName.Length > 100)
                ErrorMessages.Add(string.Concat("Preloaded Name", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            else
            {
                if ((await _dbAccess.GetByPreloadedName(param.PreloadedName)).Count() > 0)
                {
                    ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.AddLogActivityPreloaded(new Data.LogActivity.LogActivityPreloaded
                    {
                        PreloadName = param.PreloadedName,
                        IsActive = true,
                        CreatedBy = credentials.UserID
                    }
                    , param.LogActivityList?.Select(x => new Data.LogActivity.LogActivity
                    { 
                        Module = x.Module,
                        Type = x.Type,
                        //SubType = x.SubType,
                        SubType = "",
                        Title = x.Title,
                        Description = x.Description,
                        IsWithPassFail = x.IsPassFail,
                        IsWithAssignment = x.IsAssignment,
                        IsActive = true,
                        CreatedBy = credentials.UserID,
                        IsVisible = x.IsVisible,
                        AssignedUserID = x.AssignedUserID
                    }).ToList()
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> DeleteLogActivityPreloaded(APICredentials credentials, int ID)
        {
            Data.LogActivity.LogActivityPreloaded preloaded = await _dbAccess.GetLogActivityPreloadedByID(ID);
            preloaded.IsActive = false;
            preloaded.ModifiedBy = credentials.UserID;
            preloaded.ModifiedDate = DateTime.Now;

            List<Data.LogActivity.LogActivity> logActivity = (await _dbAccess.GetPreloadedItemsByID(ID))
                .Select(x => new Data.LogActivity.LogActivity
                {
                    ID = x.ID,
                    Module = x.Module,
                    Type = x.Type,
                    SubType = x.SubType,
                    Title = x.Title,
                    Description = x.Description,
                    IsWithPassFail = x.IsWithPassFail,
                    IsWithAssignment = x.IsWithAssignment,
                    LogActivityPreloadedID = x.LogActivityPreloadedID,
                    IsActive = false,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now,
                    IsVisible = x.IsVisible,
                    AssignedUserID = x.AssignedUserID
                }).ToList();

            if (await _dbAccess.EditLogActivityPreloaded(preloaded,logActivity,null,null))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> EditLogActivityPreloaded(APICredentials credentials, LogActivityPreloadedForm param)
        {
            param.PreloadedName = param.PreloadedName.Trim();
            if (string.IsNullOrEmpty(param.PreloadedName))
                ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.PreloadedName.Length > 100)
                ErrorMessages.Add(string.Concat("Preloaded Name", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            else
            {
                if ((await _dbAccess.GetByPreloadedName(param.PreloadedName)).Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
            }

            if (ErrorMessages.Count == 0)
            {

                IEnumerable<Data.LogActivity.LogActivity> GetLogActivityToDelete(List<Data.LogActivity.LogActivity> left, List<Data.LogActivity.LogActivity> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.Module, x.Type, x.SubType, x.Title },
                             y => new { y.Module, y.Type, y.SubType, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.LogActivity.LogActivity
                            {
                                ID = x.oldSet.oldSet.ID,
                                Module = x.oldSet.oldSet.Module,
                                Type = x.oldSet.oldSet.Type,
                                SubType = x.oldSet.oldSet.SubType,
                                Title = x.oldSet.oldSet.Title,
                                Description = x.oldSet.oldSet.Description,
                                IsWithPassFail = x.oldSet.oldSet.IsWithPassFail,
                                IsWithAssignment = x.oldSet.oldSet.IsWithAssignment,
                                LogActivityPreloadedID = x.oldSet.oldSet.LogActivityPreloadedID,
                                IsActive = false,
                                CreatedBy = x.oldSet.oldSet.CreatedBy,
                                CreatedDate = x.oldSet.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now,
                                IsVisible = x.oldSet.oldSet.IsVisible,
                                AssignedUserID = x.oldSet.oldSet.AssignedUserID
                            }).ToList();
                }

                IEnumerable<Data.LogActivity.LogActivity> GetLogActivityToAdd(List<Data.LogActivity.LogActivity> left, List<Data.LogActivity.LogActivity> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.Module, x.Type, x.SubType, x.Title },
                             y => new { y.Module, y.Type, y.SubType, y.Title },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.LogActivity.LogActivity
                            {
                                Module = x.newSet.newSet.Module,
                                Type = x.newSet.newSet.Type,
                                //SubType = x.newSet.newSet.SubType,
                                SubType = "",
                                Title = x.newSet.newSet.Title,
                                Description = x.newSet.newSet.Description,
                                IsWithPassFail = x.newSet.newSet.IsWithPassFail,
                                IsWithAssignment = x.newSet.newSet.IsWithAssignment,
                                LogActivityPreloadedID = x.newSet.newSet.LogActivityPreloadedID,
                                IsActive = x.newSet.newSet.IsActive,
                                CreatedBy = x.newSet.newSet.CreatedBy,
                                IsVisible = x.newSet.newSet.IsVisible,
                                AssignedUserID = x.newSet.newSet.AssignedUserID
                            }).ToList();
                }

                IEnumerable<Data.LogActivity.LogActivity> GetLogActivityToUpdate(List<Data.LogActivity.LogActivity> left, List<Data.LogActivity.LogActivity> right)
                {
                    return left.Join(
                        right,
                             x => new { x.Module, x.Type, x.SubType, x.Title },
                             y => new { y.Module, y.Type, y.SubType, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Module.Equals(x.newSet.Module) ||
                                    !x.oldSet.Type.Equals(x.newSet.Type) ||
                                    !x.oldSet.SubType.Equals(x.newSet.SubType) ||
                                    !x.oldSet.Title.Equals(x.newSet.Title) ||
                                    (x.oldSet.Description != x.newSet.Description) ||
                                    (x.oldSet.IsWithPassFail != x.newSet.IsWithPassFail) ||
                                    (x.oldSet.IsWithAssignment != x.newSet.IsWithAssignment) ||
                                    (x.oldSet.IsVisible != x.newSet.IsVisible) ||
                                    (x.oldSet.AssignedUserID != x.newSet.AssignedUserID)
                              )
                        .Select(y =>
                            new Data.LogActivity.LogActivity
                            {
                                ID = y.oldSet.ID,
                                Module = y.newSet.Module,
                                Type = y.newSet.Type,
                                //SubType = y.newSet.SubType,
                                SubType = "",
                                Title = y.newSet.Title,
                                Description = y.newSet.Description,
                                IsWithPassFail = y.newSet.IsWithPassFail,
                                IsWithAssignment = y.newSet.IsWithAssignment,
                                LogActivityPreloadedID = y.newSet.LogActivityPreloadedID,
                                IsActive = y.oldSet.IsActive,
                                CreatedBy = y.oldSet.CreatedBy,
                                CreatedDate = y.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now,
                                IsVisible = y.newSet.IsVisible,
                                AssignedUserID = y.newSet.AssignedUserID
                            }).ToList();
                }

                List<Data.LogActivity.LogActivity> OldLogActivity = (await _dbAccess.GetPreloadedItemsByID(param.ID)).ToList();

                List<Data.LogActivity.LogActivity> LogActivityToAdd = GetLogActivityToAdd(OldLogActivity,
                    param.LogActivityList.Select(x => new Data.LogActivity.LogActivity
                    {
                        Module = x.Module,
                        Type = x.Type,
                        SubType = x.SubType,
                        Title = x.Title,
                        Description = x.Description,
                        IsWithPassFail = x.IsPassFail,
                        IsWithAssignment = x.IsAssignment,
                        LogActivityPreloadedID = param.ID,
                        IsActive = true,
                        CreatedBy = credentials.UserID,
                        IsVisible = x.IsVisible,
                        AssignedUserID = x.AssignedUserID
                    }).ToList()).ToList();


                List<Data.LogActivity.LogActivity> LogActivityToUpdate = GetLogActivityToUpdate(OldLogActivity,
                    param.LogActivityList.Select(x => new Data.LogActivity.LogActivity
                    {
                        Module = x.Module,
                        Type = x.Type,
                        SubType = x.SubType,
                        Title = x.Title,
                        Description = x.Description,
                        IsWithPassFail = x.IsPassFail,
                        IsWithAssignment = x.IsAssignment,
                        LogActivityPreloadedID = param.ID,
                        IsActive = true,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now,
                        IsVisible = x.IsVisible,
                        AssignedUserID = x.AssignedUserID
                    }).ToList()).ToList();


                List<Data.LogActivity.LogActivity> LogActivityToDelete = GetLogActivityToDelete(OldLogActivity,
                    param.LogActivityList.Select(x => new Data.LogActivity.LogActivity
                    {
                        Module = x.Module,
                        Type = x.Type,
                        SubType = x.SubType,
                        Title = x.Title,
                        Description = x.Description,
                        IsWithPassFail = x.IsPassFail,
                        IsWithAssignment = x.IsAssignment,
                        LogActivityPreloadedID = param.ID,
                        IsActive = false,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now,
                        IsVisible = x.IsVisible,
                        AssignedUserID = x.AssignedUserID
                    }).ToList()).ToList();

                Data.LogActivity.LogActivityPreloaded logActivityPreloaded = await _dbAccess.GetLogActivityPreloadedByID(param.ID);
                logActivityPreloaded.PreloadName = param.PreloadedName;
                logActivityPreloaded.IsActive = true;
                logActivityPreloaded.ModifiedBy = credentials.UserID;
                logActivityPreloaded.ModifiedDate = DateTime.Now;

                await _dbAccess.EditLogActivityPreloaded(
                    logActivityPreloaded,
                    LogActivityToDelete,
                    LogActivityToAdd,
                    LogActivityToUpdate
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetPreloadedItemsByID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetPreloadedItemsByID(ID))
                .Select(x => new GetLogActivityByPreloadedIDOutput
                {
                    Module = x.Module,
                    Type = x.Type,
                    SubType = x.SubType,
                    Title = x.Title,
                    Description = x.Description,
                    IsPassFail = x.IsWithPassFail,
                    IsAssignment = x.IsWithAssignment,
                    IsVisible = x.IsVisible,
                    AssignedUserID = x.AssignedUserID
                }).ToList()
            );
        }

        public async Task<IActionResult> GetApplicantLogActivityPendingEmail(APICredentials credentials)
        {
            List<ApplicantLogActivity> result = (await _dbAccess.GetApplicantLogActivityPendingEmail()).ToList();

            result = result.Select(x =>
            {
                x.Type = (_dbReferenceService.GetByRefCodeValue("ACTIVITY_TYPE", x.Type).Result).Description;
                return x;
            }).ToList();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> GetEmployeeLogActivityPendingEmail(APICredentials credentials)
        {
            List<EmployeeLogActivity> result = (await _dbAccess.GetEmployeeLogActivityPendingEmail()).ToList();

            result = result.Select(x =>
            {
                x.Type = (_dbReferenceService.GetByRefCodeValue("ACTIVITY_TYPE", x.Type).Result).Description;
                return x;
            }).ToList();

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> UpdateApplicantLogActivityPendingEmail(APICredentials credentials, int ID)
        {
            ApplicantLogActivity result = await _dbAccess.GetApplicantLogActivityByID(ID);
            result.IsEmailSent = true;
            result.SentDateTime = DateTime.Now;

            return new OkObjectResult(await _dbAccess.UpdateApplicantLogActivityPendingEmail(result));
        }

        public async Task<IActionResult> UpdateEmployeeLogActivityPendingEmail(APICredentials credentials, int ID)
        {
            EmployeeLogActivity result = await _dbAccess.GetEmployeeLogActivityByID(ID);
            result.IsEmailSent = true;
            result.SentDateTime = DateTime.Now;

            return new OkObjectResult(await _dbAccess.UpdateEmployeeLogActivityPendingEmail(result));
        }

        public async Task<IActionResult> GetChecklistList(APICredentials credentials, GetChecklistListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarChecklist> result = await _dbAccess.GetChecklistList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetChecklistListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Type = x.Type,
                SubType = x.SubType,
                Title = x.Title,
                Description = x.Description,
                CurrentStatus = x.CurrentStatus,
                CurrentTimestamp = x.CurrentTimestamp,
                CreatedBy = x.CreatedBy,
                EmployeeID = x.EmployeeID,
                AssignedUserID = x.AssignedUserID,
                Remarks = x.Remarks,
                IsAssignment = x.IsAssignment,
                DueDate = x.DueDate
            }).ToList());
        }

        public async Task<IActionResult> UpdateEmployeeLogActivityAssignedUser(APICredentials credentials, UpdateEmployeeLogActivityAssignedUserForm param)
        {
            if (await _dbAccess.UpdateEmployeeLogActivityAssignedUser(param))
            {
                _resultView.IsSuccess = true;
            };

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> UpdateApplicantLogActivityAssignedUser(APICredentials credentials, UpdateApplicantLogActivityAssignedUserForm param)
        {
            if (await _dbAccess.UpdateApplicantLogActivityAssignedUser(param))
            {
                _resultView.IsSuccess = true;
            };

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetApplicantLogActivityList(APICredentials credentials, GetApplicantLogActivityListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarApplicantLogActivityList> result = await _dbAccess.GetApplicantLogActivityList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetApplicantLogActivityListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Type = x.Type,
                SubType = x.SubType,
                Title = x.Title,
                Description = x.Description,
                CurrentStatus = x.CurrentStatus,
                CurrentTimestamp = x.CurrentTimestamp,
                CreatedBy = x.CreatedBy,
                ApplicantID = x.ApplicantID,
                AssignedUserID = x.AssignedUserID,
                Remarks = x.Remarks,
                IsAssignment = x.IsAssignment,
                DueDate = x.DueDate
            }).ToList());
        }

        public async Task<IActionResult> GetEmployeeLogActivityList(APICredentials credentials, GetChecklistListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarChecklist> result = await _dbAccess.GetEmployeeLogActivityList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetChecklistListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Type = x.Type,
                SubType = x.SubType,
                Title = x.Title,
                Description = x.Description,
                CurrentStatus = x.CurrentStatus,
                CurrentTimestamp = x.CurrentTimestamp,
                CreatedBy = x.CreatedBy,
                EmployeeID = x.EmployeeID,
                AssignedUserID = x.AssignedUserID,
                Remarks = x.Remarks,
                IsAssignment = x.IsAssignment,
                DueDate = x.DueDate
            }).ToList());
        }

        public async Task<IActionResult> BatchUpdateLogActivity(APICredentials credentials, BatchTaskForm param)
        {

            param.Status = (param.Status ?? "").Trim();
            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.Status.Length > 20)
                ErrorMessages.Add(string.Concat("Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            param.Remarks = (param.Remarks ?? "").Trim();
            if (param.Remarks.Length > 255)
                ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (ErrorMessages.Count == 0)
            {

                if (param.ApplicantIDs != null)
                {
                    IEnumerable<Data.LogActivity.ApplicantLogActivity> lstApplicantLogActivity = await _dbAccess.GetApplicantLogActivityByIDs(param.ApplicantIDs);

                    await _dbAccess.BatchUpdateApplicantLogActivity(
                        lstApplicantLogActivity.Select(x =>
                        {
                            x.CurrentStatus = param.Status;
                            x.CurrentTimestamp = DateTime.Now;
                            x.ModifiedBy = credentials.UserID;
                            x.ModifiedDate = DateTime.Now;
                            return x;
                        }).ToList(),
                        param.ApplicantIDs.Select(x => new ApplicantLogActivityStatusHistory 
                        {
                            ApplicantLogActivityID = x,
                            Status = param.Status,
                            Timestamp = DateTime.Now,
                            Remarks = param.Remarks,
                            UserID = credentials.UserID,
                            IsPass = false
                        }).ToList()
                    );
                }

                if (param.EmployeeIDs != null)
                {
                    IEnumerable<Data.LogActivity.EmployeeLogActivity> lstEmployeeLogActivity = await _dbAccess.GetEmployeeLogActivityByIDs(param.EmployeeIDs);

                    await _dbAccess.BatchUpdateEmployeeLogActivity(
                        lstEmployeeLogActivity.Select(x =>
                        {
                            x.CurrentStatus = param.Status;
                            x.CurrentTimestamp = DateTime.Now;
                            x.ModifiedBy = credentials.UserID;
                            x.ModifiedDate = DateTime.Now;
                            return x;
                        }).ToList(),
                        param.EmployeeIDs.Select(x => new EmployeeLogActivityStatusHistory
                        {
                            EmployeeLogActivityID = x,
                            Status = param.Status,
                            Timestamp = DateTime.Now,
                            Remarks = param.Remarks,
                            UserID = credentials.UserID,
                            IsPass = false
                        }).ToList()
                    );
                }

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> UploadLogActivityInsert(APICredentials credentials, List<UploadLogActivityFile> param)
        {
            var typeList = (await _dbReferenceService.GetByRefCodes(new List<string> { "ACTIVITY_TYPE" })).Select(x => x.Value).ToList();

            /*Checking of required and invalid fields*/
            foreach (UploadLogActivityFile obj in param)
            {

                /*Employee ID*/
                if (obj.EmployeeID == 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee ID ", MessageUtilities.COMPARE_INVALID));
                }

                /*Employee Name*/
                if (string.IsNullOrEmpty(obj.EmployeeName))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee ID ", MessageUtilities.COMPARE_INVALID));
                }

                /*Type*/
                if (string.IsNullOrEmpty(obj.Type))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else 
                {
                    obj.Type = obj.Type.Trim();
                    if (obj.Type.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.Type, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (typeList.Where(x => obj.Type.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Title*/
                if (string.IsNullOrEmpty(obj.Title))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Title = obj.Title.Trim();
                    if (obj.Title.Length > 100)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
                    }
                }

                /*Description*/
                if (!string.IsNullOrEmpty(obj.Description))
                {
                    obj.Description = obj.Description.Trim();
                    if (obj.Description.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*Remarks*/
                if (!string.IsNullOrEmpty(obj.Remarks))
                {
                    obj.Remarks = obj.Remarks.Trim();
                    if (obj.Remarks.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*OrgGroup Code*/
                if (string.IsNullOrEmpty(obj.OrgGroupCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.AssignedOrgGroupID == 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.COMPARE_INVALID));
                }
                else
                {
                    obj.OrgGroupCode = obj.OrgGroupCode.Trim();
                    if (obj.OrgGroupCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
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
                    obj.Type.Equals(x.Type, StringComparison.OrdinalIgnoreCase) &
                    obj.Title.Equals(x.Title, StringComparison.OrdinalIgnoreCase) &
                    obj.OrgGroupCode.Equals(x.OrgGroupCode, StringComparison.OrdinalIgnoreCase) &
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                        Duplicates.Add("Row [" + obj.RowNum + "]");
                    }


                    /* Remove duplicates from database */
                    var duplicateFromDatabase = (await _dbAccess.GetByUniqueLogActivity(obj.EmployeeID, obj.Type, obj.Title, obj.AssignedOrgGroupID)).ToList();
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
                List<UploadLogActivityFile> logActivityList = new List<UploadLogActivityFile>();

                if (param != null)
                {
                    foreach (var obj in param)
                    {
                        logActivityList.Add(new UploadLogActivityFile
                        {
                            EmployeeID = obj.EmployeeID,
                            EmployeeName = obj.EmployeeName,
                            Email = obj.Email,
                            AssignedUserId = obj.AssignedUserId,
                            Type = obj.Type,
                            Title = obj.Title,
                            Description = obj.Description,
                            DueDate = obj.DueDate,
                            AssignedOrgGroupID = obj.AssignedOrgGroupID,
                            Remarks = obj.Remarks,
                            OrgGroupCode = obj.OrgGroupCode,
                            CurrentStatus = Transfer.Enums.ActivityStatus.NEW.ToString(),
                            CreatedBy = obj.CreatedBy
                        });
                    }

                    await _dbAccess.UploadLogActivityInsert(logActivityList);
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
                        string.Concat(param?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                            MessageUtilities.ERRMSG_DUPLICATE_APPLICANT, "<br>",
                            string.Join("<br>", Duplicates.Distinct().ToArray()))
                        );
                }
                else
                {
                    return new OkObjectResult(string.Concat(param?.Count, " Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD));
                }
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
    }
}