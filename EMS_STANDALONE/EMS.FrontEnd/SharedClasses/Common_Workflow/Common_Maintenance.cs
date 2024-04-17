using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Hosting;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Mvc;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{

    public class Common_Maintenance : Utilities
    {

        public Common_Maintenance(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<Form> GetWorkflow(int ID)
        {

            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetWorkflowDropDown(int ID = 0)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetDropDown").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetWorkflowAutoComplete(GetWorkflowStepAutoCompleteInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults, "&",
                     "WorkflowCode=", param.WorkflowCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetCodeByAutoCompleteOutput>> GetWorkflowStepAutoComplete(GetWorkflowStepAutoCompleteInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetCodeWorkflowStepByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults, "&",
                     "WorkflowCode=", param.WorkflowCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetCodeByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<AddWorkflowTransaction> GetTransactionByRecordID(GetTransactionByRecordIDInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Transaction").GetSection("GetTransactionByRecordID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "WorkflowCode=", param.WorkflowCode, "&",
                     "WorkflowID=", param.WorkflowID, "&",
                     "RecordID=", param.RecordID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new AddWorkflowTransaction(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(CurrentWorkflowStep, bool, string)> AddTransaction(AddWorkflowTransaction Form)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Transaction").GetSection("Add").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new CurrentWorkflowStep(), Form, URL);
        }

        public async Task<List<SelectListItem>> GetWorkflowStepDropDown(string WorkflowCode)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetWorkflowStepDropDown").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", WorkflowCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<GetWorkflowStepByWorkflowIDAndCodeOutput> GetWorkflowStepByWorkflowIDAndCode(GetWorkflowStepByWorkflowIDAndCodeInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetWorkflowStepByWorkflowCodeAndCode").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", param.WorkflowCode, "&",
                  "Code=", param.Code
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetWorkflowStepByWorkflowIDAndCodeOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetListOutput> GetLastStepByWorkflowCode(string WorkflowCode)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetLastStepByWorkflowCode").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", WorkflowCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetListOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetWorkflowStepByWorkflowCodeOutput>> GetWorkflowStepByWorkflowCode(string WorkflowCode)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetWorkflowStepByWorkflowCode").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", WorkflowCode);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetWorkflowStepByWorkflowCodeOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetNextWorkflowStepOutput>> GetNextWorkflowStep(GetNextWorkflowStepInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetNextWorkflowStep").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", param.WorkflowCode, "&",
                  "CurrentStepCode=", param.CurrentStepCode, "&",
                  "RoleIDDelimited=", param.RoleIDDelimited
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetNextWorkflowStepOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetNextWorkflowStepOutput>> GetWorkflowStepByRole(GetWorkflowStepByRoleInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetWorkflowStepByRole").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", param.WorkflowCode, "&",
                  "RoleIDDelimited=", param.RoleIDDelimited
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetNextWorkflowStepOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllWorkflowStepOutput>> GetAllWorkflowStep(GetAllWorkflowStepInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetAllWorkflowStep").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", param.WorkflowCode
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllWorkflowStepOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<GetRolesByWorkflowStepCodeOutput>> GetRolesByWorkflowStepCode(GetRolesByWorkflowStepCodeInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetRolesByWorkflowStepCode").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "WorkflowCode=", param.WorkflowCode, "&",
                  "StepCode=", param.StepCode
                  );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetRolesByWorkflowStepCodeOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<(List<TableVarTransactionLastUpdateOutput>, bool, string)> GetLastStatusUpdateByRecordIDs(List<int> IDs)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Transaction").GetSection("GetLastStatusUpdateByRecordIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<TableVarTransactionLastUpdateOutput>(), IDs, URL);
        }

    }
}