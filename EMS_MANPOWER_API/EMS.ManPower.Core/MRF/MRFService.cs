using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Core.Reference;
using EMS.Manpower.Data.DataDuplication.OrgGroup;
using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DataDuplication.PositionLevel;
using EMS.Manpower.Data.DataDuplication.Region;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.MRF;
using EMS.Manpower.Data.Reference;
using EMS.Manpower.Transfer;
using EMS.Manpower.Transfer.MRF;
using EMS.Manpower.Transfer.MRFApproval;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.Core.MRF
{
    public interface IMRFService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetApprovalList(APICredentials credentials, GetApprovalListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, Transfer.MRF.Form param);

        Task<IActionResult> Put(APICredentials credentials, Transfer.MRF.Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> Cancel(APICredentials credentials, int ID);

        Task<IActionResult> AddMRFApprovalHistory(APICredentials credentials, ApproverResponse param);

        Task<IActionResult> GetByIDApproval(APICredentials credentials, int ID);

        Task<IActionResult> GetApplicantByMRFID(APICredentials credentials, int MRFID);

        Task<IActionResult> RemoveApplicant(APICredentials credentials, RemoveApplicantInput param);

        Task<IActionResult> AddApplicant(APICredentials credentials, MRFPickApplicantForm param);

        Task<IActionResult> ValidateMRFExistingActual(APICredentials credentials, ValidateMRFExistingActualInput param);

        Task<IActionResult> UpdateForHiring(APICredentials credentials, UpdateForHiringApplicantInput param);

        Task<IActionResult> UpdateStatus(APICredentials credentials, UpdateStatusInput param);

        Task<IActionResult> GetApprovalHistory(APICredentials credentials, MRFApprovalHistoryForm param);

        Task<IActionResult> PostComments(APICredentials credentials, Transfer.MRF.MRFCommentsForm param);

        Task<IActionResult> GetComments(APICredentials credentials, int GetComments);
        
        Task<IActionResult> PostApplicantComments(APICredentials credentials, Transfer.MRF.MRFApplicantCommentsForm param);

        Task<IActionResult> GetApplicantComments(APICredentials credentials, int GetComments);

        Task<IActionResult> ValidateApplicantIsTagged(APICredentials credentials, int ApplicantID);

        Task<IActionResult> HRCancelMRF(APICredentials credentials, MRFCancelForm param);

        Task<IActionResult> UpdateCurrentWorkflowStep(APICredentials credentials, UpdateCurrentWorkflowStepInput param);

        Task<IActionResult> GetMRFExistingApplicantList(APICredentials credentials, GetMRFExistingApplicantListInput input);

        Task<IActionResult> GetMRFIDDropdownByApplicantID(APICredentials credentials, int ApplicantID);

        Task<IActionResult> GetApplicantByMRFIDAndID(APICredentials credentials, GetApplicantByMRFIDAndIDInput param);

        Task<IActionResult> GetByMRFTransactionID(APICredentials credentials, string MRFTransactionID);

        Task<IActionResult> Revise(APICredentials credentials, Transfer.MRF.Form param);

        //API FOR GET ONLINE MRF
        Task<IActionResult> GetListMrfOnline();
        Task<IActionResult> GetLastApplicant(string FirstName, string LastName);
        Task<IActionResult> GetKickoutQuestion(string PositionID);
        Task<IActionResult> PostApplicantKickoutQuestion(APICredentials credentials, ApplicantKickoutQuestionInput param);
        Task<IActionResult> PostKickoutQuestion(APICredentials credentials, AddKickoutQuestionInput param);
        Task<IActionResult> GetKickoutQuestionList(APICredentials credentials, AddKickoutQuestionInput param);
        Task<IActionResult> GetKickoutQuestionByID(APICredentials credentials, int ID);
        Task<IActionResult> EditKickoutQuestion(APICredentials credentials, AddKickoutQuestionInput param);
        Task<IActionResult> AddKickoutQuestionToMRF(APICredentials credentials, AddKickoutQuestionToMRFInput param);
        Task<IActionResult> GetMRFKickoutQuestionByMRFID(APICredentials credentials, int ID);
        Task<IActionResult> GetMRFKickoutQuestionByID(APICredentials credentials, int ID);
        Task<IActionResult> EditKickoutQuestionToMRF(APICredentials credentials, AddKickoutQuestionToMRFInput param);
        Task<IActionResult> RemoveKickoutQuestionToMRF(APICredentials credentials, List<int> IDs);
        Task<IActionResult> GetKickoutQuestionAutoComplete(APICredentials credentials, GetByKickoutQuestionAutoCompleteInput param);
        Task<IActionResult> GetMRFAutoCancelled(APICredentials credentials);
        Task<IActionResult> GetMRFAutoCancelledReminder(APICredentials credentials);
        Task<IActionResult> MRFChangeStatus(APICredentials credentials, MRFChangeStatusInput param);
    }

    public class MRFService : EMS.Manpower.Core.Shared.Utilities, IMRFService
    {
        private readonly IMRFDBAccess _dbAccess;
        private readonly IPositionService _servicePosition;
        private readonly IPositionLevelService _servicePositionLevel;
        private readonly IOrgGroupService _serviceOrgGroup;
        private readonly IRegionService _serviceRegion;
        private readonly IReferenceDBAccess _referenceDBAccess;

        public MRFService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IMRFDBAccess dbAccess,
            IPositionService servicePosition,
            IPositionLevelService servicePositionLevel,
            IOrgGroupService serviceOrgGroup,
            IRegionService serviceRegion,
            IReferenceDBAccess referenceDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _servicePosition = servicePosition;
            _servicePositionLevel = servicePositionLevel;
            _serviceOrgGroup = serviceOrgGroup;
            _serviceRegion = serviceRegion;
            _referenceDBAccess = referenceDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMRF> result = await _dbAccess.GetList(input, credentials.UserID, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.MRF.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                MRFID = x.MRFTransactionID,
                OrgGroupDescription = x.OrgGroupDescription,
                ScopeOrgGroup = x.ScopeOrgGroup,
                PositionLevelDescription = x.PositionLevelDescription,
                PositionDescription = x.PositionDescription,
                NatureOfEmployment = x.NatureOfEmployment,
                Purpose = x.Purpose,
                NoOfApplicant = x.NoOfApplicant,
                Status = x.Status,
                ApprovedDate = x.ApprovedDate,
                HiredDate = x.HiredDate,
                Age = x.Age,
                IsApproved = x.IsApproved,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate
            }).ToList());
        }

        public async Task<IActionResult> GetApprovalList(APICredentials credentials, GetApprovalListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMRF> result = await _dbAccess.GetApprovalList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.MRFApproval.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                MRFID = x.MRFTransactionID,
                OrgGroupDescription = x.OrgGroupDescription,
                PositionLevelDescription = x.PositionLevelDescription,
                PositionDescription = x.PositionDescription,
                NatureOfEmployment = x.NatureOfEmployment,
                NoOfApplicant = x.NoOfApplicant,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                ApprovedDate = x.ApprovedDate,
                Age = x.Age
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            EMS.Manpower.Data.MRF.MRF mrf = (await _dbAccess.GetByID(ID));

            if (mrf == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                Position position = (await _servicePosition.GetBySyncIDs(new List<int> { mrf.PositionID })).FirstOrDefault();
                if (position == null)
                    return new BadRequestObjectResult("Position " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS);

                string Status = (await _referenceDBAccess.GetByRefCodeValue(Enums.MRFReference.MRF_STATUS.ToString(), mrf.Status)).Description;

                return new OkObjectResult(
                        new Transfer.MRF.Form
                        {
                            ID = mrf.ID,
                            Status = Status,
                            StatusCode = mrf.Status,
                            MRFTransactionID = mrf.MRFTransactionID,
                            OldMRFID = mrf.OldMRFID,
                            OrgGroupID = mrf.OrgGroupID,
                            PositionLevelID = position.PositionLevelID,
                            NatureOfEmploymentValue = mrf.NatureOfEmployment,
                            PositionID = mrf.PositionID,
                            IsConfidential = mrf.IsConfidential,
                            PurposeValue = mrf.Purpose,
                            Vacancy = mrf.Vacancy,
                            TurnaroundTime = mrf.TurnaroundTime,
                            Remarks = mrf.Remarks,
                            CreatedBy = mrf.CreatedBy,
                            DateCreated = mrf.CreatedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            DateModified = mrf.ModifiedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            DateApproved = mrf.ApprovedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            ReasonForCancellation = mrf.ReasonForCancellation,
                            ApproverPositionID = mrf.ApproverPositionID,
                            ApproverOrgGroupID = mrf.ApproverOrgGroupID,
                            AltApproverPositionID = mrf.AltApproverPositionID,
                            AltApproverOrgGroupID = mrf.AltApproverOrgGroupID,
                            OnlinePosition = mrf.OnlinePosition,
                            OnlineLocation = mrf.OnlineLocation,
                            OnlineJobDescription = mrf.OnlineJobDescription,
                            OnlineJobQualification = mrf.OnlineJobQualification,
                            IsAvailableOnline = mrf.IsAvailableOnline
                        }
                    );
            }
        }

        public async Task<IActionResult> GetByIDApproval(APICredentials credentials, int ID)
        {
            EMS.Manpower.Data.MRF.MRF mrf = (await _dbAccess.GetByID(ID));

            if (mrf == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                OrgGroup orgGroup = (await _serviceOrgGroup.GetBySyncIDs(new List<int> { mrf.OrgGroupID })).FirstOrDefault();
                //Region region = (await _serviceRegion.GetBySyncIDs(new List<int> { orgGroup.RegionID })).FirstOrDefault();
                Position position = (await _servicePosition.GetBySyncIDs(new List<int> { mrf.PositionID })).FirstOrDefault();
                PositionLevel positionLevel = (await _servicePositionLevel.GetBySyncIDs(new List<int> { position == null ? 0 : position.PositionLevelID })).FirstOrDefault();

                if (position == null)
                    return new BadRequestObjectResult("Position " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS);

                string Status = (await _referenceDBAccess.GetByRefCodeValue(Enums.MRFReference.MRF_STATUS.ToString(), mrf.Status)).Description;

                return new OkObjectResult(
                        new Transfer.MRFApproval.Form
                        {
                            ID = mrf.ID,
                            AgeOfRequest = Convert.ToInt32((DateTime.Now - mrf.CreatedDate.Value).TotalDays),
                            DateOfRequest = mrf.CreatedDate.Value.ToString(),
                            Status = Status,
                            MRFTransactionID = mrf.MRFTransactionID,
                            OldMRFID = mrf.OldMRFID,
                            OrgGroupID = mrf.OrgGroupID,
                            PositionLevelID = position.PositionLevelID,
                            NatureOfEmploymentValue = mrf.NatureOfEmployment,
                            PositionID = mrf.PositionID,
                            PurposeValue = mrf.Purpose,
                            Vacancy = mrf.Vacancy,
                            TurnaroundTime = mrf.TurnaroundTime,
                            Remarks = mrf.Remarks,
                            CreatedBy = mrf.CreatedBy,
                            ApproverPositionID = mrf.ApproverPositionID,
                            ApproverOrgGroupID = mrf.ApproverOrgGroupID,
                            AltApproverPositionID = mrf.AltApproverPositionID,
                            AltApproverOrgGroupID = mrf.AltApproverOrgGroupID
                        }
                    );
            }
        }

        public async Task<IActionResult> GetNewTransactionID()
        {
            return new OkObjectResult(await _dbAccess.GetNewTransactionID());
        }

        public async Task<IActionResult> Post(APICredentials credentials, Transfer.MRF.Form param)
        {
            string TransID = "";
            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (param.OrgGroupID <= 0)
                ErrorMessages.Add(string.Concat("Organizational Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.PositionLevelID <= 0)
            //    ErrorMessages.Add(string.Concat("Position Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.NatureOfEmploymentValue = param.NatureOfEmploymentValue.Trim();
            if (string.IsNullOrEmpty(param.NatureOfEmploymentValue))
                ErrorMessages.Add(string.Concat("Nature of Employment ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.PositionID <= 0)
                ErrorMessages.Add(string.Concat("Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverPositionID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver Position ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverOrgGroupID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver OrgGroup ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.PurposeValue = param.PurposeValue.Trim();
            if (string.IsNullOrEmpty(param.PurposeValue))
                ErrorMessages.Add(string.Concat("Purpose ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.Vacancy <= 0)
                ErrorMessages.Add(string.Concat("Vacancy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.TurnaroundTime <= 0)
            //    ErrorMessages.Add(string.Concat("Turnaround Time ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Remarks = param.Remarks.Trim();
            if (string.IsNullOrEmpty(param.Remarks))
                ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                var maxLevel = (await _referenceDBAccess
                    .GetByRefCodes(new List<string> { Enums.MRFReference.SETUP_MRF_APP_LEVEL.ToString() })).ToList();

                TransID = (await _dbAccess.GetNewTransactionID()).TransID;

                var approvalHistory = await _dbAccess.GetApprovalHistory(new MRFApprovalHistoryForm {
                    RequestingOrgGroupID = param.OrgGroupID,
                    RequestingPositionID = param.PositionID
                });

                await _dbAccess.Post(new Data.MRF.MRF
                {
                    Status = Enums.MRF_STATUS.FOR_APPROVAL.ToString(),
                    MRFTransactionID = TransID,
                    OldMRFID = param.OldMRFID,
                    OrgGroupID = param.OrgGroupID,
                    PositionID = param.PositionID,
                    IsConfidential = param.IsConfidential,
                    NatureOfEmployment = param.NatureOfEmploymentValue,
                    Purpose = param.PurposeValue,
                    Vacancy = param.Vacancy,
                    TurnaroundTime = param.TurnaroundTime,
                    IsActive = true,
                    Remarks = param.Remarks,
                    CreatedBy = param.CreatedBy,
                    ApproverPositionID = approvalHistory != null ? approvalHistory.Count() > 0 ? approvalHistory.First().PositionID : 0 : 0,
                    ApproverOrgGroupID = approvalHistory != null ? approvalHistory.Count() > 0 ? approvalHistory.First().OrgGroupID : 0 : 0,
                    AltApproverPositionID = approvalHistory != null ? approvalHistory.Count() > 0 ? approvalHistory.First().AltPositionID : 0 : 0,
                    AltApproverOrgGroupID = approvalHistory != null ? approvalHistory.Count() > 0 ? approvalHistory.First().AltOrgGroupID : 0 : 0,
                    ApprovalLevel = 1, // default 1
                    MaxApprovalLevel = maxLevel != null ? maxLevel.Count > 0 ?
                    Convert.ToInt32(maxLevel.First().Value) : 0 : 0,
                    OnlinePosition = param.OnlinePosition,
                    OnlineLocation = param.OnlineLocation,
                    OnlineJobDescription = param.OnlineJobDescription,
                    OnlineJobQualification = param.OnlineJobQualification,
                    IsAvailableOnline = param.IsAvailableOnline
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(
                    string.Concat(MessageUtilities.SCSSMSG_REC_SAVE,
                    Environment.NewLine,
                    "MRF ID: ", TransID)
                    );
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Put(APICredentials credentials, Transfer.MRF.Form param)
        {
            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            if (param.OrgGroupID <= 0)
                ErrorMessages.Add(string.Concat("Organizational Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.PositionLevelID <= 0)
            //    ErrorMessages.Add(string.Concat("Position Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.NatureOfEmploymentValue = param.NatureOfEmploymentValue.Trim();
            if (string.IsNullOrEmpty(param.NatureOfEmploymentValue))
                ErrorMessages.Add(string.Concat("Nature of Employment ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.PositionID <= 0)
                ErrorMessages.Add(string.Concat("Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverPositionID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver Position ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverOrgGroupID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver OrgGroup ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            param.PurposeValue = param.PurposeValue.Trim();
            if (string.IsNullOrEmpty(param.PurposeValue))
                ErrorMessages.Add(string.Concat("Purpose ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.Vacancy <= 0)
                ErrorMessages.Add(string.Concat("Vacancy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.TurnaroundTime <= 0)
            //    ErrorMessages.Add(string.Concat("Turnaround Time ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Remarks = param.Remarks.Trim();
            if (string.IsNullOrEmpty(param.Remarks))
                ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                Data.MRF.MRF form = await _dbAccess.GetByID(param.ID);

                form.OldMRFID = param.OldMRFID;
                form.OrgGroupID = param.OrgGroupID;
                form.PositionID = param.PositionID;
                form.IsConfidential = param.IsConfidential;
                form.NatureOfEmployment = param.NatureOfEmploymentValue;
                form.Purpose = param.PurposeValue;
                form.ApproverPositionID = param.ApproverPositionID;
                form.ApproverOrgGroupID = param.ApproverOrgGroupID;
                form.AltApproverPositionID = param.AltApproverPositionID;
                form.AltApproverOrgGroupID = param.AltApproverOrgGroupID;
                form.Vacancy = param.Vacancy;
                form.Remarks = param.Remarks;
                form.TurnaroundTime = param.TurnaroundTime;
                form.ModifiedBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;
                form.OnlinePosition = param.OnlinePosition;
                form.OnlineLocation = param.OnlineLocation;
                form.OnlineJobDescription = param.OnlineJobDescription;
                form.OnlineJobQualification = param.OnlineJobQualification;
                form.IsAvailableOnline = param.IsAvailableOnline;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            EMS.Manpower.Data.MRF.MRF mrf = await _dbAccess.GetByID(ID);
            mrf.IsActive = false;
            mrf.ModifiedBy = credentials.UserID;
            mrf.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(mrf))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> Cancel(APICredentials credentials, int ID)
        {
            EMS.Manpower.Data.MRF.MRF mrf = await _dbAccess.GetByID(ID);
            mrf.Status = SharedUtilities.MRF_STATUS.CANCELLED.ToString();
            mrf.ModifiedBy = credentials.UserID;
            mrf.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(mrf))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> AddMRFApprovalHistory(APICredentials credentials, ApproverResponse param)
        {
            string successMessage = param.Result == Enums.MRF_APPROVER_STATUS.APPROVED ?
                MessageUtilities.SCSSMSG_REC_APPROVE :
                param.Result == Enums.MRF_APPROVER_STATUS.REJECTED ?
                MessageUtilities.SCSSMSG_REC_REJECT : "";

            await _dbAccess.AddMRFApprovalHistory(credentials.UserID, param);
            return new OkObjectResult(successMessage);
        }

        public async Task<IActionResult> GetApplicantByMRFID(APICredentials credentials, int MRFID)
        {
            List<MRFApplicant> applicants = (await _dbAccess.GetApplicantByMRFIDAndStatus(MRFID, true)).ToList();
            EMS.Manpower.Data.MRF.MRF mrf = (await _dbAccess.GetByID(MRFID));

            OrgGroup orgGroup = (await _serviceOrgGroup.GetBySyncIDs(new List<int> { mrf.OrgGroupID })).FirstOrDefault();
            //Region region = (await _serviceRegion.GetBySyncIDs(new List<int> { orgGroup.RegionID })).FirstOrDefault();
            Position position = (await _servicePosition.GetBySyncIDs(new List<int> { mrf.PositionID })).FirstOrDefault();
            PositionLevel positionLevel = (await _servicePositionLevel.GetBySyncIDs(new List<int> { position == null ? 0 : position.PositionLevelID })).FirstOrDefault();
            string natureOfEmployment = (await _referenceDBAccess.GetByRefCodeValue(Enums.MRFReference.NATURE_OF_EMPLOYMENT.ToString(), mrf.NatureOfEmployment)).Description;
            string Status = (await _referenceDBAccess.GetByRefCodeValue(Enums.MRFReference.MRF_STATUS.ToString(), mrf.Status)).Description;
            string Purpose = (await _referenceDBAccess.GetByRefCodeValue(Enums.MRFReference.MRF_PURPOSE.ToString(), mrf.Purpose)).Description;

            return new OkObjectResult(
                new MRFApplicantForm
                {
                    ID = MRFID,
                    MRFTransactionID = mrf.MRFTransactionID,
                    OrganizationalGroup = string.Concat(orgGroup.Code, " - ", orgGroup.Description),
                    Position = string.Concat(position.Code, " - ", position.Title),
                    PositionLevel = positionLevel.Description,
                    NatureOfEmployment = natureOfEmployment,
                    Count = mrf.Vacancy,
                    Purpose = Purpose,
                    DateCreated = mrf.CreatedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    DateModified = mrf.ModifiedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    DateApproved = mrf.ApprovedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                    Remarks = mrf.Remarks,
                    PositionID = mrf.PositionID,
                    OrgGroupID = mrf.OrgGroupID,
                    Status = Status,
                    StatusCode = mrf.Status,
                    IsConfidential = mrf.IsConfidential ? "Yes" : "No",
                    ApplicantIDs = applicants.Select(x => new ApplicantIDsAndForHiring {
                        ForHiring = x.ForHiring,
                        ApplicantID = x.ApplicantID,
                        MRFApplicantID = x.ID
                    }).ToList(),
                    SubmittedByID = mrf.CreatedBy,
                    PrintDateCreated = (mrf.CreatedDate?.ToString("ddd, MM/dd/yyyy hh:mm:ss tt")).ToUpper(),

                });
        }

        public async Task<IActionResult> RemoveApplicant(APICredentials credentials, RemoveApplicantInput param)
        {
            MRFApplicant applicant = await _dbAccess.GetApplicantByMRFIDAndID(param.MRFID, param.MRFApplicantID);
            applicant.IsActive = false;
            applicant.CreatedBy = applicant.CreatedBy;
            applicant.CreatedDate = applicant.CreatedDate;
            applicant.ModifiedBy = credentials.UserID;
            applicant.ModifiedDate = DateTime.Now;
            if (await _dbAccess.UpdateStatusApplicant(new List<MRFApplicant> { applicant }))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> AddApplicant(APICredentials credentials, MRFPickApplicantForm param)
        {
            if (param.ID <= 0)
                return new BadRequestObjectResult(string.Concat("MRF ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.WorkflowID <= 0)
                return new BadRequestObjectResult(string.Concat("WorkflowID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.Applicants?.Count <= 0)
                return new BadRequestObjectResult(string.Concat("Applicant ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            // Convert form class into MRFApplicant DB Context class
            List<MRFApplicant> newSet = param.Applicants.Select(x => new MRFApplicant
            {
                MRFID = param.ID,
                WorkflowID = param.WorkflowID,
                WorkflowStatus = Enums.WorkflowStatus.IN_PROGRESS.ToString(),
                ApplicantID = x.ApplicantID,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Suffix = x.Suffix,
                CurrentStepCode = x.CurrentStepCode,
                CurrentStepDescription = x.CurrentStepDescription,
                CurrentStepApproverRoleIDs = x.CurrentStepApproverRoleIDs,
                ResultType = x.ResultType,
                IsActive = true,
                CreatedBy = credentials.UserID
            }).ToList();

            // Get all existing applicants with IsActive = false
            List<MRFApplicant> oldSetInActive = (await _dbAccess.GetApplicantByMRFIDAndStatus(param.ID, false)).ToList();
            List<MRFApplicant> oldSetActive = (await _dbAccess.GetApplicantByMRFIDAndStatus(param.ID, true)).ToList();

            // Get all inactive records which are readded 
            List<MRFApplicant> setToActive = oldSetInActive
                .Join(newSet, x => new { x.MRFID, x.ApplicantID },
                y => new { y.MRFID, y.ApplicantID },
                (x, y) => new { oldSet = x, newSet = y })
                .Select(x => new MRFApplicant {
                    ID = x.oldSet.ID,
                    MRFID = x.newSet.MRFID,
                    WorkflowID = x.newSet.WorkflowID,
                    WorkflowStatus = Enums.WorkflowStatus.IN_PROGRESS.ToString(),
                    ApplicantID = x.newSet.ApplicantID,
                    FirstName = x.newSet.FirstName,
                    MiddleName = x.newSet.MiddleName,
                    LastName = x.newSet.LastName,
                    Suffix = x.newSet.Suffix,
                    CurrentStepCode = x.newSet.CurrentStepCode,
                    CurrentStepDescription = x.newSet.CurrentStepDescription,
                    CurrentStepApproverRoleIDs = x.newSet.CurrentStepApproverRoleIDs,
                    ResultType = x.newSet.ResultType,
                    IsActive = true,
                    CreatedBy = x.newSet.CreatedBy,
                    CreatedDate = x.newSet.CreatedDate,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now
                }).ToList();

            // Update readded records into IsActive = true
            if (!await _dbAccess.UpdateStatusApplicant(setToActive))
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_SAVE);

            static List<MRFApplicant> GetToAdd(List<MRFApplicant> left, List<MRFApplicant> right, int createdBy)
            {
                return right.GroupJoin(
                  left,
                       x => new { x.MRFID, x.ApplicantID },
                       y => new { y.MRFID, y.ApplicantID },
                  (x, y) => new { newSet = x, oldSet = y })
                  .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                  (x, y) => new { newSet = x, oldSet = y })
                  .Where(x => x.oldSet == null)
                  .Select(x =>
                      new MRFApplicant
                      {
                          MRFID = x.newSet.newSet.MRFID,
                          WorkflowID = x.newSet.newSet.WorkflowID,
                          WorkflowStatus = Enums.WorkflowStatus.IN_PROGRESS.ToString(),
                          ApplicantID = x.newSet.newSet.ApplicantID,
                          FirstName = x.newSet.newSet.FirstName,
                          MiddleName = x.newSet.newSet.MiddleName,
                          LastName = x.newSet.newSet.LastName,
                          Suffix = x.newSet.newSet.Suffix,
                          CurrentStepCode = x.newSet.newSet.CurrentStepCode,
                          CurrentStepDescription = x.newSet.newSet.CurrentStepDescription,
                          CurrentStepApproverRoleIDs = x.newSet.newSet.CurrentStepApproverRoleIDs,
                          ResultType = x.newSet.newSet.ResultType,
                          IsActive = true,
                          CreatedBy = createdBy
                      }).ToList();
            }

            // Get records to be added
            List<MRFApplicant> applicants = new List<MRFApplicant>();
            List<MRFApplicant> toAdd = GetToAdd(oldSetInActive, GetToAdd(oldSetActive, newSet, credentials.UserID), credentials.UserID);

            if (toAdd != null)
            {
                await _dbAccess.AddApplicant(toAdd);
                applicants.AddRange(toAdd);
            }

            if (setToActive != null)
            {
                applicants.AddRange(setToActive);
            }

            if (applicants != null)
            {
                return new OkObjectResult(applicants.Select(x => new Transfer.MRF.ApplicantIDsAndForHiring
                {
                    MRFApplicantID = Convert.ToInt32(x.ID),
                    ApplicantID = x.ApplicantID
                }).ToList());
            }
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_SAVE);
        }

        public async Task<IActionResult> ValidateMRFExistingActual(APICredentials credentials, ValidateMRFExistingActualInput param)
        {
            List<Data.MRF.MRF> result =
                (await _dbAccess.GetByOrgGroupPositionStatus(param.OrgGroupID, param.PositionID, Enums.MRF_STATUS.OPEN.ToString())).ToList();

            if ((param.PlannedCount - param.ActiveCount > 0)
                & param.PlannedCount - param.ActiveCount > result?.Count)
                return new OkObjectResult(true);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_MRF_REQUEST_EXCEED);
        }

        public async Task<IActionResult> UpdateForHiring(APICredentials credentials, UpdateForHiringApplicantInput param)
        {
            // For hiring
            MRFApplicant applicantForHiring = await _dbAccess.GetApplicantByMRFIDAndID(param.MRFID, param.MRFApplicantID);
            applicantForHiring.ForHiring = param.ForHiring;
            applicantForHiring.ModifiedBy = credentials.UserID;
            applicantForHiring.ModifiedDate = DateTime.Now;

            // Not for hiring
            List<MRFApplicant> applicantNotForHiring = (await _dbAccess.GetApplicantByMRFIDAndStatus(param.MRFID, true)).ToList();
            List<MRFApplicant> applicants = applicantNotForHiring
                .Where(x => x.ApplicantID != param.MRFApplicantID)
                .Select(x => { x.ForHiring = false; return x; }).ToList();
            applicants.Add(applicantForHiring);

            if (await _dbAccess.UpdateStatusApplicant(applicants))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> UpdateStatus(APICredentials credentials, UpdateStatusInput param)
        {
            Data.MRF.MRF form = await _dbAccess.GetByID(param.ID);

            form.Status = param.Status.ToString();
            form.ModifiedBy = credentials.UserID;
            form.ModifiedDate = DateTime.Now;
            if (await _dbAccess.Put(form))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetApprovalHistory(APICredentials credentials, MRFApprovalHistoryForm param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApprovalHistory(param))
                .Select(x => new MRFApprovalHistoryOutput
                {
                    HierarchyLevel = x.HierarchyLevel,
                    PositionID = x.PositionID,
                    PositionCode = x.PositionCode,
                    OrgGroupID = x.OrgGroupID,
                    OrgGroupCode = x.OrgGroupCode,
                    AltPositionID = x.AltPositionID,
                    AltOrgGroupID = x.AltOrgGroupID,
                    ApprovalStatus = x.ApprovalStatus,
                    ApprovalStatusCode = x.ApprovalStatusCode,
                    ApprovedDate = x.ApprovedDate,
                    ApproverID = x.ApproverID,
                    ApprovalRemarks = x.ApprovalRemarks
                }));
        }

        public async Task<IActionResult> PostComments(APICredentials credentials, Transfer.MRF.MRFCommentsForm param)
        {

            if (param.MRFID <= 0)
                ErrorMessages.Add(string.Concat("MRFID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Comments = param.Comments.Trim();
            if (string.IsNullOrEmpty(param.Comments))
                ErrorMessages.Add(string.Concat("Comments ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Comments.Length > 500)
                ErrorMessages.Add(string.Concat("Comments", MessageUtilities.COMPARE_NOT_EXCEED, "500 characters."));

            if (param.CreatedBy <= 0)
                ErrorMessages.Add(string.Concat("CreatedBy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostComments(new Data.MRF.MRFComments
                {
                    MRFID = param.MRFID,
                    Comments = param.Comments,
                    CreatedBy = param.CreatedBy
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetComments(APICredentials credentials, int GetComments)
        {
            return new OkObjectResult(
                (await _dbAccess.GetComments(GetComments))
                .Select(x => new MRFGetCommentsOutput
                {
                    Timestamp = x.CreatedDate.ToString("yyyy-MM-dd (hh:mm:ss tt)"),
                    Comments = x.Comments,
                    CreatedBy = x.CreatedBy
                }));
        }

        public async Task<IActionResult> PostApplicantComments(APICredentials credentials, Transfer.MRF.MRFApplicantCommentsForm param)
        {

            if (param.MRFID <= 0)
                ErrorMessages.Add(string.Concat("MRFID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Comments = param.Comments.Trim();
            if (string.IsNullOrEmpty(param.Comments))
                ErrorMessages.Add(string.Concat("Comments ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Comments.Length > 500)
                ErrorMessages.Add(string.Concat("Comments", MessageUtilities.COMPARE_NOT_EXCEED, "500 characters."));

            if (param.CreatedBy <= 0)
                ErrorMessages.Add(string.Concat("CreatedBy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.PostApplicantComments(new Data.MRF.MRFApplicantComments
                {
                    MRFID = param.MRFID,
                    Comments = param.Comments,
                    CreatedBy = param.CreatedBy
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetApplicantComments(APICredentials credentials, int GetComments)
        {
            return new OkObjectResult(
                (await _dbAccess.GetApplicantComments(GetComments))
                .Select(x => new MRFGetCommentsOutput
                {
                    Timestamp = x.CreatedDate.ToString("yyyy-MM-dd (hh:mm:ss tt)"),
                    Comments = x.Comments,
                    CreatedBy = x.CreatedBy
                }));
        }

        public async Task<IActionResult> ValidateApplicantIsTagged(APICredentials credentials, int ApplicantID)
        {
            List<MRFApplicant> applicant = (await _dbAccess.GetApplicantByApplicantID(ApplicantID)).ToList();
            return new OkObjectResult(applicant.Count > 0);
        }

        public async Task<IActionResult> HRCancelMRF(APICredentials credentials, MRFCancelForm param)
        {

            param.Reason = param.Reason.Trim();
            if (string.IsNullOrEmpty(param.Reason))
                ErrorMessages.Add(string.Concat("Reason ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                Data.MRF.MRF form = await _dbAccess.GetByID(param.MRFID);

                form.Status = Enums.MRF_STATUS.HR_CANCELLED.ToString();
                form.ReasonForCancellation = param.Reason;
                form.CancelledBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SSCSSMSG_REC_CANCEL);
            else
                return new BadRequestObjectResult(MessageUtilities.PRE_ERRMSG_REC_CANCEL);
        }

        public async Task<IActionResult> UpdateCurrentWorkflowStep(APICredentials credentials, UpdateCurrentWorkflowStepInput param)
        {
            MRFApplicant mRFApplicant = (await _dbAccess.GetMRFApplicantByID(param.ID));

            if (mRFApplicant == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);

            mRFApplicant.CurrentStepCode = param.CurrentStepCode;
            mRFApplicant.CurrentStepDescription = param.CurrentStepDescription;
            mRFApplicant.WorkflowStatus = param.WorkflowStatus;
            mRFApplicant.CurrentStepApproverRoleIDs = param.CurrentStepApproverRoleIDs;
            mRFApplicant.ResultType = param.ResultType;
            mRFApplicant.DateScheduled = string.IsNullOrEmpty(param.DateScheduled) ? default :
                    DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null);
            mRFApplicant.DateCompleted = string.IsNullOrEmpty(param.DateScheduled) ? default :
                    DateTime.ParseExact(param.DateCompleted, "MM/dd/yyyy", null);
            mRFApplicant.ApproverRemarks = param.ApproverRemarks;

            if (await _dbAccess.UpdateApplicant(mRFApplicant))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new OkObjectResult(MessageUtilities.PRE_ERRMSG_REC_UPDATE);

        }

        public async Task<IActionResult> GetMRFExistingApplicantList(APICredentials credentials, GetMRFExistingApplicantListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMRFExistingApplicant> result = await _dbAccess.GetMRFExistingApplicantList(input, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.MRF.GetMRFExistingApplicantListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                MRFApplicantID = x.MRFApplicantID,
                ApplicantName = x.ApplicantName,
                CurrentStep = x.CurrentStep,
                Status = x.Status,
                WorkflowID = x.WorkflowID,
                CurrentStepCode = x.CurrentStepCode,
                CurrentResult = x.CurrentResult,
                ResultType = x.ResultType,
                DateScheduled = x.DateScheduled,
                DateCompleted = x.DateCompleted,
                ApproverRemarks = x.ApproverRemarks,
                Points = x.Points,
                TotalPoints = x.TotalPoints,
                Flag = x.Flag
            }).ToList());
        }

        public async Task<IActionResult> GetMRFIDDropdownByApplicantID(APICredentials credentials, int ApplicantID)
        {
            return new OkObjectResult(
                SharedUtilities.GetDropdown((await _dbAccess.GetMRFIDDropdownByApplicantID(ApplicantID)).ToList()
                , "CurrentStepCode", "CurrentStepDescription", null, null));
        }

        public async Task<IActionResult> GetApplicantByMRFIDAndID(APICredentials credentials, GetApplicantByMRFIDAndIDInput param)
        {
            MRFApplicant result = (await _dbAccess.GetApplicantByMRFIDAndID(param.MRFID, param.ApplicantID));

            return new OkObjectResult(
                new MRFApplicantForm {
                    ID = (int)result.ID,
                    HiredRemarks = result.ApproverRemarks,
                    CurrentStepCode = result.CurrentStepCode
                }
                );
        }

        public async Task<IActionResult> GetByMRFTransactionID(APICredentials credentials, string MRFTransactionID)
        {
            Data.MRF.MRF result = (await _dbAccess.GetByMRFTransactionID(MRFTransactionID));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {

                return new OkObjectResult(
                        new Transfer.MRF.Form
                        {
                            ID = result.ID,
                            MRFTransactionID = result.MRFTransactionID,
                            OldMRFID = result.OldMRFID,
                            OrgGroupID = result.OrgGroupID,
                            NatureOfEmploymentValue = result.NatureOfEmployment,
                            PositionID = result.PositionID,
                            IsConfidential = result.IsConfidential,
                            PurposeValue = result.Purpose,
                            Vacancy = result.Vacancy,
                            TurnaroundTime = result.TurnaroundTime,
                            Remarks = result.Remarks,
                            CreatedBy = result.CreatedBy,
                            DateCreated = result.CreatedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            DateModified = result.ModifiedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            DateApproved = result.ApprovedDate?.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            ReasonForCancellation = result.ReasonForCancellation
                        }
                    );
            }
        }

        public async Task<IActionResult> Revise(APICredentials credentials, Transfer.MRF.Form param)
        {
            if (string.IsNullOrEmpty(param.StatusCode))
                ErrorMessages.Add(string.Concat("Status Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else {

                if (!param.StatusCode.Equals(Enums.MRF_APPROVER_STATUS.REJECTED.ToString()))
                    ErrorMessages.Add(string.Concat("Status Code must be REJECTED"));
            }

            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.OrgGroupID <= 0)
                ErrorMessages.Add(string.Concat("Organizational Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.PositionLevelID <= 0)
            //    ErrorMessages.Add(string.Concat("Position Level ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.NatureOfEmploymentValue = param.NatureOfEmploymentValue.Trim();
            if (string.IsNullOrEmpty(param.NatureOfEmploymentValue))
                ErrorMessages.Add(string.Concat("Nature of Employment ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.PositionID <= 0)
                ErrorMessages.Add(string.Concat("Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverPositionID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver Position ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            //if (param.ApproverOrgGroupID <= 0)
            //    ErrorMessages.Add(string.Concat("Approver OrgGroup ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            param.PurposeValue = param.PurposeValue.Trim();
            if (string.IsNullOrEmpty(param.PurposeValue))
                ErrorMessages.Add(string.Concat("Purpose ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.Vacancy <= 0)
                ErrorMessages.Add(string.Concat("Vacancy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //if (param.TurnaroundTime <= 0)
            //    ErrorMessages.Add(string.Concat("Turnaround Time ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Remarks = param.Remarks.Trim();
            if (string.IsNullOrEmpty(param.Remarks))
                ErrorMessages.Add(string.Concat("Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                Data.MRF.MRF form = await _dbAccess.GetByID(param.ID);

                form.Status = Enums.MRF_APPROVER_STATUS.FOR_APPROVAL.ToString();
                form.OldMRFID = param.OldMRFID;
                form.OrgGroupID = param.OrgGroupID;
                form.PositionID = param.PositionID;
                form.IsConfidential = param.IsConfidential;
                form.NatureOfEmployment = param.NatureOfEmploymentValue;
                form.Purpose = param.PurposeValue;
                form.ApproverPositionID = param.ApproverPositionID;
                form.ApproverOrgGroupID = param.ApproverOrgGroupID;
                form.AltApproverPositionID = param.AltApproverPositionID;
                form.AltApproverOrgGroupID = param.AltApproverOrgGroupID;
                form.Vacancy = param.Vacancy;
                form.Remarks = param.Remarks;
                form.TurnaroundTime = param.TurnaroundTime;
                form.ModifiedBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;


            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }


        //API FOR GET ONLINE MRF
        public async Task<IActionResult> GetListMrfOnline()
        {
            IEnumerable<TableVarMRFOnline> result = await _dbAccess.GetListMrfOnline();

            return new OkObjectResult(result.Select(x => new Transfer.MRF.GetListOutputMrfOnline
            {
                PositionID = x.PositionID,
                Position = x.OnlinePosition,
                Location = x.OnlineLocation,
                JobDescription = x.OnlineJobDescription,
                JobQualification = x.OnlineJobQualification,
                MrfId = x.ID,
                ClosedDate = x.ClosedDate,
                ApplicantCount = x.ApplicantCount,
                MrfCreatedDate = x.MrfCreatedDate,
            }).ToList());
        }
        // GET THE LAST INSERTED DATA
        public async Task<IActionResult> GetLastApplicant(string FirstName, string LastName)
        {
            var Result = await _dbAccess.GetLastApplicant(FirstName, LastName);

            if (Result == null)
                return new BadRequestResult();
            else
            {
                LastId lastId = new LastId()
                {
                    Id = Result.ID
                };
                return new OkObjectResult(lastId);
            }
        }
        public async Task<IActionResult> GetKickoutQuestion(string PositionID)
        {
            List<int> ID = new List<int>();

            var Result = await _dbAccess.GetKickoutQuestion();

            foreach (var Item in Result)
            {
                foreach (var PositionItem in Item.PositionID.Split(","))
                {
                    if (PositionItem.Equals(PositionID))
                        ID.Add(Item.ID);
                }
            }

            return new OkObjectResult((await _dbAccess.GetKickoutQuestion())
                .Where(x => ID.Contains(x.ID) || x.PositionID.Equals("0"))
                .OrderBy(x => x.Order));
        }
        public async Task<IActionResult> PostApplicantKickoutQuestion(APICredentials credentials, ApplicantKickoutQuestionInput param)
        {

            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("ApplicantID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.addApplicantKickoutQuestionInputs?.Count > 0)
            {
                foreach (var item in param.addApplicantKickoutQuestionInputs)
                {
                    if (string.IsNullOrEmpty(item.Answer))
                        ErrorMessages.Add(string.Concat("Answer ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
            }
            else
            {
                ErrorMessages.Add(string.Concat("Kickout Question ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            if (ErrorMessages.Count == 0)
            {
                List<ApplicantKickoutQuestion> paramAttachment =
                    param.addApplicantKickoutQuestionInputs.Select(x => new ApplicantKickoutQuestion
                    {
                        ApplicationId = param.ApplicantID,
                        QuestionID = x.Question,
                        Answer = x.Answer.ToUpper(),
                        IsActive = true,
                        CreatedBy = 1
                    }).ToList();

                await _dbAccess.PostApplicantKickoutQuestion(paramAttachment);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> PostKickoutQuestion(APICredentials credentials, AddKickoutQuestionInput param)
        {
            EMS.Manpower.Data.MRF.KickoutQuestion kickoutQuestion = new KickoutQuestion();
            kickoutQuestion.Question = param.Question;
            kickoutQuestion.Answer = param.Answer;
            kickoutQuestion.QuestionType = param.QuestionType;
            kickoutQuestion.IsActive = true;
            kickoutQuestion.CreatedBy = credentials.UserID;
            kickoutQuestion.CreatedDate = DateTime.Now;
            var Result = (await _dbAccess.PostKickoutQuestion(kickoutQuestion));

            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> GetKickoutQuestionList(APICredentials credentials, AddKickoutQuestionInput param)
        {
            return new OkObjectResult("true");
        }
        public async Task<IActionResult> GetKickoutQuestionByID(APICredentials credentials, int ID)
        {
            return new OkObjectResult((await _dbAccess.GetKickoutQuestionByID(ID)));
        }
        public async Task<IActionResult> EditKickoutQuestion(APICredentials credentials, AddKickoutQuestionInput param)
        {
            var KickoutQuestion = (await _dbAccess.GetKickoutQuestionByID(param.ID));
            KickoutQuestion.Question = param.Question;
            KickoutQuestion.Answer = param.Answer;
            KickoutQuestion.QuestionType = param.QuestionType;
            KickoutQuestion.ModifiedBy = credentials.UserID;
            KickoutQuestion.ModifiedDate = DateTime.Now;

            var Result = (await _dbAccess.EditKickoutQuestion(KickoutQuestion));

            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> AddKickoutQuestionToMRF(APICredentials credentials, AddKickoutQuestionToMRFInput param)
        {
            EMS.Manpower.Data.MRF.MRFKickoutQuestion mRFkickoutQuestion = new MRFKickoutQuestion();
            mRFkickoutQuestion.MRFID = param.MRFID;
            mRFkickoutQuestion.KickoutQuestionID = param.KickoutQuestionID;
            mRFkickoutQuestion.Order = param.Order;
            mRFkickoutQuestion.IsActive = true;
            mRFkickoutQuestion.CreatedBy = credentials.UserID;
            mRFkickoutQuestion.CreatedDate = DateTime.Now;
            var Result = (await _dbAccess.AddKickoutQuestionToMRF(mRFkickoutQuestion));

            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_SAVE);
        }
        public async Task<IActionResult> GetMRFKickoutQuestionByMRFID(APICredentials credentials, int ID)
        {
            var GetKickoutQuestionByMRFID = (await _dbAccess.GetMRFKickoutQuestionByMRFID(ID));
            var GetKickoutQuestionByID = (await _dbAccess.GetKickoutQuestionByIDs(GetKickoutQuestionByMRFID.Select(x => x.KickoutQuestionID).ToList()));
            var Result = (from left in GetKickoutQuestionByMRFID
                          join right in GetKickoutQuestionByID on left.KickoutQuestionID equals right.ID
                          select new MRFKickoutQuestionListOutput()
                          {
                              ID = left.ID,
                              MRFID = left.MRFID,
                              Code = right.Code,
                              Question = right.Question,
                              Answer = right.Answer,
                              QuestionType = right.QuestionType,
                              Order = left.Order
                          }).OrderBy(x => x.Order).ToList();
            return new OkObjectResult(Result);
        }
        public async Task<IActionResult> GetMRFKickoutQuestionByID(APICredentials credentials, int ID)
        {
            var GetMRFKickoutQuestionByID = (await _dbAccess.GetMRFKickoutQuestionByID(ID));
            var GetKickoutQuestionByID = (await _dbAccess.GetKickoutQuestionByID(GetMRFKickoutQuestionByID.KickoutQuestionID));
            MRFKickoutQuestionListOutput mRFKickoutQuestionListOutput = new MRFKickoutQuestionListOutput();
            mRFKickoutQuestionListOutput.ID = GetMRFKickoutQuestionByID.ID;
            mRFKickoutQuestionListOutput.MRFID = GetMRFKickoutQuestionByID.MRFID;
            mRFKickoutQuestionListOutput.KickoutQuestionID = GetMRFKickoutQuestionByID.KickoutQuestionID;
            mRFKickoutQuestionListOutput.Code = GetKickoutQuestionByID.Code;
            mRFKickoutQuestionListOutput.QuestionType = GetKickoutQuestionByID.QuestionType;
            mRFKickoutQuestionListOutput.Question = GetKickoutQuestionByID.Question;
            mRFKickoutQuestionListOutput.Answer = GetKickoutQuestionByID.Answer;
            mRFKickoutQuestionListOutput.Order = GetMRFKickoutQuestionByID.Order;

            return new OkObjectResult(mRFKickoutQuestionListOutput);
        }
        public async Task<IActionResult> EditKickoutQuestionToMRF(APICredentials credentials, AddKickoutQuestionToMRFInput param)
        {
            var mRFkickoutQuestion = (await _dbAccess.GetMRFKickoutQuestionByID(param.ID));
            mRFkickoutQuestion.MRFID = param.MRFID;
            mRFkickoutQuestion.KickoutQuestionID = param.KickoutQuestionID;
            mRFkickoutQuestion.Order = param.Order;

            var Result = (await _dbAccess.EditKickoutQuestionToMRF(mRFkickoutQuestion));

            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> RemoveKickoutQuestionToMRF(APICredentials credentials, List<int> IDs)
        {
            var mRFkickoutQuestion = (await _dbAccess.GetMRFKickoutQuestionByIDs(IDs));
            var Update = mRFkickoutQuestion.Select(x => new MRFKickoutQuestion()
            {
                ID = x.ID,
                MRFID = x.MRFID,
                KickoutQuestionID = x.KickoutQuestionID,
                Order = x.Order,
                IsActive = false,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate
            }).ToList();

            var Result = (await _dbAccess.EditKickoutQuestionToMRFs(Update));

            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetKickoutQuestionAutoComplete(APICredentials credentials, GetByKickoutQuestionAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetKickoutQuestionAutoComplete(param)).Select(x => new kickoutQuestionAutoComplete()
                {
                    ID = x.ID,
                    Description = x.Code
                }));
        }
        public async Task<IActionResult> GetMRFAutoCancelled(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetMRFAutoCancelled());
        }
        public async Task<IActionResult> GetMRFAutoCancelledReminder(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetMRFAutoCancelledReminder());
        }
        public async Task<IActionResult> MRFChangeStatus(APICredentials credentials, MRFChangeStatusInput param)
        {
            var MRF = (await _dbAccess.GetByID(param.MrfID));
            MRF.Status = param.Status;
            MRF.Remarks = param.Remarks;

            var IsSuccess = (await _dbAccess.Put(MRF));
            if (IsSuccess)
            {
                MRFStatusHistory mRFStatusHistory = new MRFStatusHistory();
                mRFStatusHistory.MRFID = MRF.ID;
                mRFStatusHistory.Status = param.Status;
                mRFStatusHistory.Remarks = param.Remarks;
                mRFStatusHistory.IsActive = true;
                mRFStatusHistory.CreatedBy = credentials.UserID;

                await _dbAccess.AddMRFStatusHistory(mRFStatusHistory);
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
    }
}