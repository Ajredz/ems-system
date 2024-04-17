using EMS.Workflow.Core.EmailServerCredential;
using EMS.Workflow.Transfer.EmailServerCredential;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    /// <summary>
    /// Workflow to be assigned
    /// </summary>
    [Route("workflow/[controller]")]
    [ApiController]
    public class EmailServerCredentialController : ControllerBase
    {
        private readonly IEmailServerCredentialService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public EmailServerCredentialController(IEmailServerCredentialService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-by-template-code")]
        public async Task<IActionResult> GetByTemplateCode([FromQuery] APICredentials credentials, [FromQuery] string TemplateCode)
        {
            return await _service.GetByTemplateCode(credentials, TemplateCode).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-pending-email")]
        public async Task<IActionResult> GetPendingEmail([FromQuery] APICredentials credentials)
        {
            return await _service.GetPendingEmail(credentials).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-email-logs")]
        public async Task<IActionResult> PostEmailLogs([FromQuery] APICredentials credentials, [FromBody] EmailLogsInput param)
        {
            return await _service.PostEmailLogs(credentials, param).ConfigureAwait(true);
        }
        [DisableRequestSizeLimit]
        [HttpPost]
        [Route("add-multiple-email-logs")]
        public async Task<IActionResult> PostMultipleEmailLogs([FromQuery] APICredentials credentials, [FromBody] List<EmailLogsInput> param)
        {
            return await _service.PostMultipleEmailLogs(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit-email-logs")]
        public async Task<IActionResult> PutEmailLogs([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.PutEmailLogs(credentials, ID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-cron-logs")]
        public async Task<IActionResult> PostCronLogs([FromQuery] APICredentials credentials, [FromBody] List<CronLogsInput> param)
        {
            return await _service.PostCronLogs(credentials, param).ConfigureAwait(true);
        }
    }
}