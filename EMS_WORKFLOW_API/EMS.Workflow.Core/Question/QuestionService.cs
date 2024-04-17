using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Question;
using EMS.Workflow.Transfer.Question;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.Question
{
    public interface IQuestionService
    {
        Task<IActionResult> GetQuestionList(APICredentials credentials);
        Task<IActionResult> GetQuestionByCategory(APICredentials credentials, string Category);
        Task<IActionResult> GetQuestionEmployeeAnswer(APICredentials credentials, string Category, long EmployeeID);
        Task<IActionResult> AddQuestionEmployeeAnswers(APICredentials credentials, List<QuestionEmployeeAnswerInput> param);
        Task<IActionResult> GetQuestionEmployeeAnswersExport(APICredentials credentials, List<QuestionEmployeeAnswerInput> param);
        Task<IActionResult> GetQuestion(APICredentials credentials);
    }

    public class QuestionService : Core.Shared.Utilities, IQuestionService
    {
        private readonly EMS.Workflow.Data.Question.IQuestionDBAccess _dbAccess;
        private readonly EMS.Workflow.Data.Reference.IReferenceDBAccess _dbReferenceService;

        public QuestionService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.Question.IQuestionDBAccess dBAccess, Data.Reference.IReferenceDBAccess dbReferenceService) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
            _dbReferenceService = dbReferenceService;
        }

        public async Task<IActionResult> GetQuestionList(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetQuestionList());
        }

        public async Task<IActionResult> GetQuestionByCategory(APICredentials credentials, string Category)
        {
            // GET ANSWER
            var GetAnswer = (await _dbAccess.GetAllAnswer())
                .Select(x => new AnswerTable()
                {
                    ID = x.ID,
                    QuestionID = x.QuestionID,
                    Answer = x.Answer,
                    AddReason = x.AddReason
                }).ToList();
            var BuildAnswer = (from l in GetAnswer
                               group l by l.QuestionID into f
                               select new { QuestionID = f.Key, AnswerTable = f.ToList() });

            // GET 1ST SUB QUESTION
            var GetSubQuestion = (await _dbAccess.GetAllQuestion())
                .Select(x=>new GetQuestionTable()
                {
                    ID = x.ID,
                    Category = x.Category,
                    Code = x.Code,
                    Question = x.Question,
                    QuestionType = x.QuestionType,
                    AnswerType = x.AnswerType,
                    ParentQuestionID = x.ParentQuestionID,
                    Tab = x.Tab,
                    Order = x.Order,
                    IsRequired = x.IsRequired
                }).Where(y => !y.ParentQuestionID.Equals(0) && y.Category.Equals(Category)).ToList();

            var AddAnswerSubQuestion = (from l in GetSubQuestion
                                        join a in BuildAnswer on l.ID equals a.QuestionID into aTable
                                        from aAns in aTable.DefaultIfEmpty()
                                        select new GetQuestionTable()
                                        {
                                            ID = l.ID,
                                            Category = l.Category,
                                            Code = l.Code,
                                            Question = l.Question,
                                            QuestionType = l.QuestionType,
                                            AnswerType = l.AnswerType,
                                            ParentQuestionID = l.ParentQuestionID,
                                            Tab = l.Tab,
                                            Order = l.Order,
                                            IsRequired = l.IsRequired,
                                            Answer = aAns == null ? null : aAns.AnswerTable
                                        }).ToList();
            
            var BuildSubQuestion = (from l in AddAnswerSubQuestion
                                    group l by l.ParentQuestionID into f
                                    select new { ParentQuestionID = f.Key, GetQuestionTable = f.OrderBy(z => z.Order).ToList() });

            // GET 2ND SUB QUESTION
            var GetAllSubQuestion = (from m in GetSubQuestion
                                     join s in BuildSubQuestion on m.ID equals s.ParentQuestionID into sTable
                                     from sQues in sTable.DefaultIfEmpty()
                                     join a in BuildAnswer on m.ID equals a.QuestionID into aTable
                                     from aAns in aTable.DefaultIfEmpty()
                                     select new GetQuestionTable()
                                     {
                                         ID = m.ID,
                                         Category = m.Category,
                                         Code = m.Code,
                                         Question = m.Question,
                                         QuestionType = m.QuestionType,
                                         AnswerType = m.AnswerType,
                                         ParentQuestionID = m.ParentQuestionID,
                                         Tab = m.Tab,
                                         Order = m.Order,
                                         IsRequired = m.IsRequired,
                                         SubQuestion = sQues == null ? null : sQues.GetQuestionTable,
                                         Answer = aAns == null ? null : aAns.AnswerTable
                                     }).ToList();

            var BuildSubQuestions = (from l in GetAllSubQuestion
                                     group l by l.ParentQuestionID into f
                                    select new { ParentQuestionID = f.Key, GetQuestionTable = f.OrderBy(z => z.Order).ToList() });

            // GET MAIN QUESTION
            var GetMainQuestion = (await _dbAccess.GetAllQuestion()).Where(x => x.ParentQuestionID.Equals(0) && x.Category.Equals(Category)).ToList();

            var Final = (from m in GetMainQuestion
                         join s in BuildSubQuestions on m.ID equals s.ParentQuestionID into sTable
                         from sQues in sTable.DefaultIfEmpty()
                         join a in BuildAnswer on m.ID equals a.QuestionID into aTable
                         from aAns in aTable.DefaultIfEmpty()
                         select new GetQuestionTable()
                         {
                            ID = m.ID,
                            Category = m.Category,
                            Code = m.Code,
                            Question = m.Question,
                            QuestionType = m.QuestionType,
                            AnswerType = m.AnswerType,
                            ParentQuestionID = m.ParentQuestionID,
                            Tab = m.Tab,
                            Order = m.Order,
                            IsRequired = m.IsRequired,
                            SubQuestion = sQues == null ? null : sQues.GetQuestionTable,
                            Answer = aAns == null ? null : aAns.AnswerTable
                         }).ToList();

            return new OkObjectResult(Final);
        }

        public async Task<IActionResult> GetQuestionEmployeeAnswer(APICredentials credentials, string Category, long EmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetQuestionEmployeeAnswer(Category, EmployeeID));
        }

        public async Task<IActionResult> AddQuestionEmployeeAnswers(APICredentials credentials, List<QuestionEmployeeAnswerInput> param)
        {
            var Insert = param.Select(x => new QuestionEmployeeAnswer()
            {
                EmployeeID = x.EmployeeID,
                QuestionID = x.QuestionID,
                AnswerID = x.AnswerID,
                AnswerDetails = x.AnswerDetails,
                IsActive = true,
                CreatedBy = credentials.UserID,
                CreatedDate = DateTime.Now
            }).ToList();

            var Result = await _dbAccess.AddQuestionEmployeeAnswers(Insert);
            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> GetQuestionEmployeeAnswersExport(APICredentials credentials, List<QuestionEmployeeAnswerInput> param)
        {
            return new OkObjectResult(await _dbAccess.GetQuestionEmployeeAnswersExport(param));
        }
        public async Task<IActionResult> GetQuestion(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetQuestion());
        }
    }
}
