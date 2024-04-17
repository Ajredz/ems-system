using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Reference;
using EMS.Workflow.Transfer.EmployeeScore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.EmployeeScore
{
    public interface IEmployeeScoreDBAccess
    {
        Task<EmployeeScore> GetEmployeeScoreByID(int ID);

        Task<bool> AddEmployeeScoreStatusHistory(EmployeeScoreApprovalHistory logActivityHistory);
        
        Task<bool> AddByBatch(List<EmployeeScoreApprovalHistory> logActivityHistory);

        Task<IEnumerable<TableVarEmployeeScoreStatusHistory>> GetEmployeeScoreStatusHistory(int TID);
    }

    public class EmployeeScoreDBAccess : IEmployeeScoreDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public EmployeeScoreDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeScore> GetEmployeeScoreByID(int ID)
        {
            return await _dbContext.EmployeeScore.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<bool> AddEmployeeScoreStatusHistory(EmployeeScoreApprovalHistory employeeScoreApprovalHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (!string.IsNullOrEmpty(employeeScoreApprovalHistory.Status))
                {
                    _dbContext.EmployeeScoreApprovalHistory.Add(employeeScoreApprovalHistory); 
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        
        public async Task<bool> AddByBatch(List<EmployeeScoreApprovalHistory> employeeScoreApprovalHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeScoreApprovalHistory.AddRange(employeeScoreApprovalHistory.Where(x => !string.IsNullOrEmpty(x.Status))); 
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarEmployeeScoreStatusHistory>> GetEmployeeScoreStatusHistory(int TID)
        {
            return await _dbContext.TableVarEmployeeScoreStatusHistory
               .FromSqlRaw("CALL sp_employee_score_status_history_get({0})", TID)
               .AsNoTracking().ToListAsync();

        }
    }
}