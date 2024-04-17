using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.Training;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{
    public class Common_Training : Utilities
    {
        public Common_Training(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) :
            base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<(List<GetListOutput>, bool, string)> GetList([FromQuery] GetListInput param, bool IsExport)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("List").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "TemplateName=", param.TemplateName, "&",
                  "CreatedDateFrom=", param.CreatedDateFrom, "&",
                  "CreatedDateTo=", param.CreatedDateTo, "&",
                  "IsExport=", IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetListOutput>(), URL);
        }
        public async Task<(bool, string)> Add(TrainingTempateInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("Add").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> Edit(TrainingTempateInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("Edit").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PutFromAPI(param, URL);
        }

        public async Task<List<SelectListItem>> GetTrainingTemplateDropdown()
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetTrainingTemplateDropdown").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<TrainingTempateInput> GetByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new TrainingTempateInput(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<TrainingTempateDetailsInput>> GetTrainingTemplateDetails(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetDetailsByTrainingTemplateID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<TrainingTempateDetailsInput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<(List<GetEmployeeTrainingListOutput>, bool, string)> GetEmployeeTrainingList([FromQuery] GetEmployeeTrainingListInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetEmployeeTrainingList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "EmployeeID=", param.EmployeeID, "&",
                  "TypeDelimited=", param.TypeDelimited, "&",
                  "Title=", param.Title, "&",
                  "StatusDelimited=", param.StatusDelimited, "&",
                  "CreatedBy=", param.CreatedBy, "&",
                  "CreatedDateFrom=", param.CreatedDateFrom, "&",
                  "CreatedDateTo=", param.CreatedDateTo, "&",
                  "ModifiedBy=", param.ModifiedBy, "&",
                  "ModifiedDateFrom=", param.ModifiedDateFrom, "&",
                  "ModifiedDateTo=", param.ModifiedDateTo, "&",
                  "DateScheduleFrom=", param.DateScheduleFrom, "&",
                  "DateScheduleTo=", param.DateScheduleTo, "&",
                  "IsAdmin=", param.IsAdminAccess, "&",
                  "IsExport=", param.IsExport, "&",
                  "UnderAccess=", param.UnderAccess);

            return await SharedUtilities.GetFromAPI(new List<GetEmployeeTrainingListOutput>(), URL);
        }
        public async Task<(bool,string)> AddEmployeeTrainingTemplate(AddEmployeeTrainingInput param)
        {
            var URL = string.Concat(_workflowBaseURL,
                 _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("AddEmployeeTrainingTemplate").Value, "?",
                 "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }

        public async Task<EmployeeTrainingForm> GetEmployeeTrainingByID(int ID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetEmployeeTrainingByID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EmployeeTrainingForm(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<(bool, string)> AddEmployeeTraining(EmployeeTrainingForm param)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("AddEmployeeTraining").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> EditEmployeeTraining(EmployeeTrainingForm param)
        {
            var URL = string.Concat(_workflowBaseURL,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("EditEmployeeTraining").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PutFromAPI(param, URL);
        }

        public async Task<(bool, string)> PostChangeStatus(ChangeStatus param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("ChangeStatus").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> UploadInsert(List<TrainingUploadFile> param)
        {
            var URL = string.Concat(_workflowBaseURL,
                               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("UploadInsert").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<List<GetEmployeeTrainingStatusHistoryOutput>> GetEmployeeTrainingStatusHistory(int EmployeeTrainingID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetEmployeeTrainingStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeTrainingID=", EmployeeTrainingID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeTrainingStatusHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<GetEmployeeTrainingScoreOutput>> GetEmployeeTrainingScore(int EmployeeTrainingID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetEmployeeTrainingScore").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "EmployeeTrainingID=", EmployeeTrainingID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeTrainingScoreOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> GetClassroomFromELMS(string term, int TopResults)
        {
            var HeaderName = "tokenizer";
            var HeaderValue = SharedUtilities.ComputeSHA256Hash("EMS" + DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).ToLower();
            var URL = "http://192.168.150.16/api-cmc/api/elms/fetchActiveClassroom";

            return await SharedUtilities.PostFromAPIWithHeader(new List<GetClassroomFromELMS>(), URL, HeaderName, HeaderValue);
        }

        public async Task<(List<EmployeeTrainingForm>,bool,string)> GetEmployeeByEmployeeTrainingIDs(List<long> IDs)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Training").GetSection("GetEmployeeByEmployeeTrainingIDs").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<EmployeeTrainingForm>(), IDs, URL);
        }
    }
}
