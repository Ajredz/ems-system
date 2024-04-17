using EMS.IPM.Core.EmployeeScoreDashboard;
using EMS.IPM.Transfer.EmployeeScoreDashboard;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// EmployeeScore to be assigned on Organizational Groups
    /// </summary>
    [Route("IPM/[controller]")]
    [ApiController]
    public class EmployeeScoreDashboardController : ControllerBase
    {
        private readonly IEmployeeScoreDashboardService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmployeeScoreDashboardController(IEmployeeScoreDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list-summary-for-evaluation")]
        public async Task<IActionResult> GetListSummaryForEvaluation([FromQuery] APICredentials credentials, [FromQuery] GetListSummaryForEvaluationInput param)
        {
            return await _service.GetListSummaryForEvaluation(credentials, param).ConfigureAwait(true);
        } 
        
        [HttpGet]
        [Route("get-list-summary-for-approval")]
        public async Task<IActionResult> GetListSummaryForApproval([FromQuery] APICredentials credentials, [FromQuery] GetListSummaryForApprovalInput param)
        {
            return await _service.GetListSummaryForApproval(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-list-regional-with-position")]
        public async Task<IActionResult> GetListRegionalWithPosition([FromQuery] APICredentials credentials, [FromQuery] GetListRegionalWithPositionInput param)
        {
            return await _service.GetListRegionalWithPosition(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-list-branches-with-position")]
        public async Task<IActionResult> GetListBranchesWithPosition([FromQuery] APICredentials credentials, [FromQuery] GetListBranchesWithPositionInput param)
        {
            return await _service.GetListBranchesWithPosition(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-list-position-only")]
        public async Task<IActionResult> GetListPositionOnly([FromQuery] APICredentials credentials, [FromQuery] GetListPositionOnlyInput param)
        {
            return await _service.GetListPositionOnly(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-list-summary-for-approval-brn")]
        public async Task<IActionResult> GetListSummaryForApprovalBRN([FromQuery] APICredentials credentials, [FromQuery] GetListSummaryForApprovalBRNInput param)
        {
            return await _service.GetListSummaryForApprovalBRN(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-list-summary-for-approval-clu")]
        public async Task<IActionResult> GetListSummaryForApprovalCLU([FromQuery] APICredentials credentials, [FromQuery] GetListSummaryForApprovalCLUInput param)
        {
            return await _service.GetListSummaryForApprovalCLU(credentials, param).ConfigureAwait(true);
        }
    }
}