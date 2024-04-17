using EMS.Manpower.Core.MRF;
using EMS.Manpower.Transfer.MRFApproval;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class MRFApprovalController : ControllerBase
    {
        private readonly IMRFService _service;

        public MRFApprovalController(IMRFService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery]APICredentials credentials, [FromQuery] GetApprovalListInput param)
        {
            return await _service.GetApprovalList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery]APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByIDApproval(credentials, ID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] ApproverResponse param)
        {
            return await _service.AddMRFApprovalHistory(credentials, param).ConfigureAwait(true);
        }

    }
}