using EMS.H2Pay.Data.OrgGroup;
using EMS.Plantilla.Core.OrgGroup;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers.H2Pay
{
    [Route("plantilla/h2pay/[controller]")]
    [ApiController]
    public class OrgGroupController : ControllerBase
    {
        private readonly IDBAccess _db;

        public OrgGroupController(IDBAccess db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromQuery] APICredentials credentials
            , [FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return new OkObjectResult(await _db.Sync(unit, value).ConfigureAwait(false));
        }

    }
}