using Utilities.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EMS.FrontEnd.SharedClasses.Common_Recruitment
{
    public class Common_Applicant : Utilities
    {

        public Common_Applicant(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetIDByAutoCompleteOutput>> GetApplicantAutoComplete(string Term, int TopResults)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                     _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetApplicantIDByAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "term=", Term, "&",
                     "topresults=", TopResults);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetIDByAutoCompleteOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<GetIDByAutoCompleteOutput> GetApplicantNameByID(int ID)
        {

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetApplicantNameByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new GetIDByAutoCompleteOutput(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

         public async Task<ApproverResponse> GetApplicantHistory(int ID)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetHistory").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new ApproverResponse(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

		public async Task<Form> GetApplicant(int ID)
        {

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
        
        public async Task<List<Form>> GetApplicants(List<int> IDs)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetByIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.PostFromAPI(new List<Form>(), IDs, URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<bool> UpdateMRFTransactionID(UpdateMRFTransactionIDForm param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                      _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UpdateMRFTransactionID").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }

        public async Task<bool> UpdateCurrentWorkflowStep(UpdateCurrentWorkflowStepInput param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                      _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UpdateCurrentWorkflowStep").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }

        public async Task<List<AttachmentForm>> GetAttachment(int ID)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                      _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetAttachment").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "ID=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<AttachmentForm>(), URL);

            if (IsSuccess)
            {
                // Get Employee Names by User IDs
                List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                   await new SharedClasses.Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                   .GetSystemUserByIDs(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy)
                   .Union(Result.Where(x => x.CreatedBy != 0).Select(x => x.CreatedBy))
                   .Distinct().ToList());

                Result = Result
                        .GroupJoin(systemUsers,
                        x => new { x.CreatedBy },
                        y => new { CreatedBy = y.ID },
                        (x, y) => new { attachment = x, employees = y })
                        .SelectMany(x => x.employees.DefaultIfEmpty(),
                        (x, y) => new { attachment = x, employees = y })
                        .Select(x => new AttachmentForm
                        {
                            AttachmentType = x.attachment.attachment.AttachmentType,
                            Remarks = x.attachment.attachment.Remarks,
                            ServerFile = x.attachment.attachment.ServerFile,
                            SourceFile = x.attachment.attachment.SourceFile,
                            Timestamp = x.attachment.attachment.Timestamp,
                            UploadedBy = x.employees == null ? "" : string.Concat(x.employees.LastName,
                                string.IsNullOrEmpty(x.employees.FirstName) ? "" : string.Concat(", ", x.employees.FirstName),
                                string.IsNullOrEmpty(x.employees.MiddleName) ? "" : string.Concat(" ", x.employees.MiddleName)),
                        }).ToList();
            }


            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> SaveAttachment(ApplicantAttachmentForm AttachmentForm)
        {
            AttachmentForm.AddAttachmentForm = AttachmentForm.AddAttachmentForm.Select(x =>
            {
                if (x.File != null)
                {
                    string dateTimePreFix = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                    x.ServerFile = string.Concat(dateTimePreFix, Guid.NewGuid().ToString("N").Substring(0, 4), "_", x.File.FileName);
                    x.SourceFile = x.File.FileName;
                }
                return x;
            }).ToList();

            var URL = string.Concat(_recruitmentBaseURL,
                    _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("PostAttachment").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(AttachmentForm, URL);

            if (IsSuccess)
            {
                foreach (var item in AttachmentForm.AddAttachmentForm)
                {
                    if (item.File != null)
                    {
                        await CopyToServerPath(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("RecruitmentService_Attachment_Path").Value), item.File, item.ServerFile);
                    }
                }

                if (AttachmentForm.DeleteAttachmentForm?.Count > 0)
                {
                    foreach (var item in AttachmentForm.DeleteAttachmentForm)
                    {
                        DeleteFileFromServer(Path.Combine(_env.WebRootPath,
                       _iconfiguration.GetSection("RecruitmentService_Attachment_Path").Value), item.ServerFile);

                    }
                }
            }
            return (IsSuccess, Message);
        }

        public async Task<bool> UpdateEmployeeID(UpdateEmployeeIDInput param)
        {
            var URL = string.Concat(_recruitmentBaseURL,
                      _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("UpdateEmployeeID").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }
        public async Task<List<GetApplicantLegalProfileOutput>> GetApplicantLegalProfile(int ApplicantId)
        {

            var URL = string.Concat(_recruitmentBaseURL,
                  _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Applicant").GetSection("GetApplicantLegalProfile").Value, "?",
                  "ApplicantId=", ApplicantId);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetApplicantLegalProfileOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }
    }
}