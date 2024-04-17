using EMS.Manpower.Transfer.MRF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.FrontEnd.SharedClasses.Common_Manpower
{
    public class Common_MRF : Utilities
    {
        public Common_MRF(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<Form> GetMRF(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetByID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<MRFApplicantForm> GetMRFApplicant(int MRFID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetApplicantByMRFID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "MRFID=", MRFID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new MRFApplicantForm(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<MRFApprovalHistoryOutput>> GetApprovalHistory(MRFApprovalHistoryForm param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetApprovalHistory").Value, "?",
                "userid=", _globalCurrentUser.UserID, "&",
                "RequestingPositionID=", param.RequestingPositionID, "&",
                "RequestingOrgGroupID=", param.RequestingOrgGroupID, "&",
                "PositionID=", param.PositionID, "&",
                "MRFID=", param.MRFID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<MRFApprovalHistoryOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<EMS.Manpower.Transfer.MRFApproval.Form> GetApprovalMRF(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRFApproval").GetSection("GetByID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "id=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new EMS.Manpower.Transfer.MRFApproval.Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<ResultView> PostValidateExistingActual(ValidateMRFExistingActualInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                        _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("ValidateMRFExistingActual").Value, "?",
                        "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PostFromAPI(param, URL);
            _resultView.IsSuccess = IsSuccess;
            _resultView.Result = Message;

            return _resultView;
        }

        public async Task<List<MRFGetCommentsOutput>> GetMRFComments(int MRFID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetComments").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "MRFID=", MRFID);


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<MRFGetCommentsOutput>(), URL);

            List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                await new Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(IDs);


            List<MRFGetCommentsOutput> resultWithUsername =
                Result.Join(
                    systemUsers,
                    x => new { x.CreatedBy },
                    y => new { CreatedBy = y.ID },
                    (x, y) => new { x, y })
                .Select(x => new MRFGetCommentsOutput
                {
                    Timestamp = x.x.Timestamp,
                    Comments = x.x.Comments,
                    Sender = x.y.Username
                }).ToList();

            if (IsSuccess)
                return resultWithUsername;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<MRFGetCommentsOutput>> GetMRFApplicantComments(int MRFID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetApplicantComments").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "MRFID=", MRFID);


            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<MRFGetCommentsOutput>(), URL);

            List<int> IDs = Result.Select(x => x.CreatedBy).Distinct().ToList();

            List<EMS.Security.Transfer.SystemUser.Form> systemUsers =
                await new Common_Security.Common_SystemUser(_iconfiguration, _globalCurrentUser, _env)
                .GetSystemUserByIDs(IDs);


            List<MRFGetCommentsOutput> resultWithUsername =
                Result.Join(
                    systemUsers,
                    x => new { x.CreatedBy },
                    y => new { CreatedBy = y.ID },
                    (x, y) => new { x, y })
                .Select(x => new MRFGetCommentsOutput
                {
                    Timestamp = x.x.Timestamp,
                    Comments = x.x.Comments,
                    Sender = x.y.Username
                }).ToList();

            if (IsSuccess)
                return resultWithUsername;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<bool> ValidateApplicantIsTagged(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("ValidateApplicantIsTagged").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "applicantid=", ID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new bool(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<bool> UpdateCurrentWorkflowStep(UpdateCurrentWorkflowStepInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("UpdateCurrentWorkflowStep").Value, "?",
                      "userid=", _globalCurrentUser.UserID);

            var (IsSuccess, Message) = await SharedUtilities.PutFromAPI(param, URL);
            if (IsSuccess)
                return true;
            else
                throw new Exception(Message);
        }

        public async Task<List<SelectListItem>> GetMRFIDDropdownByApplicantID(int ApplicantID)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFIDDropdownByApplicantID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&ApplicantID=", ApplicantID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SelectListItem>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<MRFApplicantForm> GetApplicantByMRFIDAndID(GetApplicantByMRFIDAndIDInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
               _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetApplicantByMRFIDAndID").Value, "?",
                "userid=", _globalCurrentUser.UserID,
                "&MRFID=", param.MRFID,
                "&ApplicantID=", param.ApplicantID
                );

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new MRFApplicantForm(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetByMRFTransactionID(string MRFTransactionID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                      _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetByMRFTransactionID").Value, "?",
                      "userid=", _globalCurrentUser.UserID, "&",
                      "MRFTransactionID=", MRFTransactionID);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<MRFKickoutQuestionListOutput>,bool,string)> GetMRFKickoutQuestionByMRFID(int MRFID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFKickoutQuestionByMRFID").Value, "?",
            "userid=", _globalCurrentUser.UserID, "&",
            "ID=", MRFID);

            return await SharedUtilities.GetFromAPI(new List<MRFKickoutQuestionListOutput>(), URL);
        }
        public async Task<(List<kickoutQuestionAutoComplete>, bool, string)> GetKickoutQuestionAutoComplete(GetByKickoutQuestionAutoCompleteInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetKickoutQuestionAutoComplete").Value, "?",
            "userid=", _globalCurrentUser.UserID, "&",
            "Term=", param.Term, "&",
            "TopResults=", param.TopResults);

            return await SharedUtilities.GetFromAPI(new List<kickoutQuestionAutoComplete>(), URL);
        }
        public async Task<(MRFKickoutQuestionListOutput, bool, string)> GetMRFKickoutQuestionByID(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFKickoutQuestionByID").Value, "?",
            "userid=", _globalCurrentUser.UserID, "&",
            "ID=", ID);

            return await SharedUtilities.GetFromAPI(new MRFKickoutQuestionListOutput(), URL);
        }
        public async Task<(KickoutQuestionOutput, bool, string)> GetKickoutQuestionByID(int ID)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetKickoutQuestionByID").Value, "?",
            "userid=", _globalCurrentUser.UserID, "&",
            "ID=", ID);

            return await SharedUtilities.GetFromAPI(new KickoutQuestionOutput(), URL);
        }
        public async Task<(bool, string)> AddKickoutQuestionToMRF(AddKickoutQuestionToMRFInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("AddKickoutQuestionToMRF").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> EditKickoutQuestionToMRF(AddKickoutQuestionToMRFInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("EditKickoutQuestionToMRF").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(bool, string)> RemoveKickoutQuestionToMRF(List<int> IDs)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("RemoveKickoutQuestionToMRF").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(IDs, URL);
        }
        public async Task<(List<Form>, bool, string)> GetMRFAutoCancelled()
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFAutoCancelled").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.GetFromAPI(new List<Form>(), URL);
        }
        public async Task<(List<Form>, bool, string)> GetMRFAutoCancelledReminder()
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("GetMRFAutoCancelledReminder").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.GetFromAPI(new List<Form>(), URL);
        }
        public async Task<(List<EMS.Manpower.Transfer.ApproverSetup.MRFDefinedApproverOutput>, bool, string)> GetSetupMRFApproverInsert()
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("ApproverSetup").GetSection("GetSetupMRFApproverInsert").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.GetFromAPI(new List<EMS.Manpower.Transfer.ApproverSetup.MRFDefinedApproverOutput>(), URL);
        }
        public async Task<(List<EMS.Manpower.Transfer.ApproverSetup.MRFDefinedApproverOutput>, bool, string)> GetSetupMRFApproverUpdate()
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("ApproverSetup").GetSection("GetSetupMRFApproverUpdate").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.GetFromAPI(new List<EMS.Manpower.Transfer.ApproverSetup.MRFDefinedApproverOutput>(), URL);
        }
        public async Task<(bool, string)> MRFChangeStatus(MRFChangeStatusInput param)
        {
            var URL = string.Concat(_manpowerBaseURL,
                     _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("MRF").GetSection("MRFChangeStatus").Value, "?",
            "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
    }
}