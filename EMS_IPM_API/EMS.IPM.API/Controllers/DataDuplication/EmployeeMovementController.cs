using EMS.IPM.Core.DataDuplication;
using EMS.IPM.Data.DataDuplication.EmployeeMovement;
using EMS.IPM.Transfer.DataDuplication.EmployeeMovement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.API.Controllers.DataDuplication
{
    /// <summary>
    /// EmployeeMovement
    /// </summary>
    [Route("IPM/data-duplication/[controller]")]
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

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync([FromBody] List<EmployeeMovement> param)
        {
            return await _service.Sync(param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }

    }
}