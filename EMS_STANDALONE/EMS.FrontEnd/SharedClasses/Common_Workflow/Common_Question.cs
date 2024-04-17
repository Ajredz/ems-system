using EMS.Workflow.Transfer.Question;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Workflow
{
    public class Common_Question : Utilities
    {
        public Common_Question(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) :
            base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<(List<GetQuestionOutput>, bool, string)> GetQuestionByCategory(string Category)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("GetQuestionByCategory").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "Category=", Category);

            return await SharedUtilities.GetFromAPI(new List<GetQuestionOutput>(), URL);
        }
        public async Task<(List<GetQuestionOutput>, bool, string)> GetQuestionEmployeeAnswer(string Category, int EmployeeID)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("GetQuestionEmployeeAnswer").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "Category=", Category, "&",
                  "EmployeeID=", EmployeeID);

            return await SharedUtilities.GetFromAPI(new List<GetQuestionOutput>(), URL);
        }
        public async Task<(List<SPGetQuestionEmployeeAnswerExport>, bool, string)> GetQuestionEmployeeAnswersExport(List<QuestionEmployeeAnswerInput> param)
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("GetQuestionEmployeeAnswersExport").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<SPGetQuestionEmployeeAnswerExport>(), param, URL);
        }
        public async Task<(List<QuestionTable>, bool, string)> GetQuestion()
        {
            var URL = string.Concat(_workflowBaseURL,
                  _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("GetQuestion").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.GetFromAPI(new List<QuestionTable>(), URL);
        }
    }
}
