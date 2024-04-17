using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS_SecurityService.DBContexts;
using EMS_SecurityService.SharedClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.Security.Core.SystemErrorLog;
using EMS.Security.Transfer.SystemErrorLog;

namespace EMS_SecurityService.Controllers
{
    [Route("security/[controller]")]
    [ApiController]
    public class ErrorLogController : SharedClasses.Utilities
    {
        private readonly IErrorLogService _service;

        public ErrorLogController(SystemAccessContext dbContext, IConfiguration iconfiguration,
            IErrorLogService service) : base(dbContext, iconfiguration)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery]APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

    }
}
