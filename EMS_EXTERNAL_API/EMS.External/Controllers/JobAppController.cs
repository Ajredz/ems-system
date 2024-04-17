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
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

namespace EMS.External.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobAppController : Controller
    {
        public readonly ExternalContext _externalContext;
        public ResultView _resultView = new ResultView();
        public readonly IConfiguration _iconfiguration;
        public string SystemName = "JOBAPP";

        public JobAppController(ExternalContext externalContext, IConfiguration iconfiguration)
        {
            _externalContext = externalContext;
            _iconfiguration = iconfiguration;
        }

        [HttpPost]
        [Route("mrf-list")]
        public async Task<IActionResult> GetListMrfOnline([FromQuery] APICredentials param)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
            var GetMrfURL = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetListMrfOnline").Value);

            var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<OutputMrfOnline>(), GetMrfURL);
            if (IsSuccess)
                return new OkObjectResult(Result);
            else
                return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("education-attainment")]
        public async Task<IActionResult> GetListCourse([FromQuery] APICredentials param)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else { 
                var GetCourseURL = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                    _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Reference").GetSection("GetReferenceValueList").Value, "?",
                      "RefCode=COURSE");

                var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<GetReferenceValueListOutput>(), GetCourseURL);
                _resultView.IsSuccess = IsSuccess;
                if (IsSuccess)
                    return new OkObjectResult(Result);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }


        [HttpPost]
        [Route("kickout-question")]
        public async Task<IActionResult> GetKickoutQuestion([FromQuery] APICredentials param, [FromQuery] string PositionID)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetQuestionURL = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                    _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetKickoutQuestion").Value, "?",
                      "PositionID="+ PositionID);

                var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<KickoutQuestion>(), GetQuestionURL);
                _resultView.IsSuccess = IsSuccess;
                if (IsSuccess)
                    return new OkObjectResult(Result);
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        [HttpPost]
        [Route("get-legal-profile")]
        public async Task<IActionResult> GetListLegalProfile([FromQuery] APICredentials param)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_INVALID_TOKEN);
            }
            else
            {
                var GetCourseURL = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Reference").GetSection("GetReferenceValueList").Value, "?",
                  "RefCode=LEGAL_PROFILE");

                var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new List<GetReferenceValueListOutput>(), GetCourseURL);
                _resultView.IsSuccess = IsSuccess;
                if (IsSuccess)
                    return new OkObjectResult(Result.OrderBy(y => y.Value));
                else
                    return new BadRequestObjectResult(ErrorMessage);
            }
        }

        //API FOR ADD APPLICANT FROM ONLINE FORM
        [HttpPost]
        [Route("add-applicant-online")]
        public async Task<IActionResult> AddApplicantOnline([FromQuery] APICredentials param,[FromBody] MRFPickApplicantFormOnline input)
        {
            var Token = SystemName + DateTime.Now.ToString("yyyy-MM-dd");
            var TokenResult = Utilities.ComputeSHA256Hash(Token);
            if (!TokenResult.Equals(param.Token))
            {
                _resultView.Message = MessageUtilities.ERRMSG_INVALID_TOKEN;
                return new BadRequestObjectResult(_resultView);
            }
            else
            {
                var CheckIfExists = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetLastApplicant").Value, "?",
                "FirstName=" + input.Applicants.FirstName + "&" +
                "LastName=" + input.Applicants.LastName + "&" +
                "Birthday=" + input.Applicants.BirthDate + "&" +
                "Email=" + input.Applicants.Email
                );

                var (ResultCheckIfExists, IsSuccessCheckIfExists, ErrorMessageCheckIfExists) = await Utilities.GetFromAPI(new LastId(), CheckIfExists);

                if (IsSuccessCheckIfExists)
                {
                    _resultView.Message = "Applicant " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS;
                    return new BadRequestObjectResult(_resultView);
                }
                else
                {
                    if (input.Applicants.MrfId == 0)
                    {
                        // For Personal Information
                        PersonalInformation personalInformation = new PersonalInformation()
                        {
                            FirstName = input.Applicants.FirstName,
                            MiddleName = input.Applicants.MiddleName,
                            LastName = input.Applicants.LastName,
                            Suffix = input.Applicants.Suffix,
                            CurrentPosition = input.Applicants.CurrentPosition,
                            Course = input.Applicants.Course,
                            AddressLine1 = input.Applicants.AddressLine1,
                            PSGCRegionCode = input.Applicants.PSGCRegionCode,
                            PSGCProvinceCode = input.Applicants.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = input.Applicants.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = input.Applicants.PSGCBarangayCode,
                            BirthDate = input.Applicants.BirthDate,
                            Email = input.Applicants.Email,
                            CellphoneNumber = input.Applicants.CellphoneNumber,
                        };
                        // For Attachment
                        List<Attachments> attachments = new List<Attachments> {
                        new Attachments() {
                            AttachmentType = "RESUME",
                            SourceFile = input.Applicants.Resume,
                            ServerFile = input.Applicants.ResumeAttachment,
                            CreatedBy = 1,
                        }
                    };
                        // Add Applicant
                        Form addApplicant = new Form()
                        {
                            PositionRemarks = input.Applicants.PositionRemarks,
                            ApplicationSource = input.Applicants.ApplicationSource,
                            ExpectedSalary = Convert.ToDecimal(input.Applicants.ExpectedSalary),
                            DateApplied = DateTime.Now,
                            CreatedBy = 1,
                            PersonalInformation = personalInformation,
                            Attachments = attachments,
                        };

                        var AddApplicantRecruitment = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                            _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("Add").Value, "?",
                            "userid=1");

                        var (IsSuccess, ErrorMessage) = await Utilities.PostFromAPI(addApplicant, AddApplicantRecruitment);
                        //var (IsSuccessAddApplicant, ErrorMessageAddApplicant) = await Utilities.PostFromAPI(new Form(), AddApplicantRecruitment);
                        _resultView.IsSuccess = IsSuccess;
                        _resultView.Result = ErrorMessage;
                        if (IsSuccess)
                        {
                            var GetLastApplicant = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                            _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetLastApplicant").Value, "?",
                            "FirstName=" + input.Applicants.FirstName + "&" +
                            "LastName=" + input.Applicants.LastName + "&" +
                            "Birthday=" + input.Applicants.BirthDate + "&" +
                            "Email=" + input.Applicants.Email
                            );

                            var (Result, IsSuccessLastId, ErrorMessageLastId) = await Utilities.GetFromAPI(new LastId(), GetLastApplicant);
                            _resultView.IsSuccess = IsSuccessLastId;

                            ApplicantLegalProfileInput paramLegalProfile = new ApplicantLegalProfileInput()
                            {
                                ApplicantID = Result.Id,
                                addApplicantLegalProfileInputs = input.addApplicantLegalProfileInputs
                            };

                            var AddLegalProfileURL = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("AddApplicantLegalProfile").Value, "?",
                            "userid=1");

                            var (IsSuccessLegalProfile, ErrorMessageLegalProfile) = await Utilities.PostFromAPI(paramLegalProfile, AddLegalProfileURL);

                            ApplicantKickoutQuestionInput paramKickoutQuestion = new ApplicantKickoutQuestionInput()
                            {
                                ApplicantID = Result.Id,
                                addApplicantKickoutQuestionInputs = input.addApplicantKickoutQuestionInputs
                            };

                            var AddKickoutQuestionURL = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicantKickoutQuestion").Value, "?",
                            "userid=1");

                            var (IsSuccessKickoutQuestion, ErrorMessageKickoutQuestion) = await Utilities.PostFromAPI(paramKickoutQuestion, AddKickoutQuestionURL);

                            ApplicantEmailLogsInput applicantEmailLogsInput = new ApplicantEmailLogsInput()
                            {
                                ToEmailAddress = input.Applicants.Email,
                                JobTitle = input.Applicants.PositionRemarks,
                                ApplicantName = input.Applicants.FirstName + " " + input.Applicants.LastName,
                                Status = "NEW",
                                SystemCode = "JOBAPP",
                                Subject = "Thank you - Job Application",
                                Body = "Dear " + input.Applicants.FirstName + " " + input.Applicants.LastName + "<br><br>"
                                + "Thank you for your interest with Motortrade Group." + "<br><br>"
                                + "Allow us to review your application. Our Recruitment Team will contact you if more information is required." + "<br><br>"
                                + "Sincerely." + "<br>"
                                + "Motortrade Recruitment Team"
                            };
                            await InsertEmail(applicantEmailLogsInput);

                            _resultView.IsSuccess = IsSuccess;
                            _resultView.Id = Result.Id.ToString();
                            _resultView.Message = MessageUtilities.SCSSMSG_REC_SAVE;
                        }

                        return new OkObjectResult(_resultView);
                    }
                    else
                    {

                        var GetMrfById = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                           _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetById").Value, "?",
                                           "ID=" + input.Applicants.MrfId);

                        var (MRFResult, MRFIsSuccess, MRFErrorMessage) = await Utilities.GetFromAPI(new MRFForm(), GetMrfById);

                        // For Personal Information
                        PersonalInformation personalInformation = new PersonalInformation()
                        {
                            FirstName = input.Applicants.FirstName,
                            MiddleName = input.Applicants.MiddleName,
                            LastName = input.Applicants.LastName,
                            Suffix = input.Applicants.Suffix,
                            CurrentPosition = input.Applicants.CurrentPosition,
                            Course = input.Applicants.Course,
                            AddressLine1 = input.Applicants.AddressLine1,
                            PSGCRegionCode = input.Applicants.PSGCRegionCode,
                            PSGCProvinceCode = input.Applicants.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = input.Applicants.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = input.Applicants.PSGCBarangayCode,
                            BirthDate = input.Applicants.BirthDate,
                            Email = input.Applicants.Email,
                            CellphoneNumber = input.Applicants.CellphoneNumber,
                        };
                        // For Attachment
                        List<Attachments> attachments = new List<Attachments> {
                        new Attachments() {
                            AttachmentType = "RESUME",
                            SourceFile = input.Applicants.Resume,
                            ServerFile = input.Applicants.ResumeAttachment,
                            CreatedBy = 1,
                        }
                    };
                        // Add Applicant
                        Form addApplicant = new Form()
                        {
                            PositionRemarks = input.Applicants.PositionRemarks,
                            ApplicationSource = input.Applicants.ApplicationSource,
                            ExpectedSalary = Convert.ToDecimal(input.Applicants.ExpectedSalary),
                            DateApplied = DateTime.Now,
                            CreatedBy = 1,
                            PersonalInformation = personalInformation,
                            Attachments = attachments,
                        };

                        var AddApplicantRecruitment = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                            _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("Add").Value, "?",
                            "userid=1");

                        var (IsSuccessAddApplicant, ErrorMessageAddApplicant) = await Utilities.PostFromAPI(addApplicant, AddApplicantRecruitment);
                        //var (IsSuccessAddApplicant, ErrorMessageAddApplicant) = await Utilities.PostFromAPI(new Form(), AddApplicantRecruitment);
                        _resultView.IsSuccess = IsSuccessAddApplicant;
                        if (IsSuccessAddApplicant)
                        {
                            var GetLastApplicant = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                            _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetLastApplicant").Value, "?",
                            "FirstName=" + input.Applicants.FirstName + "&" +
                            "LastName=" + input.Applicants.LastName + "&" +
                            "Birthday=" + input.Applicants.BirthDate + "&" +
                            "Email=" + input.Applicants.Email
                            );

                            var (Result, IsSuccessLastId, ErrorMessageLastId) = await Utilities.GetFromAPI(new LastId(), GetLastApplicant);
                            _resultView.IsSuccess = IsSuccessLastId;


                            UpdateMRFTransactionIDForm updateMRFTransactionIDForm = new UpdateMRFTransactionIDForm()
                            {
                                MRFTransactionID = MRFResult.MRFTransactionID,
                                HiredApplicantID = 0,
                                ApplicantIDs = new List<int>() {
                            Result.Id
                        }
                            };

                            var UpdateMrfApplicant = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UpdateMrfTransactionId").Value, "?",
                            "userid=1");

                            var (IsSuccessUpdateMrf, ErrorMessageUpdateMrf) = await Utilities.PutFromAPI(updateMRFTransactionIDForm, UpdateMrfApplicant);

                            UpdateCurrentWorkflowStepInput updateCurrentWorkflowStepInput = new UpdateCurrentWorkflowStepInput()
                            {
                                ApplicantID = Result.Id,
                                CurrentStepCode = "0-NEW",
                                CurrentStepDescription = "NEW",
                                CurrentStepApproverRoleIDs = "1",
                                WorkflowStatus = "IN_PROGRESS",
                                DateScheduled = DateTime.Now.ToString("MM/dd/yyyy"),
                                DateCompleted = DateTime.Now.ToString("MM/dd/yyyy"),
                                ApproverRemarks = "NEW"
                            };

                            var UpdateCurrentWorkflow = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                                _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UpdateCurrentWorkflowStep").Value, "?",
                            "userid=1");

                            var (IsSuccessUpdateCurrentMrf, ErrorMessageUpdateCurrentMrf) = await Utilities.PutFromAPI(updateCurrentWorkflowStepInput, UpdateCurrentWorkflow);


                            List<MRFApplicantDetails> mRFApplicantDetails = new List<MRFApplicantDetails>() {
                            new MRFApplicantDetails(){
                                ApplicantID = Result.Id,
                                FirstName = input.Applicants.FirstName,
                                MiddleName=input.Applicants.MiddleName,
                                LastName=input.Applicants.LastName,
                                Suffix=input.Applicants.Suffix,
                                CurrentStepCode="0-NEW",
                                CurrentStepDescription="NEW",
                                CurrentStepApproverRoleIDs="1"
                            }
                        };

                            MRFPickApplicantForm mRFPickApplicantForm = new MRFPickApplicantForm()
                            {
                                ID = input.Applicants.MrfId,
                                WorkflowID = 1,
                                Applicants = mRFApplicantDetails,
                            };

                            var AddMrfApplicant = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicant").Value, "?",
                            "userid=1");

                            var (IsSuccessAddMrf, ErrorMessageAddMrf) = await Utilities.PostFromAPI(mRFPickApplicantForm, AddMrfApplicant);
                            //var (IsSuccessAddMrf, ErrorMessageAddMrf) = await Utilities.PostFromAPI(new Form(), AddMrfApplicant);
                            _resultView.IsSuccess = IsSuccessAddMrf;
                            if (IsSuccessAddMrf)
                            {
                                var GetLastMrfApplicant = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                    _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetLastMrfApplicant").Value, "?",
                            "FirstName=" + input.Applicants.FirstName + "&" + "LastName=" + input.Applicants.LastName);

                                var (LastMrfResult, LastMrfIsSuccessLastId, LastMrfErrorMessageLastId) = await Utilities.GetFromAPI(new LastId(), GetLastMrfApplicant);

                                UpdateMrfCurrentWorkflowStepInput updateMrfCurrentWorkflowStepInput = new UpdateMrfCurrentWorkflowStepInput()
                                {
                                    ID = LastMrfResult.Id,
                                    CurrentStepCode = "0-NEW",
                                    CurrentStepDescription = "NEW",
                                    WorkflowStatus = "IN_PROGRESS",
                                    CurrentStepApproverRoleIDs = "1",
                                    DateScheduled = DateTime.Now.ToString("MM/dd/yyyy"),
                                    DateCompleted = DateTime.Now.ToString("MM/dd/yyyy"),
                                    ApproverRemarks = "NEW"
                                };

                                var UpdateMrfCurrentWorkflow = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                    _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("UpdateCurrentWorkflowStep").Value, "?",
                                "userid=1");

                                var (IsSuccessUpdateCurrentMrfApplicant, ErrorMessageUpdateCurrentMrfApplicant) = await Utilities.PutFromAPI(updateMrfCurrentWorkflowStepInput, UpdateMrfCurrentWorkflow);

                                AddWorkflowTransaction addWorkflowTransaction = new AddWorkflowTransaction()
                                {
                                    RecordID = LastMrfResult.Id,
                                    WorkflowCode = "RECRUITMENT",
                                    CurrentStepCode = "0-NEW",
                                    Result = "YES",
                                    Remarks = "NEW",
                                    DateScheduled = DateTime.Now.ToString("MM/dd/yyyy"),
                                    DateCompleted = DateTime.Now.ToString("MM/dd/yyyy"),
                                    StartDatetime = null
                                };

                                var AddWorkflowTransaction = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                                    _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Transaction").GetSection("Add").Value, "?",
                                "userid=1");

                                var (IsSuccessWorkflowTransaction, ErrorMessageWorkflowTransaction) = await Utilities.PostFromAPI(addWorkflowTransaction, AddWorkflowTransaction);
                                //var (IsSuccessAddMrf, ErrorMessageAddMrf) = await Utilities.PostFromAPI(new Form(), AddMrfApplicant);
                                _resultView.IsSuccess = IsSuccessWorkflowTransaction;


                                ApplicantLegalProfileInput paramLegalProfile = new ApplicantLegalProfileInput()
                                {
                                    ApplicantID = Result.Id,
                                    addApplicantLegalProfileInputs = input.addApplicantLegalProfileInputs
                                };

                                var AddLegalProfileURL = string.Concat(_iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value,
                                    _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("AddApplicantLegalProfile").Value, "?",
                                "userid=1");

                                var (IsSuccessLegalProfile, ErrorMessageLegalProfile) = await Utilities.PostFromAPI(paramLegalProfile, AddLegalProfileURL);

                                ApplicantKickoutQuestionInput paramKickoutQuestion = new ApplicantKickoutQuestionInput()
                                {
                                    ApplicantID = Result.Id,
                                    addApplicantKickoutQuestionInputs = input.addApplicantKickoutQuestionInputs
                                };

                                var AddKickoutQuestionURL = string.Concat(_iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value,
                                    _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddApplicantKickoutQuestion").Value, "?",
                                "userid=1");

                                var (IsSuccessKickoutQuestion, ErrorMessageKickoutQuestion) = await Utilities.PostFromAPI(paramKickoutQuestion, AddKickoutQuestionURL);

                                ApplicantEmailLogsInput applicantEmailLogsInput = new ApplicantEmailLogsInput()
                                {
                                    ToEmailAddress = input.Applicants.Email,
                                    JobTitle = input.Applicants.PositionRemarks,
                                    ApplicantName = input.Applicants.FirstName + " " + input.Applicants.LastName,
                                    Status = "NEW",
                                    SystemCode = "JOBAPP",
                                    Subject = "Thank you - Job Application - " + input.Applicants.PositionRemarks,
                                    Body = "Dear " + input.Applicants.FirstName + " " + input.Applicants.LastName + "<br><br>"
                                    + "Thank you for your interest with Motortrade Group." + "<br><br>"
                                    + "Allow us to review your application. Our Recruitment Team will contact you if more information is required." + "<br><br>"
                                    + "Sincerely." + "<br>"
                                    + "Motortrade Recruitment Team"
                                };

                                await InsertEmail(applicantEmailLogsInput);

                                _resultView.IsSuccess = IsSuccessAddMrf;
                                _resultView.Id = Result.Id.ToString();
                                _resultView.Message = MessageUtilities.SCSSMSG_REC_SAVE;

                                return new OkObjectResult(_resultView);
                            }
                            else
                            _resultView.Message = ErrorMessageAddMrf;
                            return new BadRequestObjectResult(_resultView);
                        }
                        else
                            _resultView.Message = ErrorMessageAddApplicant;
                            return new BadRequestObjectResult(_resultView);
                    }
                }
            }
        }



        //TO HIDE FUNCTION
        [ApiExplorerSettings(IgnoreApi = true)]
        //For Insert Email
        //[Route("add-email")]
        public async Task<IActionResult> InsertEmail(ApplicantEmailLogsInput param)
        {

            ApplicantEmailLogsInput applicantEmailLogsInput = new ApplicantEmailLogsInput()
            {
                SenderName = "MOTORTRADE CAREERS",
                FromEmailAddress = "noreply@motortrade.com.ph",
                ToEmailAddress = param.ToEmailAddress,
                JobTitle = param.JobTitle,
                ApplicantName = param.ApplicantName,
                Status = param.Status,
                SystemCode = param.SystemCode,
                Subject = param.Subject,
                Body = param.Body,
                CreatedBy = "1"
            };

            var applicantEmailLogsInputURL = string.Concat(_iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value,
                _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmailServerCredential").GetSection("AddApplicantPendingEmail").Value, "?",
            "userid=1");

            var (IsSuccess, Messages) = await Utilities.PostFromAPI(applicantEmailLogsInput, applicantEmailLogsInputURL);
            _resultView.IsSuccess = IsSuccess;

            return new OkObjectResult(Messages);
        }


        [HttpPost]
        [Route("send-email")]
        public async Task<IActionResult> sendemail([FromBody] EmailForm emailForm)
        {
            var HeaderName = "tokenizer";
            var HeaderValue = Utilities.ComputeSHA256Hash("EMS" + DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).ToLower();
            var URL = "http://172.0.3.213/api-cmc/public/api/sendEmail";
            var (IsSuccess, Messages) = await Utilities.PostFromAPIWithHeader(emailForm, URL, HeaderName, HeaderValue);
            return new OkObjectResult(IsSuccess);
        }


        [HttpPost]
        [Route("get-token")]
        public async Task<IActionResult> gettoken()
        {
            var HeaderName = "tokenizer";
            var HeaderValue = Utilities.ComputeSHA256Hash("EMS" + DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).ToLower();
            var URL = "http://172.0.3.213/api-cmc/public/api/fetchTokenizer";
            var (resut,IsSuccess, Messages) = await Utilities.GetFromAPI(new GetResult(), URL);
            return new OkObjectResult(resut);
        }
    }
}
