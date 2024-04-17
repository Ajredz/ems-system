using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.EmployeeScore;
using EMS.Workflow.Transfer.EmployeeScore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.EmployeeScore
{
    public interface IEmployeeScoreService
    {

        Task<IActionResult> AddEmployeeScoreStatusHistory(APICredentials credentials, Form param);
        
        Task<IActionResult> AddByBatch(APICredentials credentials, List<Form> param);

        Task<IActionResult> GetEmployeeScoreStatusHistory(APICredentials credentials, int TID);

        Task<IActionResult> BatchUpdateEmployeeScoreStatusHistory(APICredentials credentials, BatchEmployeeScoreForm param);
    }

    public class EmployeeScoreService : Core.Shared.Utilities, IEmployeeScoreService
    {
        private readonly EMS.Workflow.Data.EmployeeScore.IEmployeeScoreDBAccess _dbAccess;
        private readonly EMS.Workflow.Data.Reference.IReferenceDBAccess _dbReferenceService;

        public EmployeeScoreService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.EmployeeScore.IEmployeeScoreDBAccess dBAccess, Data.Reference.IReferenceDBAccess dbReferenceService) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
            _dbReferenceService = dbReferenceService;
        }

        public async Task<IActionResult> AddEmployeeScoreStatusHistory(APICredentials credentials, Form param)
        {
            DateTime currentDatetime = DateTime.Now;

            await _dbAccess.AddEmployeeScoreStatusHistory( 
                new EmployeeScoreApprovalHistory
                {
                    TID = param.TID,
                    Status = param.Status,
                    ApproverID = credentials.UserID,
                    Remarks = param.Remarks,
                    Timestamp = currentDatetime
                }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> AddByBatch(APICredentials credentials, List<Form> param)
        {
            DateTime currentDatetime = DateTime.Now;

            await _dbAccess.AddByBatch(
                param.Select(x => new EmployeeScoreApprovalHistory { 
                TID = x.TID,
                Status = x.Status,
                Remarks = x.Remarks,
                Timestamp = DateTime.Now,
                ApproverID = credentials.UserID
                }).ToList()
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> GetEmployeeScoreStatusHistory(APICredentials credentials, int TID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeScoreStatusHistory(TID))
                .Select(x => new GetEmployeeScoreStatusHistoryOutput
                {
                    Status = x.Status,
                    Timestamp = x.Timestamp,
                    UserID = x.UserID,
                    Remarks = x.Remarks ?? "",
                    User = ""

                }).ToList()
            );
        }

        public async Task<IActionResult> BatchUpdateEmployeeScoreStatusHistory(APICredentials credentials, BatchEmployeeScoreForm param)
        {
            DateTime currentDatetime = DateTime.Now;

            foreach (var obj in param.IDs)
            {
                await _dbAccess.AddEmployeeScoreStatusHistory(
                    new EmployeeScoreApprovalHistory
                    {
                        TID = obj,
                        Status = param.Status,
                        ApproverID = credentials.UserID,
                        Remarks = param.Remarks,
                        Timestamp = currentDatetime
                    }
                );
            }
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }
    }
}