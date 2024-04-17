using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Transfer.MRF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class SyncronizeController : ControllerBase
    {
        private readonly ISynchronizeService _service;

        public SyncronizeController(ISynchronizeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("position")]
        public async Task<IActionResult> SyncPosition([FromQuery]APICredentials credentials)
        {
            return await _service.SyncPosition(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("position-level")]
        public async Task<IActionResult> SyncPositionLevel([FromQuery]APICredentials credentials)
        {
            return await _service.SyncPositionLevel(credentials).ConfigureAwait(true);
        }

    }
}