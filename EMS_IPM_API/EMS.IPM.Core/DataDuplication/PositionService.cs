using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Transfer.DataDuplication.Position;
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
    public interface IPositionService
    {
        Task<IActionResult> GetIDByAutoComplete(APICredentials credentials, GetAutoCompleteInput param);

        Task<IActionResult> Sync(List<Position> param);

        Task<List<Position>> GetBySyncIDs(List<int> IDs);

        Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param);

        Task<IActionResult> GetPositionByOrgGroupDropdown(APICredentials credentials, GetDropDownInput param);
    }

    public class PositionService : Core.Shared.Utilities, IPositionService
    {
        private readonly IPositionDBAccess _dbAccess;

        public PositionService(IPMContext dbContext, IConfiguration iconfiguration,
            IPositionDBAccess dbAccess) : base(dbContext, iconfiguration)
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
                    Description = string.Concat(x.Code, " - ", x.Title)
                })
            );
        }

        public async Task<IActionResult> Sync(List<Position> param)
        {
            static List<Position> GetToAdd(List<Position> left, List<Position> right)
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
                    new Position
                    {
                        SyncID = x.newSet.newSet.ID,
                        SyncDate = DateTime.Now,
                        Code = x.newSet.newSet.Code,
                        Title = x.newSet.newSet.Title,
                        PositionLevelID = x.newSet.newSet.PositionLevelID,
                        ParentPositionID = x.newSet.newSet.ParentPositionID,
                        IsActive = x.newSet.newSet.IsActive,
                    })
                .ToList();
            }

            static List<Position> GetToUpdate(List<Position> left, List<Position> right)
            {
                return left.Join(
                right,
                x => new { x.SyncID },
                y => new { SyncID = y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Code.Equals(x.newSet.Code)
                    || !x.oldSet.Title.Equals(x.newSet.Title)
                    || x.oldSet.PositionLevelID != x.newSet.PositionLevelID
                    || x.oldSet.ParentPositionID != x.newSet.ParentPositionID
                    || x.oldSet.IsActive != x.newSet.IsActive
                )
                .Select(y => new Position
                {
                    ID = y.oldSet.ID,
                    SyncID = y.oldSet.SyncID,
                    SyncDate = DateTime.Now,
                    Code = y.newSet.Code,
                    Title = y.newSet.Title,
                    PositionLevelID = y.newSet.PositionLevelID,
                    ParentPositionID = y.newSet.ParentPositionID,
                    IsActive = y.newSet.IsActive
                })
                .ToList();
            }

            List<Position> localCopy = (await _dbAccess.GetBySyncIDs(param.Select(x => x.ID).ToList())).ToList();
            List<Position> ValueToAdd = GetToAdd(param, localCopy)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            List<Position> ValueToUpdate = GetToUpdate(localCopy, param)
            .GroupBy(x => x.SyncID)
            .Select(y => y.FirstOrDefault())
            .ToList();

            StringBuilder successMessage = new StringBuilder();

            if (ValueToAdd.Count > 0 || ValueToUpdate.Count > 0)
            {
                await _dbAccess.Sync(new List<Position>(), ValueToAdd, ValueToUpdate);
                if (ValueToAdd.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToAdd.Count
                        , " added Position record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                if (ValueToUpdate.Count > 0)
                    successMessage.Append(string.Concat(
                        ValueToUpdate.Count
                        , " updated Position record(s) "
                        , MessageUtilities.SUFF_SCSSMSG_REC_SYNCED
                        , Environment.NewLine));

                return new OkObjectResult(successMessage);
            }
            else
            {
                return new BadRequestObjectResult(string.Concat("Position ", MessageUtilities.SUFF_SCSSMSG_REC_SYNCED_UPDATED));
            }
        }

        public async Task<List<Position>> GetBySyncIDs(List<int> IDs)
        {
            return (await _dbAccess.GetBySyncIDs(IDs)).ToList();
        }

        public async Task<IActionResult> GetDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).ToList(), "SyncID", "Code", "Title")
                );
        }

        public async Task<IActionResult> GetCodeDropDown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetAll()).OrderBy(x => x.Code).ToList(), "Code", "Code", "Title", param.ID)
                );
        }

        public async Task<IActionResult> GetPositionByOrgGroupDropdown(APICredentials credentials, GetDropDownInput param)
        {
            return new OkObjectResult(
                    SharedUtilities.GetDropdown((await _dbAccess.GetPositionByOrgGroup(param)).ToList(), "SyncID", "Code", "Title")
                );
        }
    }
}