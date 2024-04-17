using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Transfer.EmailServerCredential;
using EMS.Workflow.Data.Reference;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.EmailServerCredential
{
    public interface IEmailServerCredentialDBAccess
    {
        Task<EmailServerCredential> GetByTemplateCode(string TemplateCode);
        Task<IEnumerable<EmailLogs>> GetPendingEmail();
        Task<bool> PostEmailLogs(EmailLogs param);
        Task<bool> PostMultipleEmailLogs(List<EmailLogs> param);
        Task<EmailLogs> GetEmailByID(int ID);
        Task<bool> PutEmailLogs(EmailLogs param);
        Task<bool> PostCronLogs(List<CronLogs> param);
    }

    public class EmailServerCredentialDBAccess : IEmailServerCredentialDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public EmailServerCredentialDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmailServerCredential> GetByTemplateCode(string TemplateCode)
        {
            return await _dbContext.EmailServerCredential.AsNoTracking()
                .Where(x => x.TemplateCode.Equals(TemplateCode)).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<EmailLogs>> GetPendingEmail()
        {
            return await _dbContext.EmailLogs.AsNoTracking()
                .Where(x => x.isSent.Equals(0) && x.SentStatus.Equals("PENDING")).ToListAsync();
        }
        public async Task<bool> PostEmailLogs(EmailLogs param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmailLogs.AddAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PostMultipleEmailLogs(List<EmailLogs> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmailLogs.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
        public async Task<EmailLogs> GetEmailByID(int ID)
        {
            return await _dbContext.EmailLogs.FindAsync(ID);
        }
        public async Task<bool> PutEmailLogs(EmailLogs param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> PostCronLogs(List<CronLogs> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.CronLogs.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
    }
}