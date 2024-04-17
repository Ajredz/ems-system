using EMS.Security.Data.DBContexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Security.Core.Workflow
{
    public interface IWorkflowService
    { 
    
    }
    public class WorkflowService : EMS.Security.Core.Shared.Utilities, IWorkflowService
    {

        public WorkflowService(SystemAccessContext dbContext, IConfiguration iconfiguration) :base (dbContext, iconfiguration)
        { 
        
        }
    }
}
