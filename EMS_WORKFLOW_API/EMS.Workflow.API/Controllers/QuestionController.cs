using EMS.Workflow.Core.Question;
using EMS.Workflow.Transfer.Question;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.API.Controllers
{
    [Route("workflow/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetQuestionList([FromQuery] APICredentials credentials)
        {
            return await _service.GetQuestionList(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-question-by-category")]
        public async Task<IActionResult> GetQuestionByCategory([FromQuery] APICredentials credentials, [FromQuery] string Category)
        {
            return await _service.GetQuestionByCategory(credentials, Category).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-question-employee-answer")]
        public async Task<IActionResult> GetQuestionEmployeeAnswer([FromQuery] APICredentials credentials, [FromQuery] string Category, [FromQuery] long EmployeeID)
        {
            return await _service.GetQuestionEmployeeAnswer(credentials, Category, EmployeeID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-question-employee-answer")]
        public async Task<IActionResult> AddQuestionEmployeeAnswers([FromQuery] APICredentials credentials, [FromBody] List<QuestionEmployeeAnswerInput> param)
        {
            return await _service.AddQuestionEmployeeAnswers(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-question-employee-answer-export")]
        public async Task<IActionResult> GetQuestionEmployeeAnswersExport([FromQuery] APICredentials credentials, [FromBody] List<QuestionEmployeeAnswerInput> param)
        {
            return await _service.GetQuestionEmployeeAnswersExport(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-question")]
        public async Task<IActionResult> GetQuestion([FromQuery] APICredentials credentials)
        {
            return await _service.GetQuestion(credentials).ConfigureAwait(true);
        }
    }
}
