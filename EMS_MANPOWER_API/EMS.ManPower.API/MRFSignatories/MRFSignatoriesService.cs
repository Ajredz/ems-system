using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DataDuplication.PositionLevel;
using EMS.Manpower.Data.DataDuplication.SystemRole;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.Workflow;
using EMS.Manpower.Transfer;
using EMS.Manpower.Transfer.MRFSignatories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using static Utilities.API.SharedUtilities;

namespace EMS.Manpower.Core.MRFSignatories
{
    public interface IMRFSignatoriesService
    {
        Task<IActionResult> GetByRolePosition(APICredentials credentials, GetByUserPositionInput param);

        Task<IActionResult> Put(APICredentials credentials, List<Form> param);

        Task<IActionResult> GetMRFSignatoriesAdd(APICredentials credentials, GetMRFSignatoriesAddInput param);
    }

    public class MRFSignatoriesService : EMS.Manpower.Core.Shared.Utilities, IMRFSignatoriesService
    {
        private readonly EMS.Manpower.Data.MRFSignatories.IMRFSignatoriesDBAccess _dbAccess;
        private readonly EMS.Manpower.Core.DataDuplication.ISystemRoleService _serviceSystemRole;
        private readonly EMS.Manpower.Core.DataDuplication.IPositionService _servicePosition;
        private readonly EMS.Manpower.Core.DataDuplication.IPositionLevelService _servicePositionLevel;
        
        public MRFSignatoriesService(ManpowerContext dbContext, IConfiguration iconfiguration,
            EMS.Manpower.Data.MRFSignatories.IMRFSignatoriesDBAccess dbAccess,
            EMS.Manpower.Core.DataDuplication.ISystemRoleService serviceSystemRole,
            EMS.Manpower.Core.DataDuplication.IPositionService servicePosition,
            EMS.Manpower.Core.DataDuplication.IPositionLevelService servicePositionLevel
            ) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _serviceSystemRole = serviceSystemRole;
            _servicePosition = servicePosition;
            _servicePositionLevel = servicePositionLevel;
        }

        public async Task<IActionResult> GetByRolePosition(APICredentials credentials, GetByUserPositionInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByRolePosition(param.UserID, param.PositionID))
                .Select(x => new Form
                {
                    ID = x.ID,
                    WorkflowID = x.WorkflowID,
                    WorkflowStepID = x.WorkflowStepID,
                    WorkflowStepApproverID = x.WorkflowStepApproverID,
                    RequesterID = x.UserID,
                    PositionID = x.PositionID,
                    PositionLevelID = x.PositionLevelID,
                    ApproverRoleID = x.ApproverRoleID,
                    ApproverRoleName = x.ApproverName,
                    ApproverDescription = x.ApproverDescription,
                    WorkflowStepCode = x.WorkflowStepCode,
                    TATDays = x.TATDays,
                    Order = x.Order
                }).OrderBy(y => y.Order));
        }

        public async Task<IActionResult> Put(APICredentials credentials, List<Form> param)
        {
            static List<Form> GetWorkflowStepToDelete(List<Form> left, List<Form> right)
            {
                return left.GroupJoin(
                            right,
                            x => new { x.RequesterID, x.PositionID, x.Order },
                            y => new { y.RequesterID, y.PositionID, y.Order },
                            (x, y) => new { oldSet = x, newSet = y })
                            .SelectMany(x => x.newSet.DefaultIfEmpty(),
                            (x, y) => new { oldSet = x, newSet = y })
                            .Where(x => x.newSet == null)
                            .Select(x =>
                                new Form
                                {
                                    ID = x.oldSet.oldSet.ID,
                                    WorkflowID = x.oldSet.oldSet.WorkflowID,
                                    WorkflowStepID = x.oldSet.oldSet.WorkflowStepID,
                                    WorkflowStepApproverID = x.oldSet.oldSet.WorkflowStepApproverID
                                }).ToList();
            }

            static List<Form> GetWorkflowStepToAdd(List<Form> left, List<Form> right)
            {
                return right.GroupJoin(
                  left,
                       x => new { x.RequesterID, x.PositionID, x.Order },
                       y => new { y.RequesterID, y.PositionID, y.Order },
                  (x, y) => new { newSet = x, oldSet = y })
                  .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                  (x, y) => new { newSet = x, oldSet = y })
                  .Where(x => x.oldSet == null)
                  .Select(x =>
                      new Form
                      {
                          WorkflowID = x.newSet.newSet.WorkflowID,
                          WorkflowStepID = x.newSet.newSet.WorkflowStepID,
                          WorkflowStepApproverID = x.newSet.newSet.WorkflowStepApproverID,
                          RequesterID = x.newSet.newSet.RequesterID,
                          PositionID = x.newSet.newSet.PositionID,
                          PositionLevelID = x.newSet.newSet.PositionLevelID,
                          ApproverDescription = x.newSet.newSet.ApproverDescription,
                          ApproverRoleID = x.newSet.newSet.ApproverRoleID,
                          WorkflowStepCode = x.newSet.newSet.WorkflowStepCode,
                          TATDays = x.newSet.newSet.TATDays,
                          Order = x.newSet.newSet.Order
                      }).ToList();
            }

            static List<Form> GetWorkflowStepToUpdate(List<Form> left, List<Form> right)
            {
                return left.Join(
                           right,
                          x => new { x.RequesterID, x.PositionID, x.Order },
                       y => new { y.RequesterID, y.PositionID, y.Order },
                           (x, y) => new { oldSet = x, newSet = y })
                           .Where(x => !x.oldSet.ApproverDescription.Equals(x.newSet.ApproverDescription)
                            || x.oldSet.TATDays != x.newSet.TATDays
                            || x.oldSet.ApproverRoleID != x.newSet.ApproverRoleID)
                           .Select(y => new Form
                           {
                               ID = y.oldSet.ID,
                               WorkflowID = y.oldSet.WorkflowID,
                               WorkflowStepID = y.oldSet.WorkflowStepID,
                               WorkflowStepCode = y.oldSet.WorkflowStepCode,
                               WorkflowStepApproverID = y.oldSet.WorkflowStepApproverID,
                               RequesterID = y.oldSet.RequesterID,
                               PositionID = y.oldSet.PositionID,
                               PositionLevelID = y.oldSet.PositionLevelID,
                               ApproverDescription = y.newSet.ApproverDescription,
                               ApproverRoleID = y.newSet.ApproverRoleID,
                               TATDays = y.newSet.TATDays,
                               Order = y.newSet.Order
                           }).ToList();
            }

            if (param.Count == 0)
            {
                ErrorMessages.Add("Approver " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            }
            else if (param.Count >= 1)
            {
                if (param.First().RequesterID == 0)
                    ErrorMessages.Add("Role " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

                if (param.First().PositionID == 0)
                    ErrorMessages.Add("Position " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

                if (ErrorMessages.Count <= 0)
                {
                    List<Form> oldSet =
                       (await _dbAccess.GetByRolePosition(param.First().RequesterID, param.First().PositionID))
                        .Select(x => new Form
                        {
                            ID = x.ID,
                            WorkflowID = x.WorkflowID,
                            WorkflowStepID = x.WorkflowStepID,
                            WorkflowStepApproverID = x.WorkflowStepApproverID,
                            RequesterID = x.UserID,
                            PositionID = x.PositionID,
                            PositionLevelID = x.PositionLevelID,
                            ApproverRoleID = x.ApproverRoleID,
                            ApproverDescription = x.ApproverDescription,
                            WorkflowStepCode = x.WorkflowStepCode,
                            TATDays = x.TATDays,
                            Order = x.Order
                        }).ToList();

                    List<Form> toDelete = GetWorkflowStepToDelete(oldSet, param);
                    List<Form> toAdd = GetWorkflowStepToAdd(oldSet, param);
                    List<Form> toUpdate = GetWorkflowStepToUpdate(oldSet, param);

                    // Check if record will be deleted
                    bool IsDeleted = param.Count == 1 & param.First().ApproverRoleID == 0;
                    // Check if record is existing will be edited
                    bool IsEdited = oldSet.Count > 0 & (toDelete.Count > 0 | toUpdate.Count > 0 | toAdd.Count > 0);
                    // Check if record is for adding
                    bool IsAdded = oldSet.Count == 0;

                    if (IsDeleted)
                    {
                        int MRFSignatoriesID =
                        param.Count == 1 & param.First().ApproverRoleID == 0 ? toDelete.First().ID : 0;
                        int WorkflowID =
                            param.Count == 1 & param.First().ApproverRoleID == 0 ? toDelete.First().WorkflowID : 0;

                        _resultView.IsSuccess = await _dbAccess.Delete(
                            new EMS.Manpower.Data.MRFSignatories.MRFSignatories()
                            {
                                ID = MRFSignatoriesID
                            },
                            new Workflow
                            {
                                ID = WorkflowID
                            },
                            toDelete.Select(x => new WorkflowStep
                            {
                                ID = x.WorkflowStepID
                            }).ToList(),
                            toDelete.Select(x => new WorkflowStepApprover
                            {
                                ID = x.WorkflowStepApproverID
                            }).ToList()
                        );
                    }
                    else if (IsEdited)
                    {
                        _resultView.IsSuccess = await _dbAccess.Put(
                        // To be deleted
                        toDelete.Select(x => new WorkflowStep
                        {
                            ID = x.WorkflowStepID
                        }).ToList(),
                        toDelete.Select(x => new WorkflowStepApprover
                        {
                            ID = x.WorkflowStepApproverID
                        }).ToList(),
                        // To be added
                        toAdd.Select(x => new WorkflowStep
                        {
                            WorkflowID = toAdd.First().WorkflowID,
                            Description = x.ApproverDescription,
                            TATDays = x.TATDays,
                            Order = x.Order,
                            Code =
                                string.Concat(param.First().RequesterID, "_"
                                , param.First().PositionID, "_", "APPROVER_", x.Order), // System generated
                            AllowBackflow = false,                                      // Default value
                            ResultType = Enums.ResultType.APPROVE_REJECT.ToString(),          // Default value
                            IsRequired = true,                                          // Default value
                        }).ToList(),
                        toAdd.Select(x => new WorkflowStepApprover
                        {
                            WorkflowID = toAdd.First().WorkflowID,
                            StepCode = string.Concat(param.First().RequesterID, "_"
                                , param.First().PositionID, "_", "APPROVER_", x.Order), // System generated
                            RoleID = x.ApproverRoleID,
                        }).ToList(),
                        // To be updated
                        toUpdate.Select(x => new WorkflowStep
                        {
                            ID = x.WorkflowStepID,
                            WorkflowID = x.WorkflowID,
                            Description = x.ApproverDescription,
                            TATDays = x.TATDays,
                            Order = x.Order,
                            Code = x.WorkflowStepCode,
                            AllowBackflow = false,                                      // Default value
                            ResultType = Enums.ResultType.APPROVE_REJECT.ToString(),          // Default value
                            IsRequired = true,                                          // Default value
                        }).ToList(),
                        toUpdate.Select(x => new WorkflowStepApprover
                        {
                            ID = x.WorkflowStepApproverID,
                            WorkflowID = x.WorkflowID,
                            StepCode = x.WorkflowStepCode,
                            RoleID = x.ApproverRoleID
                        }).ToList());
                    }
                    else if (IsAdded)
                    {
                        string workflowCode = string.Concat("WF_MRF_", param.First().RequesterID, "_", param.First().PositionID);
                        _resultView.IsSuccess = await _dbAccess.Post(
                            param.Select(x => new EMS.Manpower.Data.MRFSignatories.MRFSignatories
                            {
                                RequesterID = param.First().RequesterID,
                                PositionID = param.First().PositionID,
                                WorkflowCode = workflowCode
                            }).First(),
                            param.Select(x => new Workflow
                            {
                                Module = SYSTEM_MODULE.MANPOWER.ToString(),
                                Code = workflowCode,
                                Description = _MRFDescriptionDefault,
                                CreatedBy = credentials.UserID
                            }).First(),
                            param.Select(x => new WorkflowStep
                            {
                                WorkflowID = 0, // To be populated later
                                Description = x.ApproverDescription,
                                TATDays = x.TATDays,
                                Order = x.Order,
                                Code =
                                    string.Concat(param.First().RequesterID, "_"
                                    , param.First().PositionID, "_", "APPROVER_", x.Order), // System generated
                                AllowBackflow = false,                                      // Default value
                                ResultType = Enums.ResultType.APPROVE_REJECT.ToString(),          // Default value
                                IsRequired = true,                                          // Default value
                            }).ToList(),
                            param.Select(x => new WorkflowStepApprover
                            {
                                WorkflowID = 0, // To be populated later
                                StepCode =
                                    string.Concat(param.First().RequesterID, "_"
                                    , param.First().PositionID, "_", "APPROVER_", x.Order), // System generated
                                RoleID = x.ApproverRoleID,
                            }).ToList()
                            );
                    }
                    else
                    {
                        // No changes
                        return new OkObjectResult(MessageUtilities.ERRMSG_NO_CHANGES);
                    }
                }
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE); 
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetMRFSignatoriesAdd(APICredentials credentials, GetMRFSignatoriesAddInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetMRFSignatoriesAdd(param.RecordID, param.RequesterID, param.PositionID))
                .Select(x => new GetMRFSignatoriesAddOutput { 
                    ApproverDescription = x.ApproverDescription,
                    ApproverName = x.ApproverName,
                    ApprovalTAT = x.ApprovalTAT,
                    ApprovalStatus = x.ApprovalStatus,
                    ApprovalStatusCode = x.ApprovalStatusCode,
                    ApprovedDate = x.ApprovedDate
                }));
        }

    }
}