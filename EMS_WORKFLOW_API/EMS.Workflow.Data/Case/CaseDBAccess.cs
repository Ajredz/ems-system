using EMS.Workflow.Data.DBContexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Workflow.Data.Case
{
    public interface ICaseDBAccess
    {
        Task<bool> PostCaseMinorAudit(List<Case> paramCaseMinorAudit);
        Task<bool> PostCaseMinorAudit(Case paramCaseMinorAudit);
    }
    public class CaseDBAccess : ICaseDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public CaseDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> PostCaseMinorAudit(List<Case> paramCaseMinorAudit)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostCaseMinorAudit(Case paramCaseMinorAudit)
        {
            throw new NotImplementedException();
        }
    }
}
