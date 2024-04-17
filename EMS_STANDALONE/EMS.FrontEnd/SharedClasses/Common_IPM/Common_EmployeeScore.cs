using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.IPM.Transfer.EmployeeScore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EMS.FrontEnd.SharedClasses.Common_IPM
{
    public class Common_EmployeeScore : Utilities
    {
        public Common_EmployeeScore(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }
            
        public async Task<Form> GetEmployeeScore(GetByIDInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("getbyid").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", param.ID,"&",
                  "RoleIDs=", param.RoleIDs);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<ScoresOutput>> GetScores(RunScoreForm param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetScores").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "Filter=", param.Filter, "&",
                  "IDs=", param.IDs, "&",
                  "Employees=", param.Employees, "&",
                  "DateFrom=", param.DateFrom, "&",
                  "DateTo=", param.DateTo, "&",
                  "UseCurrent=", param.UseCurrent);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<ScoresOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetPositionDropdown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("Position").GetSection("GetDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<SelectListItem>> GetKPIDropdown(int id = 0)
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetDropdown").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&id=", id);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllKPIOutput>> GetAllKPI()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("KPI").GetSection("GetAll").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllKPIOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAllEmployeeScoreOutput>> GetAllEmployeeScore()
        {
            var URL = string.Concat(_ipmBaseURL,
               _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetAll").Value, "?",
                "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAllEmployeeScoreOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetEmployeeScoreStatusHistoryOutput>> GetEmployeeScoreStatusHistory(int TID)
        {
            var URL = string.Concat(_workflowBaseURL,
                     _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("GetEmployeeScoreStatusHistory").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "TID=", TID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetEmployeeScoreStatusHistoryOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
		
        public async Task<GetTransProgressOutput> GetTransProgress()
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetTransProgress").Value, "?",
                     "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetTransProgressOutput(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<GetTransEmployeeScoreSummaryOutput> GetTransSummary(int TransSummaryID)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetTransSummary").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "TransSummaryID=", TransSummaryID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetTransEmployeeScoreSummaryOutput(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>> GetSummaryAutoComplete(GetSummaryAutoCompleteInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetSummaryAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults, "&",
                     "IsAdmin=", param.IsAdmin);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.IPM.Transfer.Shared.GetAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        public async Task<List<EMS.IPM.Data.EmployeeScore.TableVarResult>> GetIPMRaterByTransID(int ID)
        {
            var URL = string.Concat(_ipmBaseURL,
                     _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetIPMRaterByTransID").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "ID=", ID, "&");

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<EMS.IPM.Data.EmployeeScore.TableVarResult>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<GetFinalScoreListOutput>, bool, string)> GetFinalScoreList([FromQuery] GetFinalScoreListInput param)
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetFinalScoreList").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "sidx=", param.sidx, "&",
                  "sord=", param.sord, "&",
                  "pageNumber=", param.pageNumber, "&",
                  "rows=", param.rows, "&",

                  "ID=", param.ID, "&",
                  "RunIDDelimited=", param.RunIDDelimited, "&",
                  "EmployeeIDDelimited=", param.EmployeeIDDelimited, "&",
                  "IPMCount=", param.IPMCount, "&",
                  "IPMMonths=", param.IPMMonths, "&",
                  "FinalScoreFrom=", param.FinalScoreFrom, "&",
                  "FinalScoreTo=", param.FinalScoreTo, "&",
                  "CreatedBy=", param.CreatedBy, "&",
                  "CreatedDateFrom=", param.CreatedDateFrom, "&",
                  "CreatedDateTo=", param.CreatedDateTo, "&",
                  "IsExport=", param.IsExport);

            return await SharedUtilities.GetFromAPI(new List<GetFinalScoreListOutput>(), URL);
        }

        public async Task<List<Dropdown>> GetRunIDDropDown()
        {
            var URL = string.Concat(_ipmBaseURL,
                  _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("GetRunIDDropDown").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<Dropdown>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}
