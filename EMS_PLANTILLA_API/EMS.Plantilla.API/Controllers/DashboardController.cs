using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Core.Dashboard;
using EMS.Plantilla.Transfer.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    [Route("plantilla/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-employee-dashboard-by-report-type")]
        public async Task<IActionResult> GetEmployeeDashboardByReportType([FromQuery] APICredentials credentials, [FromQuery] GetEmployeeDashboardInput param)
        {
            return await _service.GetEmployeeDashboardByReportType(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-plantilla-dashboard")]
        public async Task<IActionResult> GetPlantillaDashboard([FromQuery] APICredentials credentials, [FromBody] PlantillaDashboardInput param)
        {
            return await _service.GetPlantillaDashboard(credentials, param).ConfigureAwait(true);
        }
    }
}
