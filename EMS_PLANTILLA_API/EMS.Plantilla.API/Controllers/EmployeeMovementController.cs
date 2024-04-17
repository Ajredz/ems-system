 using EMS.Plantilla.Core.EmployeeMovement;
using EMS.Plantilla.Transfer.EmployeeMovement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// Employee Movement history of Employee fields
    /// </summary>
    [Route("plantilla/[controller]")]
    [ApiController]
    public class EmployeeMovementController : ControllerBase
    {
        private readonly IEmployeeMovementService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmployeeMovementController(IEmployeeMovementService service)
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

        [HttpGet]
        [Route("get-autocomplete")]
        public async Task<IActionResult> GetAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteByMovementTypeInput param)
        {
            return await _service.GetAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] UploadFile param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-print")]
        public async Task<IActionResult> GetPrint([FromQuery] APICredentials credentials, [FromQuery] GetPrintInput param)
        {
            return await _service.GetPrint(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-autopopulate")]
        public async Task<IActionResult> GetAutoPopulate([FromQuery] APICredentials credentials, [FromQuery] GetAutoPopulateByMovementTypeInput param)
        {
            return await _service.GetAutoPopulate(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] long ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }
        // For movement checker 83-89
        [HttpPost]
        [Route("get-by-ids")]
        public async Task<IActionResult> GetByIDs([FromQuery] APICredentials credentials, [FromBody] List<long> ID)
        {
            return await _service.GetByIDs(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("sync-movement")]
        public async Task<IActionResult> SyncMovement([FromQuery] APICredentials credentials)
        {
            return await _service.SyncMovement(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-field-list")]
        public async Task<IActionResult> GetEmployeeFieldList([FromQuery] APICredentials credentials, string type)
        {
            return await _service.GetEmployeeFieldList(credentials, type).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-movement-type")]
        public async Task<IActionResult> GetMovementType([FromQuery] APICredentials credentials, string MovementType)
        {
            return await _service.GetMovementType(credentials, MovementType).ConfigureAwait(true);
        }

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-movement-employee-field")]
        public async Task<IActionResult> AddMovementEmployeeField([FromQuery] APICredentials credentials, [FromBody] EmployeeFieldForm param)
        {
            return await _service.AddMovementEmployeeField(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("bulk-remove-employee-field")]
        public async Task<IActionResult> BulkRemove([FromQuery] APICredentials credentials, [FromBody] BulkRemoveForm param)
        {
            return await _service.BulkRemove(credentials, param).ConfigureAwait(true); ;
        }

        [HttpPut]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromBody] BulkRemoveForm ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Update(credentials, param).ConfigureAwait(true);
        }
        //For movement checker 151-165
        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> ChangeStatus([FromQuery] APICredentials credentials, [FromBody] ChangeStatus param)
        {
            return await _service.ChangeStatus(credentials, param).ConfigureAwait(true);
        }


        [HttpPost]
        [Route("update-date-to")]
        public async Task<IActionResult> UpdateDateTo([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.UpdateDateTo(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-employee-movement-by-employee-ids")]
        public async Task<IActionResult> GetEmployeeMovementByEmployeeIDs([FromQuery] APICredentials credentials, [FromBody] List<int> EmployeeIDs)
        {
            return await _service.GetEmployeeMovementByEmployeeIDs(credentials, EmployeeIDs).ConfigureAwait(true);
        }
    }
}