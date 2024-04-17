using EMS.Workflow.Data.Accountability;
using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.CaseManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.Case
{
    public interface ICaseService
    {
        Task<IActionResult>  PostCaseMinorAudit(APICredentials credentials, CaseForm param);
    }
    public class CaseService : Core.Shared.Utilities, ICaseService
    {
        private readonly EMS.Workflow.Data.Case.ICaseDBAccess _dbAccess;

        public CaseService(WorkflowContext dbContext, IConfiguration iconfiguration, 
            EMS.Workflow.Data.Case.ICaseDBAccess dBAccess) : base (dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
        }

        public async Task<IActionResult> PostCaseMinorAudit(APICredentials credentials, CaseForm param)
        {
            if(ErrorMessages.Count == 0)
            {
                await _dbAccess.PostCaseMinorAudit(new Data.Case.Case
                {
                    EmployeeID = param.EmployeeID,
                    Status = param.Status,
                   
                    isActive = true,
                    CreatedBy = credentials.UserID

                });


            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        private async Task<List<string>> SaveAttachments(List<IFormFile> files)
        {
            List<string> filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Save the file to the server and get its path
                    // Example: var filePath = Path.Combine(uploadsFolder, file.FileName);
                    // Save the filePath to the database or store it in a list
                 
                }
            }

            return filePaths;
        }

    }

}
