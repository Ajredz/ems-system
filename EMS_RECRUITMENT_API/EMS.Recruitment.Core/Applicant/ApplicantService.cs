using EMS.Manpower.Data.Applicant;
using EMS.Recruitment.Data.Applicant;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Data.PSGC;
using EMS.Recruitment.Data.Reference;
using EMS.Recruitment.Transfer;
using EMS.Recruitment.Transfer.Applicant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.Applicant
{
    public interface IApplicantService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetApplicantPickerList(APICredentials credentials, GetApplicantPickerListInput input);

        Task<IActionResult> GetApprovalList(APICredentials credentials, GetListInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetApplicantNameByID(APICredentials credentials, int ID);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetHistory(APICredentials credentials, int ID);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> AddWorkflowTransaction(APICredentials credentials, ApproverResponse param);

        Task<IActionResult> UpdateMRFTransactionID(APICredentials credentials, UpdateMRFTransactionIDForm param);

        Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs);

        Task<IActionResult> UpdateCurrentWorkflowStep(APICredentials credentials, UpdateCurrentWorkflowStepInput param);

        Task<IActionResult> GetAttachment(APICredentials credentials, int ID);

        Task<IActionResult> PostAttachment(APICredentials credentials, ApplicantAttachmentForm param);

        Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFile> param);

        Task<IActionResult> UpdateEmployeeID(APICredentials credentials, UpdateEmployeeIDInput param);
        // GET THE LAST INSERTED DATA
        Task<IActionResult> GetLastApplicant(string FirstName, string LastName, string Birthday, string Email);
        Task<IActionResult> GetApplicantLegalProfile(int ApplicantId);
        Task<IActionResult> PostApplicantLegalProfile(APICredentials credentials, ApplicantLegalProfileInput param);
    }

    public class ApplicantService : Shared.Utilities, IApplicantService
    {
        private readonly IApplicantDBAccess _dbAccess;
        private readonly IReferenceDBAccess _referenceDBAccess;
        private readonly IPSGCDBAccess _PSGCDBAccess;

        public ApplicantService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IApplicantDBAccess dbAccess, IReferenceDBAccess referenceDBAccess, IPSGCDBAccess PSGCDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _referenceDBAccess = referenceDBAccess;
            _PSGCDBAccess = PSGCDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarApplicant> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.Applicant.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                ScopeOrgGroup = x.ScopeOrgGroup,
                ApplicantName = x.ApplicantName,
                LastName = x.LastName.ToUpper(),
                FirstName = x.FirstName.ToUpper(),
                MiddleName = x.MiddleName.ToUpper(),
                Suffix = x.Suffix.ToUpper(),
                ApplicationSource = x.ApplicationSource,
                MRFTransactionID = x.MRFTransactionID,
                CurrentStep = x.CurrentStep,
                DateScheduled = x.DateScheduled,
                DateCompleted = x.DateCompleted,
                ApproverRemarks = x.ApproverRemarks,
                //WorkflowStatus = x.WorkflowStatus,
                //CurrentStep = x.CurrentStep,
                //WorkflowDescription = x.WorkflowDescription,
                PositionRemarks = x.PositionRemarks,
                Course = x.Course,
                CurrentPositionTitle = x.CurrentPositionTitle,
                ExpectedSalary = x.ExpectedSalary,
                DateApplied = x.DateApplied,
                EmployeeID = x.EmployeeID,
                BirthDate = x.BirthDate,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                //City = x.City,
                Email = x.Email,
                CellphoneNumber = x.CellphoneNumber,
                ReferredBy = x.ReferredBy
            }).ToList());
        }

        public async Task<IActionResult> GetApplicantPickerList(APICredentials credentials, GetApplicantPickerListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarApplicant> result = await _dbAccess.GetApplicantPickerList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.Applicant.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                ScopeOrgGroup = x.ScopeOrgGroup,
                //ApplicantName = x.ApplicantName,
                LastName = x.LastName,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                Suffix = x.Suffix,
                ApplicationSource = x.ApplicationSource,
                MRFTransactionID = x.MRFTransactionID,
                CurrentStep = x.CurrentStep,
                DateScheduled = x.DateScheduled,
                DateCompleted = x.DateCompleted,
                ApproverRemarks = x.ApproverRemarks,
                //WorkflowStatus = x.WorkflowStatus,
                //CurrentStep = x.CurrentStep,
                //WorkflowDescription = x.WorkflowDescription,
                PositionRemarks = x.PositionRemarks,
                Course = x.Course,
                CurrentPositionTitle = x.CurrentPositionTitle,
                ExpectedSalary = x.ExpectedSalary,
                DateApplied = x.DateApplied
            }).ToList());
        }

        public async Task<IActionResult> GetApprovalList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarApplicationApproval> result = await _dbAccess.GetApprovalList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.Applicant.GetApprovalListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                WorkflowID = x.WorkflowID,
                ApplicantName = x.ApplicantName,
                ApplicationSource = x.ApplicationSource,
                //CurrentStep = x.CurrentStep,
                //WorkflowDescription = x.WorkflowDescription,
                PositionRemarks = x.PositionRemarks,
                Course = x.Course,
                CurrentPositionTitle = x.CurrentPositionTitle,
                ExpectedSalary = x.ExpectedSalary,
                DateApplied = x.DateApplied,
                HasApproval = x.HasApproval

            }).ToList());
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.ApplicationSource = (param.ApplicationSource ?? "").Trim();
            param.PositionRemarks = (param.PositionRemarks ?? "").Trim();

            param.PersonalInformation.FirstName = (param.PersonalInformation.FirstName ?? "").Trim();
            param.PersonalInformation.LastName = (param.PersonalInformation.LastName ?? "").Trim();
            param.PersonalInformation.MiddleName = (param.PersonalInformation.MiddleName ?? "").Trim();
            param.PersonalInformation.Suffix = (param.PersonalInformation.Suffix ?? "").Trim();
            param.PersonalInformation.CurrentPosition = (param.PersonalInformation.CurrentPosition ?? "").Trim();
            param.PersonalInformation.Course = (param.PersonalInformation.Course ?? "").Trim();
            param.PersonalInformation.AddressLine1 = (param.PersonalInformation.AddressLine1 ?? "").Trim();
            param.PersonalInformation.AddressLine2 = (param.PersonalInformation.AddressLine2 ?? "").Trim();
            //param.PersonalInformation.GeographicalRegion = (param.PersonalInformation.GeographicalRegion ?? "").Trim();
            param.PersonalInformation.Email = (param.PersonalInformation.Email ?? "").Trim();
            param.PersonalInformation.CellphoneNumber = (param.PersonalInformation.CellphoneNumber ?? "").Trim();

            if (string.IsNullOrEmpty(param.ApplicationSource))
                ErrorMessages.Add(string.Concat("Application Source ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
               if (param.ApplicationSource.Length > 20)
                ErrorMessages.Add(string.Concat("Application Source", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (!string.IsNullOrEmpty(param.PositionRemarks))
                if (param.PositionRemarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Desired Position (remarks)", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.FirstName))
                ErrorMessages.Add(string.Concat("First Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.FirstName.Length > 50)
                ErrorMessages.Add(string.Concat("First Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.LastName))
                ErrorMessages.Add(string.Concat("Last Name ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.LastName.Length > 50)
                ErrorMessages.Add(string.Concat("Last Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.MiddleName))
                if (param.PersonalInformation.MiddleName.Length > 50)
                    ErrorMessages.Add(string.Concat("Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.Suffix))
                if (param.PersonalInformation.Suffix.Length > 10)
                    ErrorMessages.Add(string.Concat("Suffix", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.CurrentPosition))
                ErrorMessages.Add(string.Concat("Current Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.CurrentPosition.Length > 50)
                ErrorMessages.Add(string.Concat("Current Position", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.Course))
                ErrorMessages.Add(string.Concat("Course ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.Course.Length > 50)
                ErrorMessages.Add(string.Concat("Course", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.AddressLine1))
                ErrorMessages.Add(string.Concat("Address Line 1 ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.AddressLine1.Length > 255)
                ErrorMessages.Add(string.Concat("Address Line 1", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!string.IsNullOrEmpty(param.PersonalInformation.AddressLine2))
                if (param.PersonalInformation.AddressLine2.Length > 255)
                    ErrorMessages.Add(string.Concat("Address Line 2", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            //if (string.IsNullOrEmpty(param.PersonalInformation.GeographicalRegion))
            //    ErrorMessages.Add(string.Concat("Region ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //   if (param.PersonalInformation.GeographicalRegion.Length > 20)
            //    ErrorMessages.Add(string.Concat("Region", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCRegionCode))
                ErrorMessages.Add(string.Concat("Region ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCProvinceCode))
                ErrorMessages.Add(string.Concat("Province ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCCityMunicipalityCode))
                ErrorMessages.Add(string.Concat("City/Municipality ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.PersonalInformation.PSGCBarangayCode))
                ErrorMessages.Add(string.Concat("Barangay ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.PersonalInformation.Email))
                ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.Email.Length > 255)
                ErrorMessages.Add(string.Concat("Email Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (!RegexUtilities.IsValidEmail(param.PersonalInformation.Email))
            {
                ErrorMessages.Add(string.Concat("Email Address ", MessageUtilities.SUFF_ERRMSG_INVALID));
            }

            if (string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.PersonalInformation.CellphoneNumber.Length > 15)
                ErrorMessages.Add(string.Concat("Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));

            if (!Regex.IsMatch(param.PersonalInformation.CellphoneNumber, RegexUtilities.REGEX_PHONE_NUMBER))
            {
                ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
            }

            if (param.Attachments.Count <= 0)
                ErrorMessages.Add(string.Concat("Attachment ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                foreach (var item in param.Attachments)
                {
                    if (string.IsNullOrEmpty(param.PersonalInformation.CellphoneNumber))
                        ErrorMessages.Add(string.Concat("Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                        if (param.PersonalInformation.CellphoneNumber.Length > 15)
                        ErrorMessages.Add(string.Concat("Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));
                }
            }

            if (ErrorMessages.Count == 0)
            {
                _resultView.IsSuccess = await _dbAccess.Post(new EMS.Recruitment.Data.Applicant.Applicant
                {
                    ApplicationSource = param.ApplicationSource,
                    PositionRemarks = param.PositionRemarks,
                    ReferredByUserID = param.ReferredByUserID,
                    ExpectedSalary = param.ExpectedSalary,
                    DateApplied = param.DateApplied,
                    FirstName = param.PersonalInformation.FirstName,
                    LastName = param.PersonalInformation.LastName,
                    MiddleName = param.PersonalInformation.MiddleName,
                    Suffix = param.PersonalInformation.Suffix,
                    CurrentPosition = param.PersonalInformation.CurrentPosition,
                    Course = param.PersonalInformation.Course,
                    AddressLine1 = param.PersonalInformation.AddressLine1,
                    AddressLine2 = param.PersonalInformation.AddressLine2,
                    PSGCRegionCode = param.PersonalInformation.PSGCRegionCode,
                    PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode,
                    PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode,
                    PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode,
                    BirthDate = param.PersonalInformation.BirthDate,
                    Email = param.PersonalInformation.Email,
                    CellphoneNumber = param.PersonalInformation.CellphoneNumber,
                    IsActive = true,
                    CreatedBy = credentials.UserID
                },
                param.Attachments.Select(x =>
                    new EMS.Recruitment.Data.Applicant.ApplicantAttachment
                    {
                        AttachmentType = x.AttachmentType,
                        Remarks = x.Remarks,
                        SourceFile = x.SourceFile,
                        ServerFile = x.ServerFile,
                        CreatedBy = credentials.UserID
                    }).ToList()
                );
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            EMS.Recruitment.Data.Applicant.Applicant applicant = (await _dbAccess.GetByID(ID));

            if (applicant == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                List<Data.Applicant.ApplicantAttachment> attachment = (await _dbAccess.GetAttachmentsByApplicantID(ID)).ToList();
                //List<TableVarApplicantHistory> history = (await _dbAccess.GetHistory(ID)).ToList();

                //if (attachment?.Count <= 0)
                //    return new BadRequestObjectResult(string.Concat("Attachment ", MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS));
                //else
                //{
                    return new OkObjectResult(new Form
                    {
                        ID = applicant.ID,
                        PositionRemarks = applicant.PositionRemarks,
                        ReferredByUserID = applicant.ReferredByUserID,
                        ApplicationSource = applicant.ApplicationSource,
                        ExpectedSalary = applicant.ExpectedSalary,
                        DateApplied = applicant.DateApplied,
                        CreatedDate = applicant.CreatedDate,
                        MRFTransactionID = applicant.MRFTransactionID,
                        PersonalInformation = new PersonalInformation
                        {
                            FirstName = applicant.FirstName,
                            MiddleName = applicant.MiddleName,
                            LastName = applicant.LastName,
                            Suffix = applicant.Suffix,
                            CurrentPosition = applicant.CurrentPosition,
                            Course = applicant.Course,
                            AddressLine1 = applicant.AddressLine1,
                            AddressLine2 = applicant.AddressLine2,
                            //GeographicalRegion = applicant.GeographicalRegion,
                            PSGCRegionCode = applicant.PSGCRegionCode,
                            PSGCProvinceCode = applicant.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = applicant.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = applicant.PSGCBarangayCode,
                            BirthDate = applicant.BirthDate,
                            Email = applicant.Email,
                            CellphoneNumber = applicant.CellphoneNumber
                        },
                        Attachments = attachment?.Select(x => new Attachments
                        {
                            ApplicantID = x.ApplicantID,
                            AttachmentType = x.AttachmentType,
                            Remarks = x.Remarks,
                            SourceFile = x.SourceFile,
                            ServerFile = x.ServerFile,
                            CreatedDate = x.CreatedDate,
                            CreatedBy = x.CreatedBy
                        }).ToList(),
                        //ApplicationHistory = history.Select(x => new ApplicationHistory
                        //{
                        //    OrderNo = x.Order,
                        //    Step = x.Step,
                        //    Result = x.Status,
                        //    Timestamp = x.Timestamp,
                        //    Remarks = x.Remarks,
                            
                        //}).ToList()
                    });
                //}
            }
        }

        public async Task<IActionResult> GetHistory(APICredentials credentials, int ID)
        {
            Data.Applicant.Applicant applicant = await _dbAccess.GetByID(ID);
            return new OkObjectResult(
                new ApproverResponse {
                    RecordID = applicant.ID,
                    ApplicantName = string.Concat(applicant.LastName, ", ", applicant.FirstName, " " , applicant.MiddleName),
                 History = (await _dbAccess.GetHistory(ID)).Select(x => new GetHistoryOutput
                 {
                     ID = x.ID,
                     Order = x.Order,
                     Step = x.Step,
                     Status = x.Status,
                     Timestamp = x.Timestamp,
                     Remarks = x.Remarks,
                     ResultType = x.ResultType
                 }).ToList()
                });
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Recruitment.Data.Applicant.Applicant applicant = await _dbAccess.GetByID(ID);
            applicant.IsActive = false;
            applicant.ModifiedBy = credentials.UserID;
            applicant.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Delete(applicant))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            List<ApplicantAttachment> GetToAdd(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { x.ApplicantID, x.ServerFile },
                    y => new { y.ApplicantID, y.ServerFile },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new ApplicantAttachment
                    {
                        ApplicantID = x.newSet.newSet.ApplicantID,
                        AttachmentType = x.newSet.newSet.AttachmentType,
                        Remarks = x.newSet.newSet.Remarks,
                        SourceFile = x.newSet.newSet.SourceFile,
                        ServerFile = x.newSet.newSet.ServerFile,
                        CreatedBy = credentials.UserID
                    })
                .ToList();
            }

            List<ApplicantAttachment> GetToUpdate(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
            {
                return left.Join(
                right,
                x => new { x.ApplicantID, x.ServerFile },
                y => new { y.ApplicantID, y.ServerFile },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.AttachmentType.Equals(x.newSet.AttachmentType)
                    || !(x.oldSet.Remarks ?? "").Equals(x.newSet.Remarks ?? "")
                    || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                )
                .Select(y => new ApplicantAttachment
                {
                    ID = y.oldSet.ID,
                    ApplicantID = y.newSet.ApplicantID,
                    AttachmentType = y.newSet.AttachmentType,
                    Remarks = y.newSet.Remarks,
                    SourceFile = y.newSet.SourceFile,
                    ServerFile = y.newSet.ServerFile,
                    CreatedBy = y.newSet.CreatedBy,
                    CreatedDate = y.newSet.CreatedDate,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now
                })
                .ToList();
            }

            List<ApplicantAttachment> GetToDelete(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
            {
                return left.GroupJoin(
                        right,
                        x => new { x.ApplicantID, x.ServerFile },
                        y => new { y.ApplicantID, y.ServerFile },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new ApplicantAttachment
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
            }

            Data.Applicant.Applicant applicants = await _dbAccess.GetByID(param.ID);
            List<ApplicantAttachment> oldSet = (await _dbAccess.GetAttachmentsByApplicantID(param.ID)).ToList();

            List<ApplicantAttachment> paramAttachments = param.Attachments.Select(x => new ApplicantAttachment
            {
                ApplicantID = x.ApplicantID,
                AttachmentType = x.AttachmentType,
                Remarks = x.Remarks,
                SourceFile = x.SourceFile,
                ServerFile = x.ServerFile
            }).ToList();

            List<ApplicantAttachment> ValueToAdd = GetToAdd(paramAttachments, oldSet).ToList();
            List<ApplicantAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachments).ToList();
            List<ApplicantAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachments).ToList();

            if (await _dbAccess.Put(new Data.Applicant.Applicant
            {
                ID = param.ID,
                ApplicationSource = param.ApplicationSource,
                PositionRemarks = param.PositionRemarks,
                ReferredByUserID = param.ReferredByUserID,
                ExpectedSalary = param.ExpectedSalary,
                DateApplied = param.DateApplied,
                FirstName = param.PersonalInformation.FirstName,
                MiddleName = param.PersonalInformation.MiddleName,
                LastName = param.PersonalInformation.LastName,
                Suffix = param.PersonalInformation.Suffix,
                CurrentPosition = param.PersonalInformation.CurrentPosition,
                Course = param.PersonalInformation.Course,
                AddressLine1 = param.PersonalInformation.AddressLine1,
                AddressLine2 = param.PersonalInformation.AddressLine2,
                //GeographicalRegion = param.PersonalInformation.GeographicalRegion,
                PSGCRegionCode = param.PersonalInformation.PSGCRegionCode,
                PSGCProvinceCode = param.PersonalInformation.PSGCProvinceCode,
                PSGCCityMunicipalityCode = param.PersonalInformation.PSGCCityMunicipalityCode,
                PSGCBarangayCode = param.PersonalInformation.PSGCBarangayCode,
                BirthDate = param.PersonalInformation.BirthDate,
                Email = param.PersonalInformation.Email,
                CellphoneNumber = param.PersonalInformation.CellphoneNumber,
                IsActive = applicants.IsActive,
                CreatedBy = applicants.CreatedBy,
                CreatedDate = applicants.CreatedDate,
                ModifiedBy = credentials.UserID,
                ModifiedDate = DateTime.Now
            },
            ValueToAdd, ValueToDelete, ValueToUpdate))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new OkObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.ApplicantName
                })
            );
        }

        public async Task<IActionResult> GetApplicantNameByID(APICredentials credentials, int ID)
        {
            EMS.Recruitment.Data.Applicant.Applicant result = (await _dbAccess.GetByID(ID));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(new GetIDByAutoCompleteOutput
                {
                    ID = result.ID,
                    Description = result.LastName + ", " + result.FirstName + " " + result.MiddleName
                });
            }
        }

        public async Task<IActionResult> AddWorkflowTransaction(APICredentials credentials, ApproverResponse param)
        {
            await _dbAccess.AddWorkflowTransaction(credentials.UserID, param);
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        }

        public async Task<IActionResult> UpdateMRFTransactionID(APICredentials credentials, UpdateMRFTransactionIDForm param)
        {
            if (param.HiredApplicantID > 0)
            {
                List<Data.Applicant.Applicant> hiredApplicant = 
                    (await _dbAccess.GetByIDs(new List<int> { param.HiredApplicantID })).ToList();

                await _dbAccess.UpdateApplicants(hiredApplicant.Select(x =>
                {
                    x.MRFTransactionID = param.MRFTransactionID;
                    return x;
                }).ToList());

                if (param.ApplicantIDs != null)
                {
                    if (param.ApplicantIDs.Count > 0)
                    {
                        List<Data.Applicant.Applicant> failedApplicants = (await _dbAccess.GetByIDs(param.ApplicantIDs)).ToList();

                        await _dbAccess.UpdateApplicants(failedApplicants.Select(x =>
                        {
                            x.MRFTransactionID = null;
                            x.FailedMRFTransactionID = param.MRFTransactionID;
                            return x;
                        }).ToList());  
                    }
                }

                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
            {
                List<Data.Applicant.Applicant> applicants = (await _dbAccess.GetByIDs(param.ApplicantIDs)).ToList();

                if (await _dbAccess.UpdateApplicants(applicants.Select(x =>
                        {
                            x.MRFTransactionID = param.MRFTransactionID;
                            return x;
                }).ToList()))
                    return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
                else
                    return new OkObjectResult(MessageUtilities.PRE_ERRMSG_REC_UPDATE);

            }
            

        }

        public async Task<IActionResult> GetByIDs(APICredentials credentials, List<int> IDs)
        {
            List<EMS.Recruitment.Data.Applicant.Applicant> applicant = (await _dbAccess.GetByIDs(IDs)).ToList();

            return new OkObjectResult(
                applicant.Select(x => 
                        new Form
                        {
                            ID = x.ID,
                            PositionRemarks = x.PositionRemarks,
                            ReferredByUserID = x.ReferredByUserID,
                            ApplicationSource = x.ApplicationSource,
                            ExpectedSalary = x.ExpectedSalary,
                            DateApplied = x.DateApplied,
                            CreatedDate = x.CreatedDate,
                            PersonalInformation = new PersonalInformation
                            {
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.LastName,
                                Suffix = x.Suffix,
                                CurrentPosition = x.CurrentPosition,
                                Course = x.Course,
                                AddressLine1 = x.AddressLine1,
                                AddressLine2 = x.AddressLine2,
                                PSGCRegionCode = x.PSGCRegionCode,
                                PSGCProvinceCode = x.PSGCProvinceCode,
                                PSGCCityMunicipalityCode = x.PSGCCityMunicipalityCode,
                                PSGCBarangayCode = x.PSGCBarangayCode,
                                BirthDate = x.BirthDate,
                                Email = x.Email,
                                CellphoneNumber = x.CellphoneNumber
                            }
                        }).ToList()
                );
        }

        public async Task<IActionResult> UpdateCurrentWorkflowStep(APICredentials credentials, UpdateCurrentWorkflowStepInput param)
        { 
            Data.Applicant.Applicant applicant = (await _dbAccess.GetByID(param.ApplicantID));
            Data.Applicant.ApplicantHistory applicantHistory = new Data.Applicant.ApplicantHistory();

            if (applicant == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);

            applicant.CurrentStepCode = param.CurrentStepCode;
            applicant.CurrentStepDescription = param.CurrentStepDescription;
            applicant.WorkflowStatus = param.WorkflowStatus;
            applicant.CurrentStepApproverRoleIDs = param.CurrentStepApproverRoleIDs;
            applicant.DateScheduled = string.IsNullOrEmpty(param.DateScheduled) ? default(DateTime?) :
                    DateTime.ParseExact(param.DateScheduled, "MM/dd/yyyy", null);
            applicant.DateCompleted = string.IsNullOrEmpty(param.DateCompleted) ? default(DateTime?) :
                    DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null);
            applicant.ApproverRemarks = param.ApproverRemarks;

            if (await _dbAccess.UpdateApplicants(new List<Data.Applicant.Applicant> { applicant }))
            {
                // APPLICANT HISTORY
                applicantHistory.ApplicantID = applicant.ID;
                applicantHistory.Status = param.CurrentStepCode;
                applicantHistory.StatusResult = param.Result;
                applicantHistory.DateScheduled = string.IsNullOrEmpty(param.DateScheduled) ? default(DateTime?) :
                    DateTime.ParseExact(param.DateScheduled, "MM/dd/yyyy", null);
                applicantHistory.DateCompleted = string.IsNullOrEmpty(param.DateCompleted) ? default(DateTime?) :
                    DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null);
                applicantHistory.Remarks = param.ApproverRemarks;
                applicantHistory.MRFID = applicant.MRFTransactionID;
                applicantHistory.IsActive = true;
                applicantHistory.CreatedBy = credentials.UserID;
                applicantHistory.CreatedDate = DateTime.Now;

                await _dbAccess.PostApplicantHistory(applicantHistory);
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
                return new OkObjectResult(MessageUtilities.PRE_ERRMSG_REC_UPDATE);

        }

        public async Task<IActionResult> GetAttachment(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAttachmentsByApplicantID(ID))
                .Select(x => new AttachmentForm
                {
                    AttachmentType = x.AttachmentType,
                    Remarks = x.Remarks,
                    SourceFile = x.SourceFile,
                    ServerFile = x.ServerFile,
                    Timestamp = x.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    CreatedBy = x.CreatedBy
                }));
        }

        public async Task<IActionResult> PostAttachment(APICredentials credentials, ApplicantAttachmentForm param)
        {

            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.AddAttachmentForm?.Count > 0)
            {
                foreach (var item in param.AddAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.AttachmentType))
                        ErrorMessages.Add(string.Concat("AttachmentType ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.AttachmentType = item.AttachmentType.Trim();
                        if (item.AttachmentType.Length > 20)
                            ErrorMessages.Add(string.Concat("AttachmentType", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!string.IsNullOrEmpty(item.Remarks))
                    //ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    //else
                    {
                        item.Remarks = item.Remarks.Trim();
                        if (item.Remarks.Length > 255)
                            ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (string.IsNullOrEmpty(item.SourceFile))
                        ErrorMessages.Add(string.Concat("SourceFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.SourceFile = item.SourceFile.Trim();
                        if (item.SourceFile.Length > 255)
                            ErrorMessages.Add(string.Concat("SourceFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (string.IsNullOrEmpty(item.ServerFile))
                        ErrorMessages.Add(string.Concat("ServerFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.ServerFile = item.ServerFile.Trim();
                        if (item.ServerFile.Length > 255)
                            ErrorMessages.Add(string.Concat("ServerFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }
            }
            else
            {
                ErrorMessages.Add(string.Concat("AddAttachmentForm ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            if (param.DeleteAttachmentForm?.Count > 0)
            {
                foreach (var item in param.DeleteAttachmentForm)
                {
                    if (string.IsNullOrEmpty(item.ServerFile))
                        ErrorMessages.Add(string.Concat("ServerFile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        item.ServerFile = item.ServerFile.Trim();
                        if (item.ServerFile.Length > 255)
                            ErrorMessages.Add(string.Concat("ServerFile", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }
            }

            if (ErrorMessages.Count == 0)
            {
                List<ApplicantAttachment> GetToAdd(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.ApplicantID, x.ServerFile },
                        y => new { y.ApplicantID, y.ServerFile },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new ApplicantAttachment
                        {
                            ApplicantID = x.newSet.newSet.ApplicantID,
                            AttachmentType = x.newSet.newSet.AttachmentType,
                            Remarks = x.newSet.newSet.Remarks,
                            SourceFile = x.newSet.newSet.SourceFile,
                            ServerFile = x.newSet.newSet.ServerFile,
                            CreatedBy = credentials.UserID
                        })
                    .ToList();
                }

                List<ApplicantAttachment> GetToUpdate(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
                {
                    return left.Join(
                    right,
                    x => new { x.ApplicantID, x.ServerFile },
                    y => new { y.ApplicantID, y.ServerFile },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.AttachmentType.Equals(x.newSet.AttachmentType)
                        || !(x.oldSet.Remarks ?? "").Equals(x.newSet.Remarks ?? "")
                        || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                    )
                    .Select(y => new ApplicantAttachment
                    {
                        ID = y.oldSet.ID,
                        ApplicantID = y.newSet.ApplicantID,
                        AttachmentType = y.newSet.AttachmentType,
                        Remarks = y.newSet.Remarks,
                        SourceFile = y.newSet.SourceFile,
                        ServerFile = y.newSet.ServerFile,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    })
                    .ToList();
                }

                List<ApplicantAttachment> GetToDelete(List<ApplicantAttachment> left, List<ApplicantAttachment> right)
                {
                    return left.GroupJoin(
                            right,
                            x => new { x.ApplicantID, x.ServerFile },
                            y => new { y.ApplicantID, y.ServerFile },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new ApplicantAttachment
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
                }

                List<ApplicantAttachment> oldSet = (await _dbAccess.GetAttachmentsByApplicantID(param.ApplicantID)).ToList();
                List<ApplicantAttachment> paramAttachment =
                    param.AddAttachmentForm.Select(x => new ApplicantAttachment
                    {
                        ApplicantID = param.ApplicantID,
                        AttachmentType = x.AttachmentType,
                        ServerFile = x.ServerFile,
                        SourceFile = x.SourceFile,
                        Remarks = x.Remarks
                    }).ToList();

                List<ApplicantAttachment> ValueToAdd = GetToAdd(paramAttachment, oldSet).ToList();
                List<ApplicantAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachment).ToList();
                List<ApplicantAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachment).ToList();

                List<ApplicantAttachment> addAttachment = new List<ApplicantAttachment>();
                foreach (var item in param.AddAttachmentForm)
                {
                    addAttachment.Add(new ApplicantAttachment
                    {
                        ApplicantID = param.ApplicantID,
                        AttachmentType = item.AttachmentType,
                        ServerFile = item.ServerFile,
                        SourceFile = item.SourceFile,
                        Remarks = item.Remarks,
                        CreatedBy = credentials.UserID,
                    });
                }

                await _dbAccess.PostAttachment(ValueToAdd, ValueToUpdate, ValueToDelete);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFile> param)
        {

            var applicationSource = (await _referenceDBAccess.GetByRefCodes(new List<string> { "APPLICATION_SOURCE" })).Select(x => x.Value).ToList();

            var regionPSGC = (await _PSGCDBAccess.GetAllRegion()).ToList();
            var provincePSGC = (await _PSGCDBAccess.GetAllProvince()).ToList();
            var cityMunicipalityPSGC = (await _PSGCDBAccess.GetAllCityMunicipality()).ToList();
            var barangayPSGC = (await _PSGCDBAccess.GetAllBarangay()).ToList();

            /*Checking of required and invalid fields*/
            foreach (UploadFile obj in param)
            {

                /*DesiredPosition*/
                if (string.IsNullOrEmpty(obj.DesiredPosition))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Desired Position (remarks)", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DesiredPosition = obj.DesiredPosition.Trim();
                    if (obj.DesiredPosition.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Desired Position (remarks)", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*DateApplied*/
                if (string.IsNullOrEmpty(obj.DateApplied))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Applied", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DateApplied = obj.DateApplied.Trim();
                    if (!DateTime.TryParseExact(obj.DateApplied, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Applied", MessageUtilities.COMPARE_INVALID_DATE));
                    } 
                }

                /*ApplicationSource*/
                if (string.IsNullOrEmpty(obj.ApplicationSource))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Application Source ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.ApplicationSource = obj.ApplicationSource.Trim();
                    if (obj.ApplicationSource.Length > 20)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Application Source", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                    }

                    if (!Regex.IsMatch(obj.ApplicationSource, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Application Source ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                    
                    if (applicationSource.Where(x => obj.ApplicationSource.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Application Source ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*ExpectedSalary*/
                if (!string.IsNullOrEmpty(obj.ExpectedSalary))
                {
                    obj.ExpectedSalary = obj.ExpectedSalary.Trim();
                    if (!Decimal.TryParse(obj.ExpectedSalary, NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
                        , CultureInfo.CreateSpecificCulture("en-US"), out decimal number))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Expected Salary", MessageUtilities.COMPARE_INVALID_AMOUNT));
                    }
                }

                /*LastName*/
                if (string.IsNullOrEmpty(obj.LastName))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Last Name", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.LastName = obj.LastName.Trim();
                    if (obj.LastName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Last Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*FirstName*/
                if (string.IsNullOrEmpty(obj.FirstName))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "First Name", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.FirstName = obj.FirstName.Trim();
                    if (obj.FirstName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "First Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*MiddleName*/
                if (string.IsNullOrEmpty(obj.MiddleName))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Middle Name", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.MiddleName = obj.MiddleName.Trim();
                    if (obj.MiddleName.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Middle Name", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*Suffix*/
                if (!string.IsNullOrEmpty(obj.Suffix))
                {
                    obj.Suffix = obj.Suffix.Trim();
                    if (obj.Suffix.Length > 10)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Suffix", MessageUtilities.COMPARE_NOT_EXCEED, "10 characters."));
                    }
                }

                /*BirthDate*/
                if (string.IsNullOrEmpty(obj.BirthDate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Birth Date", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.BirthDate = obj.BirthDate.Trim();
                    if (!DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Birth Date", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                }


                /*AddressLine1*/
                if (string.IsNullOrEmpty(obj.AddressLine1))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address Line 1", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.AddressLine1 = obj.AddressLine1.Trim();
                    if (obj.AddressLine1.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address Line 1", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*AddressLine2*/
                if (!string.IsNullOrEmpty(obj.AddressLine2))
                {
                    obj.AddressLine2 = obj.AddressLine2.Trim();
                    if (obj.AddressLine2.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Address Line 2", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*PSGCRegionCode*/
                if (string.IsNullOrEmpty(obj.PSGCRegionCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCRegionCode = obj.PSGCRegionCode.Trim();
                    if (obj.PSGCRegionCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCRegionCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var region = regionPSGC.Where(x => obj.PSGCRegionCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (region.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Region (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCProvinceCode*/
                if (string.IsNullOrEmpty(obj.PSGCProvinceCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCProvinceCode = obj.PSGCProvinceCode.Trim();
                    if (obj.PSGCProvinceCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCProvinceCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var province = provincePSGC.Where(x => obj.PSGCProvinceCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (province.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Province (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCCityMunicipalityCode*/
                if (string.IsNullOrEmpty(obj.PSGCCityMunicipalityCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "City/Municipality (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCCityMunicipalityCode = obj.PSGCCityMunicipalityCode.Trim();
                    if (obj.PSGCCityMunicipalityCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "City/Municipality (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCCityMunicipalityCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CityMunicipality (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var cityMunicipality = cityMunicipalityPSGC.Where(x => obj.PSGCCityMunicipalityCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (cityMunicipality.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "CityMunicipality (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }
                /*PSGCBarangayCode*/
                if (string.IsNullOrEmpty(obj.PSGCBarangayCode))
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    obj.PSGCBarangayCode = obj.PSGCBarangayCode.Trim();
                    if (obj.PSGCBarangayCode.Length > 50) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC)", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));

                    if (!Regex.IsMatch(obj.PSGCBarangayCode, RegexUtilities.REGEX_CODE)) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.ERRMSG_REGEX_CODE));
                    var barangay = barangayPSGC.Where(x => obj.PSGCBarangayCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase));
                    if (barangay.Count() == 0) ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Barangay (PSGC) ", MessageUtilities.COMPARE_INVALID));
                }

                /*CurrentPosition*/
                if (string.IsNullOrEmpty(obj.CurrentPosition))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Current Position", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.CurrentPosition = obj.CurrentPosition.Trim();
                    if (obj.CurrentPosition.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Current Position", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }
                }

                /*Course*/
                if (string.IsNullOrEmpty(obj.Course))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Course", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Course = obj.Course.Trim();
                    if (obj.Course.Length > 100)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Course", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
                    }

                    if (!Regex.IsMatch(obj.Course, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Course ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }
                }


                /*EmailAddress*/
                if (string.IsNullOrEmpty(obj.EmailAddress))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Email Address", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.EmailAddress = obj.EmailAddress.Trim();
                    if (obj.EmailAddress.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Email Address", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }

                    if (!RegexUtilities.IsValidEmail(obj.EmailAddress))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Email Address ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                /*CellphoneNumber*/
                if (string.IsNullOrEmpty(obj.CellphoneNumber))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Cellphone Number", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.CellphoneNumber = obj.CellphoneNumber.Trim();
                    if (obj.CellphoneNumber.Length > 15)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Cellphone Number", MessageUtilities.COMPARE_NOT_EXCEED, "15 characters."));
                    }

                    if (!Regex.IsMatch(obj.CellphoneNumber, RegexUtilities.REGEX_PHONE_NUMBER))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Cellphone Number ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                    if (obj.CellphoneNumber.Contains("+63"))
                        obj.CellphoneNumber = obj.CellphoneNumber.Replace("+63", "0");
                }

                /*ReferredByID*/
                if (!string.IsNullOrEmpty(obj.ReferredByCode))
                {
                    if (!obj.ReferredByID.HasValue)
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Referred By (Employee Code) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    else
                    { 
                        if(obj.ReferredByID.Value == 0)
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Referred By (Employee Code) ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

            }

            List<string> Duplicates = new List<string>();

            /*Remove Duplicates*/
            if (ErrorMessages.Count == 0)
            {
                var tempParam = param.ToList();
                foreach (var obj in tempParam.ToList())
                {
                    /* Remove duplicates within file */
                    var duplicateWithinFile = param.Where(x =>
                    obj.FirstName.Equals(x.FirstName, StringComparison.OrdinalIgnoreCase) &
                    obj.MiddleName.Equals(x.MiddleName, StringComparison.OrdinalIgnoreCase) &
                    obj.LastName.Equals(x.LastName, StringComparison.OrdinalIgnoreCase) &
                    obj.BirthDate.Equals(x.BirthDate, StringComparison.OrdinalIgnoreCase) & 
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    { 
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                        Duplicates.Add("Row [" + obj.RowNum + "]");
                    }


                    /* Remove duplicates from database */
                    DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate);
                    var duplicateFromDatabase = (await _dbAccess.GetByUnique(obj.LastName, obj.FirstName, obj.MiddleName, birthDate)).ToList();
                    if (duplicateFromDatabase != null)
                    {
                        if (duplicateFromDatabase.Count() > 0)
                        {                         
                            param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        } 
                    }


                }
            }
            
            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                List<Data.Applicant.Applicant> applicants = new List<Data.Applicant.Applicant>();

                if (param != null)
                {
                    foreach (var obj in param)
                    {
                        DateTime.TryParseExact(obj.DateApplied, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied);

                        Decimal.TryParse(obj.ExpectedSalary, NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
                            , CultureInfo.CreateSpecificCulture("en-US"), out decimal expectedSalary);

                        DateTime.TryParseExact(obj.BirthDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime birthDate);


                        applicants.Add(new Data.Applicant.Applicant
                        {
                            PositionRemarks = obj.DesiredPosition,
                            DateApplied = dateApplied,
                            ApplicationSource = obj.ApplicationSource,
                            ExpectedSalary = expectedSalary,
                            LastName = obj.LastName,
                            FirstName = obj.FirstName,
                            MiddleName = obj.MiddleName,
                            Suffix = obj.Suffix,
                            BirthDate = birthDate,
                            AddressLine1 = obj.AddressLine1,
                            AddressLine2 = obj.AddressLine2,
                            PSGCRegionCode = obj.PSGCRegionCode,
                            PSGCProvinceCode = obj.PSGCProvinceCode,
                            PSGCCityMunicipalityCode = obj.PSGCCityMunicipalityCode,
                            PSGCBarangayCode = obj.PSGCBarangayCode,
                            CurrentPosition = obj.CurrentPosition,
                            Course = obj.Course,
                            Email = obj.EmailAddress,
                            CellphoneNumber = obj.CellphoneNumber,
                            ReferredByUserID = obj.ReferredByID.HasValue ? obj.ReferredByID.Value : 0,
                            IsActive = true,
                            CreatedBy = credentials.UserID

                        });

                    }

                    await _dbAccess.UploadInsert(applicants); 
                }

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
            {
                if (Duplicates.Count > 0)
                {
                    return new OkObjectResult(
                        string.Concat(param?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                            MessageUtilities.ERRMSG_DUPLICATE_APPLICANT, "<br>",
                            string.Join("<br>", Duplicates.Distinct().ToArray()))
                        );
                }
                else
                { 
                    return new OkObjectResult(string.Concat(param?.Count," Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD));
                }
            } 
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> UpdateEmployeeID(APICredentials credentials, UpdateEmployeeIDInput param)
        {
            Data.Applicant.Applicant form = await _dbAccess.GetByID(param.ApplicantID);

            form.EmployeeID = param.EmployeeID;
            form.ModifiedBy = credentials.UserID;
            form.ModifiedDate = DateTime.Now;

            if (await _dbAccess.Put(form))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        // GET THE LAST INSERTED DATA
        public async Task<IActionResult> GetLastApplicant(string FirstName, string LastName,string Birthday,string Email)
        {
            var Result = await _dbAccess.GetLastApplicant(FirstName, LastName,Birthday,Email);

            if (Result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                LastId lastId = new LastId()
                {
                        Id = Result.ID
                };
            return new OkObjectResult(lastId);
            }
        }
        public async Task<IActionResult> GetApplicantLegalProfile(int ApplicantId)
        {
            return new OkObjectResult(await _dbAccess.GetApplicantLegalProfile(ApplicantId));
        }
        public async Task<IActionResult> PostApplicantLegalProfile(APICredentials credentials, ApplicantLegalProfileInput param)
        {

            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.addApplicantLegalProfileInputs?.Count > 0)
            {
                foreach (var item in param.addApplicantLegalProfileInputs)
                {
                    if (string.IsNullOrEmpty(item.LegalNumber))
                        ErrorMessages.Add(string.Concat("Legal Number ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    if (string.IsNullOrEmpty(item.LegalAnswer))
                        ErrorMessages.Add(string.Concat("Legal Answer ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }
            else
            {
                ErrorMessages.Add(string.Concat("Legal Profile ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            if (ErrorMessages.Count == 0)
            {
                List<ApplicantLegalProfile> paramAttachment =
                    param.addApplicantLegalProfileInputs.Select(x => new ApplicantLegalProfile
                    {
                        ApplicationId = param.ApplicantID,
                        LegalNumber = x.LegalNumber,
                        LegalAnswer = x.LegalAnswer,
                        CreatedBy = 1
                    }).ToList();

                await _dbAccess.PostApplicantLegalProfile(paramAttachment);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
    }
}