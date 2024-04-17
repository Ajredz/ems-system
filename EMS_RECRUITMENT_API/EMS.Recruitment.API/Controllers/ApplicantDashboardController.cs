using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Recruitment.Core.ApplicantDashboard;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Recruitment.API.Controllers
{
    [Route("recruitment/[controller]")]
    [ApiController]
    public class ApplicantDashboardController : ControllerBase
    {
        private readonly IApplicantDashboardService _service;

        public ApplicantDashboardController(IApplicantDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery]APICredentials credentials)
        {
            return await _service.GetList(credentials).ConfigureAwait(true);
        }
    }
}