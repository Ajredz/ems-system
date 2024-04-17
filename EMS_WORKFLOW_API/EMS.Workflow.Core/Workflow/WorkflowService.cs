using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Workflow;
using EMS.Workflow.Transfer.Workflow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.Workflow
{
    public interface IWorkflowService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);
        
        Task<IActionResult> GetIDWorkflowStepByAutoComplete(APICredentials credentials, GetWorkflowStepAutoCompleteInput param);

        Task<IActionResult> GetDropDown();

        Task<IActionResult> GetTransactionByRecordID(APICredentials credentials, GetTransactionByRecordIDInput param);

        Task<IActionResult> AddTransaction(APICredentials credentials, AddWorkflowTransaction param);
        Task<IActionResult> GetLastStatusUpdateByRecordIDs(APICredentials credentials, List<int> IDs);

        Task<IActionResult> GetCodeWorkflowStepByAutoComplete(APICredentials credentials, GetWorkflowStepAutoCompleteInput param);

        Task<IActionResult> GetWorkflowStepDropDown(APICredentials credentials, string WorkflowCode);

        Task<IActionResult> GetWorkflowStepByWorkflowCodeAndCode(APICredentials credentials, GetWorkflowStepByWorkflowIDAndCodeInput param);

        Task<IActionResult> GetLastStepByWorkflowCode(APICredentials credentials, string WorkflowCode);

        Task<IActionResult> GetWorkflowStepByWorkflowCode(APICredentials credentials, string WorkflowCode);

        Task<IActionResult> GetNextWorkflowStep(APICredentials credentials, GetNextWorkflowStepInput param);

        Task<IActionResult> GetWorkflowStepByRole(APICredentials credentials, GetWorkflowStepByRoleInput param);

        Task<IActionResult> GetAllWorkflowStep(APICredentials credentials, GetAllWorkflowStepInput param);

        Task<IActionResult> GetRolesByWorkflowStepCode(APICredentials credentials, GetRolesByWorkflowStepCodeInput param);
    }

    public class WorkflowService : Core.Shared.Utilities, IWorkflowService
    {
        private readonly EMS.Workflow.Data.Workflow.IWorkflowDBAccess _dbAccess;

        public WorkflowService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.Workflow.IWorkflowDBAccess dBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarWorkflow> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Code = x.Code,
                Description = x.Description
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.Workflow.Workflow result = await _dbAccess.GetByID(ID);
            IEnumerable<Data.Workflow.WorkflowStep> _workflowStepList = await _dbAccess.GetWorkflowStep(ID);
            IEnumerable<Data.Workflow.WorkflowStepApprover> _workflowStepApproverList = await _dbAccess.GetWorkflowStepApprover(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Module = result.Module,
                    Code = result.Code,
                    Description = result.Description,
                    WorkflowStepList = _workflowStepList.Select(x => new Transfer.Workflow.WorkflowStep
                    {
                        StepCode = x.Code,
                        StepDescription = x.Description,
                        StatusColor = x.StatusColor,
                        IsRequired = x.IsRequired,
                        TATDays = x.TATDays,
                        AllowBackflow = x.AllowBackflow,
                        ResultType = x.ResultType,
                        SendEmailToRequester = x.SendEmailToRequester,
                        SendEmailToApprover = x.SendEmailToApprover,
                        Order = x.Order,
                        WorkflowStepApproverList = _workflowStepApproverList
                            .Where(y => y.StepCode == x.Code)
                            .Select(z => new Transfer.Workflow.WorkflowStepApprover
                            {
                                StepCode = z.StepCode,
                                RoleID = z.RoleID
                            }).ToList()
                    }).OrderBy(x => x.Order).ToList(),
                    CreatedBy = result.CreatedBy
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Code = param.Code.Trim();
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Code.Length > 50)
                ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            else
            {
                if ((await _dbAccess.GetByCode(param.Code)).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            //if ((await _dbAccess.GetWorkflowStepByCode(param.WorkflowStepList.Select(x => x.StepCode).ToList())).Count() > 0)
            //{
            //    ErrorMessages.Add("Step Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            //}

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.Workflow.Workflow
                {
                    Module = param.Module,
                    Code = param.Code,
                    Description = param.Description,
                    CreatedBy = param.CreatedBy
                }
                , param.WorkflowStepList?.Select(x => new Data.Workflow.WorkflowStep
                {
                    Code = x.StepCode,
                    Description = x.StepDescription,
                    StatusColor = x.StatusColor,
                    IsRequired = x.IsRequired,
                    TATDays = x.TATDays,
                    AllowBackflow = x.AllowBackflow,
                    ResultType = x.ResultType,
                    SendEmailToRequester = x.SendEmailToRequester,
                    SendEmailToApprover = x.SendEmailToApprover,
                    Order = x.Order
                }).ToList()
                , param.WorkflowStepList
                .SelectMany(x => x.WorkflowStepApproverList
                .Select(y => new Data.Workflow.WorkflowStepApprover
                {
                    StepCode = y.StepCode,
                    RoleID = y.RoleID
                })).ToList()
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
            await _dbAccess.Delete(ID);
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            param.Code = param.Code.Trim();
            param.Description = param.Description.Trim();

            if (string.IsNullOrEmpty(param.Code))
                ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Code.Length > 50)
                ErrorMessages.Add(string.Concat("Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            else
            {
                List<Data.Workflow.Workflow> form = (await _dbAccess.GetByCode(param.Code)).ToList();

                if (form.Where(x => x.ID != param.ID).Count() > 0)
                {
                    ErrorMessages.Add("Code " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                if (!Regex.IsMatch(param.Code, RegexUtilities.REGEX_CODE))
                {
                    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                }
            }

            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add("Description " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.Workflow.WorkflowStepApprover> GetWorkflowStepApproverToDelete(List<Data.Workflow.WorkflowStepApprover> left, List<Data.Workflow.WorkflowStepApprover> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.WorkflowId, x.StepCode, x.RoleID },
                             y => new { y.WorkflowId, y.StepCode, y.RoleID },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Workflow.WorkflowStepApprover
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Workflow.WorkflowStepApprover> GetWorkflowStepApproverToAdd(List<Data.Workflow.WorkflowStepApprover> left, List<Data.Workflow.WorkflowStepApprover> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.WorkflowId, x.StepCode, x.RoleID },
                             y => new { y.WorkflowId, y.StepCode, y.RoleID },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Workflow.WorkflowStepApprover
                            {
                                WorkflowId = x.newSet.newSet.WorkflowId,
                                StepCode = x.newSet.newSet.StepCode,
                                RoleID = x.newSet.newSet.RoleID
                            }).ToList();
                }

                IEnumerable<Data.Workflow.WorkflowStepApprover> GetWorkflowStepApproverToUpdate(List<Data.Workflow.WorkflowStepApprover> left, List<Data.Workflow.WorkflowStepApprover> right)
                {
                    return left.Join(
                        right,
                             x => new { x.WorkflowId, x.StepCode, x.RoleID },
                             y => new { y.WorkflowId, y.StepCode, y.RoleID },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.StepCode.Equals(x.newSet.StepCode) ||
                                    (x.oldSet.RoleID != x.newSet.RoleID)
                              )
                        .Select(y =>
                            new Data.Workflow.WorkflowStepApprover
                            {
                                ID = y.oldSet.ID,
                                WorkflowId = y.newSet.WorkflowId,
                                StepCode = y.newSet.StepCode,
                                RoleID = y.newSet.RoleID
                            }).ToList();
                }

                IEnumerable<Data.Workflow.WorkflowStep> GetWorkflowStepToDelete(List<Data.Workflow.WorkflowStep> left, List<Data.Workflow.WorkflowStep> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.WorkflowId, x.Code },
                             y => new { y.WorkflowId, y.Code },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Workflow.WorkflowStep
                            {
                                ID = x.oldSet.oldSet.ID
                            }).ToList();
                }

                IEnumerable<Data.Workflow.WorkflowStep> GetWorkflowStepToAdd(List<Data.Workflow.WorkflowStep> left, List<Data.Workflow.WorkflowStep> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.WorkflowId, x.Code },
                             y => new { y.WorkflowId, y.Code },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Workflow.WorkflowStep
                            {
                                WorkflowId = x.newSet.newSet.WorkflowId,
                                Code = x.newSet.newSet.Code,
                                Description = x.newSet.newSet.Description,
                                StatusColor = x.newSet.newSet.StatusColor,
                                IsRequired = x.newSet.newSet.IsRequired,
                                TATDays = x.newSet.newSet.TATDays,
                                AllowBackflow = x.newSet.newSet.AllowBackflow,
                                SendEmailToRequester = x.newSet.newSet.SendEmailToRequester,
                                SendEmailToApprover = x.newSet.newSet.SendEmailToApprover,
                                ResultType = x.newSet.newSet.ResultType,
                                Order = x.newSet.newSet.Order
                            }).ToList();
                }

                IEnumerable<Data.Workflow.WorkflowStep> GetWorkflowStepToUpdate(List<Data.Workflow.WorkflowStep> left, List<Data.Workflow.WorkflowStep> right)
                {
                    return left.Join(
                        right,
                             x => new { x.WorkflowId, x.Code },
                             y => new { y.WorkflowId, y.Code },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Code.Equals(x.newSet.Code) ||
                                    !x.oldSet.Description.Equals(x.newSet.Description) ||
                                    (x.oldSet.StatusColor != x.newSet.StatusColor) ||
                                    (x.oldSet.IsRequired != x.newSet.IsRequired) ||
                                    (x.oldSet.TATDays != x.newSet.TATDays) ||
                                    (x.oldSet.AllowBackflow != x.newSet.AllowBackflow) ||
                                    (x.oldSet.SendEmailToRequester != x.newSet.SendEmailToRequester) ||
                                    (x.oldSet.SendEmailToApprover != x.newSet.SendEmailToApprover) ||
                                    !x.oldSet.ResultType.Equals(x.newSet.ResultType) ||
                                    (x.oldSet.Order != x.newSet.Order)
                              )
                        .Select(y =>
                            new Data.Workflow.WorkflowStep
                            {
                                ID = y.oldSet.ID,
                                WorkflowId = y.newSet.WorkflowId,
                                Code = y.newSet.Code,
                                Description = y.newSet.Description,
                                StatusColor = y.newSet.StatusColor,
                                IsRequired = y.newSet.IsRequired,
                                TATDays = y.newSet.TATDays,
                                AllowBackflow = y.newSet.AllowBackflow,
                                SendEmailToRequester = y.newSet.SendEmailToRequester,
                                SendEmailToApprover = y.newSet.SendEmailToApprover,
                                ResultType = y.newSet.ResultType,
                                Order = y.newSet.Order
                            }).ToList();
                }

                //WorkflowStep
                List<Data.Workflow.WorkflowStep> OldWorkflowStep = (await _dbAccess.GetWorkflowStep(param.ID)).ToList();

                List<Data.Workflow.WorkflowStep> WorkflowStepToAdd = GetWorkflowStepToAdd(OldWorkflowStep,
                    param.WorkflowStepList.Select(x => new Data.Workflow.WorkflowStep
                    {
                        WorkflowId = param.ID,
                        Code = x.StepCode,
                        Description = x.StepDescription,
                        StatusColor = x.StatusColor,
                        IsRequired = x.IsRequired,
                        TATDays = x.TATDays,
                        AllowBackflow = x.AllowBackflow,
                        ResultType = x.ResultType,
                        SendEmailToRequester = x.SendEmailToRequester,
                        SendEmailToApprover = x.SendEmailToApprover,
                        Order = x.Order
                    }).ToList()).ToList();

                List<Data.Workflow.WorkflowStep> WorkflowStepToUpdate = GetWorkflowStepToUpdate(OldWorkflowStep,
                    param.WorkflowStepList.Select(x => new Data.Workflow.WorkflowStep
                    {
                        WorkflowId = param.ID,
                        Code = x.StepCode,
                        Description = x.StepDescription,
                        StatusColor = x.StatusColor,
                        IsRequired = x.IsRequired,
                        TATDays = x.TATDays,
                        AllowBackflow = x.AllowBackflow,
                        ResultType = x.ResultType,
                        SendEmailToRequester = x.SendEmailToRequester,
                        SendEmailToApprover = x.SendEmailToApprover,
                        Order = x.Order
                    }).ToList()).ToList();

                List<Data.Workflow.WorkflowStep> WorkflowStepToDelete = GetWorkflowStepToDelete(OldWorkflowStep,
                    param.WorkflowStepList.Select(x => new Data.Workflow.WorkflowStep
                    {
                        WorkflowId = param.ID,
                        Code = x.StepCode
                    }).ToList()).ToList();

                //WorkflowStepApprover
                List<Data.Workflow.WorkflowStepApprover> OldWorkflowStepApprover = (await _dbAccess.GetWorkflowStepApprover(param.ID)).ToList();

                List<Data.Workflow.WorkflowStepApprover> WorkflowStepApproverToAdd = GetWorkflowStepApproverToAdd(OldWorkflowStepApprover,
                    param.WorkflowStepList.SelectMany(x => x.WorkflowStepApproverList.Select(y => new Data.Workflow.WorkflowStepApprover
                    {
                        WorkflowId = param.ID,
                        StepCode = y.StepCode,
                        RoleID = y.RoleID
                    })).ToList()).ToList();

                List<Data.Workflow.WorkflowStepApprover> WorkflowStepApproverToUpdate = GetWorkflowStepApproverToUpdate(OldWorkflowStepApprover,
                    param.WorkflowStepList.SelectMany(x => x.WorkflowStepApproverList.Select(y => new Data.Workflow.WorkflowStepApprover
                    {
                        WorkflowId = param.ID,
                        StepCode = y.StepCode,
                        RoleID = y.RoleID
                    })).ToList()).ToList();

                List<Data.Workflow.WorkflowStepApprover> WorkflowStepApproverToDelete = GetWorkflowStepApproverToDelete(OldWorkflowStepApprover,
                    param.WorkflowStepList.SelectMany(x => x.WorkflowStepApproverList.Select(y => new Data.Workflow.WorkflowStepApprover
                    {
                        WorkflowId = param.ID,
                        StepCode = y.StepCode,
                        RoleID = y.RoleID
                    })).ToList()).ToList();

                Data.Workflow.Workflow WorkflowData = await _dbAccess.GetByID(param.ID);
                WorkflowData.Module = param.Module;
                WorkflowData.Code = param.Code;
                WorkflowData.Description = param.Description;
                WorkflowData.ModifiedBy = credentials.UserID;
                WorkflowData.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(
                                     WorkflowData
                                   , WorkflowStepToDelete
                                   , WorkflowStepApproverToDelete
                                   , WorkflowStepToAdd
                                   , WorkflowStepApproverToAdd
                                   , WorkflowStepToUpdate
                                   , WorkflowStepApproverToUpdate
                                );
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetDropDown() {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "ID", "Code", null, null));
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Code
                })
            );
        }
        
        public async Task<IActionResult> GetIDWorkflowStepByAutoComplete(APICredentials credentials, GetWorkflowStepAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetWorkflowStepAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Code
                })
            );
        }

        public async Task<IActionResult> GetCodeWorkflowStepByAutoComplete(APICredentials credentials, GetWorkflowStepAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetWorkflowStepAutoComplete(param))
                .Select(x => new GetCodeByAutoCompleteOutput
                {
                    ID = x.Code,
                    Description = x.Code
                })
            );
        }

        public async Task<IActionResult> GetTransactionByRecordID(APICredentials credentials, GetTransactionByRecordIDInput param)
        {
            if (param.WorkflowID == 0)
            {
                Data.Workflow.Workflow workflow = (await _dbAccess.GetByCode(param.WorkflowCode)).First();
                param.WorkflowID = workflow.ID;
            }

            List<GetTransactionByRecordIDOutput> transactions = (await _dbAccess.GetTransactionByRecordID(param))
                .Select(x => new GetTransactionByRecordIDOutput
                {
                    Order = x.Order,
                    WorkflowCode = x.WorkflowCode,
                    StepCode = x.StepCode,
                    Step = x.Step,
                    Status = x.Status,
                    StatusCode = x.StatusCode,
                    Timestamp = x.Timestamp,
                    DateScheduled = x.DateScheduled,
                    DateCompleted = x.DateCompleted,
                    Remarks = x.Remarks,
                    ResultType = x.ResultType,
                    StartDatetime = x.StartDatetime,
                }).ToList();

            return new OkObjectResult(
                new AddWorkflowTransaction { 
                    History = transactions
                }
            );
        }

        public async Task<IActionResult> AddTransaction(APICredentials credentials, AddWorkflowTransaction param)
        {

            //param.WorkflowCode = (await _dbAccess.GetByID(param.WorkflowID)).Code;

            var Workflow = (await _dbAccess.GetByCode(param.WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));

            var LastStep = WorkflowStep.OrderByDescending(x => x.Order).First();

            TableVarCurrentWorkflowStep result = null;

            if (param.BatchUpdateRecordIDs?.Count > 0)
            {
                if (param.CurrentStepCode.Equals(LastStep.Code) & param.BatchUpdateRecordIDs.Count > 1)
                {
                    return new BadRequestObjectResult(MessageUtilities.ERRMSG_ALLOWED_APP_MRF);
                }

                foreach (var item in param.BatchUpdateRecordIDs) {
                    param.RecordID = item;
                    result = await _dbAccess.AddTransaction(credentials.UserID, param);
                }
            }
            else { 
                result = await _dbAccess.AddTransaction(credentials.UserID, param);
            } 


            return new OkObjectResult(new CurrentWorkflowStep {
                StepCode = result.StepCode,
                StepDescription = result.StepDescription,
               ApproverRoleIDs = result.ApproverRoleIDs,
                WorkflowStatus = result.WorkflowStatus
            });

        }
        public async Task<IActionResult> GetLastStatusUpdateByRecordIDs(APICredentials credentials, List<int> IDs)
        {
            return new OkObjectResult(await _dbAccess.GetLastStatusUpdateByRecordIDs(IDs));
        }

            public async Task<IActionResult> GetWorkflowStepDropDown(APICredentials credentials, string WorkflowCode)
        {
            var Workflow = (await _dbAccess.GetByCode(WorkflowCode));

            if(Workflow == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            if(Workflow.Count() == 0)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);


            return new OkObjectResult(SharedUtilities.GetDropdown(
                (await _dbAccess.GetWorkflowStepByWorkflowCode(Workflow.First().ID)).ToList(), "Code", "Code", null, null)
                );
        }

        public async Task<IActionResult> GetWorkflowStepByWorkflowCodeAndCode(APICredentials credentials, GetWorkflowStepByWorkflowIDAndCodeInput param)
        {
            var Workflow = (await _dbAccess.GetByCode(param.WorkflowCode));

            if (Workflow == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            if (Workflow.Count() == 0)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);


            EMS.Workflow.Data.Workflow.WorkflowStep result = 
                (await _dbAccess.GetWorkflowStepByWorkflowCodeAndCode(Workflow.First().ID, param.Code));

            return new OkObjectResult(new GetWorkflowStepByWorkflowIDAndCodeOutput
            {
                Code = result.Code,
                Description = result.Description,
                ResultType = result.ResultType
            });

        }

        public async Task<IActionResult> GetLastStepByWorkflowCode(APICredentials credentials, string WorkflowCode)
        {

            //param.WorkflowCode = (await _dbAccess.GetByID(param.WorkflowID)).Code;

            var Workflow = (await _dbAccess.GetByCode(WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));

            var LastStep = WorkflowStep.OrderByDescending(x => x.Order).First();

            return new OkObjectResult(new GetListOutput
            {
                ID = LastStep.ID,
                Code = LastStep.Code,
                Description = LastStep.Description,
                WorkflowID = LastStep.WorkflowId

            });

        }

        public async Task<IActionResult> GetWorkflowStepByWorkflowCode(APICredentials credentials, string WorkflowCode)
        {

            var Workflow = (await _dbAccess.GetByCode(WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));

            return new OkObjectResult(WorkflowStep.Select(x => 
            new GetWorkflowStepByWorkflowCodeOutput
            { 
                Code = x.Code,
                Description = x.Description,
                Order = x.Order
            }).ToList() );

        }

        public async Task<IActionResult> GetNextWorkflowStep(APICredentials credentials, GetNextWorkflowStepInput param)
        {

            var Workflow = (await _dbAccess.GetByCode(param.WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));
            int nextOrder = WorkflowStep.Where(x => x.Code.Equals(param.CurrentStepCode)).First().Order + 1;
            
            var result = (await _dbAccess.GetNextWorkflowStep(param));

            return new OkObjectResult(result.Select(x =>
            new GetNextWorkflowStepOutput
            {
                Code = x.Code,
                Description = x.Description,
                Order = x.Order
            }).ToList());

        }

        public async Task<IActionResult> GetWorkflowStepByRole(APICredentials credentials, GetWorkflowStepByRoleInput param)
        {

            var Workflow = (await _dbAccess.GetByCode(param.WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));

            var result = (await _dbAccess.GetWorkflowStepByRole(param));

            return new OkObjectResult(result.Select(x =>
            new GetWorkflowStepByRoleOutput
            {
                Code = x.Code,
                Description = x.Description
            }).ToList());

        }

        public async Task<IActionResult> GetAllWorkflowStep(APICredentials credentials, GetAllWorkflowStepInput param)
        {

            var Workflow = (await _dbAccess.GetByCode(param.WorkflowCode));
            var WorkflowStep = (await _dbAccess.GetWorkflowStepByWorkflowID(Workflow.First().ID));

            var result = (await _dbAccess.GetAllWorkflowStep(param));

            return new OkObjectResult(result.Select(x =>
            new GetAllWorkflowStepOutput
            {
                Code = x.Code,
                Description = x.Description
            }).ToList());

        }
        
        public async Task<IActionResult> GetRolesByWorkflowStepCode(APICredentials credentials, GetRolesByWorkflowStepCodeInput param)
        {

            var result = (await _dbAccess.GetRolesByWorkflowStepCode(param));

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            if (result.Count() == 0)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);

            return new OkObjectResult(result.Select(x =>
            new GetRolesByWorkflowStepCodeOutput
            {
                ID = x.ID,
                WorkflowID = x.WorkflowID,
                WorkflowCode = x.WorkflowCode,
                StepCode = x.StepCode,
                RoleID = x.RoleID

            }).ToList());

        }

    }
}