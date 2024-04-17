using EMS.IPM.Core.KPIPosition;
using EMS.IPM.Transfer.KPIPosition;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.IPM.API.Controllers
{
    /// <summary>
    /// KPIPosition to be assigned on Organizational Groups
    /// </summary>
    [Route("IPM/[controller]")]
    [ApiController]
    public class KPIPositionController : ControllerBase
    {
        private readonly IKPIPositionService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public KPIPositionController(IKPIPositionService service)
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

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
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

        /// <summary>
        /// Deleting of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <param name="EffectiveDate"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.Delete(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByPositionID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetByPositionID(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-details")]
        public async Task<IActionResult> GetAllDetails([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllDetails(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("validate-upload-insert")]
        public async Task<bool> ValidateUploadInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadFileEntity> param)
        {
            return await _service.ValidateUploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadFileEntity> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-export-list")]
        public async Task<IActionResult> GetExportList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetExportList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("copy-kpi-position")]
        public async Task<IActionResult> CopyKpiPosition([FromQuery] APICredentials credentials, [FromBody] CopyKpiPositionInput param)
        {
            return await _service.CopyKpiPosition(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-copy-position")]
        public async Task<IActionResult> GetCopyPosition([FromQuery] APICredentials credentials,[FromQuery] EMS.IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return await _service.GetCopyPosition(credentials, param).ConfigureAwait(true);
        }
    }
}