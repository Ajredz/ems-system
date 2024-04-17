using EMS.Manpower.Data.DBContexts;
using System.Threading.Tasks;

namespace EMS.Manpower.Data.Workflow
{
    public interface IWorkflowDBAccess
    {
        //Task<object> GetList(int SystemUserID, int PositionID);
    }

    public class WorkflowDBAccess : IWorkflowDBAccess
    {
        private readonly ManpowerContext _dbContext;

        public WorkflowDBAccess(ManpowerContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public Task<object> GetList(int SystemUserID, int PositionID)
        //{ 
            
        //}
    }
}