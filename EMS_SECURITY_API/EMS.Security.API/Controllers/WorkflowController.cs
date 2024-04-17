using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Security.API.Controllers
{
    /// <summary>
    /// WorkFlow to be assigned on Organizational Groups
    /// </summary>
    [Route("security/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        //private readonly IWorkflowService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        //public WorkFlowController(IWorkflowService service)
        //{
        //    _service = service;
        //}

        ///// <summary>
        ///// Get records by ID
        ///// </summary>
        ///// <param name="credentials">API Credentials</param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("get-by-id")]
        //public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        //{
        //    return await _service.GetByID(credentials, param).ConfigureAwait(true);
        //}
    }
}