using EMS.IPM.Core.EmployeeScore;
using EMS.IPM.Transfer.EmployeeScore;
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
    public class EmployeeScoreKeyInApprovalController : ControllerBase
    {
        private readonly IEmployeeScoreService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmployeeScoreKeyInApprovalController(IEmployeeScoreService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get records to be displayed on JQGrid
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetKeyInApprovalList(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("batch-update-employee-score")]
        public async Task<IActionResult> BatchUpdateEmployeeScore([FromQuery] APICredentials credentials, [FromBody] BatchEmployeeScoreForm param)
        {
            return await _service.BatchUpdateEmployeeScore(credentials, param).ConfigureAwait(true);
        }
    }
}