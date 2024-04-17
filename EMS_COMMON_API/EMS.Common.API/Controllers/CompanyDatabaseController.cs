using EMS.Common.Core.CompanyDatabase;
using EMS.Common.Transfer.CompanyDatabase;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Common.API.Controllers
{
    /// <summary>
    /// Employee to be assigned on Organizational Groups
    /// </summary>
    [Route("common/[controller]")]
    [ApiController]
    public class CompanyDatabaseController : ControllerBase
    {
        private readonly ICompanyDatabaseService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public CompanyDatabaseController(ICompanyDatabaseService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-by-company-id")]
        public async Task<IActionResult> GetByCompanyID([FromQuery] APICredentials credentials, [FromQuery] string CompanyID)
        {
            return await _service.GetByCompanyID(credentials, CompanyID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-company-code")]
        public async Task<IActionResult> GetByCompanyCode([FromQuery] APICredentials credentials, [FromQuery] string CompanyCode)
        {
            return await _service.GetByCompanyCode(credentials, CompanyCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-module-code")]
        public async Task<IActionResult> GetByModuleCode([FromQuery] APICredentials credentials, [FromQuery] string ModuleCode)
        {
            return await _service.GetByModuleCode(credentials, ModuleCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] APICredentials credentials)
        {
            return await _service.GetAll(credentials).ConfigureAwait(true);
        }
    }
}