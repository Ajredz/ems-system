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
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        ///// <summary>
        ///// Adding of new records
        ///// </summary>
        ///// <param name="credentials">API Credentials</param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("add")]
        //public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        //{
        //    return await _service.Post(credentials, param).ConfigureAwait(true);
        //}

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

        /// <summary>
        /// Deleting of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromQuery] APICredentials credentials, [FromBody] BulkDeleteForm param)
        {
            return await _service.BulkDelete(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("bulk-void")]
        public async Task<IActionResult> BulkVoid([FromQuery] APICredentials credentials, [FromBody] BulkVoidForm param)
        {
            return await _service.BulkVoid(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("bulk-approved")]
        public async Task<IActionResult> BulkApproved([FromQuery] APICredentials credentials, [FromBody] BulkApprovedForm param)
        {
            return await _service.BulkApproved(credentials, param).ConfigureAwait(true);
        }
        //[HttpPost]
        //[Route("add-bulkvoid-employee-score-status-history")]
        //public async Task<IActionResult> AddBulkVoidEmployeeScoreStatusHistory([FromQuery] APICredentials credentials, [FromBody] Form param)
        //{
        //    return await _service.AddEmployeeScoreStatusHistory(credentials, param).ConfigureAwait(true);
        //}

        //[HttpPost]
        //[Route("run-scores")]
        //public async Task<IActionResult> RunScores([FromQuery] APICredentials credentials, [FromBody] SaveRunScoreForm param)
        //{
        //    return await _service.RunScores(credentials, param).ConfigureAwait(true);
        //}

        [HttpGet]
        [Route("get-scores")]
        public async Task<IActionResult> GetScores([FromQuery] APICredentials credentials, [FromQuery] RunScoreForm param)
        {
            return await _service.GetScores(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetByID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("run-trans-scores-process-by-batch")]
        public async Task<IActionResult> RunTransScores([FromQuery] APICredentials credentials, [FromBody] RunScoreForm param)
        {
            return await _service.RunTransScores(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-trans-progress")]
        public async Task<IActionResult> GetTransProgress([FromQuery] APICredentials credentials)
        {
            return await _service.GetTransProgress(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-trans-summary")]
        public async Task<IActionResult> GetTransEmployeeScoreSummary([FromQuery] APICredentials credentials, [FromQuery] int TransSummaryID)
        {
            return await _service.GetTransEmployeeScoreSummary(credentials, TransSummaryID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-summary-autocomplete")]
        public async Task<IActionResult> GetSummaryAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetSummaryAutoCompleteInput param)
        {
            return await _service.GetSummaryAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-run-description")]
        public async Task<IActionResult> UpdateRunDescription([FromQuery] APICredentials credentials, [FromBody] UpdateRunDescriptionInput param)
        {
            return await _service.UpdateRunDescription(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("employee-kpi-score-get-list")]
        public async Task<IActionResult> EmployeeKPIScoreGetList([FromQuery] APICredentials credentials, [FromQuery] EmployeeKPIScoreGetListInput param)
        {
            return await _service.EmployeeKPIScoreGetList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("rerun-trans-scores")]
        public async Task<IActionResult> RerunTransScores([FromQuery] APICredentials credentials, [FromBody] RerunForm param)
        {
            return await _service.RerunTransScores(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("run-trans-scores-initialize")]
        public async Task<IActionResult> RunTransScoreInitialize([FromQuery] APICredentials credentials, [FromBody] RunScoreForm param)
        {
            return await _service.RunTransScoreInitialize(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("create-trans-scores-summary")]
        public async Task<IActionResult> CreateTransScoreSummary([FromQuery] APICredentials credentials, [FromBody] RunScoreForm param)
        {
            return await _service.CreateTransScoreSummary(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("run-trans-scores-finalize")]
        public async Task<IActionResult> RunTransScoreFinalize([FromQuery] APICredentials credentials, [FromBody] RunScoreForm param)
        {
            return await _service.RunTransScoreFinalize(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("batch-update-employee-score")]
        public async Task<IActionResult> BatchUpdateEmployeesScore([FromQuery] APICredentials credentials, [FromBody] BatchEmployeesScoreForm param)
        {
            return await _service.BatchUpdateEmployeesScore(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-ipm-rater-by-trans-id")]
        public async Task<IActionResult> GetIPMRaterByTransID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetIPMRaterByTransID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-ipm-final-score")]
        public async Task<IActionResult> GetIPMFinalScore([FromQuery] APICredentials credentials, [FromBody] int RunID)
        {
            return await _service.GetIPMFinalScore(credentials, RunID).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-final-score-list")]
        public async Task<IActionResult> GetFinalScoreList([FromQuery] APICredentials credentials, [FromQuery] GetFinalScoreListInput param)
        {
            return await _service.GetFinalScoreList(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-run-id-dropdown")]
        public async Task<IActionResult> GetRunIDDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetRunIDDropDown(credentials).ConfigureAwait(true);
        }
    }
}