using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Manpower.Core.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Manpower.API.Controllers
{
    [Route("manpower/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery]APICredentials credentials, [FromQuery] bool isAdmin)
        {
            return await _service.GetList(credentials, isAdmin).ConfigureAwait(true);
        }
    }
}