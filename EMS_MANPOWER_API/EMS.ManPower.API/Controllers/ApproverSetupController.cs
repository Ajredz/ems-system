using EMS.Manpower.Core.ApproverSetup;
using EMS.Manpower.Transfer.ApproverSetup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class ApproverSetupController : ControllerBase
    {
        private readonly IApproverSetupService _service;

        public ApproverSetupController(IApproverSetupService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery]APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery]APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-setup-mrf-approver-insert")]
        public async Task<IActionResult> GetSetupMRFApproverInsert([FromQuery] APICredentials credentials)
        {
            return await _service.GetSetupMRFApproverInsert(credentials).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-setup-mrf-approver-update")]
        public async Task<IActionResult> GetSetupMRFApproverUpdate([FromQuery] APICredentials credentials)
        {
            return await _service.GetSetupMRFApproverUpdate(credentials).ConfigureAwait(true);
        }

    }
}