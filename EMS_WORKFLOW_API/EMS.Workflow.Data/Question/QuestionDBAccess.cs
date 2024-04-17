using EMS.Workflow.Data.Accountability;
using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Transfer.Question;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.Question
{
    public interface IQuestionDBAccess
    {
        Task<IEnumerable<QuestionTable>> GetQuestionList();
        Task<IEnumerable<TableVarQuestion>> GetQuestionByCategory(string Category);
        Task<IEnumerable<TableVarQuestionEmployeeAnswer>> GetQuestionEmployeeAnswer(string Category, long EmployeeID);
        Task<IEnumerable<QuestionTable>> GetAllQuestion();
        Task<IEnumerable<QuestionAnswer>> GetAllAnswer();
        Task<bool> AddQuestionEmployeeAnswers(List<QuestionEmployeeAnswer> param);
        Task<IEnumerable<SPGetQuestionEmployeeAnswerExport>> GetQuestionEmployeeAnswersExport(List<QuestionEmployeeAnswerInput> param);
        Task<IEnumerable<QuestionTable>> GetQuestion();
    }

    public class QuestionDBAccess : IQuestionDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public QuestionDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestionTable>> GetQuestionList()
        {
            return await _dbContext.QuestionTable.ToListAsync();
        }

        public async Task<IEnumerable<TableVarQuestion>> GetQuestionByCategory(string Category)
        {
            return await _dbContext.TableVarQuestion
                .FromSqlRaw(@"CALL sp_get_question(
                              {0}
                    )"
                    , Category ?? "")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarQuestionEmployeeAnswer>> GetQuestionEmployeeAnswer(string Category, long EmployeeID)
        {
            return await _dbContext.TableVarQuestionEmployeeAnswer
                .FromSqlRaw(@"CALL sp_get_question_employee_answer(
                            {0}
                            ,{1}
                    )"
                    , Category ?? ""
                    , EmployeeID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<QuestionTable>> GetAllQuestion()
        {
            return await _dbContext.QuestionTable.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<QuestionAnswer>> GetAllAnswer()
        {
            return await _dbContext.QuestionAnswer.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<bool> AddQuestionEmployeeAnswers(List<QuestionEmployeeAnswer> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.QuestionEmployeeAnswer.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
        public async Task<IEnumerable<SPGetQuestionEmployeeAnswerExport>> GetQuestionEmployeeAnswersExport(List<QuestionEmployeeAnswerInput> param)
        {
            return await _dbContext.SPGetQuestionEmployeeAnswerExport
                .FromSqlRaw(@"CALL sp_get_question_employee_answer_export(
                    )")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<QuestionTable>> GetQuestion()
        {
            return await _dbContext.QuestionTable.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }
    }
}
