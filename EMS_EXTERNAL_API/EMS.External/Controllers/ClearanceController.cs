using Microsoft.AspNetCore.Mvc;
using EMS.External.API.Shared;
using EMS.External.API.Model;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using EMS.External.API.DBContext;
using Microsoft.AspNetCore.Routing;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace EMS.External.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClearanceController : Controller
    {
        public ResultView _resultView = new ResultView();
        public readonly IConfiguration _iconfiguration;
        public string SystemName = "CLEARANCE";

        public ClearanceController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }


        //[HttpPost]
        //[Route("get-employee-login")]
        //public async Task<IActionResult> EmployeeLogin([FromQuery] APICredentials credentials,[FromQuery] EmployeeLogin param)
        //{
        //    var URL =
        //      string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
        //          _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetExternalEmployeeDetails").Value, "?",
        //          "EmployeeId="+param.EmployeeId+"&"+
        //          "LastName="+param.LastName);

        //    var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new EmployeeLogin(), URL);
        //    _resultView.IsSuccess = IsSuccess;
        //    if (IsSuccess)
        //        return new OkObjectResult(Result);
        //    else
        //        throw new Exception(ErrorMessage);
        //}

        /*[HttpPost]
        [Route("employee-login-old")]*/
        /*[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> EmployeeLoginOld([FromBody] InputEmployeeLogin param)
        {
            if (string.IsNullOrEmpty(param.EmployeeId) || string.IsNullOrEmpty(param.LastName))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
            }
            else
            {
                var GetEmployeeIdURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetExternalEmployeeDetails").Value, "?",
                  "EmployeeId=" + param.EmployeeId + "&" + "LastName=" + param.LastName + "&" + "BirthDate=" + param.BirthDate);

                var (ResultEmployee, IsSuccessEmployee, ErrorMessageEmployee) = await Utilities.GetFromAPI(new OutputEmployeeLogin(), GetEmployeeIdURL);
                _resultView.IsSuccess = IsSuccessEmployee;
                if (IsSuccessEmployee)
                {
                    var GetEmployeeAccountabilityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityByEmployeeID").Value, "?",
                      "EmployeeID=" + ResultEmployee.Id);

                    var (ResultEmployeeAccountability, IsSuccessEmployeeAccountability, ErrorMessageEmployeeAccountability) = await Utilities.GetFromAPI(new List<OutputEmployeeAccountability>(), GetEmployeeAccountabilityURL);
                    _resultView.IsSuccess = IsSuccessEmployeeAccountability;
                    if (IsSuccessEmployeeAccountability)
                    {
                        var GetPositionNameURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                          _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Position").GetSection("GetByID").Value, "?",
                          "ID=", ResultEmployee.Position);

                        var (ResultPositionName, IsSuccessPositionName, ErrorMessagePositionName) = await Utilities.GetFromAPI(new OutputPositionName(), GetPositionNameURL);

                        var EmployeeAccountability = ResultEmployeeAccountability.OrderByDescending(x => x.Status).ToList();

                        List<OutputClearance> DisplayResult = new List<OutputClearance>() {
                            new OutputClearance(){
                                Profile=new OutputEmployeeLogin{
                                    Id=ResultEmployee.Id,
                                    SystemAccessId=ResultEmployee.SystemAccessId,
                                    EmployeeId=ResultEmployee.EmployeeId,
                                    Firstname=ResultEmployee.Firstname,
                                    LastName=ResultEmployee.LastName,
                                    Email=ResultEmployee.Email,
                                    Position=ResultPositionName.Title,
                                    Company=ResultEmployee.Company
                                },
                                EmployeeAccountability=EmployeeAccountability
                            }
                        };

                        return new OkObjectResult(DisplayResult);
                    }
                    else
                        return new BadRequestObjectResult(ErrorMessageEmployeeAccountability);
                }
                else
                    return new BadRequestObjectResult(ErrorMessageEmployee);
            }
        }*/

        [HttpPost]
        [Route("get-accountability-comment")]
        public async Task<IActionResult> GetAccountabilityComment([FromQuery] APICredentials param, [FromBody] EmployeeAccountabilityGetCommentsInput input)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetAccountabilityCommentURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeComments").Value, "?",
                  "ID=", input.ID);

                var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<EmployeeAccountabilityGetCommentsOutput>(), GetAccountabilityCommentURL);

                List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

                var URL = string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetByIDs").Value);

                var (ResultSystemuser, IsSuccessSystemuser, ErrorMessageSystemuser) = await Utilities.PostFromAPI(new List<FormUser>(), IDs, URL);

                List<EmployeeAccountabilityGetCommentsOutput> resultWithUsername =
                    Result.Join(
                        ResultSystemuser,
                        x => new { x.CreatedBy },
                        y => new { CreatedBy = y.ID },
                        (x, y) => new { x, y })
                    .Select(x => new EmployeeAccountabilityGetCommentsOutput
                    {
                        Timestamp = x.x.Timestamp,
                        Comments = x.x.Comments,
                        Sender = x.y.FirstName + " " + x.y.LastName,
                        CreatedBy = x.x.CreatedBy
                    }).ToList();


                _resultView.IsSuccess = IsSuccess;
                if (IsSuccess)
                    return new OkObjectResult(resultWithUsername);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("add-accountability-comment")]
        public async Task<IActionResult> AddAccountabilityComment([FromQuery] APICredentials param, [FromBody] EmployeeAccountabilityCommentsForm input)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var AddAccountabilityCommentURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("PostEmployeeComments").Value);
                input.IsExternal = true;
                var (IsSuccessAddApplicant, ErrorMessageAddApplicant) = await Utilities.PostFromAPI(input, AddAccountabilityCommentURL);

                _resultView.IsSuccess = IsSuccessAddApplicant;
                if (IsSuccessAddApplicant)
                    return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
                else
                    return new BadRequestObjectResult(ErrorMessageAddApplicant);
            }
        }

        [HttpPost]
        [Route("edit-employee-email")]
        public async Task<IActionResult> UpdateEmployeeEmail([FromQuery] APICredentials param, [FromBody] UpdateEmployeeEmailInput input)
        {
            var UpdateEmployeeEmailURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("UpdateEmployeeEmail").Value);

            var (IsSuccess, ErrorMessage) = await Utilities.PutFromAPI(input, UpdateEmployeeEmailURL);

            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(ErrorMessage);
        }

        [HttpPost]
        [Route("get-all-last-comment-by-employee-id")]
        public async Task<IActionResult> GetAllLastCommentByEmployeeId([FromQuery] int EmployeeId)
        {
            var GetAllLastCommentByEmployeeIdURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAllLastCommentByEmployeeId").Value, "?",
                          "EmployeeId=", EmployeeId);

            var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<EmployeeAccountabilityCommentsForm>(), GetAllLastCommentByEmployeeIdURL);

            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                if (Result.Count > 0)
                    return new OkObjectResult(Result);
                else
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
            else
                return new BadRequestObjectResult(ErrorMessage);
        }

        [HttpPost]
        [Route("employee-login")]
        public async Task<IActionResult> EmployeeLogin([FromQuery] APICredentials param, [FromBody] InputEmployeeLogin Input)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetEmployeeIdURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetExternalEmployeeDetails").Value, "?",
                  "EmployeeId=" + Input.EmployeeId + "&" + "LastName=" + Input.LastName + "&" + "BirthDate=" + Input.BirthDate);

                var (ResultEmployee, IsSuccessEmployee, ErrorMessageEmployee) = await Utilities.GetFromAPI(new OutputEmployeeLogin(), GetEmployeeIdURL);
                _resultView.IsSuccess = IsSuccessEmployee;
                if (IsSuccessEmployee)
                {
                    return new OkObjectResult(ResultEmployee);
                }
                else
                    return new BadRequestObjectResult(0);
            }
        }
        [HttpPost]
        [Route("employee-accountability-by-id")]
        public async Task<IActionResult> EmployeeAccountabilityById([FromQuery] APICredentials param, [FromQuery] int EmpId)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetEmployeeAccountabilityURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                      _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityByEmployeeID").Value, "?",
                      "EmployeeID=" + EmpId);

                var (ResultEmployeeAccountability, IsSuccessEmployeeAccountability, ErrorMessageEmployeeAccountability) = await Utilities.GetFromAPI(new List<OutputEmployeeAccountability>(), GetEmployeeAccountabilityURL);
                _resultView.IsSuccess = IsSuccessEmployeeAccountability;
                if (IsSuccessEmployeeAccountability)
                {
                    var EmployeeAccountability = ResultEmployeeAccountability.OrderByDescending(x => x.Status).ToList();

                    /*List<OutputClearance> DisplayResult = new List<OutputClearance>() {
                            new OutputClearance(){
                                EmployeeAccountability=EmployeeAccountability
                            }
                        };

                    return new OkObjectResult(DisplayResult);*/


                    // Get OrgGroup description by OrgGroup IDs
                    var GetOrgURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetByIDs").Value);

                    var (GetOrgResult, GetOrgIsSuccess, GetOrgErrorMessage) = await Utilities.PostFromAPI(new List<OrgForm>(), (EmployeeAccountability.Where(x => x.OrgGroupID != 0).Select(x => x.OrgGroupID)
                 .Distinct().ToList()), GetOrgURL);

                    // Get Last Comment
                    /*var GetAllLastCommentByEmployeeIdURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetAllLastCommentByEmployeeId").Value, "?",
                         "EmployeeId=", EmpId);

                    var (GetCommentResult, GetCommentIsSuccess, GetCommentErrorMessage) = await Utilities.GetFromAPI(new List<EmployeeAccountabilityCommentsForm>(), GetAllLastCommentByEmployeeIdURL);*/

                    // Get Status Color
                    var GetStatusColorURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
               _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Maintenance").GetSection("GetByID").Value, "?ID=4");

                    var (StatusColor, GetStatusIsSuccess, GetStatusMessage) = await Utilities.GetFromAPI(new StatusOutput(), GetStatusColorURL);

                    var StatusColorList = StatusColor.workflowStepList;

                    EmployeeAccountability = (from left in EmployeeAccountability
                                              join rightstatus in StatusColorList on left.Status equals rightstatus.StepCode
                                              join rightOrg in GetOrgResult on left.OrgGroupID equals rightOrg.ID
                                              //join right in GetCommentResult on left.ID equals right.EmployeeAccountabilityID into joinedList
                                              //from sub in joinedList.DefaultIfEmpty()
                                              select new OutputEmployeeAccountability
                                              {
                                                  ID = left.ID,
                                                  Title = left.Title,
                                                  Type = left.Type,
                                                  Description = left.Description,
                                                  OrgGroupID = left.OrgGroupID,
                                                  OrgGroupDescription = string.Concat(rightOrg.Code, " - ", rightOrg.Description),
                                                  Status = left.Status,
                                                  StatusDescription = rightstatus.StepDescription,
                                                  StatusColor = rightstatus.StatusColor,
                                                  StatusUpdatedDate = left.StatusUpdatedDate,
                                                  StatusRemarks = left.StatusRemarks,
                                                  CreatedBy = left.CreatedBy,
                                                  CreatedDate = left.CreatedDate,
                                                  ModifiedBy = left.ModifiedBy,
                                                  ModifiedDate = left.ModifiedDate,
                                                  LastComment = left.LastComment,
                                                  LastCommentDate = left.LastCommentDate,
                                              }).ToList();


                    List<OutputClearance> DisplayResult = new List<OutputClearance>() {
                            new OutputClearance(){
                                EmployeeAccountability=EmployeeAccountability
                            }
                        };
                    return new OkObjectResult(DisplayResult);
                }
                else
                    return new BadRequestObjectResult(ErrorMessageEmployeeAccountability);
            }
        }

        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> PostChangeStatus([FromQuery] APICredentials param, [FromBody] long ID) 
        {

            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                List<long> GetID = new List<long>();
                GetID.Add(ID);
                ChangeStatusInput changeStatusInputs = new ChangeStatusInput {
                        ID = GetID,
                        Status = "REPLIED",
                        Remarks = "",
                        IsExternal = true
                };

                var URL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("ChangeStatus").Value, "?",
                         "UserID=", param.UserID);
                
                var (IsSuccess, ErrorMessage) = await Utilities.PostFromAPI(changeStatusInputs, URL);
                if (IsSuccess)
                    return new OkObjectResult(ErrorMessage);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("get-exit-clearance-question")]
        public async Task<IActionResult> GetExitClearanceQuestion([FromQuery] APICredentials param)
        {

            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var URL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("GetQuestionByCategory").Value, "?",
                         "UserID=", param.UserID, "&",
                        "Category=EXITINTERVIEW");

                var (Result,IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<GetQuestionTable>(), URL);
                if (IsSuccess)
                    return new OkObjectResult(Result);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("add-question-employee-answer")]
        public async Task<IActionResult> AddQuestionEmployeeAnswer([FromQuery] APICredentials param, [FromBody] List<QuestionEmployeeAnswerInput> input)
        {

            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var URL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Question").GetSection("AddQuestionEmployeeAnswer").Value, "?",
                         "UserID=", param.UserID);

                var (IsSuccess, ErrorMessage) = await Utilities.PostFromAPI(input, URL);
                if (IsSuccess)
                    return new OkObjectResult(IsSuccess);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("get-employee-accountability-exit-clearance")]
        public async Task<IActionResult> GetEmployeeAccountabilityExitClearance([FromQuery] APICredentials param, [FromBody] int ID)
        {

            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var URL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetEmployeeAccountabilityExitClearance").Value, "?",
                         "UserID=", param.UserID);

                var (IsSuccess, ErrorMessage) = await Utilities.PostFromAPI(ID, URL);
                if (IsSuccess)
                    return new OkObjectResult(IsSuccess);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("get-employee-last-pay-details")]
        public async Task<IActionResult> GetEmployeeLastPayDetails([FromQuery] APICredentials param, [FromBody] int EmployeeID)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetClearedEmployeeURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedEmployeeByEmployeeID").Value, "?",
                         "UserID=", param.UserID);
                var (Result1, IsSuccess1, ErrorMessage1) = await Utilities.PostFromAPI(new ClearedEmployee(),EmployeeID, GetClearedEmployeeURL);

                var ClearedEmployeeURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedEmployeeByID").Value, "?",
                         "UserID=", param.UserID);
                var (Result2, IsSuccess2, ErrorMessage2) = await Utilities.PostFromAPI(new List<ClearedEmployee>(), Result1.ID, ClearedEmployeeURL);

                var ClearedEmployeeCommentsURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                         _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("GetClearedComments").Value, "?",
                         "UserID=", param.UserID);
                var (Result3, IsSuccess3, ErrorMessage3) = await Utilities.PostFromAPI(new List<ClearedEmployeeCommentsOutput>(), Result1.ID, ClearedEmployeeCommentsURL);

               ClearedEmployee clearedEmployees = new ClearedEmployee()
                {
                    ID = Result2.Select(x=>x.ID).FirstOrDefault(),
                    Computation = Result2.Select(x=>x.Computation).FirstOrDefault(),
                    StatusCode = Result2.Select(x => x.StatusCode).FirstOrDefault(),
                    StatusDescription = Result2.Select(x=>x.StatusDescription).FirstOrDefault(),
                    clearedEmployeeCommentsOutput = Result3
                };

                if (IsSuccess1)
                    return new OkObjectResult(clearedEmployees);
                else
                    return new BadRequestObjectResult(ErrorMessage1);
            }
        }
        [HttpPost]
        [Route("add-cleared-employee-comment")]
        public async Task<IActionResult> AddClearedEmployeeComment([FromQuery] APICredentials param, [FromBody] ClearedEmployeeCommentsInput input)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var AddClearedEmployeeCommentURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddClearedEmployeeComments").Value, "?",
                         "UserID=", param.UserID);

            var (IsSuccessAddClearedEmployeeComment, ErrorMessageAddClearedEmployeeComment) = await Utilities.PostFromAPI(input, AddClearedEmployeeCommentURL);

                if (IsSuccessAddClearedEmployeeComment)
                    return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
                else
                    return new BadRequestObjectResult(ErrorMessageAddClearedEmployeeComment);
            }
        }
        [HttpPost]
        [Route("add-cleared-employee-agreed")]
        public async Task<IActionResult> AddClearedEmployeeAgreed([FromQuery] APICredentials param, [FromBody] int ID)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var AddClearedEmployeeAgreedURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
            _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Accountability").GetSection("AddClearedEmployeeAgreed").Value, "?",
                     "UserID=", param.UserID);

            var (IsSuccessAddClearedEmployeeAgreed, ErrorMessageAddClearedEmployeeAgreed) = await Utilities.PostFromAPI(ID, AddClearedEmployeeAgreedURL);

            if (IsSuccessAddClearedEmployeeAgreed)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(ErrorMessageAddClearedEmployeeAgreed);
            }
        }
    }
}
