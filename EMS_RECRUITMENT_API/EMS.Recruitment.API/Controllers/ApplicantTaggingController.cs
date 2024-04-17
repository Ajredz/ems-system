using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Recruitment.Core.ApplicantTagging;
using EMS.Recruitment.Transfer.ApplicantTagging;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    /// <summary>
    /// Reference Maintenance
    /// </summary>
    [Route("Recruitment/[controller]")]
    [ApiController]
    public class ApplicantTaggingController : ControllerBase
    {
        private readonly IApplicantTaggingService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public ApplicantTaggingController(IApplicantTaggingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }
    }
}