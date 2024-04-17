using EMS.Plantilla.Core.Reference;
using EMS.Plantilla.Transfer.Reference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using Utilities.API.ReferenceMaintenance;

namespace EMS.Plantilla.API.Controllers
{
    /// <summary>
    /// Region to be assigned on Organizational Groups
    /// </summary>
    [Route("plantilla/[controller]")]
    [ApiController]
    public class ReferenceController : ControllerBase
    {
        private readonly IReferenceService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public ReferenceController(IReferenceService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetDropDown(credentials).ConfigureAwait(true);
        }


        [HttpGet]
        [Route("get-by-ref-codes")]
        public async Task<IActionResult> GetByRefCodes([FromQuery] APICredentials credentials, [FromQuery] List<string> RefCodes)
        {
            return await _service.GetByRefCodes(credentials, RefCodes).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-codes")]
        public async Task<IActionResult> GetByCodes([FromQuery] APICredentials credentials, [FromQuery] List<string> Codes)
        {
            return await _service.GetByCodes(credentials, Codes).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
        }
        
        [HttpGet]
        [Route("get-maintainable")]
        public async Task<IActionResult> GetMaintainable([FromQuery] APICredentials credentials)
        {
            return await _service.GetMaintainable(credentials).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] List<ReferenceValue> param)
        {
            return await _service.UpdateSet(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-reference-value-list")]
        public async Task<IActionResult> GetReferenceValueList([FromQuery] APICredentials credentials, [FromQuery] string RefCode)
        {
            return await _service.GetReferenceValueList(credentials, RefCode).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-reference-value")]
        public async Task<IActionResult> AddReferenceValue([FromQuery] APICredentials credentials, [FromBody] ReferenceValue param)
        {
            return await _service.AddReferenceValue(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-reference-value")]
        public async Task<IActionResult> UpdateReferenceValue([FromQuery] APICredentials credentials, [FromBody] ReferenceValue param)
        {
            return await _service.UpdateReferenceValue(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-reference-value-by-id")]
        public async Task<IActionResult> GetReferenceValueByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetReferenceValueByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-ref-code-value")]
        public async Task<IActionResult> GetByRefCodeValue([FromQuery] APICredentials credentials, [FromQuery] GetByRefCodeValueInput param)
        {
            return await _service.GetByRefCodeValue(credentials, param).ConfigureAwait(true);
        }
    }
}