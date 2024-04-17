using EMS.IPM.Core.KPIScore;
using EMS.IPM.Transfer.KPIScore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// KPIPosition to be assigned on Organizational Groups
    /// </summary>
    [Route("IPM/[controller]")]
    [ApiController]
    public class KPIScoreController : ControllerBase
    {
        private readonly IKPIScoreService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public KPIScoreController(IKPIScoreService service)
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

        [HttpPost]
        [Route("validate-upload-scores")]
        public async Task<IActionResult> ValidateUploadScores([FromQuery] APICredentials credentials, [FromBody] List<UploadScoresFile> param)
        {
            return await _service.ValidateUploadScores(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-scores")]
        public async Task<IActionResult> UploadScores([FromQuery] APICredentials credentials, [FromBody] List<UploadScoresFile> param)
        {
            return await _service.UploadScores(credentials, param).ConfigureAwait(true);
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

        ///// <summary>
        ///// Updating of records
        ///// </summary>
        ///// <param name="credentials">API Credentials</param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("edit")]
        //public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        //{
        //    return await _service.Put(credentials, param).ConfigureAwait(true);
        //}

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
    }
}