using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.EmployeeScoreDashboard;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer;
using EMS.IPM.Transfer.EmployeeScoreDashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.EmployeeScoreDashboard
{
    public interface IEmployeeScoreDashboardService
    {
        Task<IActionResult> GetListSummaryForEvaluation(APICredentials credentials, GetListSummaryForEvaluationInput input);

        Task<IActionResult> GetListSummaryForApproval(APICredentials credentials, GetListSummaryForApprovalInput input);

        Task<IActionResult> GetListRegionalWithPosition(APICredentials credentials, GetListRegionalWithPositionInput input);

        Task<IActionResult> GetListBranchesWithPosition(APICredentials credentials, GetListBranchesWithPositionInput input);

        Task<IActionResult> GetListPositionOnly(APICredentials credentials, GetListPositionOnlyInput input);

        Task<IActionResult> GetListSummaryForApprovalBRN(APICredentials credentials, GetListSummaryForApprovalBRNInput input);

        Task<IActionResult> GetListSummaryForApprovalCLU(APICredentials credentials, GetListSummaryForApprovalCLUInput input);
    }

    public class EmployeeScoreDashboardService : Core.Shared.Utilities, IEmployeeScoreDashboardService
    {
        private readonly IEmployeeScoreDashboardDBAccess _dbAccess;

        public EmployeeScoreDashboardService(IPMContext dbContext, IConfiguration iconfiguration,
            IEmployeeScoreDashboardDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetListSummaryForEvaluation(APICredentials credentials, GetListSummaryForEvaluationInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardSummaryForEvaluation> result = await _dbAccess.GetListSummaryForEvaluation(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListSummaryForEvaluationOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Region = x.Region,
                WithCompleteScore = x.WithCompleteScore,
                WithMissingScore = x.WithMissingScore,
                NoScore = x.NoScore,
                OnGoingEvaluation= x.OnGoingEvaluation
            }).ToList());
        }

        public async Task<IActionResult> GetListSummaryForApproval(APICredentials credentials, GetListSummaryForApprovalInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardSummaryForApproval> result = await _dbAccess.GetListSummaryForApproval(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListSummaryForApprovalOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Region = x.Region,
                NoKeyIn = x.NoKeyIn,
                ForApproval = x.ForApproval,
                Finalized = x.Finalized
            }).ToList());
        }

        public async Task<IActionResult> GetListRegionalWithPosition(APICredentials credentials, GetListRegionalWithPositionInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardRegionalWithPosition> result = await _dbAccess.GetListRegionalWithPosition(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListRegionalWithPositionOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Region = x.Region,
                Position = x.Position,
                KRAGroup = x.KRAGroup,
                KPI = x.KPI,
                EE = x.EE,
                ME = x.ME,
                SBE = x.SBE,
                BE = x.BE,
            }).ToList());
        }

        public async Task<IActionResult> GetListBranchesWithPosition(APICredentials credentials, GetListBranchesWithPositionInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardBranchesWithPosition> result = await _dbAccess.GetListBranchesWithPosition(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListBranchesWithPositionOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Branch = x.Branch,
                Position = x.Position,
                KRAGroup = x.KRAGroup,
                KPI = x.KPI,
                EE = x.EE,
                ME = x.ME,
                SBE = x.SBE,
                BE = x.BE,
            }).ToList());
        }
        
        public async Task<IActionResult> GetListPositionOnly(APICredentials credentials, GetListPositionOnlyInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardPositionOnly> result = await _dbAccess.GetListPositionOnly(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListPositionOnlyOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Position = x.Position,
                KRAGroup = x.KRAGroup,
                KPI = x.KPI,
                EE = x.EE,
                ME = x.ME,
                SBE = x.SBE,
                BE = x.BE,
            }).ToList());
        }

        public async Task<IActionResult> GetListSummaryForApprovalBRN(APICredentials credentials, GetListSummaryForApprovalBRNInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardSummaryForApprovalBRN> result = await _dbAccess.GetListSummaryForApprovalBRN(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListSummaryForApprovalBRNOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Region = x.Region,
                Branch = x.Branch,
                NoKeyIn = x.NoKeyIn,
                ForApproval = x.ForApproval,
                Finalized = x.Finalized
            }).ToList());
        } 
        
        public async Task<IActionResult> GetListSummaryForApprovalCLU(APICredentials credentials, GetListSummaryForApprovalCLUInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarDashboardSummaryForApprovalCLU> result = await _dbAccess.GetListSummaryForApprovalCLU(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListSummaryForApprovalCLUOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Region = x.Region,
                Cluster = x.Cluster,
                NoKeyIn = x.NoKeyIn,
                ForApproval = x.ForApproval,
                Finalized = x.Finalized
            }).ToList());
        }


    }
}