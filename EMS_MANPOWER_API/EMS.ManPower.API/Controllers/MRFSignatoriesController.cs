using EMS.Manpower.Transfer.MRFSignatories;
using EMS.Manpower.Core.MRFSignatories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class MRFSignatoriesController : ControllerBase
    {
        private readonly IMRFSignatoriesService _service;

        public MRFSignatoriesController(IMRFSignatoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-by-role-position")]
        public async Task<IActionResult> GetByRolePosition([FromQuery] APICredentials credentials, [FromQuery] GetByUserPositionInput param)
        {
            return await _service.GetByRolePosition(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] List<Form> param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-signatories-add")]
        public async Task<IActionResult> GetMRFSignatoriesAdd([FromQuery] APICredentials credentials, [FromQuery] GetMRFSignatoriesAddInput param)
        {
            return await _service.GetMRFSignatoriesAdd(credentials, param).ConfigureAwait(true);
        }
    }
}