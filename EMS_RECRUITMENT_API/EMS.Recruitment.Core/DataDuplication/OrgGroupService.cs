using EMS.Recruitment.Data.DataDuplication.OrgGroup;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Transfer.DataDuplication.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.DataDuplication
{
    public interface IOrgGroupService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> Sync(List<OrgGroup> param);

        Task<List<OrgGroup>> GetBySyncIDs(List<int> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetIDByOrgTypeAutoComplete(APICredentials credentials, GetByOrgTypeAutoCompleteInput param);
    }

    public class OrgGroupService : Core.Shared.Utilities, IOrgGroupService
    {
        private readonly IOrgGroupDBAccess _dbAccess;

        public OrgGroupService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IOrgGroupDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
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
                        IsActive = x.newSet.newSet.IsActive
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

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("OrgGroup ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<OrgGroup>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Description")
                );
        }

        public async Task<IActionResult> GetIDByOrgTypeAutoComplete(APICredentials credentials, GetByOrgTypeAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetByOrgGroupAutoComplete(param))
                .Select(x => new GetIDByOrgTypeAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = string.Concat(x.Code, " - ", x.Description)
                })
            );
        }
    }
}