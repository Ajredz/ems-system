using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Accountability;
using EMS.Workflow.Transfer.Accountability;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;


namespace EMS.Workflow.Core.Accountability
{
    public interface IAccountabilityService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetAccountabilityListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetDetailsByAccountabilityID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, AccountabilityForm param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);
        Task<IActionResult> BulkEmployeeAccountabilityDelete(APICredentials credentials, string ID);

        Task<IActionResult> Put(APICredentials credentials, AccountabilityForm param);

        Task<IActionResult> GetAccountabilityDropdown(APICredentials credentials);

        Task<IActionResult> GetEmployeeAccountabilityByEmployeeID(APICredentials credentials, int EmployeeID);

        Task<IActionResult> GetAccountabilityDetails(APICredentials credentials, int AccountabilityID);

        Task<IActionResult> AddEmployeePreLoadedAccountability(APICredentials credentials, AddEmployeePreLoadedAccountabilityInput param);

        Task<IActionResult> AddEmployeeAccountability(APICredentials credentials, TagToEmployeeForm param);

        Task<IActionResult> GetEmployeeAccountabilityByID(APICredentials credentials, int ID);

        Task<IActionResult> GetEmployeeAccountabilityStatusHistory(APICredentials credentials, int EmployeeAccountabilityID);

        Task<IActionResult> AddEmployeeAccountabilityStatusHistory(APICredentials credentials, EmployeeAccountabilityForm param);

        Task<IActionResult> PostEmployeeComments(APICredentials credentials, EmployeeAccountabilityCommentsForm param);

        Task<IActionResult> GetEmployeeComments(APICredentials credentials, int ID);

        Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeAccountabilityAttachmentForm param);

        Task<IActionResult> GetEmployeeAttachment(APICredentials credentials, int ID);

        Task<IActionResult> GetMyAccountabilitiesList(APICredentials credentials, GetMyAccountabilitiesListInput input);

        Task<IActionResult> BatchAccountabilityAdd(APICredentials credentials, BatchAccountabilityAddInput param);

        Task<IActionResult> UploadInsert(APICredentials credentials, List<AccountabilityUploadFile> param);
        Task<IActionResult> GetAllLastCommentByEmployeeId(APICredentials credentials, int EmployeeId);
        Task<IActionResult> GetAllEmployeeAccountability(APICredentials credentials);
        Task<IActionResult> PostChangeStatus(APICredentials credentials, ChangeStatusInput param);
        Task<IActionResult> GetEmployeeByEmployeeAccountabilityIDs(APICredentials credentials, List<int> EmployeeAccountabilityID);
        Task<IActionResult> GetEmployeeAccountabilityStatusPercentage(GetEmployeeAccountabilityStatusPercentageInput param);
        Task<IActionResult> GetEmployeeAccountabilityExitClearance(int ID);
        Task<IActionResult> GetEmployeeAccountabilityList(GetMyAccountabilitiesListInput param);
        Task<IActionResult> GetAccountabilityDashboard(GetAccountabilityDashboardInput param);
        Task<IActionResult> GetCheckEmployeeCleared(string EmployeeID);
        Task<IActionResult> GetEmployeeClearedList(ClearedEmployeeListInput param);
        Task<IActionResult> GetClearedEmployeeByID(int ID);
        Task<IActionResult> PostClearedEmployeeComments(APICredentials credentials, PostClearedEmployeeCommentsInput param);
        Task<IActionResult> GetClearedEmployeeComments(int ClearedEmployeeID);
        Task<IActionResult> GetClearedEmployeeStatusHistory(int ClearedEmployeeID);
        Task<IActionResult> GetEmployeeAccountability(int EmployeeID);
        Task<IActionResult> PostClearedEmployeeComputation(APICredentials credentials, PostClearedEmployeeComputationInput param);
        Task<IActionResult> PostClearedEmployeeChangeStatus(APICredentials credentials, PostClearedEmployeeStatusInput param);
        Task<IActionResult> GetClearedEmployeeByEmployeeID(int EmployeeID);
        Task<IActionResult> AddClearedEmployeeAgreed(int ID);
    }

    public class AccountabilityService : Core.Shared.Utilities, IAccountabilityService
    {
        private readonly EMS.Workflow.Data.Accountability.IAccountabilityDBAccess _dbAccess;
        private readonly EMS.Workflow.Data.Reference.IReferenceDBAccess _dbReferenceService;

        public AccountabilityService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.Accountability.IAccountabilityDBAccess dBAccess, Data.Reference.IReferenceDBAccess dbReferenceService) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
            _dbReferenceService = dbReferenceService;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetAccountabilityListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarAccountability> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetAccountabilityListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                PreloadName = x.PreloadName,
                DateCreated = x.DateCreated
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.Accountability.Accountability result = await _dbAccess.GetByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new AccountabilityForm
                {
                    ID = result.ID,
                    PreloadedName = result.PreloadName
                });
        }

        public async Task<IActionResult> GetDetailsByAccountabilityID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetDetailsByAccountabilityID(ID))
                .Select(x => new GetDetailsByIDOutput
                {
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    OrgGroupID = x.OrgGroupID ?? 0,
                    PositionID = x.PositionID ?? 0,
                    EmployeeID = x.EmployeeID ?? 0
                }).ToList()
            );
        }

        public async Task<IActionResult> Post(APICredentials credentials, AccountabilityForm param)
        {
            param.PreloadedName = param.PreloadedName.Trim();
            if (string.IsNullOrEmpty(param.PreloadedName))
                ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.PreloadedName.Length > 100)
                ErrorMessages.Add(string.Concat("Preloaded Name", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            else
            {
                if ((await _dbAccess.GetByName(param.PreloadedName)).Count() > 0)
                {
                    ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.Accountability.Accountability
                    {
                        PreloadName = param.PreloadedName,
                        IsActive = true,
                        CreatedBy = credentials.UserID
                    }
                    , param.AccountabilityDetailsList?.Select(x => new Data.Accountability.AccountabilityDetails
                    {
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        OrgGroupID = x.OrgGroupID,
                        PositionID = x.PositionID,
                        EmployeeID = x.EmployeeID,
                        IsActive = true,
                        CreatedBy = credentials.UserID
                    }).ToList()
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            Data.Accountability.Accountability accountability = await _dbAccess.GetByID(ID);
            accountability.IsActive = false;
            accountability.ModifiedBy = credentials.UserID;
            accountability.ModifiedDate = DateTime.Now;

            List<Data.Accountability.AccountabilityDetails> accountabilityDetails = (await _dbAccess.GetDetailsByAccountabilityID(ID))
                .Select(x => new Data.Accountability.AccountabilityDetails
                {
                    ID = x.ID,
                    AccountabilityID = x.AccountabilityID,
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    OrgGroupID = x.OrgGroupID,
                    PositionID = x.PositionID,
                    EmployeeID = x.EmployeeID,
                    IsActive = false,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = credentials.UserID,
                    ModifiedDate = DateTime.Now
                }).ToList();

            if (await _dbAccess.Put(accountability, accountabilityDetails, null, null))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }

        public async Task<IActionResult> BulkEmployeeAccountabilityDelete(APICredentials credentials, string ID)
        {
            List<int> AccountablityID = (ID.Split(",")).Select(int.Parse).ToList();
            List<EmployeeAccountability> ForUpdate = new List<EmployeeAccountability>();
            foreach (var item in AccountablityID)
            {
                EmployeeAccountability employeeAccountability = (await _dbAccess.GetEmployeeAccountabilityByID(item));
                employeeAccountability.IsActive = false;
                employeeAccountability.ModifiedBy = credentials.UserID;
                employeeAccountability.ModifiedDate = DateTime.Now;
                ForUpdate.Add(employeeAccountability);
            }
            if (await _dbAccess.BulkEmployeeAccountabilityDelete(ForUpdate))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);
        }
        public async Task<IActionResult> Put(APICredentials credentials, AccountabilityForm param)
        {
            param.PreloadedName = param.PreloadedName.Trim();
            if (string.IsNullOrEmpty(param.PreloadedName))
                ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.PreloadedName.Length > 100)
                ErrorMessages.Add(string.Concat("Preloaded Name", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            else
            {
                if ((await _dbAccess.GetByName(param.PreloadedName)).Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Preloaded Name " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }
            }

            if (ErrorMessages.Count == 0)
            {

                IEnumerable<Data.Accountability.AccountabilityDetails> GetDetailsToDelete(List<Data.Accountability.AccountabilityDetails> left, List<Data.Accountability.AccountabilityDetails> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Accountability.AccountabilityDetails
                            {
                                ID = x.oldSet.oldSet.ID,
                                AccountabilityID = x.oldSet.oldSet.AccountabilityID,
                                Type = x.oldSet.oldSet.Type,
                                Title = x.oldSet.oldSet.Title,
                                Description = x.oldSet.oldSet.Description,
                                OrgGroupID = x.oldSet.oldSet.OrgGroupID,
                                PositionID = x.oldSet.oldSet.PositionID,
                                EmployeeID = x.oldSet.oldSet.EmployeeID,
                                IsActive = false,
                                CreatedBy = x.oldSet.oldSet.CreatedBy,
                                CreatedDate = x.oldSet.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now
                            }).ToList();
                }

                IEnumerable<Data.Accountability.AccountabilityDetails> GetDetailsToAdd(List<Data.Accountability.AccountabilityDetails> left, List<Data.Accountability.AccountabilityDetails> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Accountability.AccountabilityDetails
                            {
                                AccountabilityID = x.newSet.newSet.AccountabilityID,
                                Type = x.newSet.newSet.Type,
                                Title = x.newSet.newSet.Title,
                                Description = x.newSet.newSet.Description,
                                OrgGroupID = x.newSet.newSet.OrgGroupID,
                                PositionID = x.newSet.newSet.PositionID,
                                EmployeeID = x.newSet.newSet.EmployeeID,
                                IsActive = x.newSet.newSet.IsActive,
                                CreatedBy = x.newSet.newSet.CreatedBy
                            }).ToList();
                }

                IEnumerable<Data.Accountability.AccountabilityDetails> GetDetailsToUpdate(List<Data.Accountability.AccountabilityDetails> left, List<Data.Accountability.AccountabilityDetails> right)
                {
                    return left.Join(
                        right,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Type.Equals(x.newSet.Type) ||
                                    !x.oldSet.Title.Equals(x.newSet.Title) ||
                                    (x.oldSet.Description != x.newSet.Description) ||
                                    (x.oldSet.OrgGroupID != x.newSet.OrgGroupID) ||
                                    (x.oldSet.PositionID != x.newSet.PositionID) ||
                                    (x.oldSet.EmployeeID != x.newSet.EmployeeID)
                              )
                        .Select(y =>
                            new Data.Accountability.AccountabilityDetails
                            {
                                ID = y.oldSet.ID,
                                AccountabilityID = y.newSet.AccountabilityID,
                                Type = y.newSet.Type,
                                Title = y.newSet.Title,
                                Description = y.newSet.Description,
                                OrgGroupID = y.newSet.OrgGroupID,
                                PositionID = y.newSet.PositionID,
                                EmployeeID = y.newSet.EmployeeID,
                                IsActive = y.oldSet.IsActive,
                                CreatedBy = y.oldSet.CreatedBy,
                                CreatedDate = y.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now
                            }).ToList();
                }

                List<Data.Accountability.AccountabilityDetails> OldAccountabilityDetails = (await _dbAccess.GetDetailsByAccountabilityID(param.ID)).ToList();

                List<Data.Accountability.AccountabilityDetails> DetailsToAdd = GetDetailsToAdd(OldAccountabilityDetails,
                    param.AccountabilityDetailsList.Select(x => new Data.Accountability.AccountabilityDetails
                    {
                        AccountabilityID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        OrgGroupID = x.OrgGroupID,
                        PositionID = x.PositionID,
                        EmployeeID = x.EmployeeID,
                        IsActive = true,
                        CreatedBy = credentials.UserID
                    }).ToList()).ToList();


                List<Data.Accountability.AccountabilityDetails> DetailsToUpdate = GetDetailsToUpdate(OldAccountabilityDetails,
                    param.AccountabilityDetailsList.Select(x => new Data.Accountability.AccountabilityDetails
                    {
                        AccountabilityID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        OrgGroupID = x.OrgGroupID,
                        PositionID = x.PositionID,
                        EmployeeID = x.EmployeeID,
                        IsActive = true,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();


                List<Data.Accountability.AccountabilityDetails> DetailsToDelete = GetDetailsToDelete(OldAccountabilityDetails,
                    param.AccountabilityDetailsList.Select(x => new Data.Accountability.AccountabilityDetails
                    {
                        AccountabilityID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        OrgGroupID = x.OrgGroupID,
                        PositionID = x.PositionID,
                        EmployeeID = x.EmployeeID,
                        IsActive = false,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                Data.Accountability.Accountability accountability = await _dbAccess.GetByID(param.ID);
                accountability.PreloadName = param.PreloadedName;
                accountability.IsActive = true;
                accountability.ModifiedBy = credentials.UserID;
                accountability.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(
                    accountability,
                    DetailsToDelete,
                    DetailsToAdd,
                    DetailsToUpdate
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetAccountabilityDropdown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllAccountability()).ToList(), "ID", "PreloadName", null));
        }

        public async Task<IActionResult> GetEmployeeAccountabilityByEmployeeID(APICredentials credentials, int EmployeeID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeAccountabilityByEmployeeID(EmployeeID)).ToList()
            );
        }

        public async Task<IActionResult> GetAccountabilityDetails(APICredentials credentials, int AccountabilityID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAccountabilityDetails(AccountabilityID))
                .Select(x => new GetDetailsByIDOutput
                {
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    OrgGroupID = x.OrgGroupID ?? 0,
                    PositionID = x.PositionID ?? 0,
                    EmployeeID = x.EmployeeID ?? 0
                }).ToList()
            );
        }

        public async Task<IActionResult> AddEmployeePreLoadedAccountability(APICredentials credentials, AddEmployeePreLoadedAccountabilityInput param)
        {
            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.AccountabilityPreloadedIDs.Count <= 0)
                ErrorMessages.Add(string.Concat("AccountabilityPreloadedIDs ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));


            if (ErrorMessages.Count == 0)
            {
                List<EmployeeAccountabilityStatusHistory> result =
                        (await _dbAccess.AddEmployeePreLoadedAccountability(param, credentials.UserID)).ToList();
                return new OkObjectResult(string.Concat(result.First().ID, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED));
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> AddEmployeeAccountability(APICredentials credentials, TagToEmployeeForm param)
        {
            if (param.EmployeeID == 0)
                ErrorMessages.Add("EmployeeID " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Title = param.Title.Trim();
                if (param.Title.Length > 100)
                    ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (!string.IsNullOrEmpty(param.Description))
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (!string.IsNullOrEmpty(param.Remarks))
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0)
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;

            await _dbAccess.AddEmployeeAccountability(new EmployeeAccountability
            {
                EmployeeID = param.EmployeeID,
                Title = param.Title,
                Description = param.Description,
                Type = param.Type,
                Status = param.Status.ToString(),
                StatusUpdatedDate = currentDatetime,
                OrgGroupID = param.OrgGroupID,
                PositionID = param.PositionID,
                ApproverEmployeeID = param.ApproverEmployeeID,
                IsActive = true,
                CreatedBy = credentials.UserID,
                CreatedDate = currentDatetime
            },
            new EmployeeAccountabilityStatusHistory
            {
                Status = param.Status.ToString(),
                Timestamp = currentDatetime,
                Remarks = param.Remarks,
                UserID = credentials.UserID
            }
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> GetEmployeeAccountabilityByID(APICredentials credentials, int ID)
        {
            Data.Accountability.EmployeeAccountability result = await _dbAccess.GetEmployeeAccountabilityByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                List<Data.Reference.ReferenceValue> referenceValues =
                              (await _dbReferenceService.GetByRefCodes(new List<string> { "ACCOUNTABILITY_TYPE" })).Where(x => x.Value.Equals(result.Type)).ToList();

                if (referenceValues.Count == 0)
                    return new BadRequestObjectResult(string.Concat("Accountability: ", MessageUtilities.ERRMSG_REC_NOT_EXIST));

                return new OkObjectResult(
                 new GetMyAccountabilitiesListOutput
                 {
                     ID = result.ID,
                     EmployeeID = result.EmployeeID,
                     Type = result.Type,
                     Title = result.Title,
                     Description = result.Description,
                     Status = result.Status,
                     OrgGroupID = result.OrgGroupID,
                     PositionID = result.PositionID,
                     ApproverEmployeeID = result.ApproverEmployeeID
                 });
            }
        }

        public async Task<IActionResult> GetEmployeeAccountabilityStatusHistory(APICredentials credentials, int EmployeeAccountabilityID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeAccountabilityStatusHistory(EmployeeAccountabilityID))
                .Select(x => new GetEmployeeAccountabilityStatusHistoryOutput
                {
                    Status = x.Status,
                    Timestamp = x.Timestamp,
                    UserID = x.UserID,
                    Remarks = x.Remarks ?? "",
                    User = ""

                }).ToList()
            );
        }

        public async Task<IActionResult> AddEmployeeAccountabilityStatusHistory(APICredentials credentials, EmployeeAccountabilityForm param)
        {
            if (!string.IsNullOrEmpty(param.Remarks))
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 255)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }

            if (string.IsNullOrEmpty(param.Title))
                ErrorMessages.Add("Title " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
              if (param.Title.Length > 100)
            {
                param.Title = param.Title.Trim();
                ErrorMessages.Add(string.Concat("Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
            }


            if (string.IsNullOrEmpty(param.Type))
                ErrorMessages.Add("Type " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
            {
                param.Type = param.Type.Trim();
                if (param.Type.Length > 50)
                    ErrorMessages.Add(string.Concat("Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (!string.IsNullOrEmpty(param.Description))
            {
                param.Description = param.Description.Trim();
                if (param.Description.Length > 255)
                    ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            }


            if (param.CreatedBy == 0)
                ErrorMessages.Add("CreatedBy " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count > 0)
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;

            Data.Accountability.EmployeeAccountability result = await _dbAccess.GetEmployeeAccountabilityByID(param.ID);
            // Update current status and timestamp
            result.Type = param.Type;
            result.Title = param.Title;
            result.Description = param.Description;
            // Do not add new Status history if same Status is the same as current status
            /*if (!string.IsNullOrEmpty(param.Status) && !result.Status.Equals(param.Status))
            {
                result.Status = param.Status ?? "";
            }
            else
            {
                param.Status = "";
            }*/

            //result.StatusUpdatedDate = currentDatetime;
            result.OrgGroupID = param.OrgGroupID;
            result.PositionID = param.PositionID;
            result.ApproverEmployeeID = param.ApproverEmployeeID;

            await _dbAccess.UpdateEmployeeAccountability(result);
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> PostEmployeeComments(APICredentials credentials, EmployeeAccountabilityCommentsForm param)
        {

            if (param.EmployeeAccountabilityID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeAccountabilityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Comments = param.Comments.Trim();
            if (string.IsNullOrEmpty(param.Comments))
                ErrorMessages.Add(string.Concat("Comments ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else if (param.Comments.Length > 500)
                ErrorMessages.Add(string.Concat("Comments", MessageUtilities.COMPARE_NOT_EXCEED, "500 characters."));

            if (param.CreatedBy <= 0)
                ErrorMessages.Add(string.Concat("CreatedBy ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                DateTime DateNow = DateTime.Now;
                await _dbAccess.PostEmployeeComments(new EmployeeAccountabilityComments
                {
                    EmployeeAccountabilityID = param.EmployeeAccountabilityID,
                    Comments = param.Comments,
                    CreatedBy = param.CreatedBy,
                    CreatedDate = DateNow,
                    IsExternal = param.IsExternal
                });

                var GetEmployee = (await _dbAccess.GetEmployeeAccountabilityByID(param.EmployeeAccountabilityID));
                GetEmployee.LastComment = param.Comments;
                GetEmployee.LastCommentDate = DateNow;
                await _dbAccess.UpdateEmployeeAccountability(GetEmployee);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetEmployeeComments(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeComments(ID))
                .Select(x => new EmployeeAccountabilityGetCommentsOutput
                {
                    Timestamp = x.CreatedDate.ToString("dd-MMM-yyyy (HH:mm)"),
                    Comments = x.Comments,
                    CreatedBy = x.CreatedBy
                }));
        }

        public async Task<IActionResult> PostEmployeeAttachment(APICredentials credentials, EmployeeAccountabilityAttachmentForm param)
        {

            if (param.EmployeeAccountabilityID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeAccountabilityID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

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
                List<EmployeeAccountabilityAttachment> GetToAdd(List<EmployeeAccountabilityAttachment> left, List<EmployeeAccountabilityAttachment> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.EmployeeAccountabilityID, x.ServerFile },
                        y => new { y.EmployeeAccountabilityID, y.ServerFile },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new EmployeeAccountabilityAttachment
                        {
                            EmployeeAccountabilityID = x.newSet.newSet.EmployeeAccountabilityID,
                            AttachmentType = x.newSet.newSet.AttachmentType,
                            Remarks = x.newSet.newSet.Remarks,
                            SourceFile = x.newSet.newSet.SourceFile,
                            ServerFile = x.newSet.newSet.ServerFile,
                            CreatedBy = credentials.UserID
                        })
                    .ToList();
                }

                List<EmployeeAccountabilityAttachment> GetToUpdate(List<EmployeeAccountabilityAttachment> left, List<EmployeeAccountabilityAttachment> right)
                {
                    return left.Join(
                    right,
                    x => new { x.EmployeeAccountabilityID, x.ServerFile },
                    y => new { y.EmployeeAccountabilityID, y.ServerFile },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => !x.oldSet.AttachmentType.Equals(x.newSet.AttachmentType)
                        || !(x.oldSet.Remarks ?? "").Equals(x.newSet.Remarks ?? "")
                        || !x.oldSet.SourceFile.Equals(x.newSet.SourceFile)
                    )
                    .Select(y => new EmployeeAccountabilityAttachment
                    {
                        ID = y.oldSet.ID,
                        EmployeeAccountabilityID = y.newSet.EmployeeAccountabilityID,
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

                List<EmployeeAccountabilityAttachment> GetToDelete(List<EmployeeAccountabilityAttachment> left, List<EmployeeAccountabilityAttachment> right)
                {
                    return left.GroupJoin(
                            right,
                            x => new { x.EmployeeAccountabilityID, x.ServerFile },
                            y => new { y.EmployeeAccountabilityID, y.ServerFile },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new EmployeeAccountabilityAttachment
                                {
                                    ID = x.oldSet.oldSet.ID
                                }).ToList();
                }

                List<EmployeeAccountabilityAttachment> oldSet = (await _dbAccess.GetEmployeeAttachment(param.EmployeeAccountabilityID)).ToList();
                List<EmployeeAccountabilityAttachment> paramAttachment =
                    param.AddAttachmentForm.Select(x => new EmployeeAccountabilityAttachment
                    {
                        EmployeeAccountabilityID = param.EmployeeAccountabilityID,
                        AttachmentType = x.AttachmentType,
                        ServerFile = x.ServerFile,
                        SourceFile = x.SourceFile,
                        Remarks = x.Remarks
                    }).ToList();

                List<EmployeeAccountabilityAttachment> ValueToAdd = GetToAdd(paramAttachment, oldSet).ToList();
                List<EmployeeAccountabilityAttachment> ValueToUpdate = GetToUpdate(oldSet, paramAttachment).ToList();
                List<EmployeeAccountabilityAttachment> ValueToDelete = GetToDelete(oldSet, paramAttachment).ToList();

                List<EmployeeAccountabilityAttachment> addAttachment = new List<EmployeeAccountabilityAttachment>();
                foreach (var item in param.AddAttachmentForm)
                {
                    addAttachment.Add(new EmployeeAccountabilityAttachment
                    {
                        EmployeeAccountabilityID = param.EmployeeAccountabilityID,
                        AttachmentType = item.AttachmentType,
                        ServerFile = item.ServerFile,
                        SourceFile = item.SourceFile,
                        Remarks = item.Remarks,
                        CreatedBy = credentials.UserID,
                    });
                }

                await _dbAccess.PostEmployeeAttachment(ValueToAdd, ValueToUpdate, ValueToDelete);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> GetEmployeeAttachment(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeAttachment(ID))
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

        public async Task<IActionResult> GetMyAccountabilitiesList(APICredentials credentials, GetMyAccountabilitiesListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMyAccountabilities> result = await _dbAccess.GetMyAccountabilitiesList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetMyAccountabilitiesListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),

                ID = x.ID,
                EmployeeID = x.EmployeeID,
                Type = x.Type,
                Title = x.Title,
                Description = x.Description,
                OrgGroupID = x.OrgGroupID,
                PositionID = x.PositionID,
                ApproverEmployeeID = x.ApproverEmployeeID,
                Status = x.Status,
                StatusUpdatedBy = x.StatusUpdatedBy,
                StatusUpdateDate = x.StatusUpdateDate,
                StatusRemarks = x.StatusRemarks,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
                LastComment = x.LastComment,
                LastCommentDate = x.LastCommentDate
            }).ToList());
        }

        public async Task<IActionResult> BatchAccountabilityAdd(APICredentials credentials, BatchAccountabilityAddInput param)
        {
            if (param.EmployeeID == 0)
            {
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            DateTime currentDatetime = DateTime.Now;

            List<EmployeeAccountability> result = 
                (await _dbAccess.GetByEmployeeID(param.EmployeeID)).ToList();

            result = result.Where(y => y.Status.Equals(Transfer.Enums.AccountabilityStatus.NEW.ToString()) ||
                y.Status.Equals(Transfer.Enums.AccountabilityStatus.ACCEPTED.ToString())).Select(x => {
                x.Status = param.Status.ToString();
                x.StatusUpdatedDate = currentDatetime;
                return x;
            }).ToList();


            await _dbAccess.BatchAccountabilityAdd(
          result
                , result.Select(x => new EmployeeAccountabilityStatusHistory
           {
               EmployeeAccountabilityID = x.ID,
               Status = param.Status.ToString(),
               Timestamp = currentDatetime,
               Remarks = "Updated from Employee Movement",
               UserID = credentials.UserID
           }).ToList()
            );
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);

        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<AccountabilityUploadFile> param)
        {

            var typeList = (await _dbReferenceService.GetByRefCodes(new List<string> { "ACCOUNTABILITY_TYPE" })).Select(x => x.Value).ToList();

            /*Checking of required and invalid fields*/
            foreach (AccountabilityUploadFile obj in param)
            {

                /*Old Employee ID*/
                if (string.IsNullOrEmpty(obj.OldEmployeeID))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.EmployeeID == 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                }
                else
                {
                    obj.OldEmployeeID = obj.OldEmployeeID.Trim();
                    if (obj.OldEmployeeID.Length > 5)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));
                    }
                }
                

                /*Type*/
                if (string.IsNullOrEmpty(obj.Type))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Type = obj.Type.Trim();
                    if (obj.Type.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.Type, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (typeList.Where(x => obj.Type.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Title*/
                if (string.IsNullOrEmpty(obj.Title))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Title = obj.Title.Trim();
                    if (obj.Title.Length > 100)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
                    }
                }

                /*Description*/
                if (!string.IsNullOrEmpty(obj.Description))
                {
                    obj.Description = obj.Description.Trim();
                    if (obj.Description.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*Remarks*/
                if (!string.IsNullOrEmpty(obj.Remarks))
                {
                    obj.Remarks = obj.Remarks.Trim();
                    if (obj.Remarks.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*OrgGroup Code*/
                if (string.IsNullOrEmpty(obj.OrgGroupCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.OrgGroupID == 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.COMPARE_INVALID));
                }
                else
                {
                    obj.OrgGroupCode = obj.OrgGroupCode.Trim();
                    if (obj.OrgGroupCode.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org Group Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
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
                    obj.OldEmployeeID.Equals(x.OldEmployeeID, StringComparison.OrdinalIgnoreCase) &
                    obj.Type.Equals(x.Type, StringComparison.OrdinalIgnoreCase) &
                    obj.Title.Equals(x.Title, StringComparison.OrdinalIgnoreCase) &
                    obj.OrgGroupCode.Equals(x.OrgGroupCode, StringComparison.OrdinalIgnoreCase) &
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                        Duplicates.Add("Row [" + obj.RowNum + "]");
                    }


                    /* Remove duplicates from database */
                    var duplicateFromDatabase = (await _dbAccess.GetByUnique(obj.EmployeeID, obj.Type, obj.Title, obj.OrgGroupID)).ToList();
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
                List<AccountabilityUploadFile> accountabilityList = new List<AccountabilityUploadFile>();

                if (param != null)
                {
                    foreach (var obj in param)
                    {
                        accountabilityList.Add(new AccountabilityUploadFile
                        {
                            EmployeeID = obj.EmployeeID,
                            Type = obj.Type,
                            Title = obj.Title,
                            Description = obj.Description,
                            Remarks = obj.Remarks,
                            OrgGroupID = obj.OrgGroupID,
                            Status = Transfer.Enums.AccountabilityStatus.NEW.ToString(),
                            StatusUpdatedDate = DateTime.Now,
                            UploadedBy = credentials.UserID
                        });
                    }

                    await _dbAccess.UploadInsert(accountabilityList);
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
                    return new OkObjectResult(string.Concat(param?.Count, " Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD));
                }
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }


        public async Task<IActionResult> GetAllLastCommentByEmployeeId(APICredentials credentials, int EmployeeId)
        {
            if (EmployeeId == 0)
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_NO_RECORDS);
            }
            else
            {
                return new OkObjectResult(await _dbAccess.GetAllLastCommentByEmployeeId(EmployeeId));
            }
        }

        public async Task<IActionResult> GetAllEmployeeAccountability(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAllEmployeeAccountability()).ToList());
        }


        public async Task<IActionResult> PostChangeStatus(APICredentials credentials, ChangeStatusInput param)
        {
            if (!string.IsNullOrEmpty(param.Remarks))
            {
                param.Remarks = param.Remarks.Trim();
                if (param.Remarks.Length > 300)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "300 characters."));
            }

            if (ErrorMessages.Count > 0)
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

            DateTime currentDatetime = DateTime.Now;


            //FOR ADD STATUS HISTORY
            List<EmployeeAccountabilityStatusHistory> employeeAccountabilityStatusHistories = param.ID
                .Select(x => new EmployeeAccountabilityStatusHistory()
                {
                    EmployeeAccountabilityID = Convert.ToInt32(x),
                    Status = param.Status,
                    Timestamp = currentDatetime,
                    Remarks = param.Remarks,
                    UserID = credentials.UserID,
                    IsExternal = param.IsExternal
                }).ToList();

            //FOR GET EMPLOYEE ACCOUNTABILITY
            var GetEmployeeAccountability = await _dbAccess.GetEmployeeAccountabilityByIDs(param.ID);

            //FOR UPDATE EMPLOYEE ACCOUNTABILITY
            var UpdateEmployeeAccountability = GetEmployeeAccountability.Select(x => new EmployeeAccountability()
            {
                ID = x.ID,
                EmployeeID = x.EmployeeID,
                Type = x.Type,
                Title = x.Title,
                Description = x.Description,
                OrgGroupID = x.OrgGroupID,
                PositionID = x.PositionID,
                ApproverEmployeeID = x.ApproverEmployeeID,
                Status = param.Status,
                StatusUpdatedBy = credentials.UserID,
                StatusUpdatedDate = currentDatetime,
                StatusRemarks = param.Remarks,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
                LastComment = x.LastComment,
                LastCommentDate = x.LastCommentDate
            }).ToList();

            await _dbAccess.UpdateEmployeeAccountability(UpdateEmployeeAccountability);
            var Result = await _dbAccess.AddStatusHistory(employeeAccountabilityStatusHistories);

            if (Result)
            {
                if (param.Status.Equals("CLEARED") || param.Status.Equals("CANCELLED"))
                {
                    var ClearedResult = (await _dbAccess.GetCheckEmployeeCleared(string.Join(",",GetEmployeeAccountability.Select(x=>x.EmployeeID).ToList()))).Where(x => x.IsCleared).ToList();

                    var PostClearedEmployee = ClearedResult.Select(x => new ClearedEmployee()
                    {
                        EmployeeID = x.EmployeeID,
                        Accountability = x.Accountability,
                        Status = "CLEARED",
                        StatusUpdatedBy = 1,
                        StatusUpdatedDate = DateTime.Now,
                        IsActive = true,
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now
                    }).ToList();

                    await _dbAccess.PostClearedEmployee(PostClearedEmployee);

                    var GetClearedEmployee = await _dbAccess.GetClearedEmployeeByEmployeeID(PostClearedEmployee.Select(x => x.EmployeeID).ToList());

                    var PostClearedEmployeeStatusHistory = GetClearedEmployee.Select(x => new ClearedEmployeeStatusHistory()
                    {
                        ClearedEmployeeID = x.ID,
                        Status = x.Status,
                        Remarks = x.StatusRemarks,
                        IsActive = true,
                        CreatedBy = x.StatusUpdatedBy,
                        CreatedDate = x.StatusUpdatedDate
                    }).ToList();

                    await _dbAccess.PostClearedEmployeeStatusHistory(PostClearedEmployeeStatusHistory);
                }

                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
            }
        }

        public async Task<IActionResult> GetEmployeeByEmployeeAccountabilityIDs(APICredentials credentials, List<int> EmployeeAccountabilityID)
        {
            return new OkObjectResult(((await _dbAccess.GetEmployeeByEmployeeAccountabilityIDs(EmployeeAccountabilityID)).ToList()));
        }

        public async Task<IActionResult> GetEmployeeAccountabilityStatusPercentage(GetEmployeeAccountabilityStatusPercentageInput param)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeAccountabilityStatusPercentage(param));
        }
        public async Task<IActionResult> GetEmployeeAccountabilityExitClearance(int ID)
        {
            var Result = await _dbAccess.GetEmployeeAccountabilityExitClearance(ID);
            if (Result.Count() > 0)
                return new OkObjectResult(true);
            else
                return new BadRequestObjectResult(false);
        }
        public async Task<IActionResult> GetEmployeeAccountabilityList(GetMyAccountabilitiesListInput param)
        {
            int rowStart = 1;
            rowStart = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : rowStart;
            return new OkObjectResult(await _dbAccess.GetEmployeeAccountabilityList(param, rowStart));
        }
        public async Task<IActionResult> GetAccountabilityDashboard(GetAccountabilityDashboardInput param)
        {
            return new OkObjectResult(await _dbAccess.GetAccountabilityDashboard(param));
        }
        public async Task<IActionResult> GetCheckEmployeeCleared(string EmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetCheckEmployeeCleared(EmployeeID));
        } 
        public async Task<IActionResult> GetEmployeeClearedList(ClearedEmployeeListInput param)
        {
            // param.pageNumber = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : param.pageNumber;


            if (param.IsExport)
            {
                param.rows = 1;
            }
            return new OkObjectResult(await _dbAccess.GetEmployeeClearedList(param));
        }
        public async Task<IActionResult> GetClearedEmployeeByID(int ID)
        {
            return new OkObjectResult(await _dbAccess.GetClearedEmployeeByID(ID));
        }
        public async Task<IActionResult> PostClearedEmployeeComments(APICredentials credentials, PostClearedEmployeeCommentsInput param)
        {
            var DateNow = DateTime.Now;
            var ClearedEmployee = (await _dbAccess.GetClearedEmployeeByIDDefault(param.ClearedEmployeeID));
            ClearedEmployee.LastComment = param.Comments;
            ClearedEmployee.LastCommentDate = DateNow;

            await _dbAccess.PutClearedEmployee(ClearedEmployee);

            return new OkObjectResult(await _dbAccess.PostClearedEmployeeComments(new ClearedEmployeeComments() { 
                ClearedEmployeeID = param.ClearedEmployeeID,
                Comments = param.Comments,
                IsActive = true,
                CreatedBy = credentials.UserID,
                CreatedDate = DateNow
            }));
        }
        public async Task<IActionResult> GetClearedEmployeeComments(int ClearedEmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetClearedEmployeeComments(ClearedEmployeeID));
        }
        public async Task<IActionResult> GetClearedEmployeeStatusHistory(int ClearedEmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetClearedEmployeeStatusHistory(ClearedEmployeeID));
        }
        public async Task<IActionResult> GetEmployeeAccountability(int EmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeAccountability(EmployeeID));
        }
        public async Task<IActionResult> PostClearedEmployeeComputation(APICredentials credentials, PostClearedEmployeeComputationInput param)
        {
            var ClearedEmployee = (await _dbAccess.GetClearedEmployeeByIDDefault(param.ClearedEmployeeID));
            ClearedEmployee.Computation = param.Computation;

            if (await _dbAccess.PutClearedEmployee(ClearedEmployee)) 
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> PostClearedEmployeeChangeStatus(APICredentials credentials, PostClearedEmployeeStatusInput param)
        {
            DateTime currentDatetime = DateTime.Now;

            List<ClearedEmployeeStatusHistory> clearedEmployeeStatusHistories = param.ID
                .Select(x => new ClearedEmployeeStatusHistory()
                {
                    ClearedEmployeeID = Convert.ToInt32(x),
                    Status = param.Status,
                    CreatedDate = currentDatetime,
                    Remarks = param.Remarks,
                    CreatedBy = credentials.UserID,
                    IsActive = true
                }).ToList();

            var GetClearedEmployee = await _dbAccess.GetClearedEmployeeByIDsDefault(param.ID);

            var UpdateClearedEmployee = GetClearedEmployee.Select(x => new ClearedEmployee()
            {
                ID = x.ID,
                EmployeeID = x.EmployeeID,
                Accountability = x.Accountability,
                Status = param.Status,
                StatusUpdatedBy = credentials.UserID,
                StatusUpdatedDate = currentDatetime,
                StatusRemarks = param.Remarks,
                Computation = x.Computation,
                Agreed = x.Agreed,
                AgreedDate = x.AgreedDate,
                LastComment = x.LastComment,
                LastCommentDate = x.LastCommentDate,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate
            }).ToList();

            await _dbAccess.PutClearedEmployees(UpdateClearedEmployee);
            var Result = await _dbAccess.PostClearedEmployeeStatusHistory(clearedEmployeeStatusHistories);

            if (Result)
            {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
            }
        }
        public async Task<IActionResult> GetClearedEmployeeByEmployeeID(int EmployeeID)
        {
            return new OkObjectResult(await _dbAccess.GetClearedEmployeeByEmployeeID(EmployeeID));
        }
        public async Task<IActionResult> AddClearedEmployeeAgreed(int ID)
        {
            var ClearedEmployee = (await _dbAccess.GetClearedEmployeeByIDDefault(ID));
            ClearedEmployee.Agreed = true;
            ClearedEmployee.AgreedDate = DateTime.Now;

            if (await _dbAccess.PutClearedEmployee(ClearedEmployee))
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
    }
}
