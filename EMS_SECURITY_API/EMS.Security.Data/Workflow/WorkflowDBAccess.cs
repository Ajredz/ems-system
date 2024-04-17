using EMS.Security.Data.DBContexts;
using System.Threading.Tasks;

namespace EMS.Security.Data.Workflow
{
    public interface IWorkflowDBAccess
    {
        //Task<object> GetList(int SystemUserID, int PositionID);
    }

    public class WorkflowDBAccess : IWorkflowDBAccess
    {
        private readonly SystemAccessContext _dbContext;

        public WorkflowDBAccess(SystemAccessContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public Task<object> GetList(int SystemUserID, int PositionID)
        //{ 
            
        //}
    }
}