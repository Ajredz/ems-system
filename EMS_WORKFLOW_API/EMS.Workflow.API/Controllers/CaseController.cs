using EMS.Workflow.Core.Case;
using EMS.Workflow.Core.Workflow;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.CaseManagement;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities.Encoders;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    [Route("workflow/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public CaseController(ICaseService service)
        {
            _service = service;
        }
        #region CREATE/ADD
        #region Case Minor Audit
        
        [HttpPost]
        [Route("add-case-minor-audit")]
        public async Task<IActionResult> PostCaseMinorAudit([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-case-minor-audit-attachment")]
        public async Task<IActionResult> PostCaseMinorAuditAttachment([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-case-minor-audit-comments")]
        public async Task<IActionResult> PostCaseMinorAuditComments([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-case-minor-audit-noa")]
        public async Task<IActionResult> PostCaseMinorAuditNoa([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-case-minor-audit-nte")]
        public async Task<IActionResult> PostCaseMinorAuditNte([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-case-minor-audit-status-history")]
        public async Task<IActionResult> PostCaseMinorAuditStatusHistory([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        #endregion
        #endregion

        #region Read/GET
        #region Case Minor Audit
        [HttpGet]
        [Route("get-case-minor-audit")]
        public async Task<IActionResult> GetCaseMinorAudit([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-by-id")]
        public async Task<IActionResult> GetCaseMinorAuditById([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-attachment")]
        public async Task<IActionResult> GetCaseMinorAuditAttachment([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-comments")]
        public async Task<IActionResult> GetCaseMinorAuditComments([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-noa")]
        public async Task<IActionResult> GetCaseMinorAuditNoa([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-nte")]
        public async Task<IActionResult> GetCaseMinorAuditNte([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-case-minor-audit-status-history")]
        public async Task<IActionResult> GetCaseMinorAuditStatusHistory([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        #endregion
        #endregion

        #region Update/PUT
        #region Case Minor Audit
        [HttpPut]
        [Route("update-case-minor-audit")]
        public async Task<IActionResult> UpdateCaseMinorAudit([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("update-case-minor-audit-attachment")]
        public async Task<IActionResult> UpdateCaseMinorAuditAttachment([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("update-case-minor-audit-noa")]
        public async Task<IActionResult> UpdateCaseMinorAuditNoa([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("update-case-minor-audit-nte")]
        public async Task<IActionResult> UpdateCaseMinorAuditNte([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("update-case-minor-audit-status--history")]
        public async Task<IActionResult> UpdateCaseMinorAuditStatusHistory([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        #endregion
        #endregion

        #region Delete
        #region Case Minor Audit
        [HttpDelete]
        [Route("delete-case-minor-audit")]
        public async Task<IActionResult> DeleteCaseMinorAudit([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        [HttpDelete]
        [Route("delete-case-minor-audit-attachment")]
        public async Task<IActionResult> DeleteCaseMinorAuditAttachment([FromQuery] APICredentials credentials, [FromBody] CaseForm param)
        {
            return await _service.PostCaseMinorAudit(credentials, param).ConfigureAwait(true);
        }
        
        #endregion
        #endregion




    }
}
