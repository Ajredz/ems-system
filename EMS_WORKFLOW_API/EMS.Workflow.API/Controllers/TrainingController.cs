using EMS.Workflow.Core.Training;
using EMS.Workflow.Transfer.Training;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    [Route("workflow/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _service;

        public TrainingController(ITrainingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromQuery] APICredentials credentials, [FromBody] TrainingTempateInput param)
        {
            return await _service.Add(credentials, param).ConfigureAwait(true);
        }
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromQuery] APICredentials credentials, [FromBody] TrainingTempateInput param)
        {
            return await _service.Edit(credentials, param).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-training-template-dropdown")]
        public async Task<IActionResult> GetTrainingTemplateDropdown([FromQuery] APICredentials credentials)
        {
            return await _service.GetTrainingTemplateDropdown(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetByID(credentials, ID).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-details-by-training-template-id")]
        public async Task<IActionResult> GetDetailsByTrainingTemplateID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetDetailsByTrainingTemplateID(credentials, ID).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-employee-training-list")]
        public async Task<IActionResult> GetEmployeeTrainingList([FromQuery] APICredentials credentials, [FromQuery] GetEmployeeTrainingListInput param)
        {
            return await _service.GetEmployeeTrainingList(credentials, param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("add-employee-training-template")]
        public async Task<IActionResult> AddEmployeeTrainingTemplate([FromQuery] APICredentials credentials, [FromBody] AddEmployeeTrainingInput param)
        {
            return await _service.AddEmployeeTrainingTemplate(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-employee-training")]
        public async Task<IActionResult> AddEmployeeTraining([FromQuery] APICredentials credentials, [FromBody] EmployeeTrainingForm param)
        {
            return await _service.AddEmployeeTraining(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<TrainingUploadFile> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("edit-employee-training")]
        public async Task<IActionResult> EditEmployeeTraining([FromQuery] APICredentials credentials, [FromBody] EmployeeTrainingForm param)
        {
            return await _service.EditEmployeeTraining(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-training-by-id")]
        public async Task<IActionResult> GetEmployeeTrainingByID([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetEmployeeTrainingByID(credentials, ID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> ChangeStatus([FromQuery] APICredentials credentials, [FromBody] ChangeStatus param)
        {
            return await _service.ChangeStatus(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-training-status-history")]
        public async Task<IActionResult> GetEmployeeTrainingStatusHistory([FromQuery] APICredentials credentials, [FromQuery] int EmployeeTrainingID)
        {
            return await _service.GetEmployeeTrainingStatusHistory(credentials, EmployeeTrainingID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-training-score")]
        public async Task<IActionResult> GetEmployeeTrainingScore([FromQuery] APICredentials credentials, [FromQuery] int EmployeeTrainingID)
        {
            return await _service.GetEmployeeTrainingScore(credentials, EmployeeTrainingID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-employee-by-employee-training-ids")]
        public async Task<IActionResult> GetEmployeeByEmployeeTrainingIDs([FromQuery] APICredentials credentials, [FromBody] List<int> EmployeeTrainingID)
        {
            return await _service.GetEmployeeByEmployeeTrainingIDs(credentials, EmployeeTrainingID).ConfigureAwait(true);
        }
    }
}
