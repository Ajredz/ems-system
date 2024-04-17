using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Core.MRF;
using EMS.Manpower.Data.ApproverSetup;
using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.MRF;
using EMS.Manpower.Data.Reference;
using EMS.Manpower.Transfer;
using EMS.Manpower.Transfer.ApproverSetup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Manpower.Core.ApproverSetup
{
    public interface IApproverSetupService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> Put(APICredentials credentials, Form param);
        Task<IActionResult> GetSetupMRFApproverInsert(APICredentials credentials);
        Task<IActionResult> GetSetupMRFApproverUpdate(APICredentials credentials);
    }

    public class ApproverSetupService : EMS.Manpower.Core.Shared.Utilities, IApproverSetupService
    {
        private readonly IApproverSetupDBAccess _dbAccess;
        private readonly IPositionService _servicePosition;
        private readonly IPositionLevelService _servicePositionLevel;
        private readonly IOrgGroupService _serviceOrgGroup;
        private readonly IRegionService _serviceRegion;
        private readonly IReferenceDBAccess _referenceDBAccess;
        private readonly IMRFDBAccess _MRFdbAccess;

        public ApproverSetupService(ManpowerContext dbContext, IConfiguration iconfiguration,
            IApproverSetupDBAccess dbAccess,
            IPositionService servicePosition,
            IPositionLevelService servicePositionLevel,
            IOrgGroupService serviceOrgGroup,
            IRegionService serviceRegion,
            IReferenceDBAccess referenceDBAccess,
            IMRFDBAccess mrfdbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _servicePosition = servicePosition;
            _servicePositionLevel = servicePositionLevel;
            _serviceOrgGroup = serviceOrgGroup;
            _serviceRegion = serviceRegion;
            _referenceDBAccess = referenceDBAccess;
            _MRFdbAccess = mrfdbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarMRFDefinedApprover> result = await _dbAccess.GetList(input, rowStart);
            
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                OrgGroup = x.OrgGroup,
                HasApprover = x.HasApprover,
                LastModifiedDate = x.LastModifiedDate
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            List<TableVarApproverSetupGet> result = (await _dbAccess.GetByID(ID)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(
                    new Form
                    {
                        ID = result.First().ID,
                        OrgGroup = result.First().OrgGroup,
                        Approvers = result.Select(x => new Approvers {
                            PositionID = x.PositionID,
                            Position = x.Position,
                            HierarchyLevel = x.HierarchyLevel,
                            ApproverPositionID = x.ApproverPositionID,
                            ApproverPosition = x.ApproverPosition,
                            ApproverOrgGroupID = x.ApproverOrgGroupID,
                            ApproverOrgGroup = x.ApproverOrgGroup,
                            AltApproverPositionID = x.AltApproverPositionID,
                            AltApproverPosition = x.AltApproverPosition,
                            AltApproverOrgGroup = x.AltApproverOrgGroup,
                            AltApproverOrgGroupID = x.AltApproverOrgGroupID
                        }).ToList()
                    }
                    );
            }
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            if (param.ID <= 0)
                ErrorMessages.Add(string.Concat("Organizational Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.Approvers != null)
            {
                foreach (var item in param.Approvers)
                {
                    if (item.ApproverPositionID <= 0)
                        ErrorMessages.Add(string.Concat("Approver Position ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    if (item.ApproverOrgGroupID <= 0)
                        ErrorMessages.Add(string.Concat("Approver Org Group ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                } 
            }

            if (ErrorMessages.Count == 0)
            {

                List<MRFDefinedApprover> GetToAdd(List<MRFDefinedApprover> left, List<MRFDefinedApprover> right)
                {
                    return left.GroupJoin(
                        right,
                        x => new { x.HierarchyLevel, x.RequestingOrgGroupID, x.RequestingPositionID },
                        y => new { y.HierarchyLevel, y.RequestingOrgGroupID, y.RequestingPositionID },
                    (x, y) => new { newSet = x, oldSet = y })
                    .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                    (x, y) => new { newSet = x, oldSet = y })
                    .Where(x => x.oldSet == null)
                    .Select(x =>
                        new MRFDefinedApprover
                        {
                            HierarchyLevel = x.newSet.newSet.HierarchyLevel,
                            RequestingPositionID = x.newSet.newSet.RequestingPositionID,
                            RequestingOrgGroupID = x.newSet.newSet.RequestingOrgGroupID,
                            ApproverPositionID = x.newSet.newSet.ApproverPositionID,
                            ApproverOrgGroupID = x.newSet.newSet.ApproverOrgGroupID,
                            AltApproverPositionID= x.newSet.newSet.AltApproverPositionID,
                            AltApproverOrgGroupID= x.newSet.newSet.AltApproverOrgGroupID,
                            IsActive = true,
                            CreatedBy = credentials.UserID,
                            CreatedDate = DateTime.Now,
                        })
                    .ToList();
                }

                List<MRFDefinedApprover> GetToUpdate(List<MRFDefinedApprover> left, List<MRFDefinedApprover> right)
                {
                    return left.Join(
                    right,
                    x => new { x.HierarchyLevel, x.RequestingOrgGroupID, x.RequestingPositionID },
                    y => new { y.HierarchyLevel, y.RequestingOrgGroupID, y.RequestingPositionID },
                    (x, y) => new { oldSet = x, newSet = y })
                    .Where(x => x.oldSet.ApproverPositionID != x.newSet.ApproverPositionID
                        || x.oldSet.ApproverOrgGroupID != x.newSet.ApproverOrgGroupID
                        || x.oldSet.AltApproverPositionID != x.newSet.AltApproverPositionID
                        || x.oldSet.AltApproverOrgGroupID != x.newSet.AltApproverOrgGroupID
                    )
                    .Select(y => new MRFDefinedApprover
                    {
                        ID = y.oldSet.ID,
                        HierarchyLevel = y.newSet.HierarchyLevel,
                        RequestingPositionID = y.newSet.RequestingPositionID,
                        RequestingOrgGroupID = y.newSet.RequestingOrgGroupID,
                        ApproverPositionID = y.newSet.ApproverPositionID,
                        ApproverOrgGroupID = y.newSet.ApproverOrgGroupID,
                        AltApproverPositionID = y.newSet.AltApproverPositionID,
                        AltApproverOrgGroupID = y.newSet.AltApproverOrgGroupID,
                        IsActive = true,
                        CreatedBy = y.oldSet.CreatedBy,
                        CreatedDate = y.oldSet.CreatedDate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    })
                    .ToList();
                }

                List<MRFDefinedApprover> GetToDelete(List<MRFDefinedApprover> left, List<MRFDefinedApprover> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.HierarchyLevel, x.RequestingOrgGroupID, x.RequestingPositionID },
                    y => new { y.HierarchyLevel, y.RequestingOrgGroupID, y.RequestingPositionID },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new MRFDefinedApprover
                            {
                                ID = x.oldSet.oldSet.ID,
                                HierarchyLevel = x.oldSet.oldSet.HierarchyLevel,
                                RequestingPositionID= x.oldSet.oldSet.RequestingPositionID,
                                RequestingOrgGroupID= x.oldSet.oldSet.RequestingOrgGroupID,
                                ApproverPositionID = x.oldSet.oldSet.ApproverPositionID,
                                ApproverOrgGroupID = x.oldSet.oldSet.ApproverOrgGroupID,
                                AltApproverPositionID = x.oldSet.oldSet.AltApproverPositionID,
                                AltApproverOrgGroupID = x.oldSet.oldSet.AltApproverOrgGroupID,
                                IsActive = false,
                                CreatedBy = x.oldSet.oldSet.CreatedBy,
                                CreatedDate = x.oldSet.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = DateTime.Now
                            }).ToList();
                }

                List<MRFDefinedApprover> localCopy = (await _dbAccess.GetMRFDefinedApproverByOrgGroupID(param.ID)).Select(x => new MRFDefinedApprover
                { 
                    ID = x.ID,
                    HierarchyLevel = x.HierarchyLevel,
                    RequestingPositionID = x.RequestingPositionID,
                    RequestingOrgGroupID = x.RequestingOrgGroupID,
                    ApproverPositionID = x.ApproverPositionID,
                    ApproverOrgGroupID = x.ApproverOrgGroupID,
                    AltApproverPositionID = x.AltApproverPositionID,
                    AltApproverOrgGroupID = x.AltApproverOrgGroupID,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                    
                }).ToList();

                List<MRFDefinedApprover> paramCopy = param.Approvers == null ? new List<MRFDefinedApprover>() : param.Approvers.Select(x => new MRFDefinedApprover { 
                    HierarchyLevel = x.HierarchyLevel,
                    RequestingPositionID = x.PositionID,
                    RequestingOrgGroupID = param.ID,
                    ApproverPositionID = x.ApproverPositionID,
                    ApproverOrgGroupID = x.ApproverOrgGroupID,
                    AltApproverPositionID = x.AltApproverPositionID,
                    AltApproverOrgGroupID = x.AltApproverOrgGroupID
                }).ToList();


                List<MRFDefinedApprover> ValueToAdd = GetToAdd(paramCopy, localCopy).ToList();

                List<MRFDefinedApprover> ValueToUpdate = GetToUpdate(localCopy, paramCopy).ToList();

                List<MRFDefinedApprover> ValueToDelete = GetToDelete(localCopy, paramCopy).ToList();

                if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0 || ValueToDelete.Count > 0)
                {
                    await _dbAccess.Put(ValueToAdd, ValueToUpdate, ValueToDelete);
                }


                // TO EDIT AND REFLECT CHANGES APPROVER ON MRF
                foreach (var editapprover in param.Approvers)
                {
                    List<Data.MRF.MRF> getMRF = (await _MRFdbAccess.GetAll())
                        .Where(x => x.OrgGroupID.Equals(param.ID) &&
                        x.PositionID.Equals(editapprover.PositionID) &&
                        x.ApprovalLevel.Equals(editapprover.HierarchyLevel)
                        ).ToList();

                    foreach (var form in getMRF)
                    {
                        form.ApproverPositionID = editapprover.ApproverPositionID;
                        form.ApproverOrgGroupID = editapprover.ApproverOrgGroupID;
                        form.AltApproverPositionID = editapprover.AltApproverPositionID;
                        form.AltApproverOrgGroupID = editapprover.AltApproverOrgGroupID;
                        form.ModifiedBy = credentials.UserID;
                        form.ModifiedDate = DateTime.Now;

                        await _MRFdbAccess.Put(form);
                    }
                }

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetSetupMRFApproverInsert(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetSetupMRFApproverInsert());
        }
        public async Task<IActionResult> GetSetupMRFApproverUpdate(APICredentials credentials)
        {
            return new OkObjectResult(await _dbAccess.GetSetupMRFApproverUpdate());
        }
    }
}