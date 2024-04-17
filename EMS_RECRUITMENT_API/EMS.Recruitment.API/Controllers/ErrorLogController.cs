﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.API;
using EMS.Recruitment.Core.SystemErrorLog;
using EMS.Recruitment.Transfer.SystemErrorLog;

namespace EMS.Recruitment.Controllers
{
    [Route("recruitment/[controller]")]
    [ApiController]
    public class ErrorLogController : ControllerBase
    {
        private readonly IErrorLogService _service;

        public ErrorLogController(IErrorLogService service)
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
