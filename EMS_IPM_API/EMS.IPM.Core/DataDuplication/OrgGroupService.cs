using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.DataDuplication
{
    public interface IOrgGroupService
    {
        Task<IActionResult> Sync(List<OrgGroup> param);

        Task<IActionResult> SyncPosition(List<OrgGroupPosition> param);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetFilteredIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> GetRegionAutoComplete(APICredentials credentials, Transfer.Shared.GetAutoCompleteInput param);

        Task<IActionResult> GetBranchAutoComplete(APICredentials credentials, Transfer.Shared.GetAutoCompleteInput param);
    }

    public class OrgGroupService : Core.Shared.Utilities, IOrgGroupService
    {
        private readonly IOrgGroupDBAccess _dbAccess;

        public OrgGroupService(IPMContext dbContext, IConfiguration iconfiguration,
            IOrgGroupDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }
        
        public async Task<IActionResult> Sync(List<OrgGroup> param)
        {
            static List<OrgGroup> GetToAdd(List<OrgGroup> left, List<OrgGroup> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new OrgGroup
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        ParentOrgID = x.newSet.newSet.ParentOrgID,
                        Code = x.newSet.newSet.Code,
                        Description = x.newSet.newSet.Description,
                        OrgType = x.newSet.newSet.OrgType,
                        IsActive = x.newSet.newSet.IsActive,
                    })
                .ToList();
            }

            static List<OrgGroup> GetToUpdate(List<OrgGroup> left, List<OrgGroup> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || !x.oldSet.Description.Equals(x.newSet.Description)
                    || !x.oldSet.OrgType.Equals(x.newSet.OrgType)
                    || x.oldSet.ParentOrgID != x.newSet.ParentOrgID
                    || x.oldSet.IsActive != x.newSet.IsActive
                )
                .Select(y => new OrgGroup
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    ParentOrgID = y.newSet.ParentOrgID,
                    Code = y.newSet.Code,
                    Description = y.newSet.Description,
                    OrgType = y.newSet.OrgType,
                    IsActive = y.newSet.IsActive,
                })
                .ToList();
            }

            List<OrgGroup> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            //List<OrgGroup> ValueToDelete = GetToDelete(localCopy, param).ToList();
            List<OrgGroup> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<OrgGroup> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<OrgGroup>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added OrgGroup record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated OrgGroup record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage.ToString());
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("OrgGroup ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<IActionResult> SyncPosition(List<OrgGroupPosition> param)
        {
            static List<OrgGroupPosition> GetToAdd(List<OrgGroupPosition> left, List<OrgGroupPosition> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { SyncID = x.ID },
                    y => new { y.SyncID },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new OrgGroupPosition
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        OrgGroupID = x.newSet.newSet.OrgGroupID,
                        PositionID = x.newSet.newSet.PositionID,
                        IsActive = x.newSet.newSet.IsActive,
                        IsHead = x.newSet.newSet.IsHead,
                        ReportingPositionID = x.newSet.newSet.ReportingPositionID,
                    })
                .ToList();
            }

            static List<OrgGroupPosition> GetToUpdate(List<OrgGroupPosition> left, List<OrgGroupPosition> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.OrgGroupID != x.newSet.OrgGroupID
                || x.oldSet.PositionID != x.newSet.PositionID
                || x.oldSet.IsActive != x.newSet.IsActive
                || x.oldSet.IsHead != x.newSet.IsHead
                || x.oldSet.ReportingPositionID != x.newSet.ReportingPositionID
                )
                .Select(y => new OrgGroupPosition
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    OrgGroupID = y.newSet.OrgGroupID,
                    PositionID = y.newSet.PositionID,
                    IsActive = y.newSet.IsActive,
                    IsHead = y.newSet.IsHead,
                    ReportingPositionID = y.newSet.ReportingPositionID,
                })
                .ToList();
            }

            List<OrgGroupPosition> localCopy = (await _dbAccess.GetPositionBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            //List<OrgGroupPosition> ValueToDelete = GetToDelete(localCopy, param).ToList();
            List<OrgGroupPosition> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<OrgGroupPosition> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.SyncPosition(new List<OrgGroupPosition>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added OrgGroup record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated OrgGroup record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage.ToString());
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("OrgGroup ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.DataDuplication.OrgGroup.OrgGroup orgGroup = await _dbAccess.GetByID(ID);
            if (orgGroup == null)
                return new BadRequestObjectResult("Org Group " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS);
            else
                return new OkObjectResult(new Transfer.DataDuplication.OrgGroup.Form {
                    ID = orgGroup.SyncID,
                    Code = orgGroup.Code,
                    Description = orgGroup.Description,
                });
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Description")
                );
        }

        public async Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "Code", "Code", "Description", param.ID)
                );
        }

        public async Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SyncID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        public async Task<IActionResult> GetFilteredIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetFilteredIDByAutoComplete(param))
                .Select(x => new GetIDByAutoCompleteOutput
                {
                    ID = x.SyncID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }

        public async Task<IActionResult> GetRegionAutoComplete(APICredentials credentials, Transfer.Shared.GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetRegionAutoComplete(param))
                .Select(x => new Transfer.Shared.GetAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Description
                })
            );
        } 
        
        public async Task<IActionResult> GetBranchAutoComplete(APICredentials credentials, Transfer.Shared.GetAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetBranchAutoComplete(param))
                .Select(x => new Transfer.Shared.GetAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Description
                })
            );
        }
    }
}