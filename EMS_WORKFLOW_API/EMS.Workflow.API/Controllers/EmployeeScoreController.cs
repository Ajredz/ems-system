using EMS.Workflow.Core.EmployeeScore;
using EMS.Workflow.Transfer.EmployeeScore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    /// <summary>
    /// Workflow to be assigned
    /// </summary>
    [Route("workflow/[controller]")]
    [ApiController]
    public class EmployeeScoreController : ControllerBase
    {
        private readonly IEmployeeScoreService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmployeeScoreController(IEmployeeScoreService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("add-employee-score-status-history")]
        public async Task<IActionResult> AddEmployeeScoreStatusHistory([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.AddEmployeeScoreStatusHistory(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-score-status-history")]
        public async Task<IActionResult> GetEmployeeScoreStatusHistory([FromQuery] APICredentials credentials, [FromQuery] int TID)
        {
            return await _service.GetEmployeeScoreStatusHistory(credentials, TID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("batch-update-employee-score-status-history")]
        public async Task<IActionResult> BatchUpdateEmployeeScoreStatusHistory([FromQuery] APICredentials credentials, [FromBody] BatchEmployeeScoreForm param)
        {
            return await _service.BatchUpdateEmployeeScoreStatusHistory(credentials, param).ConfigureAwait(true);
        }
        

        [HttpPost]
        [Route("add-by-batch")]
        public async Task<IActionResult> AddByBatch([FromQuery] APICredentials credentials, [FromBody] List<Form> param)
        {
            return await _service.AddByBatch(credentials, param).ConfigureAwait(true);
        }
    }
}